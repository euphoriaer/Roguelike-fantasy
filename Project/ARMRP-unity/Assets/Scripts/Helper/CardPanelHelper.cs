using Battle.UI;
using UnityEngine;

namespace Battle
{
    
    public static class CardPanelHelper
    {
        public static void Open(params int[] CardID)
        {
            
            var mainUi= GameObject.Find("MainUI");//todo 优化方向，游戏一开始查找到所有常用Obj;\
            var paenl = UIManager.Instate.CreatePanel<CardPanel>("CardPanel");
            var cardPanel= GameObject.Instantiate(paenl, mainUi.transform);
            for (int i = 0; i < CardID.Length; i++)
            {
                Card card= new Card(1000, cardPanel.gameObject);
                cardPanel.AddCard(card);
            }
        }
   
    }
}