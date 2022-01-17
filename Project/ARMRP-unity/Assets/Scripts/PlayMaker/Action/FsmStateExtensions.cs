using HutongGames.PlayMaker;
using System.Collections.Generic;
using UnityEngine;

public static class FsmStateExtensions
{
    /// <summary>
    /// 以当前FsmState和当前Action为名，传递数据
    /// </summary>
    /// <param name="fsmState"></param>
    /// <param name="values"></param>
    public static void SetTransValues(this FsmStateAction fsmStateAction, params object[] values)
    {
        string fsmName = fsmStateAction.State.Name;
        string actionName = fsmStateAction.Name;

        var fsmListen = FsmListen.GetGameObjectFsmListen(fsmStateAction);

        var fsmCache = fsmListen.GetFsmCache(fsmStateAction.Fsm);
        var _stateDic = fsmCache.stateDic;

        bool isOk = _stateDic.TryGetValue(fsmStateAction, out var value);
        if (!isOk)
        {
            _stateDic.Add(fsmStateAction, new ActionObj(actionName, values));
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
        var fsmListen = FsmListen.GetGameObjectFsmListen(fsmStateAction);

        var fsmCache = fsmListen.GetFsmCache(fsmStateAction.Fsm);
        var _stateDic = fsmCache.stateDic;

        var values = _stateDic[fsmStateAction]?.valuesDic[actionName].ToArray();
        if (values == null || values.Length <= 0)
        {
            return null;
        }
        _stateDic.Remove(fsmStateAction);
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
        var fsmListen = FsmListen.GetGameObjectFsmListen(fsmStateAction);

        var fsmCache = fsmListen.GetFsmCache(fsmStateAction.Fsm);
        var _stateDic = fsmCache.stateDic;
        var lastFsmState = fsmCache.lastFsmState;

        if (lastFsmState == null)
        {
            return null;
        }

        _stateDic.TryGetValue(fsmStateAction, out var actionObj);
        return actionObj;
    }

    public static FsmState GetLastFsmState(this FsmStateAction fsmStateAction)
    {
        var fsmListen = FsmListen.GetGameObjectFsmListen(fsmStateAction);

        var fsmCache = fsmListen.GetFsmCache(fsmStateAction.Fsm);
        var lastFsmState = fsmCache.lastFsmState;

        if (lastFsmState == null)
        {
            return null;
        }
        return lastFsmState;
    }

    public static void SetLastFsmState(this FsmStateAction fsmStateAction)
    {
        var fsmListen = FsmListen.GetGameObjectFsmListen(fsmStateAction);

        var fsmCache = fsmListen.GetFsmCache(fsmStateAction.Fsm);
        fsmCache.lastFsmState = fsmStateAction.State;
    }

    public class ActionObj
    {
        public Dictionary<string, List<object>> valuesDic = new Dictionary<string, List<object>>();

        public ActionObj(string actionName, params object[] values)
        {
            valuesDic.Add(actionName, new List<object>(values));
        }
    }

    public class FsmListen : MonoBehaviour
    {
        public FsmCache GetFsmCache(Fsm fsm)
        {
            fsmCacheDic.TryGetValue(fsm, out var cache);
            return cache;
        }

        public Dictionary<Fsm, FsmCache> fsmCacheDic = new Dictionary<Fsm, FsmCache>();

        public class FsmCache
        {
            /// <summary>
            /// state name
            /// </summary>
            public Dictionary<FsmStateAction, ActionObj> stateDic = new Dictionary<FsmStateAction, ActionObj>();

            public FsmState lastFsmState;
        }

        public static FsmListen GetGameObjectFsmListen(FsmStateAction fsmStateAction)
        {
            var fsmListen = fsmStateAction.Fsm.GameObject.GetComponent<FsmListen>();
            if (fsmListen != null)
            {
                return fsmListen;
            }
            else
            {
                //没有对应的FsmListen，创建，一个Fsm监听Gameobject上的所有Fsm
                fsmListen = fsmStateAction.Fsm.GameObject.AddComponent<FsmListen>();
            }
            //没有对应的fsm，加入fsm且把当前状态，当前Action Name缓存
            fsmListen.fsmCacheDic.TryGetValue(fsmStateAction.Fsm, out var fsmCache);
            if (fsmCache == null)
            {
                var fsmCacheValue = new FsmCache();
                fsmCacheValue.stateDic.Add(fsmStateAction, null);
                fsmListen.fsmCacheDic.Add(fsmStateAction.Fsm, fsmCacheValue);
            }

            return fsmListen;
        }
    }
}