using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Delegates and Events
///The hardest part to learn about coding in Unity
///This class handles delegates
public class EventManager : MonoBehaviour
{
    protected string myFunction = "";
    protected UnityAction listener;
    public delegate void InputAction();
    //When the player moves, it should "EVENT COMPLETED
    public static event InputAction OnAnything;
    public static Dictionary<string, UnityEvent> eventDictionary;
    // Use this for initialization
    ///Sets up any method
    ///Don't forget to drag EventManager into the scene
    public static void StartListening(string method, UnityAction action)
    {
        if (eventDictionary == null)
        {
            Debug.LogError("Include instruction in Script Execution Order");
        }
        UnityEvent newEvent = null;
        if (eventDictionary.TryGetValue(method, out newEvent))
        {
            newEvent.AddListener(action);
        } else
        {
         //   Debug.LogWarning("Added "+ method);
            newEvent = new UnityEvent();
            newEvent.AddListener(action);
            eventDictionary.Add(method, newEvent);
        }
    }
    //Removes an event from the dictionary
    public static void StopListening(string method, UnityAction action)
    {
        UnityEvent newEvent = ListeningFor(method);
        if (newEvent != null)
        {
            newEvent.RemoveListener(action);
        }
    }
    public static UnityEvent ListeningFor(string method)
    {
        UnityEvent newEvent = null;
        eventDictionary.TryGetValue(method, out newEvent);
        return newEvent;
    }
    //Triggers any event
    public static void Trigger(string method)
    {
        UnityEvent newEvent = HasQuest(method);
        if (newEvent != null)
        {
       //     Debug.Log(method);
            newEvent.Invoke();
            eventDictionary.Remove(method);
        } else
        {
            Debug.LogWarning("Failed to trigger " + method);
        }
    }
    protected virtual void EventCompleted()
    {
        StopListening(myFunction, EventCompleted);
        Debug.Log("MISSION ACCOMPLISHED");
    }
    public static UnityEvent HasQuest(string method)
    {
        UnityEvent newEvent = null;
        eventDictionary.TryGetValue(method, out newEvent);
        return newEvent;
    }
}
