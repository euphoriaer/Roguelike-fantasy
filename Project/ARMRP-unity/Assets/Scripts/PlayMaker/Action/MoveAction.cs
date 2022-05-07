using Battle;
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
            //var go = Fsm.GetOwnerDefaultTarget(GameObject);
            //go.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
           
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

            //error 考虑所有系统只设定数据，类似与ECS
            //Gameobject = Entity 数据全部由Gameobject 中的脚本执行，,
            //Component = CompDef 视为其上的数据为视为Entity 的数据
            //
            //,System = MonoBehaviour 有三个选择
            //1.Gameobject 中是所有System在一个脚本进行 排序，在一个脚本获取当前所有Component数据,进行处理，System会成为方法，排序即方法的调用顺序
            //2.在每个Component,自行处理，System 是一个脚本，Component 当前类型 与对应的System 绑定，
            //system 排序通过 Project->Scripts Execute Order，或者与添加顺序相反的顺序执行，先添加的后执行
            //3.折中，Component 还是写各种System,只是更新与排序放到一个脚本进行

            //go.transform.Translate(Direction.Value * Speed.Value, Space);
            //go.transform.position = go.transform.position + Direction.Value * Speed.Value * Time.deltaTime;//相对
            //
            go.GetComponent<MoveSystem>().Speed = Speed.Value;
            go.GetComponent<MoveSystem>().Direction = Direction.Value;
            //go.transform.GetComponent<Rigidbody>().velocity = Direction.Value * Speed.Value;
            //1.适合entity机制 √√√
            //2.通过role对象传递pos √√
            //3.通过事件委托 广播pos √
            //go.transform.GetComponent<Property>().m_Position = go.transform.position;
            //EventManager.NewMessage(new EventMessage { RolePosition = go.transform.position });

        }


      
    }
}