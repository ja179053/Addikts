using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using Players;
public class MultiplayerManager : NetworkLobbyManager
{
    public static bool hostPlayer;
    //The players to be handled during the game
    public static List<Player> players;
    //Singleton of the network lobby manager
    public static MultiplayerManager multiplayerSingleton;
    //Initialises game
    public void StartLocalGame()
    {
        //  Debug.Log("starting");
        Initialise();
        //Check controller count
        //Ask to use 2 people one keyboard if PC
        //Autostart game if ready-
        ///If controllers needed, wait with message
        ///If online players needed, wait with message

    }
    TitleScreen title;
    //Sets up network lobby manager
    void OnEnable()
    {
        multiplayerSingleton = this;
        hostPlayer = false; 
        title = FindObjectOfType<TitleScreen>();
        //     DontDestroyOnLoad(gameObject);
    }
    //Starts the host
    public void Initialise()
    {
        if (matchHost == null)
        {
            NetworkServer.Reset();
            StartHost();
        }
    }
    #region Online
    //Sets players and matchmaker according to onlide mode being set
    ///A minimum of 2 players are required for online
    public void SetOnline(bool goOnline)
    {
        online = goOnline;
        if (online)
        {
            minPlayers = 2;
            StartMatchMaker();
        } else
        {
            minPlayers = 1;
            StopMatchMaker();
        }
    }
    //Called On Host
    ///Hosts an online game
    public void InitaliseTwo()
    {
        string matchCount = (matches != null) ? matches.Count.ToString() : "Zero";
        hostPlayer = true;
        title.MatchFound = "Hosting game";
        matchMaker.CreateMatch("", 2, true, "", "", "", 0, 0, OnInternetMatchCreate);
        StartCoroutine(SetReady(1));
    }
    //Hosts a new match
    private void OnInternetMatchCreate(bool success, string info, MatchInfo matchInfo)
    {
        if (success)
        {
            MatchInfo hostInfo = matchInfo;
            NetworkServer.Listen(hostInfo, 7777);
            StartHost(hostInfo);
            Debug.Log("Created new match");
        }
        else
        {
            Debug.LogError("Could not start match");
        }
    }
    //Shows available matches
    public void JoinMatch()
    {
        matchMaker.ListMatches(0, 10000, "", false, 0, 0, OnInternetMatchList);
    }
    //Updates text and joins game
    void OnInternetMatchList(bool success, string info, List<MatchInfoSnapshot> matches)
    {
        if (matches.Count > 0)
        {
            Debug.Log("Found game");
            if(title != null)
            title.MatchFound = "Found game";
            matchMaker.JoinMatch(matches[0].networkId, "", "", "", 0, 0, OnJoinMatch);
        } else
        {
            if (title != null)
                title.MatchFound = "No games available";
        }
    }
    //Adds a prefab and starts the game (2 players)oh
    void OnJoinMatch(bool success, string info, MatchInfo matchInfo)
    {
        StartClient(matchInfo);
         lobbySlots[1] = GameObject.Instantiate(lobbyPlayerPrefab, Vector3.zero, Quaternion.identity);
        //Gives error that connection is already set
        //       TryToAddPlayer();
        //   ClearPlayers();
        //   StartCoroutine(SetReady(1));
        if (title != null)
        {
            title.MatchFound = "Connected";
        }
    //    ClientScene.AddPlayer(client.connection, 1);
     //   CmdInstantiatePlayer(1);
    }
    NetworkConnection currentConnection;
    short currentConnectionID;
    //Adds player to server
    public override void OnServerAddPlayer(NetworkConnection conn, short connectionID)
    {
        Debug.Log("Adding player");
        lobbySlots[1] = GameObject.Instantiate(lobbyPlayerPrefab, Vector3.zero, Quaternion.identity);
        NetworkServer.Spawn(lobbySlots[1].gameObject);
    //    lobbySlots[1].SendReadyToBeginMessage();
        //  StartCoroutine(InstantiatePlayer(1));
        currentConnection = conn;
        currentConnectionID = connectionID;
        if (title != null)
        {
            title.MatchFound = "Press Ready to Start";
        }
    //    ClientScene.Ready(conn);
    }
    public override void OnServerDisconnect(NetworkConnection conn)
    {

    }
    //  RaycastHit rayHit;
    //  if(Vector3.Distance(transform.position, rayhit.position) < 5){
    //       }
    public static bool online = false;
    #endregion
    public int PlayersReady
    {
        get{
            int result = 0;
            for (int i = 0; i < lobbySlots.Length; i++) {
                if (lobbySlots[i] != null)
                {
                    //This was buggy so I forced level number
                    if (lobbySlots[i].readyToBegin || LevelManager.LevelNumber != 0)
                    {
                        result++;
                    }
                }
            }
            return result;
        }
    } 
    //Code for readying players on server
    public IEnumerator SetReady(int playersToReady)
    {
        bool forceSwitch = false;
        lobbySlots = new NetworkLobbyPlayer[2];
        for (int i = 0; i < playersToReady; i++)
        {
            if (lobbySlots[i] == null)
            {
                forceSwitch = true;
                lobbySlots[i] = GameObject.Instantiate(lobbyPlayerPrefab, Vector3.zero, Quaternion.identity);
                if (NetworkServer.active)
                {
                    NetworkServer.Spawn(lobbySlots[i].gameObject);
                }
            }
            else
            {
                Debug.Log("player was not null");
            }
            lobbySlots[i].readyToBegin = true;
            Debug.Log(string.Format("Player {0} ready!", i + 1));
        }
        if (players == null)
        {   
            //Limits players ready to the max players
            playersToReady = Mathf.Clamp(playersToReady, 0, maxPlayers);
            //Spawns players for each player in the game
            players = new List<Player>();
        //    players.InitializeBehaviour(this, 0);
            playersToReady += PlayersReady;

         //   Debug.Log(lobbySlots.Length + " players");
            yield return new WaitUntil(()=> (online && PlayersReady >= minPlayers)  || (!online && Input.GetJoystickNames().Length == 0 || Input.GetJoystickNames().Length == 2));
            //Sets up splitscreen
            if (!online && PlayersReady == 2)
            {
                CameraAIv2.setup = CameraSetup.SplitScreen;
            }
            if (forceSwitch)
            {
                Debug.Log(" " + PlayersReady + " players are ready");
                for (int i = 0; i < PlayersReady; i++)
                {
                    StartCoroutine(InstantiatePlayer(i));
                }
            }
        }
        yield return new WaitUntil(()=> players.Count == PlayersReady);
        Debug.Log("loading");
        CheckReady();
    }
    IEnumerator InstantiatePlayer(int i)
    {
        Debug.Log("trying to spawn players");
        GameObject g = GameObject.Instantiate(gamePlayerPrefab, Vector3.zero, Quaternion.identity);
        DontDestroyOnLoad(g);
        Player player = g.GetComponent<Player>();
        if (player != null)
        {
            player.Setup(i + 1, online ? 1 : players.Count + 1);
            Debug.Log(string.Format("{0} spawned!", player.name));
        }
        players.Add(player);
        yield return new WaitUntil(()=> NetworkServer.active);
        NetworkServer.Spawn(g);
        if (i != 0)
        {
            NetworkServer.AddPlayerForConnection(currentConnection, players[players.Count - 1].gameObject, currentConnectionID);
        }
        Debug.Log("spawned on network");
    } 
    void CheckReady()
    {
        //Checks you have the required number of players then starts
        bool b = players != null && PlayersReady >= minPlayers;
        if (b)
        {
            ServerChangeScene(playScene);
        } else if (players != null)
        {
            Debug.Log(b + PlayersReady.ToString());
        }
    }
    //Forces the game to start
    public void ForceReady()
    {
        foreach(NetworkLobbyPlayer nlp in lobbySlots)
        {
            if (nlp != null)
            {
                nlp.readyToBegin = true;
            }
        }
        CheckReady();
    }
    //Clears null players
    public static void ClearPlayers()
    {
        if (players != null)
        {
            foreach (Player p in players)
            {
                if (p != null)
                {
             //       Destroy(p.gameObject);
                }
            }
        }
    }
    //Gets rid of already set as ready
    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        string loadedSceneName = SceneManager.GetSceneAt(0).name;
        if (loadedSceneName == lobbyScene)
        {
            if (client.isConnected)
                CallOnClientEnterLobby();
        }
        else
        {
            CallOnClientExitLobby();
        }
    }
    void CallOnClientEnterLobby()
    {
        OnLobbyClientEnter();
        foreach (var player in lobbySlots)
        {
            if (player == null)
                continue;

          //  player.readyToBegin = false;
            player.OnClientEnterLobby();
        }
    }

    void CallOnClientExitLobby()
    {
        OnLobbyClientExit();
        foreach (var player in lobbySlots)
        {
            if (player == null)
                continue;

            player.OnClientExitLobby();
        }
    }
    //Matrix functions
    public void AddNewPlayer()
    {
        online = true;
        if(players == null)
        {
            players = new List<Player>();
        }
        if (players.Count < maxPlayers)
        {
            lobbySlots[players.Count] = GameObject.Instantiate(lobbyPlayerPrefab, Vector3.zero, Quaternion.identity);
            InstantiatePlayer(players.Count);
        }
    }
    public void JoinNewGame()
    {
        if (!NetworkServer.active)
        {
            SetOnline(true);
            title.MatchFound = "Searching...";
            JoinMatch();
        }
    }
    public void HostNewGame()
    {
        if (!NetworkServer.active)
        {
            //only if you're not currently playing
            SetOnline(true);
            hostPlayer = true;
            matchMaker.CreateMatch("", 2, true, "", "", "", 0, 0, OnInternetMatchCreate);
            StartCoroutine(SetReady(1));
            title.MatchFound = "Hosting game";
        }
    }
}
