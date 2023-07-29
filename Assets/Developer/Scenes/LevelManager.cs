using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelManager {
    public static void Menu()
    {
        CameraAIv2.currentCamera = null;
        MultiplayerManager.multiplayerSingleton.StopHost();
        SceneManager.LoadScene(0);
    }
    public static void NextLevel()
    {
        int i = 1 + SceneManager.GetActiveScene().buildIndex;
        if(i > SceneManager.sceneCountInBuildSettings)
        {
            Application.Quit();
            i = 0;
        }
        SceneManager.LoadScene(i);
    }
    public static int LevelNumber
    {
        get
        {
            return SceneManager.GetActiveScene().buildIndex;
        }
    }
}
