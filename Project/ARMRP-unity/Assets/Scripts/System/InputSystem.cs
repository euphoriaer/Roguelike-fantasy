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
        public bool AttackSignal=false;
        public UnityAction AttackSignalEvent;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                if (AttackSignalEvent!=null)
                {
                    AttackSignalEvent();
                }
            }
            else
            {
                AttackSignal = false;
            }
        }
    }
}
