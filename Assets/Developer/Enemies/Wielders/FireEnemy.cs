using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIAdvanced;

public class FireEnemy : Enemy2 {
    protected override void AddParameters()
    {
        Condition<float> c = new LessThan(3);
        healthParameters.Add(new HealthParameter(c, Heal()));
        Condition<float> d = new MoreThan(2);
        magicParameters.Add(new MagicParameter(d, Fire()));
        actions.Add(ValidateMyMP());
        actions.Add(ValidateMyHP());
        magic = attack * 2;
    }
}
