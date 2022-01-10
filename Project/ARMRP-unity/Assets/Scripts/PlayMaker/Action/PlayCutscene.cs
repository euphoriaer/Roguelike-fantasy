using HutongGames.PlayMaker;
using Slate;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Assets.Scripts.PlayMaker.Action
{
    public class PlayCutscene : FsmStateAction
    {
        [UnityEngine.Tooltip("Cutscene名字")]
        public string CutsceneName;

        public AnimationClip clip0;

        public AnimationClip clip1;

        public float TransTime;

        public float offsetTime;
        public float weight;

        private PlayableGraph playableGraph;

        private AnimationMixerPlayable mixerPlayable;

        private float time;

        private Cutscene _cutscene;

        public override void Awake()
        {
            base.Awake();
        }

        public override void OnEnter()
        {
            time = 0;
            isOKMix = true;
            // 创建该图和混合器，然后将它们绑定到 Animator。
            _cutscene = CutsceneInstate();
            var Animator = Fsm.GameObject.GetComponent<Animator>();

            playableGraph = PlayableGraph.Create();

            var playableOutput = AnimationPlayableOutput.Create(playableGraph, "Animation", Animator);

            mixerPlayable = AnimationMixerPlayable.Create(playableGraph, 2);

            playableOutput.SetSourcePlayable(mixerPlayable);

            // 创建 AnimationClipPlayable 并将它们连接到混合器。

            var clipPlayable0 = AnimationClipPlayable.Create(playableGraph, clip0);

            var clipPlayable1 = AnimationClipPlayable.Create(playableGraph, clip1);

            playableGraph.Connect(clipPlayable0, 0, mixerPlayable, 0);

            playableGraph.Connect(clipPlayable1, 0, mixerPlayable, 1);

            //播放该图。

            playableGraph.Play();
        }

        private Cutscene CutsceneInstate()
        {
            if (!string.IsNullOrEmpty(CutsceneName))
            {
                var _cutscene = CutsceneHelper.Instate(this.Owner, CutsceneName);
                return _cutscene;
            }

            return null;
        }

        private bool isOKMix = true;

        public override void OnUpdate()
        {
            time += Time.deltaTime;
            weight = Mathf.Lerp(0, 1, time / TransTime);
            weight = Mathf.Clamp01(weight);

            mixerPlayable.SetInputWeight(0, 1.0f - weight);

            mixerPlayable.SetInputWeight(1, weight);

            mixerPlayable.GetInput(0).SetTime(time + offsetTime);
           
            //mixerPlayable.GetInput(1).SetTime(time+ offectTime);

            if (isOKMix && Mathf.Abs(weight - 1) < 0.001)
            {
                Debug.Log("融合完成");
                playableGraph.Destroy();
                isOKMix = false;
                if (_cutscene != null)
                {
                    _cutscene.Play();
                }
            }
        }

        public override void OnExit()
        {
            if (_cutscene != null)
            {
                _cutscene.Stop();
            }

            
            //向下一个状态传递值？
            for (int i = 0; i < Fsm.ActiveState.ActiveActions.Count; i++)
            {
                var action = Fsm.ActiveState.ActiveActions[i];
                if (action is PlayCutscene)
                {
                    var playCutscene = action as PlayCutscene;
                    playCutscene.offsetTime = _cutscene.currentTime;
                    Debug.Log("状态切换，获取当前cutscene播放到的Time"+ _cutscene.currentTime);
                }
            }

            base.OnExit();
        }
    }
}