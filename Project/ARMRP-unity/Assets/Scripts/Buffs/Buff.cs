using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        /// <summary>
        /// 上一次间隔效果触发的时间
        /// </summary>
        public float LastCallTime = 0;
        public enum BuffState
        {
            New,
            Runing,
            Remove,
        }

        public Buff()
        {

        }

    }
}
