using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Messenger : MonoBehaviour
{
    Messenger thisMessenger;
    AudioSource aso;
    public bool debugMessages, changeColour;
    /// Class to add messages to the screen
    TextMeshProUGUI screenMessages;
    string myName;
    void Start()
    {
        if (myName == null)
        {
            myName = gameObject.name;
        }
        thisMessenger = GameObject.Find(myName).GetComponent<Messenger>();
           messages = new List<Message>();
        screenMessages = thisMessenger.GetComponent<TextMeshProUGUI>();
        screenMessages.text = "";
        aso = thisMessenger.GetComponent<AudioSource>();
        if (aso == null)
        {
            aso = thisMessenger.gameObject.AddComponent<AudioSource>();
        }
    }
    bool playing = false;
    List<Message> messages;
    public void AddMessage(string message, float time, AudioClip clip = null, Color? col = null)
    {
        //Clears the clip if duplicate
        if (clip != null && messages.Count > 0 && messages[messages.Count - 1].sound != null && messages[messages.Count - 1].sound == clip)
        {
            clip = null;
        }
        //Adds the message if count less than 1 or verified non-duplicate message
        Debug.Log(message + message.Length);
        if (messages.Count > 0)
        {
            Debug.Log(messages[messages.Count - 1].message + messages[messages.Count - 1].message.Length);
            Debug.Log(messages[messages.Count - 1].message.Substring(0, message.Length > 20 ? 20 : message.Length - 4));
        }
        if (messages.Count < 1 || messages[messages.Count - 1].message.Substring(0,message.Length > 20 ? 20 : message.Length - 4) != message.Substring(0, message.Length > 20 ? 20 : message.Length - 4))
        {
            int i = (message.Contains(".")) ? message.IndexOf(".") : message.Length;
            messages.Add(new Message(message.Substring(0,i), time, clip, col));
            if (message.Length - i > 2)
            {
                AddMessage(message.Substring(i + 1, message.Length - i - 2), time, clip, col);
            }
            if (!playing)
            {
                if (thisMessenger == null)
                {
                    Start();
                }
                    thisMessenger.StartCoroutine(ShowMessage());
            }
            if (debugMessages)
            {
                Debug.Log(message);
            }
        }
    }
    IEnumerator ShowMessage()
    {
        if (messages.Count > 0)
        {
            playing = true;
            //   Debug.Log("Now playing messages");
            screenMessages.text = messages[0].message;
            if (changeColour)
            {
                //Will need to change text.colour without text mesh pro
                screenMessages.faceColor = (messages[0].color == null) ? screenMessages.color : (Color)messages[0].color;
            }
            aso.PlayOneShot(messages[0].sound);
            yield return new WaitForSeconds(messages[0].time);
            messages.RemoveAt(0);
            playing = false;
            StartCoroutine(ShowMessage());
            //   Debug.Log("message deleted");
        } else
        {
            screenMessages.text = "";
        }
    }
    void OnDestroy()
    {
    //    Debug.Log("messenger destroyed");
    }
}
[System.Serializable]
public class Message
{
    public string message;
    public float time;
    public AudioClip sound;
    public Color? color;
    public Message(string Message, float Time, AudioClip soundOrEffect, Color? col)
    {
        //THIS IS WHERE A MESSAGE GETS SET UP
        message = Message;
        time = Time;
        if (col != null)
        {
            Color c = (Color)col;
            col = (c.a < 0.5f) ? null : col;
        }
        color = col;
        if (soundOrEffect != null)
        {
            sound = soundOrEffect;
        }
    }
}
