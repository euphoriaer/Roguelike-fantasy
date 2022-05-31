using Battle;
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
    public AnimationClip animationClip;
    [SerializeField, HideInInspector]
    public bool isRootMotionPreBaked;

    [LabelText("循环播放(动画长度超过原动作片段时，循环动作)")]
    [SerializeField]
    public bool Loop = false;

    [LabelText("播放速度")]
    [SerializeField]
    public float PlaySpeed = 1;

    public float StarOffsetTime = 0;
    //bake Position
    public bool useRootMotion = true;

    private const int ROOTMOTION_FRAMERATE = 60;
    private Animator _animator;
    /// <summary>
    /// 与下一个动画融合时长，默认为0 不融合
    /// </summary>
    public float CorssTransTime=0;

    [HideInInspector]
    [SerializeField] private float _length = 1f;

    private AnimationClipPlayable playableClip;
    private PlayableGraph playableGraph;
    [SerializeField, HideInInspector]
    private List<Vector3> rmPositions;

    [SerializeField, HideInInspector]
    private List<Quaternion> rmRotations;

    private bool useBakedRootMotion;
    
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

    public override bool canCrossBlend
    {
        get { return false; }
    }

    public override string info
    {
        get { return animationClip != null ? animationClip.name : base.info; }
    }

    //Todo OnGui 红色表示动画长度
    public override float length
    {
        get { return _length; }
        set { _length = value; }
    }

    [Button("刷新", 40)]
    public override void Refresh()
    {
        //length 实际是 cutscene 片段总时长
        length = (animationClip.length) / PlaySpeed - StarOffsetTime;
    }

    protected override void OnClipGUI(Rect rect)
    {
        //偏移绘制
        float offsetDraw = (StarOffsetTime / length) * rect.width;//根据偏移占据的width 绘制
        Rect rec = new Rect(rect.x, rect.y, offsetDraw, rect.height);
        UnityEditor.Handles.DrawSolidRectangleWithOutline(rec, Color.black, Color.blue);

        //循环绘制,如果有偏移不绘制循环
        if ((!Loop)||(StarOffsetTime>0))
        {
            return;
        }
        //if (CurClip != null)  //默认颜色较暗淡，使用下方自定义颜色
        //{
        //    EditorTools.DrawLoopedLines(rect, CurClip.length / PlaySpeed,this.length,0);
        //}

        if (animationClip == null)
            return;
        //length 
        float cycleLength = animationClip.length / PlaySpeed; //每帧长度  

        cycleLength = Mathf.Abs(cycleLength);

        if (cycleLength != 0 && length != 0)
        {
            //UnityEditor.Handles.color = new Color(84, 255, 159, 0.2f);
            //Color rectangleColor = new Color(84, 255, 159, 0.2f);
            UnityEditor.Handles.color = Color.red;
            for (float curFrame = 0; (curFrame < _length); curFrame += cycleLength)//循环绘制
            {
                var posX = (curFrame / length) * rect.width;//根据占比绘制
                UnityEditor.Handles.DrawLine(new Vector2(posX, 0), new Vector2(posX, rect.height));

            }


        }
    }

    protected override void OnCreate()
    {
        Refresh();
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

    protected override void OnExit()
    {

        base.OnExit();
    }

    protected override bool OnInitialize()
    {
        //EventManager.OnNewMessage += GetMessage;
        return base.OnInitialize();
    }
    protected override void OnUpdate(float time)
    {
        var curClipLength = animationClip.length;
        float normalizedBefore = (time+StarOffsetTime)* PlaySpeed;
        if (Loop && normalizedBefore > curClipLength)
        {
            //要跳转到的动画时长  超过动画长度 ，取余  只有循环动画才取余
            normalizedBefore = normalizedBefore % curClipLength;
        }
        
        
        playableClip.SetTime(normalizedBefore);

        if (useRootMotion && useBakedRootMotion)
        {
            ApplyBakedRootMotion((time + StarOffsetTime));
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
}