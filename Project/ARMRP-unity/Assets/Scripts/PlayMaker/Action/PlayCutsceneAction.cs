using HutongGames.PlayMaker;
using Sirenix.OdinInspector;
using Slate;

namespace Assets.Scripts.PlayMaker.Action
{
    [ActionCategory("Cutscene")]
    [HutongGames.PlayMaker.Tooltip("用于普通Cutscene")]
    public class PlayCutsceneAction : FsmStateActionBase
    {
        public Cutscene Cutscene;

        public FsmOwnerDefault FsmGameObject;
        public bool isFollow;
        private Cutscene _cutscene;
        private bool isLoopCutscene;

        [LabelText("距离最后一刻的偏移")]
        public float OffsetFinalTime = 0.03f;

        public override void OnEnter()
        {
            if (Cutscene != null)
            {
                var go = Fsm.GetOwnerDefaultTarget(FsmGameObject);
                var _cutscene = CutsceneHelper.Instate(go, Cutscene);
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
        }

        private void _cutscene_OnStop()
        {
            Finish();
        }

        public override void OnUpdate()
        {
            if (isLoopCutscene &&
                _cutscene.length - _cutscene.currentTime < OffsetFinalTime)
            {
                //防止循环Cutscene 拉回原点
                _cutscene.currentTime = 0;
            }
        }

        public override void Exit()
        {
            _cutscene.OnStop -= _cutscene_OnStop;
            _cutscene.Stop();
            base.Exit();
        }
    }
}