using NodeCanvas.Framework;
using Slate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
    [ParadoxNotion.Design.Name("PlaySampleCutscene")]
    [ParadoxNotion.Design.Category("My Nodes/Cutscene")]
    public class PlaySampleCutscene : ActionTask<Transform>
    {
        public BBParameter<Cutscene> Cutscene;

        protected override void OnExecute()
        {
            base.OnExecute();
            CutsceneHelper.InstateAction(Cutscene.value, agent.gameObject, MultCutsceneFinish: () => {
                EndAction();
            });
        }
    }
}
