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
                //��Ҵ����˵���  �������չ����Ǽ���/���� ȡ�����ⲿ���㣬�˴�ֻ��������˺�
                this.gameObject.GetComponent<PropertySystem>().Blood -= hurt;
            }

            if (source.layer ==EnemyLayer && this.gameObject.layer == PlayerLayer)
            {
                //���˴��������  �������չ����Ǽ���/���� ȡ�����ⲿ���㣬�˴�ֻ��������˺�
                this.gameObject.GetComponent<PropertySystem>().Blood -= hurt;
            }
        }

        private void OnValidate()
        {
            //�ƶ��ű���ĩβ
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