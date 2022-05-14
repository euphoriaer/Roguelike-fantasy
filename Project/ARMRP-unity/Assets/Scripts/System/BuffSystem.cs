using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Battle
{
    [UnityEngine.DisallowMultipleComponent]
    [UnityEngine.DefaultExecutionOrder(SystemOrder.BuffSystem)]
    [UnityEngine.AddComponentMenu("System/BuffSystem")]
    public class BuffSystem : SystemMonoBehaviour
    {
        private DistributeUtil<UnityAction<GameObject, Buff>, BuffTypeAttr, BuffSystem>
            buffDistributeUtil;

        private void Start()
        {
            buffDistributeUtil = new DistributeUtil<UnityAction<GameObject, Buff>, BuffTypeAttr, BuffSystem>(this); ;
        }

        public List<Buff> buffs;

        private void Update()
        {
            foreach (var buff in buffs)
            {
                #region Add

                //判断是否生效中
                if (buff.State == Buff.BuffState.New)
                {
                    //error 获取效果名,需要数据表

                    //分发效果
                    var buffmethod = buffDistributeUtil.GetMethod("Bleed_Add");
                    buffmethod.Invoke(buff.source, buff);
                }

                #endregion Add

                #region Runing

                //调用间隔效果
                buff.IntervalCallAction(buff);

                buff.BuffTime -= this.GetComponent<PropertySystem>().DeltaTime;

                #endregion Runing

                if (buff.BuffTime <= 0)
                {
                    buff.State = Buff.BuffState.Remove;
                }

                if (buff.State == Buff.BuffState.Remove)
                {
                    //error 获取效果名,需要数据表
                    var buffmethod = buffDistributeUtil.GetMethod("Bleed_Remove");
                    buffmethod.Invoke(buff.source, buff);
                    //分发效果

                    buffs.Remove(buff);
                }
            }
        }

        [BuffTypeAttr(TypeName = "Bleed_Add")]
        public void Bleed_Add(GameObject Source, Buff buff)
        {
        }

        [BuffTypeAttr(TypeName = "Bleed_Remove")]
        public void Bleed_Remove(GameObject Source, Buff buff)
        {
        }
    }
}