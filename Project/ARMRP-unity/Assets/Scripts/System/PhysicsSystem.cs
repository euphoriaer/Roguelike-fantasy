using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Battle
{
    [UnityEngine.DisallowMultipleComponent]
    [UnityEngine.DefaultExecutionOrder(SystemOrder.PhysicsSystem)]
    [UnityEngine.AddComponentMenu("System/PhysicsSystem")]
    public class PhysicsSystem : MonoBehaviour
    {
        public void OnCollisionExit(Collision collision)
        {
            //����˫����ײ��

            if (this.TryGetComponent<Rigidbody>(out Rigidbody curRigbody))
            {
                curRigbody.GetComponent<Rigidbody>().velocity = Vector3.zero;
            };

            if (collision.rigidbody != null)
            {
                collision.rigidbody.velocity = Vector3.zero;
            };
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
