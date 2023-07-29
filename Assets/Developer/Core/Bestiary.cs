using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Bestiary : MonoBehaviour
{
    public GameObject bestiaryText;
    static List<BestiaryEntry> entries;
    static bool latest = true;
    public static void NewEnemy(Enemy2 enemy)
    {
        if (entries == null)
        {
            entries = new List<BestiaryEntry>();
        }
        BestiaryEntry be = new BestiaryEntry(enemy);
        bool updated = false;
        foreach(BestiaryEntry oldEntry in entries)
        {
            if(oldEntry.Comparable == be)
            {
                oldEntry.killedCount++;
                updated = true;
                break;
            }
        }
        if (!updated)
        {
            entries.Add(be);
        }
        latest = false;
    }
    void OnEnable()
    {
        if (!latest)
        {
            for(int i =0; i < entries.Count; i++)
            {
                if(entries[i].enemyName == null)
                {
                    entries[i].SpawnText(bestiaryText, transform, Vector3.zero, i);
                } else
                {
                    entries[i].UpdateText();
                }
            }
        }
    }
    class BestiaryEntry
    {
        public TextMeshProUGUI enemyName, lore, killed;
        public int killedCount = 0;
        public string name;
        public BestiaryEntry Comparable;
        public BestiaryEntry(Enemy2 enemy)
        {
            name = enemy.nameString;
            Comparable = this;
        }
        public void UpdateText()
        {
            killed.text = killedCount.ToString();
        }
        public void SpawnText(GameObject prefab, Transform parent, Vector3 offset, int height)
        {
            enemyName = GameObject.Instantiate(prefab, parent).GetComponent<TextMeshProUGUI>();
            enemyName.gameObject.transform.localPosition = offset;
            enemyName.gameObject.transform.localPosition += Vector3.up * height;
            enemyName.text = name;
            killed = GameObject.Instantiate(prefab, parent).GetComponent<TextMeshProUGUI>();
            killed.gameObject.transform.localPosition = offset + (Vector3.right * 200);
            killed.gameObject.transform.localPosition += Vector3.up * height * 50;
            killed.text = killedCount.ToString();
        }
        public void Clear()
        {
            if(enemyName != null)
            {
                Destroy(enemyName);
            }
            if (lore != null)
            {
                Destroy(lore);
            }
        }
    }
}
