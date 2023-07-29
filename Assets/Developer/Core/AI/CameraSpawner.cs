using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CameraSpawner : MonoBehaviour
{
    public GameObject cameraMan, quests, enemies, allies;
    List<CameraAIv2> list;
    // Use this for initialization
    void Start()
    {
        //Actual camera spawning
        //   Debug.Log(MultiplayerManager.multiplayerSingleton.PlayersReady);
        int camsToLoad = (CameraAIv2.setup == CameraSetup.SplitScreen ? 2 : 1);
        for (int i = 0; i < camsToLoad; i++)
        {
            GameObject g = Instantiate(cameraMan, transform);
            g.name = "Director " + (i + 1);
        }
        CameraAIv2[] cams = FindObjectsOfType<CameraAIv2>();
        for (int i = 0; i < cams.Length; i++)
        {
            int j;
            string s = cams[i].name[cams[i].name.Length - 1].ToString();
         //   Debug.Log(cams[i].name + s);
            if (!int.TryParse(s, out j))
            {
                Debug.LogError("You left director in the scene... great job");
            }
            if (!MultiplayerManager.online)
            {
                camsToLoad = j;
            }
            StartCoroutine(cams[i].SetID(camsToLoad));
        }
        if (quests != null)
        {
            quests.SetActive(true);
        }
        if (enemies != null)
        {
            enemies.SetActive(true);
        }
        if (allies != null)
        {
            allies.SetActive(true);
        }
    }
}
