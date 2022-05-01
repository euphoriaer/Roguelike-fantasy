using HutongGames.PlayMaker;
using Slate;
using UnityEngine;

namespace Assets.Scripts.PlayMaker.Action
{
    [ActionCategory("Cutscene")]
    [HutongGames.PlayMaker.Tooltip("用于普通Cutscene")]
    public class PlayCutsceneAction : FsmStateActionBase
    {
        public Cutscene Cutscene;

        public FsmOwnerDefault FsmGameObject;

        private Cutscene m_cutscene;
        private bool isLoopCutscene;
        private float time;

        public override void Exit()
        {
            m_cutscene.OnStop -= _cutscene_OnStop;
            GameObject.Destroy(m_cutscene.gameObject);

            base.Exit();
        }

        public override void OnEnter()
        {
            if (Cutscene != null)
            {
                time = 0;
                var go = Fsm.GetOwnerDefaultTarget(FsmGameObject);
                m_cutscene = CutsceneHelper.InstateAction(out isLoopCutscene, Cutscene, go);
                m_cutscene.updateMode = Cutscene.UpdateMode.Manual;
                //防止拉回原点

                if (!isLoopCutscene)
                {
                    //非循环，加入播放完成事件
                    m_cutscene.OnStop += _cutscene_OnStop;
                }

                m_cutscene.Play();
            }
            else
            {
                Finish();
            }
        }

        public override void OnUpdate()
        {
            time += Time.deltaTime;
            if (isLoopCutscene)
            {
                m_cutscene.Sample(time % m_cutscene.length);
            }
            else
            {
                m_cutscene.Sample(time);
                if (time >= m_cutscene.length)
                {
                    m_cutscene.Stop();
                }
            }
        }

        private void _cutscene_OnStop()
        {
            Finish();
        }
    }
}