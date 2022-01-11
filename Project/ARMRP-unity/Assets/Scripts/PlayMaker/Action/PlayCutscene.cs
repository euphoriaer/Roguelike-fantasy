using System.Linq;
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

        public FsmFloat offsetTime;
        public float weight;

        private PlayableGraph playableGraph;

        private AnimationMixerPlayable mixerPlayable;

        private float time;

        private Cutscene _cutscene;

        public override void Awake()
        {
            base.Awake();
        }

        private AnimationClipPlayable clipPlayable0;
        private AnimationClipPlayable clipPlayable1;

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

             clipPlayable0 = AnimationClipPlayable.Create(playableGraph, clip0);

             clipPlayable1 = AnimationClipPlayable.Create(playableGraph, clip1);

            playableGraph.Connect(clipPlayable0, 0, mixerPlayable, 0);

            playableGraph.Connect(clipPlayable1, 0, mixerPlayable, 1);

            clipPlayable1.Pause();//动画过渡融合的关键，停掉第二个，保证动画过渡到第二个动画第一帧

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

            clipPlayable0.SetTime(time + offsetTime.Value);

            mixerPlayable.SetInputWeight(0, 1.0f - weight);

            mixerPlayable.SetInputWeight(1, weight);

            if (isOKMix && Mathf.Abs(weight - 1) < 0.0001)
            {
                Debug.Log("融合完成"+ offsetTime.Value);
                playableGraph.Stop();
                isOKMix = false;
                if (_cutscene != null)
                {
                    _cutscene.Play();
                }
            }
        }

        public override void OnExit()
        {
            offsetTime.Value = _cutscene.currentTime;
            //error 需要向下一个状态state 传递当前cutscene动画
            if (_cutscene != null)
            {
                _cutscene.Stop();
            }
            base.OnExit();
        }
    }
}