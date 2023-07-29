using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateSliderValue : MonoBehaviour {
    public void Operate()
    {
        Text t = GetComponentInChildren<Text>();
        t.text = GetComponent<Slider>().value.ToString();
    }
}
