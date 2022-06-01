using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
   
    [ParadoxNotion.Design.Category("My Nodes/Move")]
    public class LookAtDir : ActionTask<Transform>
    {
        [RequiredField]
        public BBParameter<Vector3> lookDir;

        public bool repeat = false;

        public Vector3 upVector=Vector3.up;

        protected override string info
        {
            get { return "LookAt " + lookDir; }
        }

        protected override void OnExecute()
        { DoLook(); }

        protected override void OnUpdate()
        { DoLook(); }

        private void DoLook()
        {
            //agent.forward = lookDir.value;
            agent.rotation= Quaternion.LookRotation(lookDir.value, upVector);
            if (!repeat)
                EndAction(true);
        }
    }
}