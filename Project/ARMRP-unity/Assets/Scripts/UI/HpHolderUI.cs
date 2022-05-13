using Battle;
using Battle.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpHolderUI : UIPanel
{
    public GameObject player;
    public Transform HpHolder;
    private Image HpImage;
    private PropertySystem property;
    [Range(0, 1)]
    public float vaule;
    private void Start()
    {
        //player = GameObject.Find("player1");
        //property = player.GetComponent<PropertySystem>();
        //HpImage =transform.GetChild(0).GetComponent<Image>();
    }
    private void Update()
    {
        //vaule = (float)property.CurBlood/ property.MaxBlood;
        //HpImage.fillAmount = vaule;
        
    }

    public override UIPanel ShowPanel()
    {
        return this;
    }

    public override void HidePanel()
    {
        
    }
}
