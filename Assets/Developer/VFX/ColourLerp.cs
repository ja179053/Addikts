using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ColourLerp : MonoBehaviour {
    public float leeway = 0.1f, speed = 1;
    public Color start, end;
    private Vector4 target;
    protected void Settings()
    {
        target = end;
    }
	
	// Update is called once per frame
	protected Vector4 NewColour (Color c) {
        Vector4 newColour;
        newColour = Vector4.Lerp(
            c, 
            target, 
            speed * Time.deltaTime);
        if(Vector4.Distance(c, target) < leeway)
        {
            target = (Vector4.Distance(target,start) 
                > Vector4.Distance(target, end))
                ? start : end;
        }
        return newColour;

	}
}
