using System.Collections;
using UnityEngine;

public abstract class EventAfterTime : EventMain
{
    //Assign IDNumbers to events
    public int time = 5;
    protected override bool Setup()
    {
        myFunction = "Wait" + time;
        StartCoroutine(Wait());
        //Instruction is set up here
        //Cannot share a canvas
        return base.Setup();
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(time);
        Trigger(myFunction);
    }
    protected override void EventCompleted()
    {
        Activation(true);
        base.EventCompleted();
    }
}
