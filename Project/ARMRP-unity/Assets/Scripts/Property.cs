using UnityEngine;

namespace Battle
{
    public class Property : MonoBehaviour
    {
        public static int PlayerLayer = 6;
        public static int EnemyLayer = 8;

        public string Name;

        public int Blood;
        public int MaxBlood;

        public int Blue;
        public int MaxBlue;

        public int Attack;

        public Vector3 m_Position;

        public AnimationClip LastPlayClip;
        public float curPlayClipOffset;

        public void Injured(GameObject source, int hurt)
        {
            if (source.layer == PlayerLayer && this.gameObject.layer == EnemyLayer)
            {
                //玩家打中了敌人  具体是普攻还是技能/特殊 取决于外部计算，此处只计算相对伤害
                this.Blood -= hurt;
            }

            if (source.layer == EnemyLayer && this.gameObject.layer == PlayerLayer)
            {
                //敌人打中了玩家  具体是普攻还是技能/特殊 取决于外部计算，此处只计算相对伤害
                this.Blood -= hurt;
            }
        }

        //public void OnCollisionEnter(Collision collision)
        //{

        //}

        public void OnCollisionExit(Collision collision)
        {
            //避免双方被撞飞

            if (this.TryGetComponent<Rigidbody>(out Rigidbody curRigbody))
            {
                curRigbody.GetComponent<Rigidbody>().velocity = Vector3.zero;
            };

            if (collision.rigidbody != null)
            {
                collision.rigidbody.velocity = Vector3.zero;
            };
        }
    }
}