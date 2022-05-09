﻿using Sirenix.OdinInspector;
using UnityEngine;

namespace Battle
{
    [UnityEngine.DisallowMultipleComponent]
    [UnityEngine.DefaultExecutionOrder(SystemOrder.PhysicsSystem)]
    [UnityEngine.AddComponentMenu("System/PropertySystem")]
    internal class PropertySystem : SystemMonoBehaviour
    {
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

        [LabelText("移动速度")]
        public int MoveSpeed;

        [LabelText("攻击速度")]
        public float AttackSpeed;

        public float curPlayClipOffset;

        public AnimationClip LastPlayClip;
        //public void OnCollisionEnter(Collision collision)
        //{
        //}
    }
}