using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshMove : MonoBehaviour {
    NavMeshAgent nma;
    public Vector3 pos;
    public Transform target;
    public bool input;
    public string playerNumber;
	// Use this for initialization
	void Start () {
        nma = GetComponent<NavMeshAgent>();
        //Dude just add the nav mesh agent
        ///THE SCRIPT IS CALLED NAV MESH MOVE
        if (nma == null)
        {
            nma = gameObject.AddComponent<NavMeshAgent>();
        }
        if(target != null)
        {
            pos = target.localPosition;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (input)
        {
            pos = Vector3.right * Input.GetAxis(playerNumber);
        }
        nma.SetDestination(transform.position + pos);
	}
    //what the hell was i doing
    /// <summary>
    /// 1 the thing was following itself
    /// 2 that thing was too fast
    /// 3  now it should be fine, but unity crashed 
    /// 4 now it is time to...
    /// </summary>
    /// <param name="f"></param>
    public void SetSpeed(float f)
    {
        nma.speed = nma.acceleration = f;
    }
}
