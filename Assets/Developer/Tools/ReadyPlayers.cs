using UnityEngine;

public class ReadyPlayers : MonoBehaviour {

    public void Ready(int i)
    {
        Debug.Log("clicked!");
        MultiplayerManager manager = FindObjectOfType<MultiplayerManager>();
        StopCoroutine(manager.SetReady(i));
    //    MultiplayerManager.ClearPlayers();
        StartCoroutine(manager.SetReady(i));
    }
    public void FlagReady()
    {

    }
    public void ForceReady()
    {
        MultiplayerManager.multiplayerSingleton.ForceReady();
    }
}
