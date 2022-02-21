using HutongGames.PlayMaker;
using UnityEngine;

namespace Assets.Scripts.PlayMaker.Action
{
    [ActionCategory("Role")]
    [HutongGames.PlayMaker.Tooltip("得到当前entity到某个角色的方向")]
    public class GetEntityDirection : FsmStateActionBase
    {
        public FsmOwnerDefault GameObject;//哪个角色
        [UIHint(UIHint.Variable)]
        [HutongGames.PlayMaker.Tooltip("Store the magnitude of the vector. Useful if you want to measure the strength of the input and react accordingly. " +
                 "Hint: Use {{Float Compare}}.")]
        public FsmFloat storeMagnitude;
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
            //计算两者向量 point1-point2=vector2->1

            //赋值storeMagnitude
        }
    }
}
