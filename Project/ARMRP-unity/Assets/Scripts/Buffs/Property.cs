using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Battle
{
    public partial class BuffSystem
    {
        //属性加强Buff
        //参数1，Property Name
        //参数2，数值增加
        //参数3，百分比增加

        [BuffTypeAttr(TypeName = "Property_Add")]
        public void Property_Add(GameObject source, Buff buff)
        {
            Debug.Log("属性增加效果");
            
        }

        [BuffTypeAttr(TypeName = "Property_Remove")]
        public void Property_Remove(GameObject source, Buff buff)
        {
            Debug.Log("属性增加效果移除");
        }
    }
}