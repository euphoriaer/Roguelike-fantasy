using Battle;
using Battle.UI;
using UnityEngine;
using UnityEngine.UI;

public class HpUiPanel : UIPanel
{
    private Slider HpSlider;
    private PropertySystem property;
    public float Fill = 0.1f;

    [Range(0, 1)]
    public float vaule;

    private void Start()
    {
        property = this.transform.GetComponentInParent<PropertySystem>();
        HpSlider = transform.GetComponent<Slider>();
    }

    private void Update()
    {
        vaule = (float)property.CurBlood / property.MaxBlood;
        HpSlider.value = vaule;
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