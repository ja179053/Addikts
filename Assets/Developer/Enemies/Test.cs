using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIAdvanced;

public class Test : Enemy2 {
    Renderer r;
    AI finalTarget;
	// Update is called once per frame
	protected override void Update () {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Damage(1, myCharacter, myType);
            if(r == null)
            {
                r = GetComponent<Renderer>();
            }
            r.material.color = Color.red;
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            StartCoroutine(ChargeAttack());
        }
        if(finalTarget != currentTarget)
        {
            finalTarget = currentTarget;
            Debug.Log(finalTarget);
        }
        base.Update();
    }
    protected override void AddParameters()
    {
        Condition<float> c = new LessThan(healAt);
        healthParameters.Add(new HealthParameter(c, Heal()));
        actions.Add(ValidateMyHP());
    }/*
    protected override IEnumerator ChargeAttack()
    {
        Debug.Log("charging is actually being called");
        yield return new WaitForSeconds(0);
        Intelligence();
    }*/

}
