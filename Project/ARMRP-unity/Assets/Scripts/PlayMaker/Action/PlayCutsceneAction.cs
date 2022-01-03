using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityEngine;

[ActionCategory("Cutscene")]
public class PlayCutsceneAction : FsmStateAction
{
    [UnityEngine.Tooltip("Cutscene名字")]
    public string CutsceneName;


    public override void Reset()
    {
        base.Reset();
    }

    public override void OnEnter()
    {
        CutsceneHelper.Play(Fsm.GameObject, CutsceneName);
    }

    public override void OnUpdate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            //error 检测是否切换状态，或者在Cutscene 加入输入轨道
            Fsm.SendEventToFsmOnGameObject(this.Owner,this.Fsm.Name,"IdleToRun");
            Finish();
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}