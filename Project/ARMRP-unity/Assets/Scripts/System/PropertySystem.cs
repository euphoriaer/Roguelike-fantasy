using UnityEngine;

namespace Battle
{
    public class PropertySystem : MonoBehaviour
    {
        public static int PlayerLayer = 6;
        public static int EnemyLayer = 8;

        public string Name;

        public int Blood;
        public int MaxBlood;

        public int Blue;
        public int MaxBlue;

        public int Attack;

        public AnimationClip LastPlayClip;

        public float curPlayClipOffset;

        

        //public void OnCollisionEnter(Collision collision)
        //{

        //}

       
    }
}