using System.Collections.Generic;
using UnityEngine;
using AIAdvanced;
using ManagerClasses;
using Players;
public class Enemy2 : AIAdvance
{
    public int expValue = 1;
    #region overrides
    protected override void CharacterSettings()
    {
        SetupParameters();
        behaviour = AIBehaviourEnum.Wandering;
        base.CharacterSettings();
        myText = null;
    }
    public int wanderRange = 5;
    protected override void Update()
    {
        if (currentTarget != null)
        {
         //   Debug.Log(currentTarget + time.ToString());
        }
        base.Update();
        switch (behaviour)
        {
            case AIBehaviourEnum.Wandering:
                dir = (new Vector4(Functions.RandomInt(wanderRange), Functions.RandomInt(wanderRange), transform.position.x, transform.position.z));
                break;
        }
    }
    //Make a coroutine for wandering in ai advance
    public override void Die(Character character)
    {
        AwardXP(character);
        Bestiary.NewEnemy((Enemy2)myCharacter);
        if (myText != null)
        {
            Destroy(myText.gameObject);
        }
        state = CharacterStates.Dead;
        GetComponentInChildren<Animator>().enabled = false;
        Harvest.EnemyToHarvest = this;
        Player.targeter.Target = null;
        Vector3 v = animator.transform.rotation.eulerAngles;
        v.z += 90;
        animator.transform.rotation = Quaternion.Euler(v);
        nma.isStopped = true;
    }
    #endregion
    //Should bad guys get xp too?
    //Gives the enemies xp when they die, clears this target from the battle
    //Whoever killed this guy gets bonus xp
    void AwardXP(Character target)
    {
        expValue *= level;
        Ally2 victor = null;
        foreach (Ally2 ally in targets)
        {
            if (ally != null)
            {
                ally.AwardEXP(expValue);
                ally.ForgetTarget((Enemy2)myCharacter);
                if (ally == target)
                {
                    victor = (Ally2)target;
                }
            }
        }
        float bonusEXP = Mathf.Ceil((float)expValue / 2);
        target.AwardEXP((int)bonusEXP);
        //Victory sound
        Character.notifications.AddMessage(string.Format("{0} killed {1} and earned a bonus of {2} EXP!", target.nameString, nameString, bonusEXP), 5);
        AwardLoot(victor);
    }
    void AwardLoot(Ally2 character)
    {
        if (inventory.Count != 0)
        {
            int i = Random.Range(0, inventory.Count - 1);
            character.GetLoot(inventory[i]);
        }
        else
        {
            //Sad no loot dropped sound...?
            //Music is a reward not a privilege
            Character.notifications.AddMessage("No loot was dropped", 3);
        }
    }
    //A hero can start a battle with an enemy a any time
    public void StartBattle(AI other, List<AI> myEnemy)
    {
        myTransform.LookAt(other.transform);
        targets = myEnemy;
        if (target == null)
        {
            PlayBattlecry();
            manager.PlaySound(ManagerStates.Battle);
            UpdateBehaviour(AIBehaviourEnum.Battling);
            target = CurrentTarget = other;
            HideMyText();
            Targeter.Teleport(transform.position);
        }
    }
    protected override void AddParameters()
    {
        Condition<float> c = new LessThan(3);
        healthParameters.Add(new HealthParameter(c, Heal()));
        actions.Add(ValidateMyHP());
    }
}
