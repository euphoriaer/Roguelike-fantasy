using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sirenix.OdinInspector;
using Slate;
using UnityEngine;


namespace NodeCanvas.Tasks.Actions
{

    [ParadoxNotion.Design.Name("Play PlayCutscene Node TransForm")]
    [ParadoxNotion.Design.Category("Animator")]
    public class PlayCutsceneNodeTransForm : ActionTask<Transform>
    {
        public Cutscene Cutscene;
        public GameObject CutscenePlayer;

        public bool isFollow;
        [LabelText("距离最后一刻的偏移")]
        public float OffsetFinalTime = 0.03f;

        private Cutscene _cutscene;
        private bool isLoopCutscene;
        private float time;

        protected override void OnStop()
        {
            _cutscene.OnStop -= _cutscene_OnStop;
            _cutscene.Stop();
           
            base.OnStop();
        }

        private void _cutscene_OnStop()
        {
            EndAction();

        }

        protected override void OnExecute()
        {
            if (Cutscene != null)
            {
                var _cutscene = CutsceneHelper.Instate(CutscenePlayer, Cutscene);
                _cutscene.updateMode = Cutscene.UpdateMode.Manual;
                //修改Loop 防止拉回原点
                if (_cutscene.defaultWrapMode == Cutscene.WrapMode.Loop)
                {
                    isLoopCutscene = true;
                }
                else
                {
                    isLoopCutscene = false;
                }
                _cutscene.Play();
                //检测播放完成 Finish
                _cutscene.OnStop += _cutscene_OnStop;
            }
            else
            {
               EndAction();
            }
        }

        protected override void OnUpdate()
        {
            time += Time.deltaTime;
            if (isLoopCutscene)
            {
                _cutscene.Sample(time % _cutscene.length);
            }
            else
            {
                _cutscene.Sample(time);
            }

        }

        protected override void OnStop(bool interrupted)
        {
           
        }

        protected override void OnResume()
        {
           
        }
    }
}