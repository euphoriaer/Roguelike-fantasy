using HutongGames.PlayMaker;
using Slate;

namespace Assets.Scripts.PlayMaker.Action
{
    [ActionCategory("Cutscene")]
    [HutongGames.PlayMaker.Tooltip("用于CutsceneSystem")]
    public class PlayCutsceneSimple : FsmStateActionBase
    {
        public Cutscene Cutscene;

        public FsmOwnerDefault FsmGameObject;

        //需要知道是否完成多段攻击播放
        public override void OnEnter()
        {
            var go = Fsm.GetOwnerDefaultTarget(FsmGameObject);
            CutsceneHelper.InstateAction(Cutscene, go, MultCutsceneFinish: () => {
                Finish();
            });
        }
    }
}