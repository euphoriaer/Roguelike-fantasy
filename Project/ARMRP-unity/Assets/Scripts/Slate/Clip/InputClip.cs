using Battle;
using Sirenix.OdinInspector;
using Slate;
using UnityEngine;

[Name("技能阶段响应")]
[Attachable(typeof(InputTrack))]
public class InputClip : CutsceneClip<Animator>, IDirectable
{

    [LabelText("下一阶段技能Cutscene")]
    public Cutscene AttackCutscene;

    [HideInInspector]
    [SerializeField] private float _length = 1f;

    public override float length
    {
        get { return _length; }//将默认长度变为当前动画长度
        set { _length = value; }
    }


    protected override void OnUpdate(float time)
    {

    }


    public override void Refresh()
    {
       
    }

    protected override void OnEnter()
    {
        //将按键事件加入
        actor.GetComponent<InputSystem>().AttackSignalEvent += KeyEvent;
    }

    private void KeyEvent()
    {
        CutsceneHelper.InstateAction(AttackCutscene, actor);
    }

    protected override void OnExit()
    {
        actor.GetComponent<InputSystem>().AttackSignalEvent -= KeyEvent;
    }
}

