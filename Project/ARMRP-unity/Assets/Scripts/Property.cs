using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Battle
{
    public class Property:MonoBehaviour
    {
        public string Name;

        public int Blood;
        public int MaxBlood;
        
        public int Blue;
        public int MaxBlue;

        public int Attack;

        public Vector3 m_Position;

        public AnimationClip LastPlayClip;
        public float curPlayClipOffset;

        public Action<Collider> CollisionEnterAction;
        public Action<Collider> CollisionStayAction;
        public Action<Collider> CollisionExitAction;

        private void OnCollisionEnter(Collision collision)
        {
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (CollisionEnterAction==null)
            {
                return;
            }
            CollisionEnterAction(other);
        }

        private void OnTriggerStay(Collider other)
        {
            if (CollisionStayAction==null)
            {
                return;
            }
            CollisionStayAction(other);
        }

        private void OnTriggerExit(Collider other)
        {
            if (CollisionExitAction==null)
            {
                return;
            }
            CollisionExitAction(other);
        }
    }
}
