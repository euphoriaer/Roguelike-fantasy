using Sirenix.OdinInspector;
using Slate;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

[Name("动画混合测试")]
[Attachable(typeof(AnimTrack))]
public class AnimPlayableMax : CutsceneClip<Animator>
{
    public AnimationClip animationClip1;

    public AnimationClip animationClip2;

    public float weight;
    [LabelText("播放速度")]
    [SerializeField]
    public float PlaySpeed = 1;

    [LabelText("循环播放")]
    [SerializeField]
    public bool Loop = false;

    [HideInInspector]
    [SerializeField] private float _length = 1 / 30f;

    protected override void OnCreate()
    {
        Refresh();
    }
    
    private AnimationClipPlayable playableClip1;
    private AnimationClipPlayable playableClip2;
    private PlayableGraph playableGraph;
    private AnimationMixerPlayable mixerPlayable;
    protected override void OnEnter()
    {


        playableGraph = PlayableGraph.Create();

        var playableOutput = AnimationPlayableOutput.Create(playableGraph, "Animation", ActorComponent);

        mixerPlayable = AnimationMixerPlayable.Create(playableGraph, 2);
        // 将剪辑包裹在可播放项中

        playableClip1 = AnimationClipPlayable.Create(playableGraph, animationClip1);
        
        playableClip2 = AnimationClipPlayable.Create(playableGraph, animationClip2);

        // 将可播放项连接到输出

        playableGraph.Connect(playableClip1, 0, mixerPlayable, 0);

        playableGraph.Connect(playableClip2, 0, mixerPlayable, 1);


        playableOutput.SetSourcePlayable(mixerPlayable);

        Debug.Log(10);
       

    }

    private bool once = true;
    private float mixerTime;
    protected override void OnUpdate(float time)
    {
        weight = GetClipWeight(time);
        playableClip1.SetTime(time);
        weight = Mathf.Clamp01(weight);

        mixerPlayable.SetInputWeight(0, 1.0f - weight);

        mixerPlayable.SetInputWeight(1, weight);

        playableGraph.Evaluate(time);

        if (weight == 1)
        {
            if (once)
            {
                once = false;
                mixerTime = time;
            }
            var playable = mixerPlayable.GetInput(1);
            playable.SetTime(time - mixerTime);
        }
    }



    protected override void OnExit()
    {
        base.OnExit();
    }

    public override void Refresh()
    {
    }

    //Todo OnGui 红色表示动画长度

    public override string info
    {
        get { return animationClip1 != null ? animationClip1.name : base.info; }
    }

    [HideInInspector]
    private float _blendIn = 0f;

    [SerializeField]
    [HideInInspector]
    private float _blendOut = 0f;

    public override float length
    {
        get { return _length; }
        set { _length = value; }
    }

    public override float blendIn
    {
        get { return _blendIn; }
        set { _blendIn = value; }
    }

    public override float blendOut
    {
        get { return _blendOut; }
        set { _blendOut = value; }
    }

    public override bool canCrossBlend
    {
        get { return true; }
    }

}