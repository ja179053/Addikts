using System.Collections.Generic;
using UnityEngine;
using ManagerClasses;

public class Enemy : AI
{
    
    public int expValue = 1;
    protected override void CharacterSettings()
    {
        behaviour = AIBehaviourEnum.Wandering;
        base.CharacterSettings();
        nma.stoppingDistance = 0.1f;
    }
/*    public override void Attack(Character character)
    {
        target = GameObject.FindObjectOfType<Ally>();
        base.Attack(character);
    }*/
    public override void Die(Character character)
    {
        AwardXP(character);
        base.Die(character);
    }
    //Should bad guys get xp too?
    //Gives the enemies xp when they die, clears this target from the battle
    //Whoever killed this guy gets bonus xp
    void AwardXP(Character target)
    {
        expValue *= level;
        Ally victor = null;
        foreach(Ally ally in targets)
        {
            if (ally != null)
            {
                ally.AwardEXP(expValue);
                ally.ForgetTarget((Enemy)myCharacter);
                if (ally == target)
                {
                    victor = (Ally)target;
                }
            }
        }
        float bonusEXP = Mathf.Ceil((float)expValue / 2);
        target.AwardEXP((int)bonusEXP);
        //Victory sound
        Character.notifications.AddMessage(string.Format("{0} killed {1} and earned a bonus of {2} EXP!", target.name, this.name, bonusEXP),5);
        AwardLoot(victor);
    }
    void AwardLoot(Ally character)
    {
        if (inventory.Count != 0)
        {
            int i = Random.Range(0, inventory.Count - 1);
            character.GetLoot(inventory[i]);
        } else
        {
            //Sad no loot dropped sound...?
            //Music is a reward not a privilege
            Character.notifications.AddMessage("No loot was dropped",3);
        }
    }
    public int wanderRange = 5;
    protected override void Update()
    {
        base.Update();
        switch (behaviour)
        {
            case AIBehaviourEnum.Wandering:
                dir = (new Vector4(Functions.RandomInt(wanderRange), Functions.RandomInt(wanderRange), transform.position.x, transform.position.z));
                break;
        }
    }
    //A hero can start a battle with an enemy a any time
    public void StartBattle(AI other, List<AI> myEnemy)
    {
        transform.LookAt(other.transform);
        targets = myEnemy;
        if (target == null)
        {
            target = other;
        }
        behaviour = AIBehaviourEnum.Battling;
        manager.PlaySound(ManagerStates.Battle);
        HideMyText();
        PlayBattlecry();
    }
}
