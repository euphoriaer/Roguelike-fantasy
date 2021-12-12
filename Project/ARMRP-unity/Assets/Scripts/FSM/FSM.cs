using System.Collections.Generic;

public class FSM
{
    /// <summary>
    /// 当前状态
    /// </summary>
    private IState _curState;

    /// <summary>
    /// 状态字典
    /// </summary>
    private Dictionary<State, IState> _dicState = new Dictionary<State, IState>();

    public enum State
    {
       idle,run,dogge
    }

    /// <summary>
    /// 初始状态
    /// </summary>
    /// <param name="state">状态名</param>
    /// <param name="firstState">状态</param>
    public FSM(State state, IState firstState)
    {
        firstState.Enter();
        _dicState[state] = firstState;
        _curState = firstState;
    }

    /// <summary>
    /// 添加/修改的状态
    /// </summary>
    /// <param name="state"></param>
    /// <param name=""></param>
    public void AddState(State state, IState addState)
    {
        //如果字典有key 修改，没有key 添加
        _dicState[state] = addState;
    }

    public void RemoveState(State state)
    {
        //如果字典有key 修改，没有key 添加
        _dicState.Remove(state);
    }

    public void StateUpdate()
    {
        _curState.Update();
    }

    /// <summary>
    /// 要改变到的状态
    /// </summary>
    /// <param name="state"></param>
    public void TransformState(State state)
    {
        _curState.Exit();
        _curState = _dicState[state];
        _curState.Enter();
    }
}