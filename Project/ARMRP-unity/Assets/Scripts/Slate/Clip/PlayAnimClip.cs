using Assets.Scripts.Slate.Base;
using Sirenix.OdinInspector;
using Slate;
using System.Linq;
using UnityEngine;

[Name("���Ŷ���")]
[Attachable(typeof(AnimTrack))]
public class PlayAnimCutsceneClip : CutsceneClip<Animator>
{
    [LabelText("������")]
    [ValueDropdown("ClipsName")]
    [SerializeField]
    public string _animName;

    [LabelText("�����ٶ�")]
    [SerializeField]
    public float _playSpeed = 1;

    [LabelText("ѭ������")]
    [SerializeField]
    public bool _loop = false;

    [HideInInspector]
    [SerializeField] private float _length = 1 / 30f;

    private AnimationClip CurClip;

    /// <summary>
    /// Ĭ�ϳ���Ϊ����������ʱ��
    /// </summary>
    public override float length
    {
        get { return _length; }
        set { _length = value; }
    }

    private ValueDropdownList<string> ClipsName = new ValueDropdownList<string>();

    protected override void OnCreate()
    {
        Debug.Log("��ȡ��ǰActor������Clip");
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
                Debug.LogError("û�ж�Ӧ�Ķ������Բ���");
            }
            CurClip = playClips.First().clip;
            ActorComponent.Play(_animName);
        }
    }

    protected override void OnUpdate(float time)
    {
        //�༭ģʽԤ������

        //�õ���ǰ�Ķ�������
        var curClipLength = CurClip.length;
        float normalizedBefore = time * _playSpeed;
        if (!Application.isPlaying)
        {
            if (_loop && time > curClipLength)
            {
                //Ҫ��ת���Ķ���ʱ�� ������Update Time ȡ�� ����Ҫ��һ��ʱ��
                normalizedBefore = time * _playSpeed % curClipLength;
            }
            //normalzedTime,0-1 ��ʾ��ʼ�� ���Ž�����
            ActorComponent.Play(_animName, 0, normalizedBefore / curClipLength);
            ActorComponent.Update(0);
        }
        else
        {
            //����ģʽֱ�Ӳ���
            ActorComponent.Play(_animName);
        }
    }

    public override void Refresh()
    {
       
    }
}