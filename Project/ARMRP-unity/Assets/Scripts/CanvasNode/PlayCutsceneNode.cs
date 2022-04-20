using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using Sirenix.OdinInspector;
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
                cutscene = CutsceneHelper.Instate(CutscenePlayer.value, Cutscene);
                cutscene.updateMode = Cutscene.UpdateMode.Manual;
                //修改Loop 防止拉回原点
                if (cutscene.defaultWrapMode == Cutscene.WrapMode.Loop)
                {
                    isLoopCutscene = true;
                }
                else
                {
                    isLoopCutscene = false;
                    //非Loop 动画，播放完算结束
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
            time += Time.deltaTime;
            if (isLoopCutscene)
            {
                cutscene.Sample(time % cutscene.length);
            }
            else
            {
                cutscene.Sample(time);
                if (time>=cutscene.length)
                {
                    cutscene.Stop();
                }
            }

        }

        protected override void OnStop(bool interrupted)
        {
            cutscene.OnStop -= _cutscene_OnStop;
            base.OnStop();
        }

        protected override void OnResume()
        {
           
        }
    }
}