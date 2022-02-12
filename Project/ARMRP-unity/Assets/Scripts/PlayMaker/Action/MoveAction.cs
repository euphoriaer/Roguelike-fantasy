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

            go.transform.Translate(Direction.Value * Speed.Value, Space);
        }
    }
}