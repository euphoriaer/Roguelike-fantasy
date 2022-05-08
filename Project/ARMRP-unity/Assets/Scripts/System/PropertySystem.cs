using UnityEngine;

namespace Battle
{
    [UnityEngine.DisallowMultipleComponent]
    [UnityEngine.DefaultExecutionOrder(SystemOrder.PhysicsSystem)]
    [UnityEngine.AddComponentMenu("System/PropertySystem")]
    internal class PropertySystem : SystemMonoBehaviour
    {
        public int Attack;

        public int Blood;

        public int Blue;

        public float curPlayClipOffset;

        public AnimationClip LastPlayClip;

        public int MaxBlood;

        public int MaxBlue;

        public string Name;

        //public void OnCollisionEnter(Collision collision)
        //{
        //}
    }
}