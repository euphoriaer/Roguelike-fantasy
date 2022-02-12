using HutongGames.PlayMaker;
using Slate;

namespace Assets.Scripts.PlayMaker.Action
{
    [ActionCategory("Cutscene")]
    [HutongGames.PlayMaker.Tooltip("用于普通Cutscene")]
    public class PlayCutscene : FsmStateActionBase
    {
        public Cutscene Cutscene;
        public override void OnEnter()
        {
            if (Cutscene != null)
            {
                var _cutscene = CutsceneHelper.Instate(this.Owner, Cutscene);
                _cutscene.Play();
            }
        }
    }
}