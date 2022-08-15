using Sirenix.OdinInspector;
using UnityEngine;

namespace Battle
{
    [RequireComponent(typeof(PropertySystem))]
    [RequireComponent(typeof(BehitSystem))]
    [UnityEngine.DisallowMultipleComponent]
    [UnityEngine.DefaultExecutionOrder(SystemOrder.MoveSystem)]
    [UnityEngine.AddComponentMenu("System/MoveSystem")]
    internal class MoveSystem : SystemMonoBehaviour
    {
        public Vector3 Direction;

        [LabelText("当前移动速度")]
        public float CurSpeed;

        private void FixedUpdate() 
        {
            //todo 速度可以增量平滑，为过渡动画留出时间 learp
            //CurSpeed = Mathf.Lerp(0, MaxSpeed, LerpTime);
            //LerpTime += LerpSpeed * GetComponent<PropertySystem>().DeltaTime;
            this.transform.position = this.transform.position + CurSpeed * Direction.normalized *
                this.transform.GetComponent<PropertySystem>().FixedDeltaTime;
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
