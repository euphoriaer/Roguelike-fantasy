using HutongGames.PlayMaker;
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
        Debug.Log("播放Cuescene");
        CutsceneHelper.Play(Fsm.GameObject, CutsceneName);
        Finish();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}