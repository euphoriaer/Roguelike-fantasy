using HutongGames.PlayMaker;
using System.Collections.Generic;

public static class FsmStateExtensions
{
    private static Dictionary<string, ActionObj> _stateDic = new Dictionary<string, ActionObj>();
    private static FsmState lastFsmState;

    /// <summary>
    /// 以当前FsmState和当前Action为名，传递数据
    /// </summary>
    /// <param name="fsmState"></param>
    /// <param name="values"></param>
    public static void SetTransValues(this FsmStateAction fsmStateAction, params object[] values)
    {
        string fsmName = fsmStateAction.State.Name;
        string actionName = fsmStateAction.Name;

        bool isOk = _stateDic.TryGetValue(fsmName, out var value);
        if (!isOk)
        {
            _stateDic.Add(fsmName, new ActionObj(actionName, values));
        }
        else
        {
            value.valuesDic[actionName].AddRange(values);
        }

        return;
    }

    /// <summary>
    /// 从状态获取数据
    /// </summary>
    /// <param name="fsmStateAction"></param>
    /// <param name="fsmStateName"></param>
    /// <param name="actionName"></param>
    /// <returns></returns>
    public static object[] GetTransValues(this FsmStateAction fsmStateAction, string fsmStateName, string actionName)
    {
        var values = _stateDic[fsmStateName]?.valuesDic[actionName].ToArray();
        if (values == null || values.Length <= 0)
        {
            return null;
        }
        _stateDic.Remove(fsmStateName);
        return values;
    }

    /// <summary>
    /// 从上一个状态获取数据，返回状态数据合集
    /// </summary>
    /// <param name="fsmStateAction"></param>
    /// <param name="fsmStateName"></param>
    /// <param name="actionName"></param>
    /// <returns></returns>
    public static ActionObj GetTransValues(this FsmStateAction fsmStateAction)
    {
        if (lastFsmState == null)
        {
            return null;
        }

        _stateDic.TryGetValue(lastFsmState.Name, out var actionObj);
        return actionObj;
    }

    public static FsmState GetLastFsmState(this FsmStateAction fsmStateAction)
    {
        return lastFsmState;
    }

    public static void SetLastFsmState(this FsmStateAction fsmStateAction)
    {
        lastFsmState = fsmStateAction.State;
    }

    public class ActionObj
    {  
        public Dictionary<string, List<object>> valuesDic = new Dictionary<string, List<object>>();

        public ActionObj(string actionName, params object[] values)
        {
            valuesDic.Add(actionName, new List<object>(values));
        }
    }
}