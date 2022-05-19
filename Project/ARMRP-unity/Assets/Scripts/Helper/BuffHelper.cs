using UnityEngine;

namespace Battle
{
    public static class BuffHelper
    {
        public static void AddBuff(GameObject target, int buffId)
        {
            if (buffId == 0)
            {
                //不存在Buff 0
                return;
            }
            Buff buff = new Buff(buffId);
            var findBuff = target.GetComponent<BuffSystem>()?.FindBuff(buff);
            //检测
            switch (buff.BuffType)
            {
                case 1:
                    target.GetComponent<BuffSystem>()?.buffs.Add(buff);
                    break;

                case 2:
                    //唯一不重复

                    if (findBuff != null)
                    {
                        target.GetComponent<BuffSystem>()?.buffs.Add(buff);
                    }

                    break;

                case 3:
                    //重复刷新时间，否则添加
                    //var findBuff = target.GetComponent<BuffSystem>()?.FindBuff(buff);
                    if (findBuff == null)
                    {
                        target.GetComponent<BuffSystem>()?.buffs.Add(buff);
                    }
                    else
                    {
                        findBuff.BuffTime = buff.BuffTime;
                    }
                    break;

                case 4:
                    //永久类型，存在不添加，反之添加
                    if (findBuff == null)
                    {
                        target.GetComponent<BuffSystem>()?.buffs.Add(buff);
                    }
                    break;

                default:
                    break;
            }
        }
    }
}