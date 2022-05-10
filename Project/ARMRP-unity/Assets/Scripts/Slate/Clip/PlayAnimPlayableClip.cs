using Sirenix.OdinInspector;
using Slate;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

[Name("播放动画Playable")]
[Attachable(typeof(AnimTrack))]
public class PlayAnimPlayableClip : CutsceneClip<Animator>, IDirectable
{
    private const int ROOTMOTION_FRAMERATE = 60;

    public AnimationClip animationClip;

    [LabelText("播放速度")]
    [SerializeField]
    public float PlaySpeed = 1;

    [LabelText("循环播放(动画长度超过原动作片段时，循环动作)")]
    [SerializeField]
    public bool Loop = false;

    [HideInInspector]
    [SerializeField] private float _length = 1f;

    private AnimationClipPlayable playableClip;
    private PlayableGraph playableGraph;

    //bake Position
    public bool useRootMotion = true;

    private bool useBakedRootMotion;

    [SerializeField, HideInInspector]
    public bool isRootMotionPreBaked;

    [SerializeField, HideInInspector]
    private List<Vector3> rmPositions;

    [SerializeField, HideInInspector]
    private List<Quaternion> rmRotations;

    private Animator _animator;

    public Animator animator
    {
        get
        {
            if (_animator == null || _animator.gameObject != actor.gameObject)
            {
                _animator = ActorComponent;
            }
            return _animator;
        }
    }

    protected override void OnCreate()
    {
        Refresh();
    }

    protected override bool OnInitialize()
    {
        //EventManager.OnNewMessage += GetMessage;
        return base.OnInitialize();
    }

    protected override void OnEnter()
    {
        playableGraph = PlayableGraph.Create();

        var playableOutput = AnimationPlayableOutput.Create(playableGraph, "Animation", ActorComponent);

        // 将剪辑包裹在可播放项中

        playableClip = AnimationClipPlayable.Create(playableGraph, animationClip);

        // 将可播放项连接到输出

        playableOutput.SetSourcePlayable(playableClip);

        // 播放该图。

        playableGraph.Play();

        playableClip.SetPlayState(PlayState.Paused);
        //使时间停止自动前进。

        //是否烘焙position 位置
        if (useRootMotion)
        {
            BakeRootMotion();
        }
    }

    private void BakeRootMotion()
    {
        if (isRootMotionPreBaked)
        {
            animator.applyRootMotion = false;
            useBakedRootMotion = true;
            return;
        }

        var rb = animator.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.MovePosition(animator.transform.localPosition);
            rb.MoveRotation(animator.transform.localRotation);
        }

        useBakedRootMotion = false;
        animator.applyRootMotion = true;
        rmPositions = new List<Vector3>();
        rmRotations = new List<Quaternion>();
        var tempActiveClips = 0;
        var updateInterval = (1f / ROOTMOTION_FRAMERATE);
        for (var time = startTime - updateInterval; time <= endTime + updateInterval; time += updateInterval)
        {
            EvaluateTrackClips(time, time - updateInterval, ref tempActiveClips);

            if (tempActiveClips > 0)
            {
                playableGraph.Evaluate(updateInterval);
            }

            //apparently animator automatically sets rigidbody pos/rot if attached on same go when evaluated.
            //thus we read pos/rot from rigidbody in such cases.
            var pos = rb != null ? rb.position : animator.transform.localPosition;
            var rot = rb != null ? rb.rotation : animator.transform.localRotation;
            rmPositions.Add(pos);
            rmRotations.Add(rot);
        }
        animator.applyRootMotion = false;
        useBakedRootMotion = true;
    }

    private void EvaluateTrackClips(float time, float previousTime, ref int tempActiveClips)
    {
        IDirectable clip = this;
        if (time >= clip.startTime && previousTime < clip.startTime)
        {
            tempActiveClips++;
            //clip.Enter();
        }

        if (time >= clip.startTime && time <= clip.endTime)
        {
            clip.Update(time - clip.startTime, previousTime - clip.startTime);
        }

        if ((time > clip.endTime || time >= this.endTime) && previousTime <= clip.endTime)
        {
            tempActiveClips--;
            //clip.Exit();
        }
    }

    protected override void OnUpdate(float time)
    {
        var curClipLength = animationClip.length;
        float normalizedBefore = time * PlaySpeed;
        if (Loop && time > curClipLength)
        {
            //要跳转到的动画时长 ，根据Update Time 取余 ，需要归一化时间
            normalizedBefore = time * PlaySpeed % curClipLength;
        }
        playableClip.SetTime(normalizedBefore);

        if (useRootMotion && useBakedRootMotion)
        {
            ApplyBakedRootMotion(time);
        }
    }

    private void ApplyBakedRootMotion(float time)
    {
        var frame = Mathf.FloorToInt(time * ROOTMOTION_FRAMERATE);
        var nextFrame = frame + 1;
        nextFrame = nextFrame < rmPositions.Count ? nextFrame : rmPositions.Count - 1;

        var tNow = frame * (1f / ROOTMOTION_FRAMERATE);
        var tNext = nextFrame * (1f / ROOTMOTION_FRAMERATE);

        var posNow = rmPositions[frame];
        var posNext = rmPositions[nextFrame];
        var pos = Vector3.Lerp(posNow, posNext, Mathf.InverseLerp(tNow, tNext, time));
        animator.transform.localPosition = pos;

        var rotNow = rmRotations[frame];
        var rotNext = rmRotations[nextFrame];
        var rot = Quaternion.Lerp(rotNow, rotNext, Mathf.InverseLerp(tNow, tNext, time));
        animator.transform.localRotation = rot;
    }

    protected override void OnExit()
    {
        base.OnExit();
    }

    [Button("刷新", 40)]
    public override void Refresh()
    {
        length = animationClip.length / PlaySpeed;
    }

    //Todo OnGui 红色表示动画长度

    public override string info
    {
        get { return animationClip != null ? animationClip.name : base.info; }
    }

    public override float length
    {
        get { return _length; }
        set { _length = value; }
    }

    public override bool canCrossBlend
    {
        get { return false; }
    }
}