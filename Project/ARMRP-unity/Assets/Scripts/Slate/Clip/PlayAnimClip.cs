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

    protected override void OnCreate()
    {
        Debug.Log("��ȡ��ǰActor������Clip");
      
    }

    protected override void OnEnter()
    {
        if (!Application.isPlaying)
        {
            ActorComponent.speed = _playSpeed;
            var playClips = ActorComponent.GetCurrentAnimatorClipInfo(0).Where(p => p.clip.name == _animName);
            if (playClips.ToList().Count<=0)
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
        float normalizedBefore = time;//0-1 ��ʾ��ʼ�� ���Ž�����
        if (!Application.isPlaying)
        {
            if (_loop && time > curClipLength)
            {
                //Ҫ��ת���Ķ���ʱ�� ������Update Time ȡ�� ����Ҫ��һ��ʱ��
                normalizedBefore = time % curClipLength;
            }

            ActorComponent.Play(_animName, 0, normalizedBefore/curClipLength);
            ActorComponent.Update(0);
        }
        else
        {
            //����ģʽֱ�Ӳ���
            ActorComponent.Play(_animName);
        }
    }
}