using UnityEngine;
using TMPro;

public class TitleScreen : MonoBehaviour {
    public float lightSpeed = 1;
    Light l;
    //Gameobjects for local player setup
    public GameObject players, startButton;
    //GameObjects for online setup
    public GameObject onlineManager,onlineButton, serverButtons, hostButton;
    public TextMeshProUGUI matchCount;
    public string MatchFound
    {
        set { matchCount.text = value; }
    }
    public void StartGame()
    {
        l = FindObjectOfType<Light>();
        MultiplayerManager.multiplayerSingleton.Initialise();
        startButton.SetActive(false);
        onlineButton.SetActive(false);
        players.SetActive(true);
    }
    public void OnlineGame()
    {
        l = FindObjectOfType<Light>();
        startButton.SetActive(false);
        onlineButton.SetActive(false);
        serverButtons.SetActive(true);
        MultiplayerManager.multiplayerSingleton.SetOnline(true);
        matchCount = GameObject.Find("Matches").GetComponent<TextMeshProUGUI>();
    }
    public void Back()
    {
        startButton.SetActive(true);
        onlineButton.SetActive(true);
        serverButtons.SetActive(false);
        players.SetActive(false);
        MultiplayerManager.multiplayerSingleton.SetOnline(false);

    }
    public void HostGame()
    {
        MultiplayerManager manager = FindObjectOfType<MultiplayerManager>();
          manager.InitaliseTwo();
      //  manager.CreateMatchHack();
        hostButton.SetActive(false);
    }
    void Update()
    {
        if (l != null)
        {
            l.intensity -= lightSpeed * Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Application.isEditor)
            {
                Debug.Break();
            }
            else
            {
                Application.Quit();
            }
        }
    }
}
