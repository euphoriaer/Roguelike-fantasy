using UnityEngine;

namespace Battle
{
    [UnityEngine.DisallowMultipleComponent]
    [UnityEngine.DefaultExecutionOrder(SystemOrder.PhysicsSystem)]
    [UnityEngine.AddComponentMenu("System/PhysicsSystem")]
    internal class PhysicsSystem : SystemMonoBehaviour
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