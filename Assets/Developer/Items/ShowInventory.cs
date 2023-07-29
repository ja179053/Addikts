using Players;
using UnityEngine;
//UPDATE TO WORK FOR ALL PLAYERS AND ALLIES
public class ShowInventory : MonoBehaviour {
    int lastShown = 0;
    public void ShowPlayerInventory()
    {
        int i = lastShown + 1;
        if(i >= MultiplayerManager.multiplayerSingleton.PlayersReady)
        {
            i = 0;
        }
        Player p = MultiplayerManager.players[i];
        lastShown = i;
        StartCoroutine(p.ShowInventory());
    }
}
