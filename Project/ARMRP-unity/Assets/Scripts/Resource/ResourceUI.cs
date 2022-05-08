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
    public class ResourceUI
    {
        public static UIPanel LoadPanel(string name)
        {
            //
            return ResourceManager.Load<UIPanel>(name);
        }
    }
}
