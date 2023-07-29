using UnityEngine.UI;

public class ImageColourLerp : ColourLerp
{
    Image t;
    // Use this for initialization
    void Start()
    {
        t = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        t.color = NewColour(t.color);
    }
}
