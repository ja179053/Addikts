using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceOnStart : MonoBehaviour {
    public bool onStart = true;
    public Vector3 force;
	// Use this for initialization
	void Start () {
        if (onStart)
        {
            Force(force);
        }
	}
    protected void Force(Vector3 f)
    {
        GetComponent<Rigidbody>().AddForce(f);
    }
}
