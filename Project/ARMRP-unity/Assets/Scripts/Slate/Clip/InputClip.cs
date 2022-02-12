using Sirenix.OdinInspector;
using Slate;
using UnityEngine;

[Name("输入检测")]
[Attachable(typeof(InputTrack))]
public class InputClip : CutsceneClip<Animator>
{
    public override void Refresh()
    {
       
    }

    [HideInInspector]
    [SerializeField] private float _length = 1 / 30f;

    public override float length
    {
        get { return _length; }
        set { _length = value; }
    }
    protected override void OnEnter()
    {
        base.OnEnter();
    }

    protected override void OnExit()
    {
        base.OnExit();
    }

    protected override void OnUpdate(float time)
    {
        base.OnUpdate(time);
        //error 检测输入  输入与移动解耦,移动在Playmaker 的Action中


    }
}