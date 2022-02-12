using HutongGames.PlayMaker;
using Slate;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Assets.Scripts.PlayMaker.Action
{
    [ActionCategory("Cutscene")]
    [HutongGames.PlayMaker.Tooltip("用于带有角色动作融合的Cutscene")]
    public class PlayCutsceneRole : FsmStateActionBase
    {
        public Cutscene Cutscene;

        private AnimationClip clip0;

        private AnimationClip clip1;

        public float TransTime;

        /// <summary>
        /// 随Fsm的offsetTime 自动设置融合动画偏移
        /// </summary>
        private float offsetTime = 0;

        private float weight;

        private PlayableGraph playableGraph;

        private AnimationMixerPlayable mixerPlayable;

        private float time;

        private Cutscene _cutscene;

        private bool isCross = false;

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
            //从记录的上一个状态获取clip  //todo 只有动作需要融合，所以需要区分播放动作的Cutscene和非播放动作的Cutscene
            if (Fsm.LastTransition == null)
            {
                _cutscene.Play();
                return;
            }
            var lastplayCutscene = LastFsmState?.Actions.Where(p =>
            {
                if (p is PlayCutsceneRole)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            })?.First() as PlayCutsceneRole;

            if (lastplayCutscene == null|| lastplayCutscene.Enabled==false)
            {
                //不需要融合
                isCross = false;
                _cutscene.Play();
            }
            else
            {
                //获取上一个状态的 播放时间，为融合动作做准备
                offsetTime = lastplayCutscene.offsetTime;

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
            
            //检测播放完成 Finish
            _cutscene.OnStop += _cutscene_OnStop;
        }

        private Cutscene CutsceneInstate()
        {
            if (Cutscene != null)
            {
                var _cutscene = CutsceneHelper.Instate(this.Owner, Cutscene);
                return _cutscene;
            }
            return null;
        }

        private bool isOKMix = true;

        public override void OnUpdate()
        {
            if (isCross)
            {
                //需要融合
                time += Time.deltaTime;
                weight = Mathf.Lerp(0, 1, time / TransTime);
                weight = Mathf.Clamp01(weight);

                clipPlayable0.SetTime(time + offsetTime);

                mixerPlayable.SetInputWeight(0, 1.0f - weight);

                mixerPlayable.SetInputWeight(1, weight);

                if (isOKMix && Mathf.Abs(weight - 1) < 0.0001)
                {
                    Debug.Log("融合完成" + offsetTime);
                    playableGraph.Stop();
                    isOKMix = false;
                    _cutscene.Play();
                }
            }
            
        }

        private void _cutscene_OnStop()
        {
            Finish();
        }

        public override void Exit()
        {
            var animClip = _cutscene.GetCutsceneClip<PlayAnimPlayable>().First().animationClip;
            offsetTime = _cutscene.currentTime;
            if (_cutscene != null)
            {
                _cutscene.Stop();
            }
            base.Exit();
        }
    }
}