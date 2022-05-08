using UnityEngine;

namespace Battle
{
    [RequireComponent(typeof(BehitSystem))]
    [UnityEngine.DisallowMultipleComponent]
    [UnityEngine.DefaultExecutionOrder(SystemOrder.MoveSystem)]
    [UnityEngine.AddComponentMenu("System/MoveSystem")]
    internal class MoveSystem : SystemMonoBehaviour
    {
        public Vector3 Direction;

        public float Speed;

        private void FixedUpdate()
        {
            this.transform.position = this.transform.position + Speed * Direction.normalized * Time.deltaTime;
            //避免持续碰撞时  相机抖动
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