using Battle.UI;
using System.Collections.Generic;
using UnityEngine;

public class CardPanel : UIPanel
{
    private List<Card> cards = new List<Card>();

    private void Start()
    {
    }

    public void Update()
    {
        foreach (var card in cards)
        {
            if (card.isEnable)
            {
                card.Update();
            }
        }
    }

    public void AddCard(int cardID)
    {
        GameObject gameobject = this.gameObject;
        Card card = new Card(cardID, gameobject);
        card.Show();
        cards.Add(card);
    }

    public void ClearCard()
    {
        //todo 卡牌销毁

        cards.Clear();
    }

    public override void HidePanel()
    {
        this.gameObject.SetActive(false);
    }

    public override UIPanel ShowPanel()
    {
        //todo 通过MonsterManager 获取当前所有怪物，时停
        this.gameObject.SetActive(true);
        return this;
    }

    public override void DestoryPanel()
    {
        base.DestoryPanel();
        //选牌结束，取消时停
    }
}