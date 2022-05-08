using UnityEngine;

namespace Battle
{
    public class SystemMonoBehaviour : MonoBehaviour
    {
        //System 负责循环处理 更新状态 与数据
        //Manager 负责添加、删除对象
        internal void OnValidate()
        {
            //移动脚本到末尾
            while (UnityEditorInternal.ComponentUtility.MoveComponentDown(this))
            {
            }
        }
    }
}