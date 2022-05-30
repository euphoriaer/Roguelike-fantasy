using Slate;
using UnityEngine;
using UnityEngine.Events;

namespace Battle
{
    [UnityEngine.DisallowMultipleComponent]
    [UnityEngine.DefaultExecutionOrder(SystemOrder.CutsceneSystem)]
    [UnityEngine.AddComponentMenu("System/CutsceneSystem")]
    public class CutsceneSystem : SystemMonoBehaviour
    {
        private GameObject RoleActionCutscene;

        private Cutscene curCutscene;
        private float time;
        private bool isLoopCutscene;

        /// <summary>
        /// 播放完成
        /// </summary>
        public UnityAction FinishEvent;
        /// <summary>
        /// 被中断
        /// </summary>
        public UnityAction BreakEvent;
        /// <summary>
        /// 多段Cutscene 结束后的完成
        /// </summary>
        public UnityAction MultCutsceneFinishEvent;
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

        private void SetNewCutscene(Cutscene value)
        {
            if (curCutscene != null)
            {
                //销毁旧的
                if (BreakEvent != null)
                {
                    BreakEvent();
                }

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
            curCutscene.Play();
            curCutscene.transform.SetParent(RoleActionCutscene.transform, false);
        }

        private void Awake()
        {
            RoleActionCutscene = new GameObject("ActionCutscene");
            RoleActionCutscene.transform.SetParent(transform, false);
        }

        private void Update()
        {
            //error 动作融合
            time += this.GetAddComponent<PropertySystem>().DeltaTime;
            if (isLoopCutscene)
            {
              CurCutscene.Sample(time % CurCutscene.length);
            }
            else
            {
                CurCutscene.Sample(time);
                if (time >= CurCutscene.length)
                {
                    CurCutscene.Stop();
                    if (MultCutsceneFinishEvent!=null)
                    {
                        MultCutsceneFinishEvent();
                        MultCutsceneFinishEvent = null;
                    }
                    if (FinishEvent != null)
                    {
                        FinishEvent();
                    }
                }
            }
        }
    }
}