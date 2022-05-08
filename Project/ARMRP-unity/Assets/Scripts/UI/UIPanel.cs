using Battle;
using UnityEngine;

namespace Battle.UI
{

    public abstract class UIPanel : UISystem
    {
        //UI 内部不需要时序，只要保证UISystem的顺序即可

        public abstract string Name { get; }

        public abstract UIPanel ShowPanel();

        public abstract void HidePanel();

        public abstract void DestoryPanel();
    }
}