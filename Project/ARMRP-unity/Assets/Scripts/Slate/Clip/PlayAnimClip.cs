using Assets.Scripts.Slate.Base;
using Sirenix.OdinInspector;
using Slate;
using System.Linq;
using UnityEngine;

[Name("���Ŷ���")]
[Attachable(typeof(AnimTrack))]
public class PlayAnimClip : ClipBase<Animator>
{
    [LabelText("������")]
    [SerializeField]
    public string _animName = null;

    [LabelText("�����ٶ�")]
    [SerializeField]
    public float _playSpeed = 1;

    [LabelText("ѭ������")]
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
            Debug.Log("�༭״̬");
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
            Debug.Log("�༭״̬");
            ActorComponent.Play(_animName);
        }
    }

    public float subClipOffset { get; set; }
    public float subClipSpeed { get; }
    public float subClipLength { get; }
}