using Sirenix.OdinInspector;
using Slate;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace Battle
{
    [UnityEngine.DisallowMultipleComponent]
    [UnityEngine.DefaultExecutionOrder(SystemOrder.CutsceneSystem)]
    [UnityEngine.AddComponentMenu("System/CutsceneSystem")]
    public class CutsceneSystem : SystemMonoBehaviour
    {
        /// <summary>
        /// 被中断
        /// </summary>
        public UnityAction BreakEvent;

        /// <summary>
        /// 播放完成
        /// </summary>
        public UnityAction FinishEvent;

        /// <summary>
        /// 多段Cutscene 结束后的完成
        /// </summary>
        public UnityAction MultCutsceneFinishEvent;

        [LabelText("动画过渡时间")]
        public float TransTime;

        private AnimationClipPlayable clipPlayable0;
        private AnimationClipPlayable clipPlayable1;
        private AnimationClip curClip;
        private Cutscene curCutscene;
        private bool isCross = false;
        private bool isLoopCutscene;
        private AnimationClip lastClip;
        private AnimationMixerPlayable mixerPlayable;
        private float offsetTime = 0;
        private PlayableGraph playableGraph;
        private GameObject RoleActionCutscene;
        private float time;
        private float weight;

        public Cutscene CurCutscene
        {
            get
            {
                return curCutscene;
            }
            set
            {
                SetNewCutscene(value);
            }
        }

        private void Awake()
        {
            RoleActionCutscene = new GameObject("ActionCutscene");
            RoleActionCutscene.transform.SetParent(transform, false);
        }

        private void Corss(AnimationClip lastAnim, float StopTime, AnimationClip cutAnim, float StartTime)
        {
            //获取上一个状态的 播放时间，为融合动作做准备
            //offsetTIme = lastplayCutscene.offsetTime;

            // 创建该图和混合器，然后将它们绑定到 Animator

            var Animator = GetComponent<Animator>();

            playableGraph = PlayableGraph.Create();

            var playableOutput = AnimationPlayableOutput.Create(playableGraph, "Animation", Animator);

            mixerPlayable = AnimationMixerPlayable.Create(playableGraph, 2);

            playableOutput.SetSourcePlayable(mixerPlayable);

            // 创建 AnimationClipPlayable 并将它们连接到混合器。

            clipPlayable0 = AnimationClipPlayable.Create(playableGraph, lastAnim);

            clipPlayable1 = AnimationClipPlayable.Create(playableGraph, cutAnim);

            //加上两个动作的偏移
            clipPlayable0.SetTime(StopTime);

            clipPlayable1.SetTime(StartTime);

            playableGraph.Connect(clipPlayable0, 0, mixerPlayable, 0);

            playableGraph.Connect(clipPlayable1, 0, mixerPlayable, 1);

            clipPlayable1.Pause();//动画过渡融合的关键，停掉第二个，保证动画过渡到第二个动画第一帧

            //播放该图。
            playableGraph.Play();
            Debug.Log("开始融合");
        }

        private void SetNewCutscene(Cutscene value)
        {
            //Cutscene之间的动画融合

            float stopTime = 0;
            if (curCutscene != null)
            {
                //销毁旧的
                if (BreakEvent != null)
                {
                    BreakEvent();
                }
                //上一个动画片段
                lastClip = curCutscene.GetCutsceneClip<PlayAnimPlayableClip>()[0].animationClip;
                //上一个动画结尾
                stopTime = curCutscene.currentTime;
                TransTime = curCutscene.GetCutsceneClip<PlayAnimPlayableClip>()[0].CorssTransTime;
                curCutscene.Stop();
                GameObject.Destroy(curCutscene.gameObject);
            }

            BreakEvent = null;
            FinishEvent = null;
            //播放新的
            curCutscene = value;
            if (curCutscene.defaultWrapMode == Cutscene.WrapMode.Loop)
            {
                isLoopCutscene = true;
            }
            else
            {
                isLoopCutscene = false;
            }
            curCutscene.updateMode = Cutscene.UpdateMode.Manual;
            time = 0;
            //当前动画片段
            curClip = curCutscene.GetCutsceneClip<PlayAnimPlayableClip>()[0].animationClip;
            var starTime = curCutscene.GetCutsceneClip<PlayAnimPlayableClip>()[0].StarOffsetTime;
            curCutscene.transform.SetParent(RoleActionCutscene.transform, false);

            //判断是否需要融合
            if ((lastClip != null) && (curClip != null) && (TransTime >= 0))
            {
                isCross = true;
                Corss(lastClip, stopTime, curClip, starTime);
            }
            else
            {
                //不需要融合，播放Cutscene
                isCross = false;
                curCutscene.Play();
            }
        }

        //#region 手动改帧间隔控制速度

        //private void Update()
        //{
        //    if (isCross)
        //    {   //动作融合
        //        //error 需要从节点Cutscene 获取cutscene 播放速度，
        //        //因为有攻击cutscene ,有移动 ，播放速度由节点从PropertySystem获取
        //        time += this.GetAddComponent<PropertySystem>().LogicDeltaTime;//* this.GetAddComponent<PropertySystem>().AttackSpeed;

        //        weight = Mathf.Lerp(0, 1, time / TransTime);

        //        //clipPlayable0.SetTime(time + offsetTime);

        //        //clipPlayable1.SetTime();

        //        mixerPlayable.SetInputWeight(0, 1.0f - weight);

        //        mixerPlayable.SetInputWeight(1, weight);
        //        //
        //        if (Mathf.Abs(weight - 1) < 0.0001)
        //        {
        //            Debug.Log("融合完成  偏移时间" + offsetTime);
        //            playableGraph.Stop();
        //            isCross = false;
        //            //使cutscene 从0开始播放，计时重置,融合到下一个Cutscene 第一帧
        //            time = 0;
        //            curCutscene.Play();
        //        }
        //    }

        //    if (!isCross)
        //    {
        //        time += this.GetAddComponent<PropertySystem>().LogicDeltaTime;/** this.GetAddComponent<PropertySystem>().AttackSpeed*/;

        //        if (isLoopCutscene)
        //        {
        //            CurCutscene.Sample(time % CurCutscene.length);
        //        }
        //        else
        //        {
        //            if (time >= CurCutscene.length)
        //            {
        //                CurCutscene.Stop();
        //                if (MultCutsceneFinishEvent != null)
        //                {
        //                    MultCutsceneFinishEvent();
        //                    MultCutsceneFinishEvent = null;
        //                }
        //                if (FinishEvent != null)
        //                {
        //                    FinishEvent();
        //                }
        //            }
        //            else
        //            {
        //                CurCutscene.Sample(time);
        //            }
        //        }
        //    }
        //}

        //#endregion 手动改帧间隔控制

        #region FixedUpdate补帧
        private void FixedUpdate()
        {
            if (isCross)
            {   //动作融合
                //error 需要从节点Cutscene 获取cutscene 播放速度，
                //因为有攻击cutscene ,有移动 ，播放速度由节点从PropertySystem获取
                time += this.GetAddComponent<PropertySystem>().FixedDeltaTime;//* this.GetAddComponent<PropertySystem>().AttackSpeed;

                weight = Mathf.Lerp(0, 1, time / TransTime);

                //clipPlayable0.SetTime(time + offsetTime);

                //clipPlayable1.SetTime();

                mixerPlayable.SetInputWeight(0, 1.0f - weight);

                mixerPlayable.SetInputWeight(1, weight);
                //
                if (Mathf.Abs(weight - 1) < 0.0001)
                {
                    Debug.Log("融合完成  偏移时间" + offsetTime);
                    playableGraph.Stop();
                    isCross = false;
                    //使cutscene 从0开始播放，计时重置,融合到下一个Cutscene 第一帧
                    time = 0;
                    curCutscene.Play();
                }
            }

            if (!isCross)
            {
                time += this.GetAddComponent<PropertySystem>().FixedDeltaTime;/** this.GetAddComponent<PropertySystem>().AttackSpeed*/;

                if (isLoopCutscene)
                {
                    CurCutscene.Sample(time % CurCutscene.length);
                }
                else
                {
                    if (time >= CurCutscene.length)
                    {
                        CurCutscene.Stop();
                        if (MultCutsceneFinishEvent != null)
                        {
                            MultCutsceneFinishEvent();
                            MultCutsceneFinishEvent = null;
                        }
                        if (FinishEvent != null)
                        {
                            FinishEvent();
                        }
                    }
                    else
                    {
                        CurCutscene.Sample(time);
                    }
                }
            }
        }
        #endregion
    }
}