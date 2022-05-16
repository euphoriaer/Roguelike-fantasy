using Battle.UI;
using UnityEngine;

public class Card : UIPanel
{
    private GameObject card;

    public Card(int cardId, GameObject parent)
    {
        //读取Json,获取ID;
        card = GameObject.Instantiate(UIManager.Instate.CreateGizmos("Card"), parent.transform);
       
        //error 通过ID 读取图片路径，替换
        //读取描述
        //增加按下事件 buff

    }

    public override void HidePanel()
    {
        card.SetActive(false);
    }

    public override UIPanel ShowPanel()
    {
        card.SetActive(true);
        //error 旋转动画
        card.transform.rotation = Quaternion.EulerAngles(new Vector3(0,0,0));
        return this;
    }
}