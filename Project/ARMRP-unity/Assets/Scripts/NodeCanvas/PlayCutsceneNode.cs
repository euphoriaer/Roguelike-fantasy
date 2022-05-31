using Battle;
using NodeCanvas.Framework;
using ParadoxNotion;
using Slate;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
    [ParadoxNotion.Design.Name("Play PlayCutscene Node")]
    [ParadoxNotion.Design.Category("Animator")]
    public class PlayCutsceneNode : ActionTask<Transform>
    {
        public Cutscene Cutscene;
        public BBParameter<GameObject> CutscenePlayer;
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
                cutscene = CutsceneHelper.InstateAction(out isLoopCutscene, Cutscene, CutscenePlayer.value);

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
            time += agent.GetComponent<PropertySystem>().DeltaTime;
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

        protected override void OnResume()
        {
        }
    }
}