using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugOnEnable : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnEnable()
    {
        if (Application.isEditor)
        {
            Debug.Log("enabled " + name);
        } else
        {
            Destroy(this);
        }
    }
}
