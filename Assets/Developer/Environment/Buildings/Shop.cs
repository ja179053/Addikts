using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManagerClasses;

public class Shop : MonoBehaviour {
    static ShopManager manager;
    public AudioClip shopkeeperVO, nooneHereVO;
    public bool autoShop;
    public List<Item> wares;
    public string shopkeeperSpeech;
    public int priceMultiplier = 1;
    bool canShop;
    void Start()
    {
        if (manager == null)
        {
            manager = FindObjectOfType<ShopManager>();
        }
        foreach(Item item in wares)
        {
            item.cost *= priceMultiplier;
        }
    }
    public float waitTime = 5;
    bool wait = false;
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitTime);
        wait = false;
    }
    void OnTriggerEnter(Collider c)
    {
        if (!wait && c.gameObject.tag == "Hero")
        {
            if (wares.Count > 0)
            {
                canShop = wait = true;
                StartCoroutine(Wait());
                //Can send a message to the subtitles when you get close enough
                if (shopkeeperSpeech == "")
                {
                    shopkeeperSpeech = string.Format(
                        "{0}'s for sale!", wares[0].name);
                }
                Character.subtitles.AddMessage(shopkeeperSpeech, 2, shopkeeperVO);
            }
            else
            {
                Character.subtitles.AddMessage(
                    "There's no-one selling anything here...", 3, nooneHereVO);
            }
        }
    }
    void OnTriggerExit(Collider c)
    {
        if (c.gameObject.tag == "Hero")
        {
            canShop = false;
        }
    }
    void Update()
    {
        //THe shop can be used at any time. Just dont let your friends die.
        if(Manager.GameState != ManagerStates.Shopping)
        {
            if (canShop)
            {
                if(autoShop || Input.GetButton("Action"))
                {
                    if(manager == null)
                    {
                        manager = FindObjectOfType<ShopManager>();
                    }
                    EventManager.Trigger("Interact");
                            manager.StartShopping(wares, name);
                }
            }
        }
    }
}
