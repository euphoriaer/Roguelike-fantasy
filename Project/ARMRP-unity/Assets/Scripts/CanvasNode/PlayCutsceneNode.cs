using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sirenix.OdinInspector;
using Slate;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace NodeCanvas.Tasks.Actions
{

    [ParadoxNotion.Design.Name("Play PlayCutscene Node")]
    [ParadoxNotion.Design.Category("Animator")]
    public class PlayCutsceneNode : ActionTask<Transform>
    {
        public Cutscene Cutscene;
        public GameObject CutscenePlayer;

        public float TransTime;
        public float Speed = 1;


        private Cutscene cutscene;
        
        private bool isLoopCutscene;
        private float time;

        private BBParameter<AnimationClip> CurClip;

        private AnimationClipPlayable clipPlayable0;

        private AnimationClipPlayable clipPlayable1;

        private bool isCross = true;

        private AnimationMixerPlayable mixerPlayable;

        /// <summary>
        /// 随Fsm的offsetTime 自动设置融合动画偏移
        /// </summary>
        private float offsetTime = 0;

        
        private PlayableGraph playableGraph;
        private float weight;

        protected override void OnStop()
        {
            cutscene.OnStop -= _cutscene_OnStop;
            cutscene.Stop();
            CutscenePlayer.GetComponent<Battle.Property>().curPlayClipOffset = cutscene.currentTime;
            CutscenePlayer.GetComponent<Battle.Property>().LastPlayClip = cutscene.GetCutsceneClip<PlayAnimPlayableClip>().First().animationClip;
            base.OnStop();
        }

        private void _cutscene_OnStop()
        {
            EndAction();

        }

        protected override void OnExecute()
        {
            var LastClip= CutscenePlayer.GetComponent<Battle.Property>().LastPlayClip;
            if (Cutscene != null)
            {
                time = 0;
                cutscene = CutsceneInstate();
             
            
               if (LastClip == null)
                {
                    //不需要融合
                    isCross = false;
                    cutscene.Play();
                }
                else
                {
                    //获取上一个状态的 播放时间，为融合动作做准备
                    // 创建该图和混合器，然后将它们绑定到 Animator
                    CurClip = cutscene.GetCutsceneClip<PlayAnimPlayableClip>().First().animationClip;

                    offsetTime= CutscenePlayer.GetComponent<Battle.Property>().curPlayClipOffset;

                    var Animator = CutscenePlayer.GetComponent<Animator>();

                    playableGraph = PlayableGraph.Create();

                    var playableOutput = AnimationPlayableOutput.Create(playableGraph, "Animation", Animator);

                    mixerPlayable = AnimationMixerPlayable.Create(playableGraph, 2);

                    playableOutput.SetSourcePlayable(mixerPlayable);

                    // 创建 AnimationClipPlayable 并将它们连接到混合器。

                    clipPlayable0 = AnimationClipPlayable.Create(playableGraph, CurClip.value);

                    clipPlayable1 = AnimationClipPlayable.Create(playableGraph, LastClip);

                    playableGraph.Connect(clipPlayable0, 0, mixerPlayable, 0);

                    playableGraph.Connect(clipPlayable1, 0, mixerPlayable, 1);

                    clipPlayable1.Pause();//动画过渡融合的关键，停掉第二个，保证动画过渡到第二个动画第一帧

                    //播放该图。
                    playableGraph.Play();

                    isCross = true;
                }
                //检测播放完成 Finish
                cutscene.OnStop += _cutscene_OnStop;
            }
            else
            {
               EndAction();
            }
        }
        private Cutscene CutsceneInstate()
        {
            if (Cutscene != null)
            {
                var _cutscene = CutsceneHelper.Instate(CutscenePlayer, Cutscene);

                GameObject RoleActionCutscene = CutscenePlayer.transform.Find("RoleActionCutscene")?.gameObject;
                if (RoleActionCutscene == null)
                {
                    RoleActionCutscene = new GameObject("RoleActionCutscene");
                    RoleActionCutscene.transform.SetParent(CutscenePlayer.transform, false);
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
        protected override void OnUpdate()
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
                    cutscene.Play();
                }
            }

            if (!isCross)
            {
                time += Time.deltaTime * Speed;
                //防止循环Cutscene 拉回原点
                //_cutscene.currentTime = time % _cutscene.length;
                if (isLoopCutscene)
                {
                    cutscene.Sample(time % cutscene.length);
                }
                else
                {
                    //cutscene 需要手动Stop
                    if (cutscene.length < time)
                    {
                        cutscene.Stop();
                    }
                    cutscene.Sample(time);
                }
            }


        }

        protected override void OnStop(bool interrupted)
        {
           
        }

        protected override void OnResume()
        {
           
        }
    }
}