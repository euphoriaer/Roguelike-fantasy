using Battle.Resource;
using System.Collections.Generic;
using UnityEngine;

namespace Battle.UI
{
    public class UIManager : BaseManager<UIManager>
    {
        private Dictionary<string, UIPanel> uiPanelDic = new Dictionary<string, UIPanel>();

        public UIPanel CreatePanel(string panelName)
        {
            //先找缓存  面板是否隐藏，没有面板再创建
            if (uiPanelDic.ContainsKey(panelName))
            {
                return uiPanelDic[panelName];
            }

            UIPanel panel = ResourceUI.Instate.LoadPanel(panelName);
            //资源加载是更底层的东西，根据分层理念，外接调用panel panel 再处理加载

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
            UIPanel panel = ResourceUI.Instate.LoadPanel(panelName);
            //资源加载是更底层的东西，根据分层理念，外接调用panel panel 再处理加载

            uiPanelDic.Add(panelName, panel);
            return (T)panel;
        }

        public GameObject CreateGizmos(string gizmosName)
        {
            return ResourceUI.Instate.LoadGizmo(gizmosName);
        }

        public void DestoryPanel(string panelName)
        {
            GameObject.Destroy(uiPanelDic[panelName]);
            ResourceUI.Instate.Unload(uiPanelDic[panelName]);
        }

        public void DestoryPanel(UIPanel panel)
        {
            GameObject.Destroy(panel);
            ResourceUI.Instate.Unload(panel);
        }
    }
}