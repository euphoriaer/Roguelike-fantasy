using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Battle
{
    [UnityEngine.DisallowMultipleComponent]
    [UnityEngine.DefaultExecutionOrder(SystemOrder.BuffSystem)]
    [UnityEngine.AddComponentMenu("System/BuffSystem")]
    public partial class BuffSystem : SystemMonoBehaviour
    {
        /// <summary>
        /// 可以采用类类型对象，替代buff 效果分发
        /// 类类型对象会复杂些，但是结构更加清晰
        /// </summary>
        private DistributeUtil<UnityAction<GameObject, Buff>, BuffTypeAttr, BuffSystem>
            buffDistributeUtil;

        private void Start()
        {
            buffDistributeUtil = new DistributeUtil<UnityAction<GameObject, Buff>, BuffTypeAttr, BuffSystem>(this); ;
        }
        [ShowInInspector]
        public List<Buff> buffs = new List<Buff>();

        private void Update()
        {
            for (int i = 0; i < buffs.Count; i++)
            {
                var buff = buffs[i];

                #region Add

                //判断是否生效中
                if (buff.State == Buff.BuffState.New)
                {
                    buff.State = Buff.BuffState.Runing;
                    // 获取效果名,需要数据表

                    //分发效果
                    var buffmethod = buffDistributeUtil.GetMethod(buff.BuffEffectData[0] + "_Add");
                    buffmethod.Invoke(buff.source, buff);
                }

                #endregion Add

                #region Runing

                //调用间隔效果
                if (buff.IntervalCallAction != null)
                {
                    buff.IntervalCallAction(buff);
                }

                buff.BuffTime -= this.GetComponent<PropertySystem>().FixedDeltaTime;
               

                #endregion Runing

                if (buff.BuffTime <= 0 && buff.BuffType != 4)
                {
                    buff.State = Buff.BuffState.Remove;
                }

                if (buff.State == Buff.BuffState.Remove)
                {
                    //获取效果名,数据表
                    var buffmethod = buffDistributeUtil.GetMethod(buff.BuffEffectData[0] + "_Remove");
                    buffmethod.Invoke(buff.source, buff);
                    //分发效果

                    buffs.Remove(buff);
                }
            }
        }

        public Buff FindBuff(Buff buff)
        {
            foreach (var item in buffs)
            {
                if (item.BuffId == buff.BuffId)
                {
                    return item;
                }
            }

            return null;
        }

        public Buff FindBuff(int buffID)
        {
            foreach (var item in buffs)
            {
                if (item.BuffId == buffID)
                {
                    return item;
                }
            }

            return null;
        }
    }
}