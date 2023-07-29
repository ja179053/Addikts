namespace AIAdvanced
{
    using System.Collections.Generic;
    using System.Collections;
    using ManagerClasses;
    using UnityEngine;
    public abstract class AIAdvance : AI
    {
        public int healAt = 1, healValue = 1, harvestPoints;
        public bool unlocked = false;
        protected AI CurrentTarget
        {
            get
            {
                return currentTarget;
            }
            set
            {
                currentTarget = value;
                Intelligence();
            }
        }
        //Cooldowns
        // Use this for initialization
        //Happens once on play
        #region setup
        //Actions cannot be used
        protected List<IEnumerator> actions;
        protected void SetupParameters()
        {
            healthParameters = new List<HealthParameter>();
            magicParameters = new List<MagicParameter>();
            targetParameters = new List<TargetParameter>();
            actions = new List<IEnumerator>();
            AddParameters();
            currentBehaviour = idleBehaviour = wanderBehaviour = DoNothing();
            attackBehaviour = ChargeAttack();
        }
        public override void Damage(int damage, Character character, AttackType type)
        {
            if (damage != 0)
            {
                base.Damage(damage, character, type);
                Intelligence();
            }

        }
        protected void Intelligence()
        {if (state != CharacterStates.Dead)
            {
                currentAction = ValidateMyMP();
                if (currentAction == null)
                {
                    currentAction = ValidateMyHP();
                }
                int index = currentAction.ToString().IndexOf("<") + 1;
                //Have to convert to string in order to call coroutine more than once
                string s = currentAction.ToString().Substring(index, currentAction.ToString().IndexOf(">") - index);
                StartCoroutine(s);
                //       Debug.Log(name + s);
            }
        }
        public bool attackRandom;
        /// <summary>
        /// Add parameters to AI to make them behave intelligently
        /// </summary>
        protected virtual void AddParameters()
        {
            //Will not heal AI that heal at 1 or less
            Condition<float> c = new LessThan(healAt);
            healthParameters.Add(new HealthParameter(c, Heal()));
            actions.Add(ValidateMyHP());
            if (!attackRandom)
            {
                //Will not heal AI that heal at 1 or less
                Condition<float> always = new MoreThan(0);
                targetParameters.Add(new TargetParameter(always, ClosestTarget()));
                actions.Add(ValidateMyHP());
            }
        }
#endregion
        // Update is called once per frame
        IEnumerator attackBehaviour, idleBehaviour, wanderBehaviour;
        IEnumerator currentBehaviour, currentAction;
        protected void UpdateBehaviour(AIBehaviourEnum newBehaviour)
        {
            behaviour = newBehaviour;
            switch (behaviour)
            {
                case AIBehaviourEnum.Battling:
                    currentBehaviour = attackBehaviour;
                    break;
                case AIBehaviourEnum.Wandering:
                    currentBehaviour = wanderBehaviour;
                    break;
            }
            currentAction = currentBehaviour;
        }
        protected override void Behave()
        {
            switch (behaviour)
            {
                case AIBehaviourEnum.Battling:
                    if(currentTarget == null && targets.Contains(currentTarget))
                    {
                        targets.Remove(currentTarget);
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
                    Follow:
                    MoveTo(followTarget.dir);
                    currentBehaviour = idleBehaviour;
                    break;
            }

        }
        protected List<MagicParameter> magicParameters;
        protected List<HealthParameter> healthParameters;
        protected List<TargetParameter> targetParameters;
        protected AI ChooseTarget()
        {
            //     Debug.Log(nameString + " chose a target");
            if (targets != null && targets.Count > 0)
            {
                List<Parameter> list = new List<Parameter>();
                list.AddRange(targetParameters);
                if (targetParameters.Count > 0 && enabledParameters(list) > 0)
                {
                    for (int i = 0; i < targetParameters.Count; i++)
                    {
                        if (!targetParameters[i].disabled)
                        {
                            if (targetParameters[i].condition.Validate(1))
                            {
                                return targetParameters[i].target;
                            }
                        }
                    }
                }
                return targets[Random.Range(0, targets.Count - 1)];
            }
            return null;
        }
        protected AI ClosestTarget()
        {
            if (targets != null && targets.Count > 0)
            {
                AI closestTarget = null;
                if (targets.Count < 2)
                {
                    //Old way to autotarget target 
                    ///Suitable when one target is available
                    return targets[0];
                }
                else
                {
                    foreach (AI targ in targets)
                    {
                        if (closestTarget == null)
                        {
                            closestTarget = targ;
                        }
                        else
                        {
                            Vector3 pos1 = targ.transform.position - transform.position;
                            Vector3 pos2 = closestTarget.transform.position - transform.position;
                            if (pos1.x > 0)
                            {
                                if (pos1.z < pos2.z)
                                {
                                    closestTarget = targ;
                                }
                            }
                        }
                    }
                }
                return closestTarget;
            }
            else
            {
                return null;
            }
        }
        #region routines
        protected IEnumerator ValidateTargetsHP()
        {
            if (targets != null && targets.Count > 0)
            {
                List<Parameter> list = new List<Parameter>();
                list.AddRange(healthParameters);
                if (healthParameters.Count > 0 && enabledParameters(list) > 0)
                {
                    for (int i = 0; i < healthParameters.Count; i++)
                    {
                        if (!healthParameters[i].disabled)
                        {
                            if (healthParameters[i].condition.Validate(hP))
                            {
                                return healthParameters[i].function;
                            }
                        }
                    }
                }
            }
            return currentBehaviour;
        }
        protected IEnumerator ValidateMyHP()
        {
            //Calculates what to do depending on remaining hp
            ///Gets more desperate as time geos on
            ///Will only restore health when hp is not full
            if (hP < maxHP)
            {
                //ONLY USED FOR HEALTH RESTORATION ATM
                List<Parameter> list = new List<Parameter>();
                list.AddRange(healthParameters);
                if (healthParameters.Count > 0 && enabledParameters(list) > 0)
                {
                    for (int i = 0; i < healthParameters.Count; i++)
                    {
                        if (!healthParameters[i].disabled)
                        {
                             //   Debug.Log("HP is " + hP);   
                            if (healthParameters[i].condition.Validate(hP))
                            {
                                return healthParameters[i].function;
                            }
                        }
                    }
                }
            }
            return currentBehaviour;
        }
        protected IEnumerator ValidateMyMP()
        {
            //Calculates what to do depending on remaining mp
            ///Will attack normally when mp is empty and cannot flee
            if (mP > 0)
            {
                //USED FOR ATTACKING ATM
                List<Parameter> list = new List<Parameter>();
                list.AddRange(magicParameters);
                if (magicParameters.Count > 0 && enabledParameters(list) > 0)
                {
                    for (int i = 0; i < magicParameters.Count; i++)
                    {
                        if (!magicParameters[i].disabled)
                        {
                            //   Debug.Log("HP is " + hP);   
                            if (magicParameters[i].condition.Validate(mP))
                            {
                                return magicParameters[i].function;
                            }
                        }
                    }
                }
            }
            return null;
        }

        #region Actions
        protected IEnumerator FullHeal()
        {
            yield return new WaitForSeconds(healTime);
            Damage(-maxHP, myCharacter, myType);
            currentAction = null;

        }
        protected IEnumerator Heal()
        {
            yield return new WaitForSeconds(healTime);
            Damage(-healValue, myCharacter, myType);
            currentAction = null;
            //   GetComponent<Renderer>().material.color = Color.green;
        }
        protected IEnumerator MakePeace()
        {
            //Can be used for a slowdown animation
            yield return new WaitForSeconds(0);
            SetTargets(null);
        }
        protected IEnumerator ChargeAttack()
        {
            if (Manager.GameState == ManagerStates.Battle)
            {
                yield return new WaitForSeconds(attackTime);
                //   if (time > 10 && autoAttack)
                //  {
                //AI will attack random for now, this will change when gambits are developed                                
                //Ally will forget its target if the battle is ended early
           //     Debug.Log(nameString +  " is attacking");
            //    AttackRandom(myCharacter);
                //   }
                AttemptAttack(myType);
                currentAction = null;
            }
        }
        protected IEnumerator Fire()
        {
            if (Manager.GameState == ManagerStates.Battle)
            {
                yield return new WaitForSeconds(spellCastTime);
                AttemptAttack(AttackType.Fire);
                currentAction = null;
            }
        }
        IEnumerator DoNothing()
        {
            yield return new WaitForSeconds(0);
            //    Debug.Log("I AM DOING NOTHING");
            currentAction = null;
        }
        #endregion
        #endregion
        int enabledParameters(List<Parameter> parameters)
        {
            int i = 0;
            foreach (Parameter p in parameters)
            {
                if (!p.disabled)
                {
                    i++;
                }
            }
            return i;
        }
    }    
}
public enum AttackType
{
    Normal = 0,
    Fire = 1,
    Earth = 2,
    Water = 3,
    Wind = 4
}
public enum CounterType
{
    Fire = 2,
    Earth = 4,
    Water = 1,
    Wind = 3,
}
