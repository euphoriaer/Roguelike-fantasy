using Battle.UI;
using System.Collections.Generic;

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

    public void AddCard(Card card)
    {
        card.Show();
        cards.Add(card);
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