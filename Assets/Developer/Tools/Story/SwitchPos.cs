using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPos : MonoBehaviour {
    public string switchKey;
    public Transform[] positions;
    public MoveType moveType;
    int currentPosition = 0;
	// Use this for initialization
	void Start () {
        if(positions.Length == 0)
        {
            Destroy(this);
        }
        MoveTo();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(switchKey))
        {
            switch (moveType)
            {
                case MoveType.Cycle:
                    currentPosition = Functions.Loop(currentPosition + 1, positions.Length);
                    break;
                case MoveType.Random:
                    goto Randomise;
                case MoveType.RandomDifferent:
                    int i = currentPosition;
                    while (i == currentPosition)
                    {
                        goto Randomise;
                    }
                    break;
                    Randomise:
                    currentPosition = Random.Range(0, positions.Length - 1);
                    break;
            }
            MoveTo();
        }
	}
    void MoveTo()
    {
        transform.position = positions[currentPosition].position;
        transform.rotation = positions[currentPosition].rotation;
    }
}
public enum MoveType {
    Cycle,
    Random,
    RandomDifferent
}
