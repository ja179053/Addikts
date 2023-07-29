using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class BattleTargets : MonoBehaviour
{
    public static ArrowController arrow;
    List<TargetText> texts;
    Slider[] battleSliders;
    public Vector2 spacing = new Vector2(0, 10);
    public Vector3 offset;
    int maxEnemies;
    public GameObject enemyTextPrefab;

    // Use this for initialization
    void Start()
    {
        arrow = GetComponent<ArrowController>();
        if(arrow == null)
        {
            arrow = gameObject.AddComponent<ArrowController>();
        }
        ResetList();
    }
    public void Setup(List<AI> list)
    {
        battleSliders = GetComponentsInChildren<Slider>();
        //   Debug.Log(list.Count + " health bars " + battleSliders.Length + " sliders");
        int listCount = Mathf.Clamp(list.Count, 0, battleSliders.Length - 1);
        for (int i = 0; i < listCount; i++)
        {
            battleSliders[i].GetComponentInChildren<TextMeshProUGUI>().text = list[i].nameString;
            list[i].timer = battleSliders[i];
        }
        for (int i = list.Count; i < battleSliders.Length; i++)
        {
            battleSliders[i].gameObject.SetActive(false);
        }
        arrow.SetUpArrows();
    }

    // Update is called once per frame
    public void UpdateMe(List<AI> l)
    {
        if (l == null)
        {
            ResetList();
        }
        else
        {
            for (int i = 0; i < l.Count; i++)
            {
                Enemy2 enemy = (Enemy2)l[i];
                if (enemy != null)
                {
                    if (!texts.Contains(enemy.myText))
                    {
                        enemy.myText = GameObject.Instantiate(enemyTextPrefab, transform).GetComponent<TargetText>();
                        enemy.myText.GetComponent<RectTransform>().position = offset;
                        enemy.myText.transform.localPosition +=
                            new Vector3(transform.position.x, transform.position.y + (i * -spacing.y));
                        enemy.myText.GetComponent<TextMeshProUGUI>().text = enemy.nameString;
                        enemy.myText.ai = enemy;
                        texts.Add(enemy.myText);
                    }
                    else
                    {
                        int incredible = texts.IndexOf(enemy.myText);
                        if (texts[incredible] == null)
                        {
                            Destroy(texts[incredible]);
                        }
                    }
                }
            }
        }
    }
    void ResetList()
    {
        if (texts != null)
        {
            foreach (TargetText g in texts)
            {
                if (g != null)
                {
                    Destroy(g);
                }
            }
        }
        texts = new List<TargetText>();
    }
}
