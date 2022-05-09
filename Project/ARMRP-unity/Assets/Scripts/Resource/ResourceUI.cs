using Battle;
using Battle.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using UI;

namespace Battle.Resource
{
    public class ResourceUI: BaseManager<ResourceUI>
    {
        public  UIPanel LoadPanel(string name)
        {
            //
            return ResourceManager.Instate.Load<UIPanel>(name);
        }

        public void Unload(UnityEngine.Object uiPanel)
        {
            ResourceManager.Instate.Unload(uiPanel);
        }
        
    }
}
