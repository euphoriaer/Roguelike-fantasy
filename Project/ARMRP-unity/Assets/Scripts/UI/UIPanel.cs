using Battle;
using UnityEngine;

namespace Battle.UI
{

    public abstract class UIPanel : UISystem
    {
        //UI 内部不需要时序，只要保证UISystem的顺序即可
        public virtual bool isEnabel { 
            get { return this.gameObject.activeSelf; } 
        }
        public virtual string Name { get; }

        public abstract UIPanel ShowPanel();


        public abstract void HidePanel();
      

        public virtual void DestoryPanel()
        {
            GameObject.Destroy(this.gameObject);
            //error 需要测试是否需要先Destory //Destory 与 Resource.Unload 的关系？

            UIManager.Instate.DestoryPanel(this);
        }
    }
}