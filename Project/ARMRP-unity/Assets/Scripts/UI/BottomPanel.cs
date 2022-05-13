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
        private GameObject blueNumSignObj;
        public override string Name
        {
            get
            {
                return "BottomPanel";
            }
        }

        public void Update()
        {
            //血量相关，增大最大血量会 长度不会增加，每次扣血会减少
            //todo , 改为扣血不变，长度根据最大血量增加
            BloodSlider.maxValue = property.MaxBlood;
            BloodSlider.value = property.CurBlood;

            //蓝量相关，根据气槽回复速度，不停增长，满格后增加1标记，再重新计算气槽
            property.CurBlue = property.CurBlue+property.BuleAddSpeed*0.01f;
            BlueSlider.value=property.CurBlue;


            if (BlueSlider.value>=BlueSlider.maxValue)
            {

                if (property.CurBlueSignNum == property.MaxBlueSignNum)
                {
                    //达到最大标记，停止气槽的增长
                    property.CurBlue = BlueSlider.maxValue;
                }
                else
                {
                    property.CurBlue = 0;
                }
                property.CurBlueSignNum += 1;
                property.CurBlueSignNum = property.CurBlueSignNum >= property.MaxBlueSignNum ? property.MaxBlueSignNum: property.CurBlueSignNum;
                
            }


            int i = 0;
            //有多少个蓝标就创建多少个
            for (; i < property.CurBlueSignNum; i++)
            {

                if (i > blueNumSignObj.transform.childCount - 1)
                {//蓝标数量大于 标记物体数量
                 //UIManager.Instate.CreateGizmos("BlueSign");
                    GameObject.Instantiate(UIManager.Instate.CreateGizmos("BlueSign"), blueNumSignObj.transform);
                }
                else
                {//蓝标数量小于/等于 标记物体数量 直接设置为True 即可
                    blueNumSignObj.transform.GetChild(i).gameObject.SetActive(true);
                }
            }
            int j = blueNumSignObj.transform.childCount;
            //多出的标记隐藏
            for (; j > property.CurBlueSignNum; j--)
            {
                blueNumSignObj.transform.GetChild(j - 1).gameObject.SetActive(false);
            }

        }

        private void UpdateBlueSign()
        {

        }

        public void Start()
        {
            BloodSlider = this.transform.Find("BloodSlider").GetComponent<Slider>();
            BlueSlider = this.transform.Find("BlueSlider").GetComponent<Slider>();
            blueNumSignObj = this.transform.Find("BlueNumber").gameObject;
            player = GameObject.Find("Player");
            property = player.GetComponent<PropertySystem>();
            BlueSlider.maxValue = 100;//气槽固定 0-100，不需要增加长度
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