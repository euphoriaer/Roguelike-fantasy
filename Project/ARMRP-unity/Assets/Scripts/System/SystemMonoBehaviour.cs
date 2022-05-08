using UnityEngine;

namespace Battle
{
    internal class SystemMonoBehaviour : MonoBehaviour
    {
        internal void OnValidate()
        {
            //移动脚本到末尾
            while (UnityEditorInternal.ComponentUtility.MoveComponentDown(this))
            {
            }
        }
    }
}