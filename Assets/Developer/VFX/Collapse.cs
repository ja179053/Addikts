using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collapse : MonoBehaviour {
    public GameObject cloth;
    public float upDistance= 0.25f;
	// Update is called once per frame
	void OnTriggerStay (Collider c) {
        if (c.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            GameObject.Instantiate(cloth, transform.position + (Vector3.up * upDistance), Quaternion.identity);
            Destroy(this.gameObject);
        }
	}
}
