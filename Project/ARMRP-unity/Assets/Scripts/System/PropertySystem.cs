using Sirenix.OdinInspector;
using Slate;
using UnityEngine;

namespace Battle
{
    [UnityEngine.DisallowMultipleComponent]
    [UnityEngine.DefaultExecutionOrder(SystemOrder.PhysicsSystem)]
    [UnityEngine.AddComponentMenu("System/PropertySystem")]
    internal class PropertySystem : SystemMonoBehaviour
    {
        public static float MathfDeltatime;

        public string Name;

        [LabelText("攻击力")]
        public int Attack;

        [LabelText("当前血量")]
        public int CurBlood;

        [LabelText("最大血量")]
        public int MaxBlood;

        /// <summary>
        /// 当前气槽进度
        /// </summary>
        [LabelText("当前气槽进度")]
        public float CurBlue;
        /// <summary>
        /// 当前蓝标存储数量
        /// </summary>
        [LabelText("当前蓝标存储数量")]
        public int CurBlueSignNum;
        /// <summary>
        /// 最大蓝标存储数量
        /// </summary>
        [LabelText("最大蓝标存储数量")]
        public int MaxBlueSignNum;

        /// <summary>
        /// 气槽回复速度，满格后存储+1
        /// </summary>
        [LabelText("气槽回复速度")]
        public float BuleAddSpeed = 10;


        [LabelText("攻击速度")]
        public float AttackSpeed;

        public float curPlayClipOffset;

        public AnimationClip LastPlayClip;

        public float FixedDeltaTime=0.01f;

        public float TimeScale=1f;
        
  
        public void LateUpdate()
        {
            //DeltaTime=UnityEngine.Time.deltaTime;
            //DeltaTime= FrameDtime;

            //Todo LogicDeltaTime 需要根据渲染帧进行计算，本次应该是多少,
            // 即计算帧间长度，渲染帧小，长度需加大，渲染帧大，长度需减小->补帧
            // 补帧方式：
            // 1多跑几次，使用 UnityFixedUpdate 每帧50次  不够自动补，优点：简单方便，适用于Unity 缺点：无法定制，漏帧等待导致Bug
            // 2帧间隔变化 ，使用Update，根据设置帧率跑，手动,根据渲染帧计算帧间长度，优点：可控性高，缺点：较复杂，需手写碰撞


            //总结：
            //1.要么锁定间隔，通过函数调用次数控制补帧，UnityFixedUpdate
            //2.要么动态间隔，通过计算帧长度平滑，Update,长度计算方式：FPS：
            //m_frameDeltaTime = (Time.realtimeSinceStartup - m_lastUpdateShowTime) / m_frames;
            //FixedDeltaTime  = MathfDeltatime;
        }
    }
}