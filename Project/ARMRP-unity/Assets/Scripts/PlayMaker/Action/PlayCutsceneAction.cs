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
        public bool isFollow;
        [LabelText("距离最后一刻的偏移")]
        public float OffsetFinalTime = 0.03f;

        private Cutscene _cutscene;
        private bool isLoopCutscene;
        private float time;
        public override void Exit()
        {
            _cutscene.OnStop -= _cutscene_OnStop;
            _cutscene.Stop();
            base.Exit();
        }

        public override void OnEnter()
        {
            if (Cutscene != null)
            {
                var go = Fsm.GetOwnerDefaultTarget(FsmGameObject);
                var _cutscene = CutsceneHelper.Instate(go, Cutscene);
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
                Finish();
            }
        }

        public override void OnUpdate()
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

        private void _cutscene_OnStop()
        {
            Finish();
        }
    }
}