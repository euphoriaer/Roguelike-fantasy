using Battle.UI;
using System.Collections.Generic;
using UnityEngine;

public class CardPanel : UIPanel
{
    
    private List<GameObject> cards=new List<GameObject>();

    private void Start()
    {
        

    }

    public void AddCard(Card card)
    {
        card.ShowPanel();
    }

    public override void HidePanel()
    {
        
    }

    public override UIPanel ShowPanel()
    {
        //todo 通过MonsterManager 获取当前所有怪物，时停
        return this;
    }

    public override void DestoryPanel()
    {
        base.DestoryPanel();
        //选牌结束，取消时停
    }
}