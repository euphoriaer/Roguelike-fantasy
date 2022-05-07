using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    
    [UnityEngine.DisallowMultipleComponent]
    [UnityEngine.DefaultExecutionOrder(SystemOrder.BehitSystem)]
    [UnityEngine.AddComponentMenu("System/BehitSystem")]
    public class BehitSystem : MonoBehaviour
    {
        public static int EnemyLayer = 8;

        public static int PlayerLayer = 6;
        public void Injured(GameObject source, int hurt)
        {
            if (source.layer == PlayerLayer && this.gameObject.layer ==EnemyLayer)
            {
                //玩家打中了敌人  具体是普攻还是技能/特殊 取决于外部计算，此处只计算相对伤害
                this.gameObject.GetComponent<PropertySystem>().Blood -= hurt;
            }

            if (source.layer ==EnemyLayer && this.gameObject.layer == PlayerLayer)
            {
                //敌人打中了玩家  具体是普攻还是技能/特殊 取决于外部计算，此处只计算相对伤害
                this.gameObject.GetComponent<PropertySystem>().Blood -= hurt;
            }
        }

        private void OnValidate()
        {
            //移动脚本到末尾
            while (UnityEditorInternal.ComponentUtility.MoveComponentDown(this))
            {

            }

        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}