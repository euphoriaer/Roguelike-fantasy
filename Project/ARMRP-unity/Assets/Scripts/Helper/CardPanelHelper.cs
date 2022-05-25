using Battle.UI;
using UnityEngine;

namespace Battle
{
    
    public static class CardPanelHelper
    {
        public static void Open(params int[] CardID)
        {

            var panel = UIManager.Instate.CreatePanel<CardPanel>("CardPanel");
            for (int i = 0; i < CardID.Length; i++)
            {
                panel.AddCard(CardID[i]);
            }
        }
   
    }
}