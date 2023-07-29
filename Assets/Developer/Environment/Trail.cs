using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : Hover {
	// Update is called once per frame
	void Update () {
        if (target == null) {
            AssignTarget();
            transform.position = target.position;
        }
        if(Vector3.Distance(transform.position, target.position) < 1)
        {
            AssignTarget();
        }
	}
    void AssignTarget()
    {
        int i = Random.Range(0, Stars.stars.Count);
        target = Stars.stars[i].transform;
    }
}
