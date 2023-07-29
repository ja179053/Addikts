using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCutscene : Cutscene {

    public bool damageNotDie;
    protected override bool Setup()
    {
        myFunction = string.Format("Cutscene{0}{1}",
            (damageNotDie) ? "Damage" : "Die", name);
        return base.Setup();
    }
}
