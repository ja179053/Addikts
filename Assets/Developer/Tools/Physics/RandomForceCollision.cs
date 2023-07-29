using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomForceCollision : RandomForceOnStart
{
    void OnCollisionEnter(Collision c)
    {
        Rigidbody r = GetComponent<Rigidbody>();
        r.velocity *= -1.5f;
    }
}
