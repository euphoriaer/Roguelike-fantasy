using System;
using Battle;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BottomPanel : MonoBehaviour
    {
        private Slider BloodSlider;
        private Slider BlueSlider;

        private GameObject player;
        private PropertySystem property;

        public void Start()
        {
            BloodSlider = this.transform.Find("BloodSlider").GetComponent<Slider>();
            BlueSlider = this.transform.Find("BlueSlider").GetComponent<Slider>();
            player = GameObject.Find("Player");
            property = player.GetComponent<PropertySystem>();
        }

        private void Update()
        {
            BloodSlider.maxValue = property.MaxBlood;
            BloodSlider.value = property.Blood;
            BlueSlider.maxValue = property.MaxBlue;
            BlueSlider.value = property.Blue;
        }
    }
}