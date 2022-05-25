using Battle.Resource;
using System.Collections.Generic;
using UnityEngine;

namespace Battle.UI
{
    public class UIManager : BaseManager<UIManager>
    {
        private Dictionary<string, UIPanel> uiPanelDic;

        public UIManager()
        {
            uiPanelDic = new Dictionary<string, UIPanel>();
        }

        public UIPanel CreatePanel(string panelName)
        {
            //先找缓存  面板是否隐藏，没有面板再创建
            if (uiPanelDic.ContainsKey(panelName))
            {
                return uiPanelDic[panelName];
            }

            UIPanel panel = ResourceUI.Instate.LoadPanel(panelName);
            //资源加载是更底层的东西，根据分层理念，外接调用panel panel 再处理加载
            var mainUi = GameObject.Find("MainUI");//todo 优化方向，游戏一开始查找到所有常用Obj;
            var cardPanel = GameObject.Instantiate(panel, mainUi.transform);

            uiPanelDic.Add(panelName, cardPanel);
            return cardPanel;
        }

        public UIPanel GetPanel(string panelName)
        {
            return uiPanelDic[panelName];
        }

        public T CreatePanel<T>(string panelName) where T : UIPanel
        {

            //退出，单例缓存未清除？

            //先找缓存 面板是否隐藏，没有面板再创建
            if (uiPanelDic.ContainsKey(panelName))
            {
                return (T)uiPanelDic[panelName];
            }

            //todo 通过资源管理器加载Panel,同时 Star 然后添加到Update 中
            UIPanel panel = ResourceUI.Instate.LoadPanel(panelName);
            //资源加载是更底层的东西，根据分层理念，外接调用panel panel 再处理加载
            var mainUi = GameObject.Find("MainUI");//todo 优化方向，游戏一开始查找到所有常用Obj;
            var cardPanel = GameObject.Instantiate(panel, mainUi.transform);
            cardPanel.name = panelName;
            uiPanelDic.Add(panelName, cardPanel);
            return (T)cardPanel;
        }
     
        public GameObject CreateGizmos(string gizmosName,GameObject parent)
        {
            return GameObject.Instantiate(ResourceUI.Instate.LoadGizmo(gizmosName), parent.transform);
             
        }

        public void DestoryPanel(string panelName)
        {
            uiPanelDic.Remove(panelName);
            GameObject.Destroy(uiPanelDic[panelName]);
        }

        public void DestoryPanel(UIPanel panel)
        {
            uiPanelDic.Remove(panel.name);
            GameObject.Destroy(panel);
        }

        
    }
}