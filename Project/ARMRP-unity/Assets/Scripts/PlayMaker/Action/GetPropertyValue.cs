using HutongGames.PlayMaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battle;

namespace Assets.Scripts.PlayMaker.Action
{
    [ActionCategory("Role")]
    [HutongGames.PlayMaker.Tooltip("获取人物属性")]
    public class GetPropertyValue: FsmStateActionBase
    {
        public FsmFloat StoreAttackSpeed;

        public override void Exit()
        {
            base.Exit();
        }

        public override void OnEnter()
        {
            StoreAttackSpeed.Value = Fsm.GameObject.GetComponent<PropertySystem>().AttackSpeed;
            Finish();
        }

        public override void OnUpdate()
        {
            
        }
    }
}
