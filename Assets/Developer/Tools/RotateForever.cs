using UnityEngine;

public class RotateForever : MonoBehaviour {
    public Vector3 rotation;
	// Update is called once per frame
	void Update () {
        transform.Rotate(rotation);
	}
}
