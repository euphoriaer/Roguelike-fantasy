using Sirenix.OdinInspector;
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

        [LabelText("常规移动速度")]
        public float MaxSpeed = 10;

        [LabelText("当前移动速度")]
        public float CurSpeed=10;

        private void FixedUpdate()
        {
            this.transform.position = this.transform.position + CurSpeed * Direction.normalized * this.transform.GetComponent<PropertySystem>().DeltaTime;
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