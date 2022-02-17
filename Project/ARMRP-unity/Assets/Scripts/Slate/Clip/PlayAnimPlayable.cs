using Assets.Scripts;
using Sirenix.OdinInspector;
using Slate;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

[Name("播放动画Playable")]
[Attachable(typeof(AnimTrack))]
public class PlayAnimPlayable : CutsceneClip<Animator>
{
    public AnimationClip animationClip;

    [LabelText("播放速度")]
    [SerializeField]
    public float PlaySpeed = 1;

    [LabelText("循环播放")]
    [SerializeField]
    public bool Loop = false;

    [HideInInspector]
    [SerializeField] private float _length = 1 / 30f;

    private AnimationClipPlayable playableClip;
    private PlayableGraph playableGraph;
    private Vector3 rolePosition = Vector3.zero;

    protected override void OnCreate()
    {
        Refresh();
    }

    protected override bool OnInitialize()
    {
        //EventManager.OnNewMessage += GetMessage;
        return base.OnInitialize();
    }

    private void GetMessage(object sender, EventMessage e)
    {
        rolePosition = e.RolePosition;
    }

    protected override void OnEnter()
    {
        //Error  适合entity 获取位置
        //actor.transform.position = FsmStateExtensions.RolePosition;
        //actor.transform.position = actor.transform.GetComponent<Property>().m_Position;
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
        playableGraph.Evaluate(time);
    }

    protected override void OnExit()
    {
        var pos = actor.transform.position;
        base.OnExit();
    }

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