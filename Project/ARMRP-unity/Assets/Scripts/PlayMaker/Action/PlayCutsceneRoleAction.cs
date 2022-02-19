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
    public class PlayCutsceneRoleAction : FsmStateActionBase
    {
        public Cutscene Cutscene;

        public FsmOwnerDefault GameObject;
        public float TransTime;
        private Cutscene _cutscene;
        private FsmState _lastFsmState;
        private AnimationClip clip0;

        private AnimationClip clip1;

        private AnimationClipPlayable clipPlayable0;

        private AnimationClipPlayable clipPlayable1;

        private bool isCross = true;

        private bool isLoopCutscene;

        private AnimationMixerPlayable mixerPlayable;

        /// <summary>
        /// 随Fsm的offsetTime 自动设置融合动画偏移
        /// </summary>
        private float offsetTime = 0;

        private GameObject Owner;
        private PlayableGraph playableGraph;
        private float time;
        private float weight;

        public override void Exit()
        {
            var animClip = _cutscene.GetCutsceneClip<PlayAnimPlayableClip>().First().animationClip;
            offsetTime = _cutscene.currentTime;
            if (_cutscene != null)
            {
                _cutscene.Stop();
            }
            _cutscene.OnStop -= _cutscene_OnStop;
            base.Exit();
        }

        public override void OnEnter()
        {
            Owner = Fsm.GetOwnerDefaultTarget(GameObject);

            time = 0;

            _cutscene = CutsceneInstate();
            clip1 = _cutscene.GetCutsceneClip<PlayAnimPlayableClip>().First().animationClip;
            //从记录的上一个状态获取clip  //todo 只有动作需要融合，所以需要区分播放动作的Cutscene和非播放动作的Cutscene
            if (Fsm.LastTransition == null)
            {
                //不需要融合
                isCross = false;
                _cutscene.Play();
                return;
            }
            var lastplayCutscene = LastFsmState?.Actions.Where(p =>
            {
                if (p is PlayCutsceneRoleAction)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            })?.First() as PlayCutsceneRoleAction;

            if (lastplayCutscene == null || lastplayCutscene.Enabled == false)
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

                var Animator = Owner.GetComponent<Animator>();

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

                isCross = true;
            }
            //检测播放完成 Finish
            _cutscene.OnStop += _cutscene_OnStop;
        }

        public override void OnUpdate()
        {
            if (isCross)
            {
                time += Time.deltaTime;
                //需要融合
                weight = Mathf.Lerp(0, 1, time / TransTime);
                weight = Mathf.Clamp01(weight);

                clipPlayable0.SetTime(time + offsetTime);

                mixerPlayable.SetInputWeight(0, 1.0f - weight);

                mixerPlayable.SetInputWeight(1, weight);

                if (Mathf.Abs(weight - 1) < 0.0001)
                {
                    Debug.Log("融合完成" + offsetTime);
                    playableGraph.Stop();
                    isCross = false;
                    //使cutscene 从0开始播放，计时重置
                    time = 0;
                    _cutscene.Play();
                }
            }

            if (!isCross)
            {
                time += Time.deltaTime;
                //防止循环Cutscene 拉回原点
                //_cutscene.currentTime = time % _cutscene.length;
                if (isLoopCutscene)
                {
                    _cutscene.Sample(time % _cutscene.length);
                }
                else
                {
                    _cutscene.Sample(time);
                }
            }

            base.OnUpdate();
        }

        private void _cutscene_OnStop()
        {
            Finish();
        }

        private Cutscene CutsceneInstate()
        {
            if (Cutscene != null)
            {
                var _cutscene = CutsceneHelper.Instate(Owner, Cutscene);

                GameObject RoleActionCutscene = Owner.transform.Find("RoleActionCutscene")?.gameObject;
                if (RoleActionCutscene == null)
                {
                    RoleActionCutscene = new GameObject("RoleActionCutscene");
                    RoleActionCutscene.transform.SetParent(Owner.transform, false);
                }
                else
                {
                    //销毁原本播放的Cutscene
                    UnityEngine.GameObject.Destroy(RoleActionCutscene.transform.GetChild(0).gameObject);
                }

                _cutscene.transform.SetParent(RoleActionCutscene.transform, false);

                //修改Loop 防止拉回原点
                if (_cutscene.defaultWrapMode == Cutscene.WrapMode.Loop)
                {
                    isLoopCutscene = true;
                }
                else
                {
                    isLoopCutscene = false;
                }
                _cutscene.updateMode = Cutscene.UpdateMode.Manual;
                return _cutscene;
            }
            return null;
        }
    }
}