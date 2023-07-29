using System.Collections.Generic;
using UnityEngine;
using Players;

public class Ally : AI
{
    public AudioClip loot;
    List<AI> allies;
    protected override void CharacterSettings()
    {
        float r = range;
        base.CharacterSettings();
        allies = new List<AI>();
        allies.AddRange(GameObject.FindObjectsOfType<Ally>());
    //    SetTargets(GameObject.FindObjectsOfType<Enemy>());
        behaviour = AIBehaviourEnum.Following;
        followTarget = FindObjectOfType<Player>();
        //Weapons are possible to change the range of a character
        range = r;
    //nma.acceleration = 2;
}
    
    public void GetLoot(Item item)
    {
        inventory.Add(item);
        Character.notifications.AddMessage(
            string.Format("{0} got a {1}!", this.name, item.gameObject.name), 3, loot);
    }
    //OnTriggerEnter doesn't work every time...but its fine for now
    //Make sure enemies are tagged as enemies
    void OnTriggerEnter(Collider c)
    {
        if(c.gameObject.tag == "Enemy")
        {
            Enemy2 enemy = c.gameObject.GetComponent<Enemy2>();
            if (!targets.Contains(enemy))
            {
                foreach (Ally ally in allies)
                {
                    ally.EncounterEnemy(enemy);
                }
                string s = allies == null ? "Allies are null"
                    : string.Format("{0} allies", allies.Count);
                enemy.StartBattle((AI)myCharacter, allies);
                subtitles.AddMessage(string.Format("Encountered {0}!", enemy.name), 2);
            }
        }
    }
    public void EncounterEnemy(Enemy2 enemy)
    {
        AddTarget(enemy);
        behaviour = AIBehaviourEnum.Battling;
        Debug.Log(string.Format("Party of {0} VS {1}. {0} VS {3} {2}!",allies.Count, enemy.name, behaviour, targets.Count));
    }
}
