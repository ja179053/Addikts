namespace ManagerClasses
{
    using System.Collections.Generic;
    using UnityEngine;
    using Players;
    using TMPro;
    using ManagerClasses.Text.Effects;
    //UI manager used to show inventories of shops, players and allies
    public class ShopManager : MonoBehaviour
    {
        static ShopManager singleton;
        public GameObject textPrefab, useItemPrefab;
        public int ySpacing = 1, xSpacing = 10;
        public Vector2 offset;
        public AudioClip enterShop, leaveShop;
        List<Item> allWares;
        GameObject shopMenu;
        void Start()
        {
            if(singleton == null)
            {
                singleton = this;
            //    Debug.Log("shop manager loaded");
                shopMenu = GameObject.Find("ShopMenu");
                if (shopMenu == null)
                {
                    Debug.LogError("YOU FORGOT TO TURN SHOP MENU ON IN THE CANVAS");
                    Debug.Break();
                }
                shopMenu.transform.parent.gameObject.SetActive(false);
                allWares = new List<Item>();
            } else
            {
                Destroy(this);
            }
        }
        #region Shopping
        //Shopping function MAKING A SHOPMANAGER
        /// wares are the items to be displayed for sale
        /// You cannot use instance references in a static function
        /// (e.g. instantiate prefabs)
        public void StartShopping(List<Item> wares, string shopName)
        {
            FlipTextColour.AutoDark();
            //Shows a shop menu with the wares
            Manager.GameState = ManagerStates.Shopping;
            Character.subtitles.AddMessage(string.Format(
                "Welcome to {0}!", shopName), 2, enterShop);
            shopMenu.GetComponent<TextMeshProUGUI>().text = shopName;
            ShowUI(wares, shopMenu.transform, textPrefab, offset, new Vector2(xSpacing, ySpacing), 2);
            shopMenu.transform.parent.gameObject.SetActive(true);
        }
        //At the end of shopping, a pre-programmed event can happen
        //Short strings are faster :)
        public void StopShopping()
        {
            FlipTextColour.ReturnColour();
            ForceClear();
            allWares = new List<Item>();
            shopMenu.transform.parent.gameObject.SetActive(false);
            Character.subtitles.AddMessage(
                "Thank you, come again!!", 5, leaveShop);
            Manager.GameState = ManagerStates.Exploration;
            EventManager.Trigger("Shopped");
        }
        public void Buy(string s)
        {
            Item foundItem = FindItem(allWares, s);
            if (foundItem != null)
            {
                Player p = FindObjectOfType<Player>();
                p.GetLoot(foundItem);
                foundItem.ClearTexts();
                allWares.Remove(foundItem);
            }
        }
        //Works the first time, but not the others
        public static Item FindItem(List<Item> wares, string name)
        {

            for (int i = wares.Count - 1; i >= 0; i--)
            {
                if (wares[i].Name == name)
                {
                    return wares[i];
                }
            }
            return null;
        }
        #endregion
        bool Same(List<Item> wares)
        {
            if (wares.Count != allWares.Count)
            {
                return false;
            }
            for (int i = 0; i < allWares.Count; i++)
            {
                if (wares[i] != allWares[i])
                {
                    return false;
                }
            }
            return true;
        }
        void ForceClear()
        {
            shopMenu.BroadcastMessage("Destroy");
        }
        #region TextBased
        public static bool showingInventory = false;
        public void ShowInventory(List<Item> wares, Transform newTransform, Visibility action)
        {
            switch (action)
            {
                case Visibility.Opposite:
                    showingInventory = !showingInventory;
                    if (showingInventory)
                    {
                        goto Show;
                    }
                    else
                    {
                        HideUI();
                    }
                    break;
                case Visibility.Update:
                    if (showingInventory)
                    {
                        HideUI();
                        goto Show;
                    }
                    break;
                case Visibility.Hide:
                    showingInventory = false;
                    HideUI();
                    break;
                case Visibility.Show:
                    showingInventory = true;
                    if (showingInventory)
                    {
                        ShowInventory(wares, newTransform, Visibility.Update);
                    } else
                    {
                        goto Show;
                    }
                    break;
                    Show:
                    BattleTargets targets = Manager.battlePanel.GetComponent<BattleTargets>();
                    ShowUI(wares, targets.transform, useItemPrefab, targets.offset, targets.spacing);
                    break;
            }
        }
        void HideUI(bool destroy = false)
        {
         //   Debug.Log("Hiding");
            foreach (Item ware in allWares)
            {
                ware.ClearTexts(destroy);
            }
        }
        void ShowUI(List<Item> wares, Transform newTransform, GameObject prefab, Vector2 newOffset, Vector2 spacing, int multiplier = 1)
        {
            bool same = Same(wares);
            if (!same)
            {
                HideUI(true);
                allWares = new List<Item>();
            }
            //inventory doesnt show cost...
            for (int i = 0; i < wares.Count; i++)
            {
                //The problem is, wares 0 is being set
                ///THEN wares 1 is being set to the same thing
                ///THEN wares 2 is being set to the same thing
                ///so when i delete the item for wares 2, it thinks they are all deleted
                DisplayWare(wares[i], newTransform, prefab, i, !same, multiplier, newOffset, spacing);
                allWares.Add(wares[i]);
            }
        }
        void DisplayWare(Item item, Transform newTransform, GameObject prefab, int i, bool newItem, int multiplier, Vector2 newOffset, Vector2 spacing)
        {
            if (newItem)
            {
                SpawnText(Allignment.Left, item, newTransform, prefab, multiplier, i, newOffset, spacing);
                if (prefab == textPrefab)
                {
                    SpawnText(Allignment.Middle, item, newTransform, prefab, multiplier, i, newOffset, spacing);
                }
                SpawnText(Allignment.Right, item, newTransform, prefab, multiplier, i, newOffset, spacing);
            }
            else
            {

                item.nameText.color = new Vector4(item.nameText.color.r, item.nameText.color.g, item.nameText.color.b, 1);
                if (prefab == textPrefab)
                {
                    item.costText.color = new Vector4(item.nameText.color.r, item.nameText.color.g, item.nameText.color.b, 1);
                }
                item.quantityText.color = new Vector4(item.nameText.color.r, item.nameText.color.g, item.nameText.color.b, 1);

            }
        }
        Vector3 spawnPos;
        void SpawnText(Allignment pos, Item item, Transform newTransform, GameObject prefab, int multiplier, int i, Vector2 newOffset, Vector2 spacing)
        {
            Vector3 finalSpawnPos = (spawnPos == null) ? newTransform.localPosition : spawnPos;
            switch (pos)
            {
                case Allignment.Left:
                    item.nameText = GameObject.Instantiate(prefab, newTransform).GetComponent<TextMeshProUGUI>();
                    item.nameText.transform.localPosition = newOffset + new Vector2(spacing.x * 0, spacing.y * -(i + 1));
                    item.nameText.text = item.nameText.name = item.Name;
                    break;
                case Allignment.Middle:
                    item.costText = GameObject.Instantiate(prefab, newTransform).GetComponent<TextMeshProUGUI>();
                    item.costText.transform.localPosition = newOffset + new Vector2(spacing.x * 1 * multiplier, spacing.y * -(i + 1));
                    item.costText.text = item.cost.ToString();
                    break;
                case Allignment.Right:
                    item.quantityText = GameObject.Instantiate(prefab, newTransform).GetComponent<TextMeshProUGUI>();
                    item.quantityText.transform.localPosition = newOffset + new Vector2(spacing.x * 2 * multiplier, spacing.y * -(i + 1));
                    item.quantityText.text = item.quantity.ToString();
                    break;
            }
        }
        #endregion
    }
}
enum Allignment
{
    Left, Middle, Right
}
public enum Visibility
{
    Opposite,
    Update,
    Show,
    Hide
}
