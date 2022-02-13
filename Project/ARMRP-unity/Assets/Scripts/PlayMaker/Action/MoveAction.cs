using HutongGames.PlayMaker;
using UnityEngine;

namespace Assets.Scripts.PlayMaker.Action
{
    [ActionCategory("Role")]
    [HutongGames.PlayMaker.Tooltip("角色移动")]
    public class MoveAction : FsmStateActionBase
    {
        public FsmVector3 Direction;

        public FsmFloat Speed;

        public Space Space;

        public FsmOwnerDefault GameObject;

        public override void Exit()
        {
            base.Exit();
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnUpdate()
        {
            var go = Fsm.GetOwnerDefaultTarget(GameObject);
            if (go == null) return;

            //go.transform.Translate(Direction.Value * Speed.Value, Space);
            go.transform.position = go.transform.position + Direction.Value * Speed.Value * Time.deltaTime;//相对
            //1.适合entity机制 √√√
            //2.通过role对象传递pos √√
            //3.通过事件委托 广播pos √
            //go.transform.GetComponent<Property>().m_Position = go.transform.position;
            EventManager.NewMessage(new EventMessage { RolePosition = go.transform.position });
        
        }
    }
}