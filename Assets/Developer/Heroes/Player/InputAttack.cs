using Players;
using UnityEngine;

public class InputAttack : MonoBehaviour {
    public void PlayerAttack()
    {
        Player player = MultiplayerManager.players[0];
        player.ClickTarget();
    }
}
