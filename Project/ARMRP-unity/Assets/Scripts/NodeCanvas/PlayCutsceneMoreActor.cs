using Battle;
using NodeCanvas.Framework;
using ParadoxNotion;
using Slate;
using System.Collections.Generic;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
    [ParadoxNotion.Design.Name("PlayCutsceneMoreActor")]
    [ParadoxNotion.Design.Category("My Nodes/Cutscene")]
    public class PlayCutsceneMoreActor : ActionTask<Transform>
    {
        public Cutscene Cutscene;

        public BBParameter<List<GameObject>> CutscenePlayers;

        public CompactStatus CutsceneFinish = CompactStatus.Success;

        private Cutscene cutscene;
        private bool isLoopCutscene;
        private float time;

        private void _cutscene_OnStop()
        {
            EndAction(CutsceneFinish == CompactStatus.Success ? true : false);
        }

        protected override void OnExecute()
        {
            if (Cutscene != null)
            {
                time = 0;
                //播放Action
                //cutscene = CutsceneHelper.Instate(CutscenePlayer.value, Cutscene);
                //
                cutscene = CutsceneHelper.InstateAction(out isLoopCutscene, Cutscene, actors:CutscenePlayers.value.ToArray());

                cutscene.updateMode = Cutscene.UpdateMode.Manual;
                // 防止拉回原点
                if (!isLoopCutscene)
                {
                    //非循环，加入播放完成事件
                    cutscene.OnStop += _cutscene_OnStop;
                }

                cutscene.Play();
                //检测播放完成 Finish
            }
            else
            {
                EndAction(CutsceneFinish == CompactStatus.Success ? true : false);
            }
        }

        protected override void OnUpdate()
        {
            time += agent.GetComponent<PropertySystem>().FixedDeltaTime;
            if (isLoopCutscene)
            {
                cutscene.Sample(time % cutscene.length);
            }
            else
            {
                cutscene.Sample(time);
                if (time >= cutscene.length)
                {
                    cutscene.Stop();
                }
            }
        }

        protected override void OnStop(bool interrupted)
        {
            cutscene.OnStop -= _cutscene_OnStop;
            cutscene.Stop();
            base.OnStop();
        }
    }
}