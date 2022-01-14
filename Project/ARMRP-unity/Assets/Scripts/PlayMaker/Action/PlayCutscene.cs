using System.Collections.Generic;
using HutongGames.PlayMaker;
using Slate;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using Sirenix.OdinInspector;

namespace Assets.Scripts.PlayMaker.Action
{
    public class PlayCutscene : FsmStateActionBase
    {
        [LabelText("Cutscene名字")]
        public Cutscene cutscene;

        private AnimationClip clip0;

        private AnimationClip clip1;

        public float TransTime;

        /// <summary>
        /// error 随Fsm的offsetTime 自动
        /// </summary>
        public FsmFloat offsetTime;
        private float weight;

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
        private FsmState _lastFsmState;

        public override void OnEnter()
        {
            time = 0;
            isOKMix = true;
            _cutscene = CutsceneInstate();
            clip1 = _cutscene.GetCutsceneClip<PlayAnimPlayable>().First().animationClip;
            //方式1 ，从记录的上一个状态获取clip
            if (Fsm.LastTransition==null)
            {
                _cutscene.Play();
                return;
            }
            var lastplayCutscene = LastFsmState?.Actions.Where(p =>
            {
                if (p is PlayCutscene)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            })?.First() as PlayCutscene;
            if (lastplayCutscene == null)
            {
                //不需要融合
                _cutscene.Play();
               
            }
            else
            {
                // 创建该图和混合器，然后将它们绑定到 Animator
                clip0 = lastplayCutscene.clip1;
         
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
        }

        private Cutscene CutsceneInstate()
        {
            if (cutscene!=null)
            {
                var _cutscene = CutsceneHelper.Instate(this.Owner, cutscene);
                return _cutscene;
            }

            var state = Fsm.ActiveState;

            return null;
        }

        private bool isOKMix = true;

        public override void OnUpdate()
        {
            if (clip0 == null)
            {
                //不需要融合
            }
            else
            {
                time += Time.deltaTime;
                weight = Mathf.Lerp(0, 1, time / TransTime);
                weight = Mathf.Clamp01(weight);

                clipPlayable0.SetTime(time + offsetTime.Value);

                mixerPlayable.SetInputWeight(0, 1.0f - weight);

                mixerPlayable.SetInputWeight(1, weight);

                if (isOKMix && Mathf.Abs(weight - 1) < 0.0001)
                {
                    Debug.Log("融合完成" + offsetTime.Value);
                    playableGraph.Stop();
                    isOKMix = false;
                    if (_cutscene != null)
                    {
                        _cutscene.Play();
                    }
                }
            }
        }

        public override void Exit()
        {
            var animClip = _cutscene.GetCutsceneClip<PlayAnimPlayable>().First().animationClip;
            offsetTime.Value = _cutscene.currentTime;
            if (_cutscene != null)
            {
                _cutscene.Stop();
            }
            base.Exit();
        }
    }

}