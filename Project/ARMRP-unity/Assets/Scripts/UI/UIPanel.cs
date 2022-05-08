using UnityEngine;

namespace UI
{
    public abstract class UIPanel : MonoBehaviour
    {
        public abstract string Name { get; }

        public abstract UIPanel ShowPanel();

        public abstract void HidePanel();

        public abstract void DestoryPanel();

        internal abstract void UpdatePanel();

        internal abstract void StarPanel();
    }
}