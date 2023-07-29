using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectColourLerp : ColourLerp {
    Renderer r;
    // Use this for initialization
    void Start()
    {
        r = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update () {
        r.material.color = NewColour(r.material.color);
    }
}
