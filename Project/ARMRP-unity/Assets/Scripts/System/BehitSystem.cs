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
                //��Ҵ����˵���  �������չ����Ǽ���/���� ȡ�����ⲿ���㣬�˴�ֻ��������˺�
                this.gameObject.GetComponent<PropertySystem>().Blood -= hurt;
            }

            if (source.layer == PropertySystem.EnemyLayer && this.gameObject.layer == PropertySystem.PlayerLayer)
            {
                //���˴��������  �������չ����Ǽ���/���� ȡ�����ⲿ���㣬�˴�ֻ��������˺�
                this.gameObject.GetComponent<PropertySystem>().Blood -= hurt;
            }
        }
    }
}