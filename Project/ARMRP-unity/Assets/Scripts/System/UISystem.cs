using System.Collections.Generic;
using UI;

namespace Battle
{
    [UnityEngine.DisallowMultipleComponent]
    [UnityEngine.DefaultExecutionOrder(SystemOrder.UiSystem)]
    [UnityEngine.AddComponentMenu("System/UISystem")]
    internal class UISystem : SystemMonoBehaviour
    {
        private List<UIPanel> _uiPanels = new List<UIPanel>();


        private void Start()
        {
           
        }

        private void Update()
        {
            for (int i = 0; i < _uiPanels.Count; i++)
            {
                _uiPanels[i].UpdatePanel();
            }
        }

        public void CeeatePanel(string panelName)
        {
            //todo 通过资源管理器加载Panel,同时 Star 然后添加到Update 中
        }

    }
}