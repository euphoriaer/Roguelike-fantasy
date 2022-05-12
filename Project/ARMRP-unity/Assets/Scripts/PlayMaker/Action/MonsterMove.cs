using Battle;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityEngine;

namespace Assets.Scripts.PlayMaker.Action
{
    [ActionCategory("Role")]
    [HutongGames.PlayMaker.Tooltip("怪物移动移动")]
    public class MonsterMove : FsmStateActionBase
    {
        public FsmEventTarget fsmEventTarget;

        public GameObject target;

        public FsmFloat arriveDistance =1;

        public FsmFloat Speed;

        public Space Space;

        public FsmOwnerDefault GameObject;

        public FsmEvent sendEvent;
        public MonsterMove()
        {
        }
        public override void OnUpdate()
        {
            var go = Fsm.GetOwnerDefaultTarget(GameObject);
            if (go == null) return;
            var dir = target.transform.position - go.transform.position;
            //go.transform.Translate(Direction.Value * Speed.Value, Space);
            //go.transform.position = go.transform.position + dir.normalized * Speed.Value * go.transform.GetComponent<PropertySystem>().Time;//相对
            go.transform.GetComponent<MoveSystem>().Direction = dir;
            


            var distance = Vector3.Distance(go.transform.position, target.transform.position);
            if (distance <= arriveDistance.Value)
            {
                Fsm.Event(fsmEventTarget, sendEvent);
                Finish();
            }
            //1.适合entity机制 √√√
            //2.通过role对象传递pos √√
            //3.通过事件委托 广播pos √
            //go.transform.GetComponent<Property>().m_Position = go.transform.position;
            //EventManager.NewMessage(new EventMessage { RolePosition = go.transform.position });
        }
    }
}
