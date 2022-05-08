using Resource;
using System.Collections.Generic;
using UI;

namespace Battle
{
    [UnityEngine.DisallowMultipleComponent]
    [UnityEngine.DefaultExecutionOrder(SystemOrder.UiSystem)]
    [UnityEngine.AddComponentMenu("System/UISystem")]
    internal class UISystem : SystemMonoBehaviour
    {
        private Dictionary<string,UIPanel> uiPanelDic = new Dictionary<string, UIPanel>();


        private void Start()
        {
           
        }

        private void Update()
        {
            foreach (var uiPanel in uiPanelDic)
            {
                uiPanel.Value.UpdatePanel();
            }
        }

        public UIPanel CreatePanel(string panelName)
        {
            //先找缓存  面板是否隐藏，没有面板再创建
            if (uiPanelDic.ContainsKey(panelName))
            {
                return uiPanelDic[panelName];
            }

            //todo 通过资源管理器加载Panel,同时 Star 然后添加到Update 中
            UIPanel panel= ResourceUI.LoadPanel(panelName);
            //资源加载是更底层的东西，根据分层理念，外接调用panel panel 再处理加载

            panel.StarPanel();
            uiPanelDic.Add(panelName, panel);
            return panel;
        }

        public T CreatePanel<T>(string panelName) where T : UIPanel
        {
            //先找缓存 面板是否隐藏，没有面板再创建
            if (uiPanelDic.ContainsKey(panelName))
            {
                return (T)uiPanelDic[panelName];
            }

            //todo 通过资源管理器加载Panel,同时 Star 然后添加到Update 中
            UIPanel panel = ResourceUI.LoadPanel(panelName);
            //资源加载是更底层的东西，根据分层理念，外接调用panel panel 再处理加载

            panel.StarPanel();
            uiPanelDic.Add(panelName, panel);
            return (T)panel;
        }

        
    }
}