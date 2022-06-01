using Battle;
using NodeCanvas.Framework;
using ParadoxNotion;
using Slate;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
    [ParadoxNotion.Design.Name("InputMoveAcion")]
    [ParadoxNotion.Design.Category("My Nodes/Move")]
    public class InputMoveAcion : ActionTask<Transform>
    {
        [BlackboardOnly]
        public BBParameter<Vector3> SaveDirAs;

        //传递数据给MoveSystem
        protected override void OnExecute()
        {
            base.OnExecute();
            agent.gameObject.GetComponent<InputSystem>().MoveEvent += MoveEvent;
          
        }

        private void MoveEvent(Vector3 InputDir)
        {
            SaveDirAs.value = InputDir;
            agent.GetComponent<MoveSystem>().Direction = InputDir;
        }

        protected override void OnStop()
        {
            agent.GetComponent<MoveSystem>().Direction = Vector3.zero;
            agent.gameObject.GetComponent<InputSystem>().MoveEvent -= MoveEvent;
        }
    }
}
