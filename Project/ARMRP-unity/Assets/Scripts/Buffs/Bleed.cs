using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Battle
{
    public partial class BuffSystem
    {
        //流血
        //参数1，流血多久间隔触发一次
        //参数2，每次触发伤害

        [BuffTypeAttr(TypeName = "Bleed_Add")]
        public void Bleed_Add(GameObject Source, Buff buff)
        {
            Debug.Log("流血效果");
        }

        [BuffTypeAttr(TypeName = "Bleed_Remove")]
        public void Bleed_Remove(GameObject Source, Buff buff)
        {
            Debug.Log("流血效果移除");
        }
    }
}
