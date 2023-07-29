using ManagerClasses;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public abstract class Character : NetworkBehaviour
{
    [HideInInspector]
    //Character's name
    public string nameString;
    //Playing charactr sound effecrs
    protected AudioSource[] aso;
    public AudioSource mainAso;
    public AudioClip battlecry, footsteps;
    //Animator
    protected Animator animator;
    #region stats
    //Increasable stats
    public int level = 1, maxHP = 1, maxMP = 0, ap = 0;
    //Calculated stats
    public int attack = 1, defense = 0, magic = 0, magicDefense = 0, speed = 1, evasion = 0, magicEvasion = 0, absorb = 0, luck = 0, vitality = 0, accuracy = 1;
  //  public int fireResistance = 0, windResistance = 0, earthResistance = 0, waterResistance = 0;
    //Dynamic stats
    protected int hP, mP, exp = 0;
    //base stats
    int baseattack = 1, basedefense = 0, basemagic = 0, basemagicDefense = 0, basespeed = 1, baseevasion = 0, basemagicEvasion = 0, baseabsorb = 0, baseluck = 0, basevitality = 0, baseAccuracy = 1;
    int baseFireResistance = 0, baseWindResistance = 0, baseEarthResistance = 0, baseWaterResistance = 0;
    #endregion
    [HideInInspector]
    public CharacterStates state;
    public List<AI> targets;
    protected AI target;
    protected Character myCharacter;
    //Charge time in battle
    protected float time;
    protected NavMeshAgent nma;
    protected Transform myTransform;
    //Attachable inventory
    public List<Item> inventory;
    //Movement criteria
    [HideInInspector]
    public float maxMoveDistance = 0;
    public float range;
    //Messengers
    public static Messenger notifications, subtitles;
    //Managers
    protected static Manager manager;
    public static ShopManager inventoryManager;
    //variables for UI
    SliderController healthBar;
    [HideInInspector]
    public Slider timer;
    protected Cutscene cutscene;
    public Color textColour = new Color(0,0,0,0);
    public float healTime = 5, attackTime = 10, itemTime = 3, spellCastTime = 10;
    public AttackType myType;
    [Range(0,1)]
    public float spikiness = 0;


    protected int Exp
    {
        get { return exp; }
        set
        {
            exp = value;
            if (exp >= level)
            {
                LevelUp();
            }
        }
    }

    // Use this for initialization
    public void Start()
    {
        CharacterSettings();
    }
    protected virtual void CharacterSettings()
    {
        nameString = NameString();
        //This destroys the loot that was assigned oops 
        //(public variables already have an instance)
        if (inventory == null)
        {
            inventory = new List<Item>();
        }
        AI[] newtargets = (targets == null) ? new AI[0] : targets.ToArray();
        SetTargets(newtargets);
        state = CharacterStates.Living;
        healthBar = GetComponentInChildren<SliderController>();
        animator = GetComponentInChildren<Animator>();
        nma = GetComponentInChildren<NavMeshAgent>();
        if (nma == null)
        {
            nma = this.gameObject.AddComponent<NavMeshAgent>();
        }
        cutscene = GetComponent<Cutscene>();
        myTransform = nma.transform;
        myCharacter = GetComponent<Character>();
        inventoryManager = FindObjectOfType<ShopManager>();
        manager = FindObjectOfType<Manager>();
        GameObject g = GameObject.Find("OnScreenMessages");
        if(g != null && (notifications == null || subtitles == null))
        {
            notifications = g.GetComponent<Messenger>();
            subtitles = GameObject.Find("Subtitles").GetComponent<Messenger>();
        }
        mP = maxMP;
        hP = maxHP;
        if(healthBar != null)
        {
            healthBar.SetHealth(maxHP);
        }
        if (range == 0)
        {
            range = GetComponent<CapsuleCollider>().radius;
        }
        attackTime = attackTime == 0 ? healTime : attackTime;
        GetMainAso();
    }
    protected void AddTarget(AI ai)
    {
        targets.Add(ai);
    }
    protected void SetTargets(AI[] list)
    {
        if (list == null)
        {
            list = new AI[0];
        }
        if (list.Length < 1)
        {
            //  Debug.Log(nameString + " Reset target");
            targets = new List<AI>();
        }
        targets.AddRange(list);
        HideMyText(false);
    }
    //Make sure you assign randomise target to something
    protected AI RandomiseTarget()
    {
        int i = Random.Range(0, targets.Count - 1);
        return targets[i];
    }

    void FixedUpdate()
    {
        switch (state)
        {
            case CharacterStates.Living:
                if (targets != null && Manager.GameState == ManagerStates.Battle)
                {
                    time += Time.fixedDeltaTime * speed;

                }
                if (timer != null)
                {
                    timer.value = time;
                }
                break;
            case CharacterStates.Dead:
                break;
        }
    }
    //Its a lot better to use IDs instead of strings, but im too lazy
    public void UseItem(string itemName)
    {
        Item foundItem = ShopManager.FindItem(inventory, itemName);
        currentTarget = RandomiseTarget();
        //Item sound effect
        Character.notifications.AddMessage(
            string.Format("{0} used a {1}!", nameString, itemName, Manager.music.item), 1);
        foundItem.Use(myCharacter, currentTarget);
        time -= itemTime;
    }
    public void DropItem(Item item)
    {
        inventory.Remove(item);
        //Drop sound effect
        Character.notifications.AddMessage(string.Format("{0} dropped a {1}", nameString, item.name), 3, Manager.music.drop);
    }
    protected AI currentTarget;
    public virtual void Damage(int damage, Character character,AttackType type)
    {
        if (damage != 0 && state != CharacterStates.Dead && character.state != CharacterStates.Dead)
        {
            //Code to calculate if correct element
            if (myType == AttackType.Normal || myType == type || ((CounterType)(int)type).ToString() == myType.ToString())
            {
                //    float playerDistance = Vector3.Distance(transform.position, CameraAIv2.mainCamera.transform.position);
                //  Debug.Log(string.Format("{0} was damaged. Target is {1}/{2} away", nameString, range, playerDistance));
                //Calculated damage/healing and adds a notification
                //   Debug.Log(string.Format("{3}: {1}/{0}/{2} by {4}", hP, damage,maxHP, nameString, character.nameString));
                if (damage > 0)
                {
                    damage -= defense;
                    damage = Mathf.Clamp(damage, 0, damage);
                    Character.notifications.AddMessage(string.Format("{0} was hit for {1} HP!", nameString, damage), 1, Manager.music.hit);
                }
                else
                {
                    Character.notifications.AddMessage(string.Format("{0} was healed for {1} HP!", nameString, -damage), 1, Manager.music.heal);
                }
                hP -= damage;
                hP = Mathf.Clamp(hP, 0, maxHP);
                if (healthBar != null)
                {
                    healthBar.UpdateHealth(hP);
                }
                character.Damage((int)Mathf.Round(spikiness * damage), myCharacter, myType);
                //Die plays after hit
                string s = "";
                if (hP == 0)
                {
                    if (cutscene != null)
                    {
                        s = "CutsceneDie" + name;
                        EventManager.Trigger(s);
                    }
                    Die(character);
                }
                else
                {
                    if (cutscene != null)
                    {
                        s = "CutsceneDamage" + name;
                        EventManager.Trigger(s);

                    }

                }
                //   Debug.Log(s);
            }
        }
    }
    public virtual void Die(Character character)
    {
        state = CharacterStates.Dead;
        Destroy(this.gameObject);
    }
    protected void LevelUp()
    {
        exp -= level;
        level++;
        //Level up sound effect
        Character.notifications.AddMessage(string.Format("{0} reached level {1}!", nameString, level), 3, Manager.music.levelUp);
        //Write what levels up here
        ///HP is increased and restored
        maxHP++;
        hP = maxHP;
        evasion++;
        luck++;
        if (healthBar != null)
        {
            healthBar.SetHealth(maxHP);
        }
    }
    //Code evasion and luck into miss chance
    //Then code director ai
    public void AwardEXP(int gainedExp)
    {
        Exp += gainedExp;
    }
    TextMeshPro[] myLabels;
    protected void HideMyText(bool hide = true)
    {
        if (myLabels == null)
        {
            myLabels = GetComponentsInChildren<TextMeshPro>();
        }
        if (myLabels != null && myLabels.Length > 0)
        {
            foreach (TextMeshPro t in myLabels)
            {
                t.gameObject.SetActive(!hide);
            }
        }
    }
    string NameString()
    {
        int i = name.Contains("(") ? name.IndexOf('(') : name.Length;
        return name.Substring(0, i).Trim();
    }
    #region Audio
    void GetMainAso()
    {
        //   Debug.Log(nameString + " got aso");
        aso = GetComponentsInChildren<AudioSource>();
        if (aso == null || aso.Length == 0)
        {
            aso = new AudioSource[1];
            aso[0] = this.gameObject.AddComponent<AudioSource>();
        }
        mainAso = aso[0];
    }
    //THis section is for character souund
    protected void PlayBattlecry()
    {
        if (mainAso == null)
        {
            GetMainAso();
        }
        if (battlecry != null)
        {
            mainAso.PlayOneShot(battlecry);
        }
    }
    protected void Footsteps(bool on)
    {
        //   Debug.Log(on);
        int i = 0;
        foreach (AudioSource asource in aso)
        {
            i++;
            if (on)
            {
                if ((asource.clip == footsteps || aso.Length == 1) && !asource.isPlaying)
                {
                    asource.clip = footsteps;
                    asource.PlayDelayed(i);
                }
            }
            else
            {
                if (asource.isPlaying)
                {
                    asource.Stop();
                }
            }
        }
    }
    #endregion
}
