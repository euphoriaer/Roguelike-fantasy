using UnityEngine;
using UnityEngine.UI;

namespace Battle.UI
{
    internal class BottomPanel : UIPanel
    {
        private Slider BloodSlider;
        private Slider BlueSlider;

        private GameObject player;
        private PropertySystem property;

        public override string Name
        {
            get
            {
                return "BottomPanel";
            }
        }

        public void Update()
        {
            BloodSlider.maxValue = property.MaxBlood;
            BloodSlider.value = property.Blood;
            BlueSlider.maxValue = property.MaxBlue;
            BlueSlider.value = property.Blue;
        }

        public void Start()
        {
            BloodSlider = this.transform.Find("BloodSlider").GetComponent<Slider>();
            BlueSlider = this.transform.Find("BlueSlider").GetComponent<Slider>();
            player = GameObject.Find("Player");
            property = player.GetComponent<PropertySystem>();
        }

        public override void DestoryPanel()
        {
        }

        public override void HidePanel()
        {
        }

        public override UIPanel ShowPanel()
        {
            //todo ，路径加载,需要资源管理，人/物/ui/场景/地图
            return null;
        }
    }
}