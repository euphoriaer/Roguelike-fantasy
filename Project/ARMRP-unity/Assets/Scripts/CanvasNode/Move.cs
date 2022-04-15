using UnityEngine;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
 
namespace NodeCanvas.BehaviourTrees
{

	[Category("My Nodes")]
	[ParadoxNotion.Design.Icon("SomeIcon")]
	[Description("MoveGameobject")]
	public class SimpleMove : BTNode
	{
		public BBParameter<GameObject> MoveObj;
		public BBParameter<float> Speed=1;
		public BBParameter<float> ArriveRange=1;
		public BBParameter<GameObject> Target;



		protected override Status OnExecute(Component agent, IBlackboard blackboard)
		{
			var direction = Target.value.transform.position - MoveObj.value.transform.position;

			var distance = direction.magnitude;

			if (distance>= ArriveRange.value)
            {
				return Status.Success;
			}
			
			
			MoveObj.value.transform.position = MoveObj.value.transform.position + direction.normalized * Speed.value * Time.deltaTime;//相对
			return Status.Running;
		}

		protected override void OnReset()
		{

			
		}
	}
}