namespace Players
{
    using ManagerClasses;
    using UnityEngine;
    using UnityEngine.Networking;
    using TMPro;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    public class Player : Ally2, IComparable<Player>
    {
        public static BattleTargets targetsUI;
        protected override void CharacterSettings()
        {
            base.CharacterSettings();
            //    SetTargets(GameObject.FindObjectsOfType<Enemy>());
            PlayerMove playerMove = GetComponent<PlayerMove>();
            if (playerMove == null)
            {
                gameObject.AddComponent<PlayerMove>();
            }
            targeter = FindObjectOfType<Targeter>();
            if (targeter != null)
            {
                targeter.Target = currentTarget = target = null;
            }
            //   nma.speed = speed;
            DontDestroyOnLoad(gameObject);
            //   Debug.Log(MultiplayerManager.players.Length);
            if (MultiplayerManager.players != null && MultiplayerManager.players.Count > 0)
            {
                if (playerID > MultiplayerManager.players.Count)
                {
                    Destroy(gameObject);
                }
                else
                {
                    MultiplayerManager.players[playerID - 1] = this;
                }
            }
            allies = null;
        }
        // Update is called once per frame
        protected override void Update()
        {
            switch (Manager.GameState)
            {
                case ManagerStates.Battle:
                    BattleInputs();
                    goto Move;
                case ManagerStates.Exploration:
                    goto Move;
                    Move:
                    Inputs();
                 //   AllInput();
                    //forward direction
                    Vector3 newDir = transform.forward * dir.y;
                    //right direction
                    newDir += transform.right * dir.x;
                    dir.x = newDir.x;
                    dir.y = newDir.z;
                    MoveTo(dir);
                    break;
                case ManagerStates.Paused:
                //    AllInput();
                    break;
                case ManagerStates.GameOver:
                    if (Input.anyKeyDown)
                    {
                        LevelManager.Menu();
                      //  Destroy(gameObject);
                    }
                    break;
            }
        }
        void Inputs()
        {
            if (Input.GetButtonDown("Attack " + controlID))
            {
                if(Manager.battlePanel.activeSelf) {
                    //Dos the arrow stuff
                    switch (ArrowController.GetPosition(controlID))
                    {
                        case 0:
                            ClickTarget();
                            if (currentTarget != null && currentTarget.myText != null)
                            {
                                ArrowController.Action(currentTarget.myText, controlID);
                            }
                            break;
                        case 1:
                            //   inventory[0].Use(currentTarget,myCharacter);
                            StartCoroutine(ShowInventory());
                   //         BattleTargets.Action(inventory[0].nameText, controlID);
                            break;
                    }
                } else
                {
                    ClickTarget();
                }
            }
        }
        void AllInput()
        {
            if (Input.GetButtonDown("Cancel"))
            {
                Manager.Pause();
            }
        }
        public void UpdateBattleTargets()
        {
            if (targetsUI == null)
            {
                targetsUI = FindObjectOfType<BattleTargets>();
            }
            if (targetsUI != null)
            {
                targetsUI.Setup(allies);
            }
        }
        public override void EncounterEnemy(Enemy2 enemy)
        {
            UpdateBattleTargets();
            base.EncounterEnemy(enemy);
        }
        void BattleInputs()
        {
            BattleUpdate();
            if (autoAttack)
            {
                if (Input.GetButtonDown("AutoTarget"))
                {
                    autoAttack = !autoAttack;
                }
            }
            if (time > 10)
            {
                if (targets.Count != 0)
                {
                    if (currentTarget != null)
                    {
                        AttemptAttack(myType);
                    }
                    else
                    {
                        if (targets.Contains(currentTarget))
                        {
                            targets.Remove(currentTarget);
                        }
                        else
                        {
                            for (int i = 0; i < targets.Count; i++)
                            {
                                if (targets[i] == null)
                                {
                                    targets.RemoveAt(i);
                                }
                            }
                        }
                    }
                    //    Debug.Log("current target is " + currentTarget);
                }
                else
                {
                    foreach (AI ally in allies)
                    {
                        ally.ClearTargets();
                    }
                    EndBattle();
                }
            }
        }
        public override void ClearTargets()
        {
            targetsUI.UpdateMe(null);
            base.ClearTargets();
        }
        public override void Die(Character character)
        {
            targeter.Target = null;
            state = CharacterStates.Dead;
            Manager.GameOver();
        }
        //Make sure the player has items
        bool startShowing = false;
        public IEnumerator ShowInventory(Visibility action = Visibility.Opposite)
        {
            if (!startShowing)
            {
                startShowing = true;
                if (inventory != null && inventoryManager != null)
                {
                    inventoryManager.ShowInventory(inventory, Manager.battlePanel.transform, action);
                    if (inventory.Count > 0 && ShopManager.showingInventory)
                    {
                        ArrowController.Action(inventory[0].nameText, controlID);
                    } else
                    {
                        ArrowController.Action(1,controlID);
                    }
                }
                yield return new WaitForSeconds(0.5f);
                startShowing = false;
                //   Debug.Log(name + inventory.Count);
                //Hide Inventory when shown
                //Sometimes gets called twice in one frame
            }
        }
        public void ClickTarget()
        {
            UpdateBattleTargets();
            if (Manager.GameState == ManagerStates.Battle)
            {
             //   Debug.Log("Triggered Attack event");
                EventManager.Trigger("Attack");
                if (targeter.Target)
                {
                    currentTarget = targeter.Target;
                }
                else
                {
                    targeter.Target = ClosestTarget();
                }
            }
            else
            {
                Manager.ShowBattleMenu();
                if (targetsUI != null)
                {
                    if (ArrowController.battleChoices == null)
                    {
                        targetsUI.Setup(targets);
                    }
                    ArrowController.Action(0, controlID);
                }
            }
        }
        void BattleUpdate()
        {
            if (targets != null && targets.Count > 0)
            {
                if (targetsUI != null)
                {
                    targetsUI.UpdateMe(targets);
                }
                else
                {
                    CharacterSettings();
                    if (targetsUI != null)
                    {
                        SetUpAllies();
                    }
                    else
                    {
                        Debug.LogWarning("Battle Targets not found");
                    }
                }
            }
        }
        public override void GetLoot(Item item)
        {
            base.GetLoot(item);
            StartCoroutine(ShowInventory(Visibility.Update));
        }
        void SetUpAllies()
        {
            if (allies == null)
            {
                allies = new List<AI>();
                allies.AddRange(GameObject.FindObjectsOfType<Ally2>());
                foreach (Ally2 ally in allies)
                {
                    if(ally.name[0] == 'A')
                    {
                        ally.UpdateTarget(this);
                    }
                }
            }
            if (targetsUI != null)
            {
                targetsUI.Setup(allies);
            }
        }
        public void StartPosition()
        {
           // Debug.Log(GetComponent<PlayerMove>().syncedPlayers.Count + " synced players " + LevelManager.LevelNumber);
         //   Debug.Log(players == null ? "null" : players.Count + " players");
            if (MultiplayerManager.multiplayerSingleton.startPositions.Count >= playerID && nameString != string.Empty)
            {
                if (nma != null && MultiplayerManager.multiplayerSingleton.startPositions[playerID -1] != null) {
                    Debug.Log(MultiplayerManager.multiplayerSingleton.startPositions.Count + " START POSITIONS From " + nameString);
                    transform.position = MultiplayerManager.multiplayerSingleton.startPositions[playerID - 1].position;
                    dir = new Vector4(0, 0, transform.position.x, transform.position.z);
                    nma.Warp(MultiplayerManager.multiplayerSingleton.startPositions[playerID - 1].position);
                }
            }
            SetUpAllies();
         //   CharacterSettings();
        }
        void OnDestroy()
        {
            Debug.Log("destroyed");
        }
        public void Setup(int id, int control, string newName = "Player ")
        {
            playerID = id;
            controlID = control;
            name = nameString = newName + playerID;
            Debug.Log(nameString + " set up");
        }
        void OnEnable()
        {
         //   Debug.Log("Players is " + MultiplayerManager.singleton.players);
            int i;
            if(nameString == string.Empty || !int.TryParse(nameString[nameString.Length-1].ToString(), out i))
            {
                //Destroys an object if it is already contained
                if (MultiplayerManager.players != null && MultiplayerManager.players.Contains(this))
                {
                      Destroy(gameObject);
                }
                Setup(3, 1, "PLAYER ");
            }
            StartPosition();
        }
        [HideInInspector][SyncVar]
        public int playerID = 1, controlID;
        public int CompareTo(Player otherPlayer)
        {
            return otherPlayer.playerID;
        }
    }
}
