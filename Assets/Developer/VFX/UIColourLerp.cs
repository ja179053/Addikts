using UnityEngine;
using UnityEngine.UI;
using TMPro;
[ExecuteInEditMode]
public class UIColourLerp : ColourLerp
{
    TextMeshProUGUI t;
    // Use this for initialization
    void Start()
    {
        t = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        t.color = NewColour(t.color);
    }
}
