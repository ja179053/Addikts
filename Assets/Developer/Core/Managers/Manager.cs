namespace ManagerClasses
{
    using UnityEngine;
    using ManagerClasses.Music;
    using ManagerClasses.Text;
    using ManagerClasses.Text.Effects;

    //This is the batttle manager and exploration manager
    public class Manager : MonoBehaviour
    {
        public static GameObject bestiary;
        public static MusicManager music;
        public static GameObject gameOver, battlePanel, pausePanel, settingsPanel;
        static ManagerStates gameState, lastState;
        //When a state is changed, the timescale is changed
        public static ManagerStates GameState
        {
            get
            {
                return gameState;
            }
            set
            {
                gameState = value;
                switch (value)
                {
                    case ManagerStates.Battle:
                        Freeze(false);
                        battlePanel.SetActive(true);
                        CameraAIv2.ZoomTo(true);
                        CameraAIv2.Activate(1, 2);
                        break;
                    case ManagerStates.Shopping:
                        //You cant pause while shopping, it's too rude.
                        DisablePanel(battlePanel);
                        CameraAIv2.ZoomTo(true);
                        break;
                    case ManagerStates.GameOver:
                        FlipTextColour.ClearQuests();
                        DisablePanel(battlePanel);
                        gameOver.SetActive(true);
                        HideText.MakeMainText(gameOver);
                     //   Freeze(true);
                        CameraAIv2.mainCamera.GetComponent<CameraAIv2>().Run();
                        break;
                    case ManagerStates.Paused:
                        DisablePanel(battlePanel);
                        break;
                    case ManagerStates.Exploration:
                        DisablePanel(battlePanel);
                        CameraAIv2.ZoomTo(false);
                        CameraAIv2.Activate(0);
                        Freeze(false);
                        break;

                }
                if (music != null){
                    music.Play(value);
                }
            }
        }
        public static void Pause()
        {
            //When pause is pressed, the manager checks if it is already paused
            //Then it sets the state to paused, or the last state it was in.
            //Then it updates the last state it was in.
            ManagerStates thisState = Manager.GameState;
            bool wasntPaused = !(Manager.GameState != ManagerStates.Paused);
            Manager.GameState = (!wasntPaused)
                ? ManagerStates.Paused : lastState;
            lastState = thisState;
            //   Debug.Log(wasntPaused ? "PAUSED" : "UNPAUSED");
            pausePanel.SetActive(!wasntPaused);

            Freeze(!wasntPaused);
        }
        static void Freeze(bool freeze)
        {
            Time.timeScale = (freeze) ? 0 : 1;
        }
        static void DisablePanel(GameObject g)
        {
            if (g == null)
            {
                Debug.LogError("TURN ON THE PANEL");
            }
            g.SetActive(false);
        }
        static void DisableAll()
        {
            DisablePanel(settingsPanel);
            DisablePanel(gameOver);
            DisablePanel(battlePanel);
            DisablePanel(pausePanel);
            DisablePanel(bestiary);
        }
        // Use this for initialization
        //Battle panel needs to be handled in a battle manager
        public static Manager singleton;
        void Start()
        {
            if (singleton == null)
            {
                singleton = this;
                music = FindObjectOfType<MusicManager>();
                bestiary = GameObject.Find("Bestiary");
                gameOver = GameObject.Find("Game Over");
                battlePanel = GameObject.Find("Battle Panel");
                pausePanel = GameObject.Find("Pause Panel");
                settingsPanel = GameObject.Find("Settings Panel");
                GameState = lastState = ManagerStates.Exploration;
                if (music == null)
                {
                    music = gameObject.AddComponent<MusicManager>();
                }
                DisableAll();
            }
            else
            {
                Destroy(this);
            }
        }
        public static void GameOver()
        {
            GameState = ManagerStates.GameOver;
        }
        public static bool Frozen()
        {
            return Time.timeScale == 0;
        }
        public void PlaySound(ManagerStates newState)
        {
            GameState = newState;
            music.PlaySound(newState);
        }
        public static void ShowBattleMenu()
        {
            battlePanel.SetActive(!battlePanel.activeSelf);
        }
    }
    public enum ManagerStates
    {
        Exploration,
        Battle,
        Paused,
        Shopping,
        GameOver
    }
}
