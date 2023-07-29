using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIAdvanced;
using Players;
//Follow breaks in single player mode
public class Ally2 : AIAdvance
{
    public AudioClip loot;
    [HideInInspector]
    public List<AI> allies;
    protected override void CharacterSettings()
    {
        float r = range;
        SetupParameters();
        base.CharacterSettings();
        if (allies == null || allies.Count == 0)
        {
            Player player = ResetAllies();
            if (player != null)
            {
                player.StartPosition();
            }
        }
        //    SetTargets(GameObject.FindObjectsOfType<Enemy>());
        behaviour = AIBehaviourEnum.Following;
        //Weapons are possible to change the range of a character
        range = r;
        //nma.acceleration = 2;
    }
    public void UpdateTarget(Player player)
    {
        if(followTarget == null)
        {
            followTarget = player;
            player.allies = allies;
            player.UpdateBattleTargets();
            if (MultiplayerManager.players == null || MultiplayerManager.players.Count < MultiplayerManager.multiplayerSingleton.minPlayers)
            {
                MultiplayerManager.players = new List<Player>();
                MultiplayerManager.players.AddRange(FindObjectsOfType<Player>());
                MultiplayerManager.players.Sort();
            }
        }
    }
    public virtual void GetLoot(Item item)
    {
        if (inventory.Contains(item))
        {
            inventory[inventory.IndexOf(item)].quantity++;
        //    Debug.Log("you already have a " + item.name);
        }
        else
        {
            int i = inventory.Count;
            inventory.Add(item);
        //    Debug.Log(string.Format("{2} {0}/{1}",i, inventory.Count, name));
        }
        Character.notifications.AddMessage(
            string.Format("{0} got a {1}!", nameString, item.Name), 3, loot, textColour);
    }
    //OnTriggerEnter doesn't work every time...but its fine for now
    //Make sure enemies are tagged as enemies
    Player ResetAllies()
    {
        Player player = null;
        allies = new List<AI>();
        List<AI> alliesToRemove = new List<AI>();
        allies.AddRange(GameObject.FindObjectsOfType<Ally2>());
        foreach (AIAdvance a in allies)
        {
            Ally2 ally = (Ally2)a;
            ally.allies = allies;
            if (a.name[0] == 'P')
            {
                player = (Player)ally;
                if (unlocked)
                {
                    UpdateTarget(player);
                }
            } else if (!a.unlocked)
            {
                alliesToRemove.Add(a);
            }
        }
        foreach (AIAdvance a in alliesToRemove)
        {
            allies.Remove(a);
        }
        return player;
    }
    bool canInteract;
    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "Enemy")
        {
            ResetAllies();
            Enemy2 enemy = c.gameObject.GetComponent<Enemy2>();
            if (enemy.state == CharacterStates.Dead)
            {
                //Add to harvestable 
                Harvest.EnemyToHarvest = enemy;
                Harvest.healTarget = this;
                if (autoAttack)
                {
                    FindObjectOfType<Harvest>().HarvestEnemy();
                }
            }
            else { 
                enemy.StartBattle((AI)myCharacter, allies);
                if (!targets.Contains(enemy))
                {
                    foreach (Ally2 ally in allies)
                    {
                        if (ally != null && ally.state != CharacterStates.Dead)
                        {
                            ally.allies = allies;
                            ally.EncounterEnemy(enemy);
                        }
                    }
                  //  string s = allies == null ? "Allies are null"
                  //      : string.Format("{0} allies", allies.Count);
                    notifications.AddMessage(string.Format("Encountered {0}!", enemy.nameString), 2);
                }
            }
        }
    }
    void OnTriggerExit(Collider c)
    {
        if (c.gameObject.tag == "Enemy")
        {
            //Remove from harvestable 
            Enemy2 e = c.gameObject.GetComponent<Enemy2>();
            if (Harvest.EnemyToHarvest == e && e.state == CharacterStates.Dead)
            {
                Harvest.EnemyToHarvest = null;
            }
        }
    }
    public virtual void Interact()
    {
        if (cutscene != null)
        {
            EventManager.Trigger("Cutscene" + name);
        }
    }
    public virtual void EncounterEnemy(Enemy2 enemy)
    {
        AddTarget(enemy);
        UpdateBehaviour(AIBehaviourEnum.Battling);
        target = enemy;
        if (autoAttack)
        {
            CurrentTarget = target;
        }
        HideMyText();
        if (allies == null)
        {
            ResetAllies();
            Debug.Log(nameString +" has no allies");
        }
        if (targets == null)
        {
            Debug.Log(nameString + " has no targets");
        }
        Debug.Log(string.Format("Party of {0} VS {1}. {0} VS {3} {2}! {4}", allies.Count, enemy.nameString, behaviour, targets.Count, nameString));
    }
    public void StartHarvest(int points)
    {
        Damage(points, myCharacter, myCharacter.myType);
        harvestPoints += points;

    }
}
