using Battle;
using Sirenix.OdinInspector;
using Slate;
using UnityEngine;

[Name("技能阶段响应")]
[Attachable(typeof(InputTrack))]
public class InputClip : CutsceneClip<Animator>, IDirectable
{
    [LabelText("按键")]
    public KeyCode KeyCode;
    [LabelText("下一阶段技能Cutscene")]
    public Cutscene AttackCutscene;

    [HideInInspector]
    [SerializeField] private float _length = 1f;

    public override float length
    {
        get { return _length; }//将默认长度变为当前动画长度
        set { _length = value; }
    }

    private bool isLoop;
    private bool isPressed = false;
    protected override void OnUpdate(float time)
    {
        //检测是否按下对应按键，按下则进入下一攻击阶段
        if (Input.GetKeyDown(KeyCode)&&!isPressed)
        {
           Debug.Log("按下了按键");
           actor.transform.GetComponent<PropertySystem>().isFinishAttack = false;
           var cutscene= CutsceneHelper.InstateAction(out isLoop, AttackCutscene, actor);
           Debug.Log("当前进攻状态 Action Name" + actor.transform.GetComponent<PlayMakerFSM>().Fsm.ActiveState.ActiveAction.Name);
           cutscene.updateMode = Cutscene.UpdateMode.Normal;
           cutscene.Play();
           isPressed=true;
        }

    }


    public override void Refresh()
    {
       
    }
}

