using LitJson;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Battle
{
    public class Buff
    {
        public float BuffTime;
        public int BuffId;
        public GameObject source;
        public string BuffName;
        public BuffState State;

        /// <summary>
        /// 间隔触发，每帧调用，需在内部处理间隔时间
        /// </summary>
        public UnityAction<Buff> IntervalCallAction;

        public JsonData BuffJson;

        /// <summary>
        /// 1.带持续时间,表示独立Buff,重复添加相互独立计算
        /// 2.表示唯一Buff,不会重复添加，直到旧的结束，新的才能添加上
        /// 3.表示唯一Buff，重复添加是刷新时间
        /// 4.永久类型Buff,不重复，不随时间结束停止
        /// </summary>
        public int BuffType;

    
        /// <summary>
        /// buff效果数据，0是效果类型
        /// 0用来分发
        /// </summary>
        public List<string> BuffEffectData=new List<string>();
        /// <summary>
        /// 上一次间隔效果触发的时间
        /// </summary>
        public float LastCallTime = 0;

        public Buff(int buffID)
        {
            BuffId = buffID;
            //读取Json, 获取Buff
            BuffJson =BuffConfig.GetBuff(buffID);
            BuffTime = int.Parse(BuffJson[BuffConfig.TIME].ToString());
            State = BuffState.New;
            BuffName = (BuffJson[BuffConfig.NAME].ToString());
            BuffType = int.Parse(BuffJson[BuffConfig.TYPE].ToString());
            BuffEffectData.AddRange((BuffJson[BuffConfig.EFFECT]).ToString().Split(","));
        }

        public enum BuffState
        {
            New,
            Runing,
            Remove,
        }

    }
}