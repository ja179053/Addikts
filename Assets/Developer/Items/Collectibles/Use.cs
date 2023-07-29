using UnityEngine.EventSystems;
using Players;

public class Use : Buy, IPointerDownHandler
{
    //For now this will automatically make the user target a random enemy/ally

/// <summary>
/// When the player uses smelling salts, it restorex max health (1 HP)
/// The player clicks on the item button
/// A list of available items appears
/// The player clicks on an item,
/// This script tells the player script which item to use
/// (For now, only the player will use items)
/// </summary>
/// <param name="eventData"></param>
    public override void OnPointerDown(PointerEventData eventData)
    {
        Player player = MultiplayerManager.players[0];
        player.UseItem(this.name);
    }
}
