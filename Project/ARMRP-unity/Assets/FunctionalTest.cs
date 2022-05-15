using Battle;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FunctionalTest : MonoBehaviour
{
    public Button CardButton;
    // Start is called before the first frame update
    void Start()
    {
        CardButton.onClick.AddListener(CardOpen);
    }

    private void CardOpen()
    {
        CardPanelHelper.Open(2000,2001,2002);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
