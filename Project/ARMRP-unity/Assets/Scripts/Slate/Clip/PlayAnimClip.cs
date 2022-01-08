using Sirenix.OdinInspector;
using Slate;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Name("���Ŷ���")]
[Attachable(typeof(AnimTrack))]
public class PlayAnimClip : CutsceneClip<Animator>
{
    [LabelText("������")]
    [ValueDropdown(nameof(GetString))]
    [SerializeField]
    [OnValueChanged("Refresh")]
    public string AnimName;

    [LabelText("�����ٶ�")]
    [SerializeField]
    public float PlaySpeed = 1;

    [LabelText("ѭ������")]
    [SerializeField]
    public bool Loop = false;

    private float _usedBlendAnimTime;

    [HideInInspector]
    [SerializeField] private float _length = 1 / 30f;

    private AnimationClip CurClip;

    [HideInInspector]
    public bool IsCrossing = false;

    /// <summary>
    /// Ĭ�ϳ���Ϊ����������ʱ��
    /// </summary>
    public override float length
    {
        get { return _length; }//��Ĭ�ϳ��ȱ�Ϊ��ǰ��������
        set { _length = value; }
    }

    private List<string> GetString()
    {
        List<string> ClipsNames = new List<string>();
        ClipsNames.Clear();
        var Animclips = ActorComponent.runtimeAnimatorController.animationClips;
        //todo ���˫�� �������������һ��Ĭ�Ͻ�ɫ
        foreach (var animatorClipInfo in Animclips)
        {
            ClipsNames.Add(animatorClipInfo.name);
        }

        return ClipsNames.ToList();
    }

    protected override void OnCreate()
    {
        Refresh();
    }

    protected override void OnEnter()
    {
        ActorComponent.speed = PlaySpeed;
        var playClips = ActorComponent.runtimeAnimatorController.animationClips.Where(p => p.name == AnimName);
        if (playClips.ToList().Count <= 0)
        {
            Debug.LogError("û�ж�Ӧ�Ķ������Բ���");
        }
        CurClip = playClips.First();
    }

    protected override void OnUpdate(float time)
    {
        //todo ����time����ʵʱ��Ļ���

        //�༭ģʽԤ������
        if (IsCrossing)//�����ں��У�����������
        {
            Debug.Log("���ڶ����ں�");
            return;
        }

        var curClipLength = CurClip.length;
        float normalizedBefore = time * PlaySpeed;
        if (Loop && time > curClipLength)
        {
            //Ҫ��ת���Ķ���ʱ�� ������Update Time ȡ�� ����Ҫ��һ��ʱ��
            normalizedBefore = time * PlaySpeed % curClipLength;
        }
        //normalzedTime,0-1 ��ʾ��ʼ�� ���Ž�����
        ActorComponent.Play(AnimName, 0, normalizedBefore / curClipLength);
        ActorComponent.Update(0);
    }

    protected override void OnExit()
    {

        base.OnExit();
    }

    public override void Refresh()
    {   //����Length Ϊ��Ӧ_animName�ĳ��� �벥���ٶȳɱ���
        var m = AnimName;
        length = ActorComponent.runtimeAnimatorController.animationClips.Where(p => p.name == AnimName).First().length;
        length = length / PlaySpeed;
    }

    //Todo OnGui ��ɫ��ʾ��������
}