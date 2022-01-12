using HutongGames.PlayMaker;

public class FsmStateActionBase : FsmStateAction
{
    public sealed override void OnExit()
    {   //需要向下一个状态state 传递当前cutscene动画
        this.SetLastFsmState();
        Exit();
    }

    public virtual void Exit()
    {

    }
}