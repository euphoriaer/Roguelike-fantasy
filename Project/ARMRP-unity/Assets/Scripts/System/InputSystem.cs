using UnityEngine;
using UnityEngine.Events;

namespace Battle
{
    [UnityEngine.DisallowMultipleComponent]
    [UnityEngine.DefaultExecutionOrder(SystemOrder.InputSystem)]
    [UnityEngine.AddComponentMenu("System/InputSystem")]
    internal class InputSystem : SystemMonoBehaviour
    {
        //todo 输入与响应分离，这样可以Pc 按键与 轮盘指示共存
        //private bool AttackSignal = false;

        public UnityAction AttackSignalEvent;

        public UnityAction<Vector3> MoveEvent;

        private Vector3 InputDirector;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                if (AttackSignalEvent != null)
                {
                    AttackSignalEvent();
                }
            }

            //有缓动，可以用于力度走跑，移动速度的加速
            //如果有手机端，可根据轮盘距离决定移动速度数值

            //取消缓动使用GetAxisRaw
            var h = Input.GetAxis("Horizontal");
            var v = Input.GetAxis("Vertical");

            //GetAxisRaw 判断是否松开
            var hCancel = Input.GetAxisRaw("Horizontal");
            var vCancel = Input.GetAxisRaw("Vertical");

            //XZ 平面
            //相对方向
            //var forward = transform.TransformDirection(Vector3.forward);
            //绝对方向
            var forward = Vector3.forward;
            forward.y = 0;
            forward = forward.normalized;
            Vector3 right = new Vector3(forward.z, 0, -forward.x);

            InputDirector = h * right * Mathf.Abs(hCancel) + v * forward * Mathf.Abs(vCancel);
            //InputDirector = h * right  + v * forward;
            if (MoveEvent != null)
            {
                MoveEvent(InputDirector);
            }
            //松开按键
        }
    }
}