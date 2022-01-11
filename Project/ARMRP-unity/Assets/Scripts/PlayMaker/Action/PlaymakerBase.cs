using HutongGames.PlayMaker;

public abstract class PlayMakerBase : FsmStateAction, INextValue, ILastValue
{
    public virtual void NextState(params object[] objects)
    {
    }

    public virtual void LastState(FsmState fsmState, params object[] objects)
    {
    }
}

internal interface INextValue
{
    public void NextState(params object[] objects);
}

internal interface ILastValue
{
    public void LastState(FsmState fsmState, params object[] objects);
}