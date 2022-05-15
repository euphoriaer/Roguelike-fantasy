using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Battle
{
    public static class BuffHelper
    {
        public static void AddBuff(GameObject target,int buffId)
        {
            if (buffId==0)
            {
                //不存在Buff 0
                return;
            }
            Buff buff = new Buff(buffId);
            target.GetComponent<BuffSystem>()?.buffs.Add(buff);
        }

    }
}
