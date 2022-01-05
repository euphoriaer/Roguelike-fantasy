using System.Collections;
using Sirenix.OdinInspector;
using Slate;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[Name("播放动画")]
[Attachable(typeof(AnimTrack))]
public class PlayAnimClip : CutsceneClip<Animator>
{
    [LabelText("动作名")]
    [ValueDropdown(nameof(GetString))]
    [SerializeField]
    [OnValueChanged("Refresh")]
    public string AnimName;

    [LabelText("播放速度")]
    [SerializeField]
    public float PlaySpeed = 1;

    [LabelText("循环播放")]
    [SerializeField]
    public bool Loop = false;

    private float _usedBlendAnimTime;

    [HideInInspector]
    [SerializeField] private float _length = 1 / 30f;

    private AnimationClip CurClip;

    /// <summary>
    /// 默认长度为整个动画的时长
    /// </summary>
    public override float length
    {
        get { return _length; }//将默认长度变为当前动画长度
        set { _length = value; }
    }

    private List<string> GetString()
    {
        List<string> ClipsNames = new List<string>();
        ClipsNames.Clear();
        var Animclips = ActorComponent.runtimeAnimatorController.animationClips;
        //todo 如果双击 无人物，可以增加一个默认角色
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

        //StartCoroutine(Wait());

        ActorComponent.speed = PlaySpeed;
        var playClips = ActorComponent.runtimeAnimatorController.animationClips.Where(p => p.name == AnimName);
        if (playClips.ToList().Count <= 0)
        {
            Debug.LogError("没有对应的动画可以播放");
        }
        CurClip = playClips.First();
        //进入时动画动作融合,进行一个等待
    }

    IEnumerator Wait()
    {
        Debug.Log("融合等待");
        yield return new WaitForSeconds(3);
        Debug.Log("融合等待结束");
    }

    protected override void OnUpdate(float time)
    {
        //if (!Application.isPlaying)
        //{//编辑模式预览动画
        //得到当前的动画长度

        var curClipLength = CurClip.length;
        float normalizedBefore = time * PlaySpeed;
        if (Loop && time > curClipLength)
        {
            //要跳转到的动画时长 ，根据Update Time 取余 ，需要归一化时间
            normalizedBefore = time * PlaySpeed % curClipLength;
        }

        //normalzedTime,0-1 表示开始与 播放结束，
        ActorComponent.Play(AnimName, 0, normalizedBefore / curClipLength);
        ActorComponent.Update(0);

        //}
    }

    public override void Refresh()
    {   //设置Length 为对应_animName的长度 与播放速度成比例
        var m = AnimName;
        length = ActorComponent.runtimeAnimatorController.animationClips.Where(p => p.name == AnimName).First().length;
        length = length / PlaySpeed;
    }

    //Todo OnGui 红色表示动画长度
}