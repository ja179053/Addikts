using UnityEngine;

public class Cutscene : Convo
{
    //Has to be attached to a character
    protected override bool Setup()
    {
        myFunction = string.Format("Cutscene{0}", name);
        return base.Setup();
    }
}
