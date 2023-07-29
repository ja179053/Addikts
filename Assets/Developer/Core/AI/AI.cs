using ManagerClasses;
using UnityEngine;
using UnityEngine.Networking;

public abstract class AI : Character {
    [HideInInspector]
    public TargetText myText;
    [HideInInspector][SyncVar]
    public Vector4 dir;
    public Vector3 offset;
    public bool autoAttack = false;
    protected AIBehaviourEnum behaviour;
    protected AI followTarget;
    public static Targeter targeter;

    #region Battle
    public virtual void ClearTargets()
    {
        SetTargets(new AI[0]);
        behaviour = AIBehaviourEnum.Following;
        if (myText != null)
        {
            myText.enabled = true;
        }
        time = 0;
        StopAllCoroutines();
        LookForHarvest();
    }
    public void LookForHarvest()
    {
        if (autoAttack && Manager.GameState != ManagerStates.Battle && Harvest.EnemyToHarvest != null && nma != null)
        {
            nma.SetDestination(Harvest.EnemyToHarvest.transform.position);
        }

    }
    protected virtual void Update()
    {
        switch (Manager.GameState)
        {
            case (ManagerStates.Battle):
                Behave();
                break;
            case (ManagerStates.Exploration):
                Behave();
                break;
        }
    }
    protected virtual void Behave()
    {
        switch (behaviour)
        {
            case AIBehaviourEnum.Wandering:
                break;
            case (AIBehaviourEnum.Battling):
                {
                    //AI is switching from Battling to following?
                    ///Time still increasaes, must be inspector glitch
                }
                break;
            case AIBehaviourEnum.Chasing:
                //Not Moving the player
                if (currentTarget != null)
                {
                    MoveTo(currentTarget.transform.position);
                    //Won't attack until within range
                    float currentDistance = Vector3.Distance(myCharacter.transform.position, currentTarget.transform.position);
                    if (currentDistance < range)
                    {
                        AttemptAttack(myType);
                        behaviour = AIBehaviourEnum.Battling;
                    }
                }
                break;
            case AIBehaviourEnum.Following:
                if (followTarget != null)
                {
                    MoveTo(followTarget.dir);
                }
                break;
        }
    }
    protected void AttackRandom(Character character, AttackType type)
    {
       //    Debug.Log(nameString + " ATTACKING " + targets.Count);
        if (targets.Count == 0)
        {
            EndBattle();
        }
        else
        {
            //Make sure you assign randomise target to something
            currentTarget = RandomiseTarget();
            if (currentTarget == null || currentTarget.state == CharacterStates.Dead)
            {
                ForgetTarget(currentTarget);
                Debug.Log("Recalculating target");
                AttackRandom(myCharacter, type);
            }
            else
            {
                AttemptAttack(type);
            }
        }
    }
    public void Aim(AI enemy)
    {
        currentTarget = enemy;
    }
    //The attack function that gets called for enemies.
    protected void AttemptAttack(AttackType type)
    {
        if (myCharacter != null && currentTarget != null)
        {
            if (currentTarget.state == CharacterStates.Dead)
            {
                currentTarget = null;
            }
            else
            {
                float f = Vector3.Distance(myCharacter.transform.position, currentTarget.transform.position);
                if ((int)(f) > (int)range)
                {
                    notifications.AddMessage(string.Format("{0} is too far away {1}m", currentTarget.nameString, (int)f), 1);
                    behaviour = AIBehaviourEnum.Chasing;
                }
                else
                {
                    Attack(myCharacter, type);
                }
            }
        }
    }
    private void Attack(Character character, AttackType type)
    {
        time = 0;
        //Code to calculate hit chane
        if (evasion > 0 && character.accuracy > 0)
        {
            float f = Random.Range(0, evasion);
            if (f > character.accuracy)
            {
                Character.notifications.AddMessage(string.Format("{0}'s attack missed!", character.nameString), 1);
                return;
            }

        }
        //Code to damage
        int power = (type != myType && type != AttackType.Normal) ? magic: attack;
        currentTarget.Damage(power, character, type);
    }
    #endregion
    protected void EndBattle()
    {
        Debug.Log(nameString + " ended the battle");
        //WE USE CAPITAL LETTERS NOW GRRR #MANLY
        //THIS IS CALLED WHEN THE ENEMY IS DEFEATED
        manager.PlaySound(ManagerStates.Exploration);
        if (targeter != null)
        {
            targeter.Target = null;
        }
        time = 0;
    }
    #region Movement
    //Base to move the character, can only be overwritten by player input.
    public void MoveTo(Vector3 pos)
    {
        nma.SetDestination(pos);
    }
    //Offset cannot be applied for chasing. Apply beforehand with a Dir property
    public void MoveTo(Vector4 direction)
    {
        //Updates direction 
        dir = direction;
        //Multplies direction by speed
        direction.x *= speed;
        direction.y *= speed;
        //Converts direction into a vector3 for the nav mesh
        Vector3 pos;
        pos = new Vector3(direction.z + dir.x, transform.position.y, direction.w + dir.y);
        //Playing with different movements (didnt work)
        //pos = Functions.MultiplyVector3(pos, transform.forward);
        //Adds an offset to the position
        pos += offset;
        //Doesn't move if too close.
        float f = Vector3.Distance(transform.position, pos);
        if (f <= maxMoveDistance)
        {
            pos = transform.position;
            dir = new Vector4(0, 0, dir.z, dir.w);
            Footsteps(false);
        } else
        {
            Footsteps(true);
        }
        nma.SetDestination(pos);
    }
    #endregion
    public void ForgetTarget(AI c)
    {
        targets.Remove(c);
    }
    public bool debugState;
}
