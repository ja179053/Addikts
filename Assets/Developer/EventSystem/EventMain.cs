using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class EventMain : EventManager
{
    protected bool setup;
    public GameObject[] activateOnEventCompleted;
    // Use this for initialization
    void Start()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, UnityEvent>();
        }
        if (!Setup())
        {
         //   Debug.Break();
        }
    }
    protected virtual bool Setup()
    {
        Activation(false);
        listener = new UnityAction(EventCompleted);
        StartListening(myFunction, listener);
        setup = true;
        return true;
    }
    protected void Activation(bool b)
    {
        foreach (GameObject g in activateOnEventCompleted)
        {
            if (g != null)
            {
                g.SetActive(b);
            }
        }
    }
}
