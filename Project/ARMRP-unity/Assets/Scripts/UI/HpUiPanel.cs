using Battle;
using Battle.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpUiPanel : UIPanel
{
    private Image HpImage;
    private PropertySystem property;
    public float Fill = 0.1f;
   
    [Range(0, 1)]
    public float vaule;
    private void Start()
    {
        property = this.transform.GetComponentInParent<PropertySystem>();
        HpImage =transform.GetChild(0).GetComponent<Image>();
    }
    private void Update()
    {
        vaule = (float)property.CurBlood/ property.MaxBlood;
        Debug.Log(vaule);
        HpImage.fillAmount = vaule;
        //ÑªÁ¿³¯ÏòÉãÏñ»ú
        this.transform.forward = Camera.main.transform.forward;
    }

    public override UIPanel ShowPanel()
    {
        return this;
    }

    public override void HidePanel()
    {
        
    }
}
