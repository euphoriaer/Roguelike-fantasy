using HutongGames.PlayMaker;
using Sirenix.OdinInspector;
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
                m_cutscene = CutsceneHelper.Instate(go, Cutscene);
                m_cutscene.updateMode = Cutscene.UpdateMode.Manual;
                //修改Loop 防止拉回原点
                if (m_cutscene.defaultWrapMode == Cutscene.WrapMode.Loop)
                {
                    isLoopCutscene = true;
                }
                else
                {
                    isLoopCutscene = false;
                    //检测播放完成 Finish
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