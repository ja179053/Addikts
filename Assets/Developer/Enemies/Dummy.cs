using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : AI {
    void OnMouseOver()
    {
        List<AI> dummies = new List<AI>();
        dummies.AddRange(FindObjectsOfType<Dummy>());
        Enemy2 enemy = FindObjectOfType<Enemy2>();
        enemy.StartBattle((AI)myCharacter, dummies);

    }
}
