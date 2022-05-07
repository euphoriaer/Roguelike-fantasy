using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class BehitSystem : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Injured(GameObject source, int hurt)
        {
            if (source.layer == PropertySystem.PlayerLayer && this.gameObject.layer == PropertySystem.EnemyLayer)
            {
                //玩家打中了敌人  具体是普攻还是技能/特殊 取决于外部计算，此处只计算相对伤害
                this.gameObject.GetComponent<PropertySystem>().Blood -= hurt;
            }

            if (source.layer == PropertySystem.EnemyLayer && this.gameObject.layer == PropertySystem.PlayerLayer)
            {
                //敌人打中了玩家  具体是普攻还是技能/特殊 取决于外部计算，此处只计算相对伤害
                this.gameObject.GetComponent<PropertySystem>().Blood -= hurt;
            }
        }
    }
}