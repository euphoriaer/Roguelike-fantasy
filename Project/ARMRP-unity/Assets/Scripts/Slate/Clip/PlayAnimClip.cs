using Assets.Scripts.Slate.Base;
using Sirenix.OdinInspector;
using Slate;
using System.Linq;
using UnityEngine;

[Name("播放动画")]
[Attachable(typeof(AnimTrack))]
public class PlayAnimClip : ClipBase<Animator>
{
    [LabelText("动作名")]
    [SerializeField]
    public string _animName = null;

    [LabelText("播放速度")]
    [SerializeField]
    public float _playSpeed = 1;

    [LabelText("循环播放")]
    [SerializeField]
    public bool _loop = false;

    [HideInInspector]
    [SerializeField] private float _length = 1 / 30f;

    public override float length
    {
        get { return _length; }
        set { _length = value; }
    }

    protected override void OnEnter()
    {
        if (!Application.isPlaying)
        {
            Debug.Log("编辑状态");
            ActorComponent.speed = _playSpeed;
            var playClip = ActorComponent.GetCurrentAnimatorClipInfo(0).Where(p => p.clip.name == _animName);
            // playClip?.First().clip.isLooping = true;
            ActorComponent.Play(_animName);
        }
    }

    protected override void OnUpdate(float time)
    {
        if (!Application.isPlaying)
        {
            Debug.Log("编辑状态");
            ActorComponent.Play(_animName);
        }
    }

    public float subClipOffset { get; set; }
    public float subClipSpeed { get; }
    public float subClipLength { get; }
}