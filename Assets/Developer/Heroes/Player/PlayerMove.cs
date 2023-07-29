namespace Players
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.AI;
    using UnityEngine.Networking;
    using ManagerClasses;

    public class PlayerMove : NetworkBehaviour
    {
        Player player;
        public SyncedPlayers syncedPlayers;
        Animator animator;
        //Insert/Delete static to test these variables during playS
        public static float sensitivity = 1, minInput;
        public bool debug;
        [Command]
        public void CmdSetPlayers(int player)
        {
            //    Debug.Log(fd);
            if (syncedPlayers == null)
            {
                syncedPlayers = new SyncedPlayers();
                syncedPlayers.InitializeBehaviour(this, 0);
            }
            PlayerData data = new PlayerData();
            data.ID = player;
            data.player = this;
            if (syncedPlayers.Contains(data))
            {
                syncedPlayers.Add(data);
            }
            Debug.Log("players have been synced.");
        }
        // Update is called once per frame
        void Update()
        {
            #region setup
            //Initialises player
            if (player == null || animator == null)
            {
                player = GetComponent<Player>();
                player.dir = new Vector4(0, 0, transform.position.x, transform.position.z);
                animator = GetComponentInChildren<Animator>();
                minInput = GetComponent<NavMeshAgent>().stoppingDistance;
                player.maxMoveDistance = minInput;
                UpdateMoveDistance(sensitivity);
            }
            //Syncs player lists
            if (MultiplayerManager.players == null || MultiplayerManager.players.Count == 0)
            {
                Debug.Log("resyncing players");
                MultiplayerManager.players = new List<Player>();
                foreach (PlayerData data in syncedPlayers)
                {
                    if (data.player == this)
                    {
                        GetComponent<Player>().playerID = data.ID;
                    }
                    MultiplayerManager.players.Add(data.player.GetComponent<Player>());
                }
            }
            #endregion
            #region Debugs
            bool thisPlayer = MultiplayerManager.players != null && player.playerID <= MultiplayerManager.players.Count && MultiplayerManager.players[player.playerID - 1] != null && MultiplayerManager.players[player.playerID - 1] == player;
            if (!thisPlayer)
            {
                //   Debug.Log(Player.players[player.playerID - 1].playerID + " and " + player.playerID);
            }
            #endregion
            #region Player Inputs
            //   if (LevelManager.LevelNumber != 0 && 
            bool canControl = ((MultiplayerManager.online == false) || (MultiplayerManager.online && MultiplayerManager.hostPlayer && player.playerID == 1) ||
                (MultiplayerManager.online && !MultiplayerManager.hostPlayer && player.playerID == 2));
            //  Debug.Log(b);
            if (canControl)
            {
                float x = Input.GetAxis("Horizontal " + player.controlID);
                float y = Input.GetAxis("Vertical " + player.controlID);
                Vector4 v = (new Vector4(x, y, transform.position.x, transform.position.z));
                UpdateArrow(v);
                if (debug)
                {
                    Debug.Log(v);
                }
                //inputs are never exactly 0, so this will change in a minute
                float minimum = minInput / sensitivity;
                if (Functions.InBounds(x, minimum) && Functions.InBounds(y, minimum))
                {
                    if (animator != null)
                    {
                        animator.SetBool("Move", false);
                    }
                }
                else
                {
                    Moving(v);
                }
            }
            #endregion
        }
        int inventorySelection = 0;
        void UpdateArrow(Vector4 v)
        {
            int x = (int)Mathf.Ceil(v.x);
            int y = (int)Mathf.Ceil(v.y);
            if (Manager.battlePanel != null && Player.targetsUI != null && Manager.battlePanel.activeSelf)
            {
                if (x != 0)
                {
                    StartCoroutine(BattleTargets.arrow.ChangeBattleChoice(player.controlID));
                }
                else if (player.inventory.Count > 1 && y != 0)
                {
                    StartCoroutine(BattleTargets.arrow.ChangeTargetChoice(player.inventory[inventorySelection + y].nameText, player.controlID));
                }
                if (ArrowController.GetPosition(player.controlID) == 1)
                {
                    player.ShowInventory(Visibility.Hide);
                }
            }
        }
        public static void UpdateMoveDistance(float sense)
        {
            sensitivity = sense;
            /*   if (player != null)
               {
                   //   player.maxMoveDistance = minInput / sensitivity;
                   //   Debug.Log(player.maxMoveDistance);
               }*/
        }
        void Moving(Vector4 v)
        {
            //   Debug.Log("moving");
            player.dir = v;
            animator.SetBool("Move", true);
            CameraAI3.moveEventPresent = false;
            EventManager.Trigger("Move");
        }
        public class SyncedPlayers : SyncListStruct<PlayerData> { }

        public struct PlayerData
        {
            public int ID;
            public PlayerMove player;
        }
    }
}
