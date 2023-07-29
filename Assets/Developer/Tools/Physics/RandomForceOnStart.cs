using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomForceOnStart : ForceOnStart {
    public int randomRadius;
	// Use this for initialization
	void Start () {
        RandomForce();
	}
    protected void RandomForce()
    {
        Force(Functions.RandomVector3YPlus(randomRadius) + force);

    }

}
