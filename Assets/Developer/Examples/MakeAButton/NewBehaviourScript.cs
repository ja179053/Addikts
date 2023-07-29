using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
    public static int height = 50;
	// Update is called once per frame
	void Update () {
        int i = Screen.width / 27;
        int j = Screen.height / height;
        transform.position = Input.mousePosition + new Vector3(i, j, 0);
	}
    public void DoSomething()
    {
        Debug.Log("I am doing something");
    }
    //Event theat Unity recognises (just like Update)
    void OnGUI()
    {
        if(GUI.Button(new Rect(10,10,150,150),"I am doing something else"))
        {
            Debug.Log("I am doing something else");
        }
    }
}
