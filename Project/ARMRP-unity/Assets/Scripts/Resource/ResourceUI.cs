using Battle;
using Battle.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
//using UI;

namespace Battle.Resource
{
    public class ResourceUI: BaseManager<ResourceUI>
    {
        public  UIPanel LoadPanel(string name)
        {
            string path = "Prefabs/UI/Panel/"+ name;
            
            return ResourceManager.Instate.Load<UIPanel>(path);
        }

        public GameObject LoadGizmo(string gizmoName)
        {
            string path = "Prefabs/UI/Gizmos/" + gizmoName;
            return ResourceManager.Instate.Load<GameObject>(path);
        }

        public void Unload(UnityEngine.Object uiPanel)
        {
            ResourceManager.Instate.Unload(uiPanel);
        }
        
    }
}
