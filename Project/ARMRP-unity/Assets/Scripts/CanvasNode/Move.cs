using UnityEngine;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using ParadoxNotion;
using Battle;

namespace NodeCanvas.BehaviourTrees
{

	[Category("My Nodes")]
	[ParadoxNotion.Design.Icon("SomeIcon")]
	[Description("MoveGameobject")]
	public class SimpleMove : ActionTask<Transform>
	{
		public BBParameter<GameObject> MoveObj;
		public BBParameter<float> Speed=1;
		public BBParameter<float> ArriveRange=1;
		public BBParameter<GameObject> Target;
		public CompactStatus ArriveTarget  = CompactStatus.Success;



		protected override void OnExecute()
        {
			
		}

        protected  void OnReset()
		{

			
		}

        protected override void OnStop()
        {
			MoveObj.value.GetComponent<MoveSystem>().Speed = 0;
			base.OnStop();
        }

        protected override void OnUpdate()
        {
			var direction = Target.value.transform.position - MoveObj.value.transform.position;

			var distance = direction.magnitude;

			if (distance <= ArriveRange.value)
			{
				EndAction(ArriveTarget == CompactStatus.Success ? true : false);
			}

			//MoveObj.value.transform.position = MoveObj.value.transform.position + direction.normalized * Speed.value * Time.deltaTime;//相对
			MoveObj.value.GetComponent<MoveSystem>().Speed = Speed.value;
			MoveObj.value.GetComponent<MoveSystem>().Direction = direction;
		}
		
		
	}
}