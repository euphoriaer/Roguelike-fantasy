using Battle;
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

        private Cutscene cutscene;
        private bool isLoopCutscene;
        private float time;

        public override void Exit()
        {
            cutscene.OnStop -= _cutscene_OnStop;
            GameObject.Destroy(cutscene.gameObject);

            base.Exit();
        }

        public override void OnEnter()
        {
            if (Cutscene != null)
            {
                time = 0;
                var go = Fsm.GetOwnerDefaultTarget(FsmGameObject);
                cutscene = CutsceneHelper.InstateAction(out isLoopCutscene, Cutscene, go);
                cutscene.updateMode = Cutscene.UpdateMode.Manual;
                //防止拉回原点

                if (!isLoopCutscene)
                {
                    //非循环，加入播放完成事件
                    cutscene.OnStop += _cutscene_OnStop;
                }

                cutscene.Play();
            }
            else
            {
                Finish();
            }
        }

        public override void OnUpdate()
        {
            time += Fsm.GameObject.GetComponent<PropertySystem>().DeltaTime;
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

        private void _cutscene_OnStop()
        {
            Finish();
        }
    }
}