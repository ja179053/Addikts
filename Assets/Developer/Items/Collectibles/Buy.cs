using UnityEngine;
using UnityEngine.EventSystems;
using ManagerClasses;

public class Buy : MonoBehaviour, IPointerDownHandler {
    public AudioClip buySound;
    //Ok I forgot how to use OnPointerDown 
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        ShopManager manager = FindObjectOfType<ShopManager>();
        manager.Buy(this.name);
        Character.notifications.AddMessage(string.Format("Bought {0}.", this.name), 2, buySound);
    }
    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
