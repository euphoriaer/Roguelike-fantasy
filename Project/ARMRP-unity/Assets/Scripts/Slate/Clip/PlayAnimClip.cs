using Sirenix.OdinInspector;
using Slate;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Name("播放动画Animator")]
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
    [SerializeField] private float _length = 1f;

    private AnimationClip CurClip;

    [HideInInspector]
    public bool IsCrossing = false;

    #region  todo 动画内部支持动作融合

    [SerializeField]
    [HideInInspector]
    private float _blendIn = 0f;
    [SerializeField]
    [HideInInspector]
    private float _blendOut = 0f;

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

    #endregion
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
        if (actor == null)
        {
            return null;
        }
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
        ActorComponent.speed = PlaySpeed;
        var playClips = ActorComponent.runtimeAnimatorController.animationClips.Where(p => p.name == AnimName);
        if (playClips.ToList().Count <= 0)
        {
            Debug.LogError("没有对应的动画可以播放");
        }
        CurClip = playClips.First();
    }

    protected override void OnUpdate(float time)
    {
        //todo 测试time与真实时间的换算

        //编辑模式预览动画
        if (IsCrossing)//动作融合中，动画不播放
        {
            Debug.Log("处于动作融合");
            return;
        }

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
    }

    protected override void OnExit()
    {
        base.OnExit();
    }

    public override void Refresh()
    {   //设置Length 为对应_animName的长度 与播放速度成比例
        if (actor == null)
            return;
        length = ActorComponent.runtimeAnimatorController.animationClips.Where(p => p.name == AnimName).First().length;
        length = length / PlaySpeed;
    }

    //OnGui 红色表示动画长度


    ///----------------------------------------------------------------------------------------------
    ///---------------------------------------UNITY EDITOR-------------------------------------------
#if UNITY_EDITOR


    //动画长度提示
    protected override void OnClipGUI(Rect rect)
    {

        if (!Loop)
        {
            return;
        }
        //if (CurClip != null)  //默认颜色较暗淡，使用下方自定义颜色
        //{
        //    EditorTools.DrawLoopedLines(rect, CurClip.length / PlaySpeed,this.length,0);
        //}

        if (CurClip == null)
            return;
        //length 
        float cycleLength = CurClip.length / PlaySpeed; //每帧长度  

        cycleLength = Mathf.Abs(cycleLength);

        if (cycleLength != 0 && length != 0)
        {
            //UnityEditor.Handles.color = new Color(84, 255, 159, 0.2f);
            //Color rectangleColor = new Color(84, 255, 159, 0.2f);

            for (float curFrame = 0; (curFrame < _length); curFrame += cycleLength)//循环绘制
            {
                var posX = (curFrame / length) * rect.width;
                UnityEditor.Handles.DrawLine(new Vector2(posX, 0), new Vector2(posX, rect.height));
            }

            UnityEditor.Handles.color = Color.red;
        }
    }



#endif

}