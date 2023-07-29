using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycle : MonoBehaviour {
    public float speed = 1;
    public float baseExposure = 1;
	// Update is called once per frame
	void FixedUpdate () {
        //Because sin waves go from negative to positive, we need to keep it above 0
        float f = 1 + (baseExposure * Mathf.Sin(Time.time * speed));
        RenderSettings.skybox.SetFloat("_Exposure", f);
    }
}
