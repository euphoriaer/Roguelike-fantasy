using UnityEngine;

namespace Battle
{
    public class Property:MonoBehaviour
    {
        public string Name;

        public int Blood;

        public int Attack;

        public Vector3 m_Position;

        public AnimationClip LastPlayClip;
        public float curPlayClipOffset;
    }
}
