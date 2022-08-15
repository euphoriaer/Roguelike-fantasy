using Battle;
using NodeCanvas.Framework;
using Slate;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
    [ParadoxNotion.Design.Name("PlayCutscene")]
    [ParadoxNotion.Design.Category("My Nodes/Monster")]
    public class MonsterPlayCutsceneAction : ActionTask<Transform>
    {
        public BBParameter<Cutscene> Cutscene;

        protected override void OnExecute()
        {
            base.OnExecute();
            //所有驱动在各类System,外部工具只设置数据
            CutsceneHelper.InstateAction(Cutscene.value, agent.gameObject, MultCutsceneFinish: () => {
                EndAction();
            });
        }

        protected override string OnInit()
        {
            return base.OnInit();
        }

        protected override void OnStop()
        {

        }

        protected override void OnUpdate()
        {

        }
    }
}
