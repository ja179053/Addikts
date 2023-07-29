using System.Collections.Generic;
using UnityEngine;

public class Stars : MonoBehaviour {
    [Range(1, 10000)]
    public int radius;
    [Range(0,10000)]
    public int joiners;
    [Range(2,10000)]
    public int density;
    public GameObject star, trailRenderer;
    public static List<GameObject> stars;
	// Use this for initialization
	void Start () {
        stars = new List<GameObject>();
        for (int i = 0; i < density; i++)
        {
            stars.Add(GameObject.Instantiate(star, Functions.RandomVector3(radius), Quaternion.identity));
        }
        TrailRenderer[] renders = new TrailRenderer[joiners];
        for(int i = 0; i < joiners; i++)
        {
            renders[i] = GameObject.Instantiate(
                trailRenderer, transform.position, Quaternion.identity).
                GetComponent<TrailRenderer>();
        }
	}
}
