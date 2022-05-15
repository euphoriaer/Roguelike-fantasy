using LitJson;
using UnityEngine;
using UnityEngine.Events;

namespace Battle
{
    public class Buff
    {
        public float BuffTime;
        public GameObject source;
        public GameObject BuffID;
        public string BuffName;
        public BuffState State;

        /// <summary>
        /// 间隔触发，每帧调用，需在内部处理间隔时间
        /// </summary>
        public UnityAction<Buff> IntervalCallAction;

        public JsonData BuffJson;

        /// <summary>
        /// 1.带持续时间
        /// 2.永久增益
        /// </summary>
        public int BuffType;

        /// <summary>
        /// buff效果，分发方法用
        /// </summary>
        public string BuffEffect;

        /// <summary>
        /// 上一次间隔效果触发的时间
        /// </summary>
        public float LastCallTime = 0;

        public Buff(int buffID)
        {
            //读取Json, 获取Buff
            BuffJson=BuffConfig.GetBuff(buffID);
            BuffTime = ((int)BuffJson[BuffConfig.TIME]);
            State = BuffState.New;
            BuffName = ((string)BuffJson[BuffConfig.NAME]);
            BuffType = ((int)BuffJson[BuffConfig.TYPE]);
            BuffEffect = ((string)BuffJson[BuffConfig.EFFECT]).Split(",")[0];
        }

        public enum BuffState
        {
            New,
            Runing,
            Remove,
        }

    }
}