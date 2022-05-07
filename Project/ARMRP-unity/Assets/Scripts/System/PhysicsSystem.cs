using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Battle
{
    public class PhysicsSystem : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public void OnCollisionExit(Collision collision)
        {
            //±ÜÃâË«·½±»×²·É

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
