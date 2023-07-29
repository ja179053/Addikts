using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    public Slider healthBar;
    public Image handle, fill;
    public Sprite[] handles;
    public Sprite[] bars;
    Color[] healthColor =
    {
      Color.red,
      Color.yellow,
      Color.green
    };
    public float speed = 1, minValue = 0.01f;
    public void SetHealth(int maxHealth)
    {
        healthBar.value = healthBar.maxValue = health = maxHealth;
        UpdateHealth(maxHealth);
    }
    float health;
    public void UpdateHealth(int h)
    {
        health = h;
        //  healthBar.value = health;
        float i = (int)(health / healthBar.maxValue);
        handle.sprite = handles[(int)Mathf.Round(i * (handles.Length - 1))];
        //    Debug.Log("Handle " +(int)i * (handles.Length - 1));
        int j = (int)Mathf.Round(i * (bars.Length - 1));
        fill.sprite = bars[j];
        fill.color = healthColor[j];
        //   Debug.Log("Fill " + (int)i * (bars.Length - 1));
    }
    void Update()
    {
        if (healthBar.value + minValue < health || healthBar.value - minValue > health)
        {
            healthBar.value = Mathf.Lerp(healthBar.value, health, Time.deltaTime * speed);
            //   Debug.Log(healthBar.value - health);
        }
    }
}
