using UnityEngine;

public class SetRot : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        transform.rotation = 
            Quaternion.Euler(new Vector3(0, 0, 0));
	}
}
