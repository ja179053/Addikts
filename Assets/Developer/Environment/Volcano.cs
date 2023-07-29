using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volcano : MonoBehaviour {
    public int spawnCount, radius;
    public Vector3 offset, velocity;
    public GameObject spawnObject;
    void OnMouseDown()
    {
        //Volcano
        for(int i =0; i < spawnCount; i++)
        {
            GameObject.Instantiate(
                spawnObject, transform.position + offset,
                Quaternion.identity).GetComponent<Rigidbody>().velocity =
                velocity + Functions.RandomVector3(radius);
        }
        Debug.Log(string.Format("Spawning {0} {1}",spawnCount,spawnObject.name));
    }
}
