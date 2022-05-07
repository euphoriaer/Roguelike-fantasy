using UnityEngine;

namespace Battle
{
    [RequireComponent(typeof(BehitSystem))]
    [UnityEngine.DisallowMultipleComponent]
    [UnityEngine.DefaultExecutionOrder(SystemOrder.MoveSystem)]
    [UnityEngine.AddComponentMenu("System/MoveSystem")]
    public class MoveSystem : MonoBehaviour
    {
        public Vector3 Direction;

        public float Speed;

        private void FixedUpdate()
        {
            this.transform.position = this.transform.position + Speed * Direction.normalized * Time.deltaTime;
            //避免持续碰撞时  相机抖动
        }

        private void OnValidate()
        {
            //移动脚本到末尾
            while (UnityEditorInternal.ComponentUtility.MoveComponentDown(this))
            {
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