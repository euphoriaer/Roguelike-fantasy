using UnityEngine;

namespace Battle
{
    [UnityEngine.DisallowMultipleComponent]
    [UnityEngine.DefaultExecutionOrder(SystemOrder.PhysicsSystem)]
    [UnityEngine.AddComponentMenu("System/PropertySystem")]
    public class PropertySystem : MonoBehaviour
    {
       

        public int Attack;

        public int Blood;

        public int Blue;

        public float curPlayClipOffset;

        public AnimationClip LastPlayClip;

        public int MaxBlood;

        public int MaxBlue;

        public string Name;

        private void OnValidate()
        {
            //移动脚本到末尾
            while (UnityEditorInternal.ComponentUtility.MoveComponentDown(this))
            {

            }

        }
        //public void OnCollisionEnter(Collision collision)
        //{
        //}
    }
}