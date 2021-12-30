using Assets.Scripts.Slate.Base;
using Sirenix.OdinInspector;
using Slate;
using System.Linq;
using UnityEngine;

[Name("播放动画")]
[Attachable(typeof(AnimTrack))]
public class PlayAnimCutsceneClip : CutsceneClip<Animator>
{
    [LabelText("动作名")]
    [ValueDropdown("ClipsName")]
    [SerializeField]
    public string _animName;

    [LabelText("播放速度")]
    [SerializeField]
    public float _playSpeed = 1;

    [LabelText("循环播放")]
    [SerializeField]
    public bool _loop = false;

    [HideInInspector]
    [SerializeField] private float _length = 1 / 30f;

    private AnimationClip CurClip;

    /// <summary>
    /// 默认长度为整个动画的时长
    /// </summary>
    public override float length
    {
        get { return _length; }
        set { _length = value; }
    }

    private ValueDropdownList<string> ClipsName = new ValueDropdownList<string>();

    protected override void OnCreate()
    {
        Debug.Log("获取当前Actor的所有Clip");
        var Animclips = ActorComponent.GetCurrentAnimatorClipInfo(0);

        foreach (var animatorClipInfo in Animclips)
        {
            ClipsName.Add(animatorClipInfo.clip.name);
        }
    }

    protected override void OnEnter()
    {
        if (!Application.isPlaying)
        {
            ActorComponent.speed = _playSpeed;
            var playClips = ActorComponent.GetCurrentAnimatorClipInfo(0).Where(p => p.clip.name == _animName);
            if (playClips.ToList().Count <= 0)
            {
                Debug.LogError("没有对应的动画可以播放");
            }
            CurClip = playClips.First().clip;
            ActorComponent.Play(_animName);
        }
    }

    protected override void OnUpdate(float time)
    {
        //编辑模式预览动画

        //得到当前的动画长度
        var curClipLength = CurClip.length;
        float normalizedBefore = time * _playSpeed;
        if (!Application.isPlaying)
        {
            if (_loop && time > curClipLength)
            {
                //要跳转到的动画时长 ，根据Update Time 取余 ，需要归一化时间
                normalizedBefore = time * _playSpeed % curClipLength;
            }
            //normalzedTime,0-1 表示开始与 播放结束，
            ActorComponent.Play(_animName, 0, normalizedBefore / curClipLength);
            ActorComponent.Update(0);
        }
        else
        {
            //运行模式直接播放
            ActorComponent.Play(_animName);
        }
    }

    public override void Refresh()
    {
       
    }
}