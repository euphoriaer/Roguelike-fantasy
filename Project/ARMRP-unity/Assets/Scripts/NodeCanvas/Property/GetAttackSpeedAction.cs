using Battle;
using NodeCanvas.Framework;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
    [ParadoxNotion.Design.Category("My Nodes/Property")]
    public class GetAttackSpeedAction : ActionTask<Transform>
    {
        public BBParameter<float> SaveAs;

        protected override void OnExecute()
        {
        }

        protected override void OnUpdate()
        {
            SaveAs.value = agent.GetComponent<PropertySystem>().AttackSpeed;
        }
    }
}
