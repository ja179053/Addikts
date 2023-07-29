using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Convo : EventMain {

    public Message[] conversation;
    protected override bool Setup()
    {
        if (conversation.Length == 0)
        {
            Debug.LogWarning(
                "You forgot to setup speech in TextOnShop");
        }
        return base.Setup();
    }
    protected override void EventCompleted()
    {
        for (int i = 0; i < conversation.Length; i++)
        {
            Character.subtitles.AddMessage(conversation[i].message, conversation[i].time,
                conversation[i].sound);
        }
        base.EventCompleted();
    }
}
