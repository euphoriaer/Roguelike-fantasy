using UnityEngine;

namespace Battle
{
    [UnityEngine.DisallowMultipleComponent]
    [UnityEngine.DefaultExecutionOrder(SystemOrder.BehitSystem)]
    [UnityEngine.AddComponentMenu("System/BehitSystem")]
    internal class BehitSystem : SystemMonoBehaviour
    {
        public static int EnemyLayer = 8;

        public static int PlayerLayer = 6;

        public void Injured(GameObject source, int hurt)
        {
            if (source.layer == PlayerLayer && this.gameObject.layer == EnemyLayer)
            {
                //玩家打中了敌人  具体是普攻还是技能/特殊 取决于外部计算，此处只计算相对伤害
                this.gameObject.GetComponent<PropertySystem>().CurBlood -= hurt;
            }

            if (source.layer == EnemyLayer && this.gameObject.layer == PlayerLayer)
            {
                //敌人打中了玩家  具体是普攻还是技能/特殊 取决于外部计算，此处只计算相对伤害
                this.gameObject.GetComponent<PropertySystem>().CurBlood -= hurt;
            }
        }

        // Start is called before the first frame update
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
        }
    }
}