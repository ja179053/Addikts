using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCutscene : Cutscene {
    public int idNumber;
    public SubtitlesOnKey subonkey;
    protected override bool Setup()
    {
        myFunction = string.Format("DialogueCutscene{0}",idNumber);
        subonkey.Action();
        return base.Setup();
    }
}
