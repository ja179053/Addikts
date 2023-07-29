using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ArrowController : MonoBehaviour {

    //References to the arrow hover scripts
    static Hover[] arrows;
    public static Button[] battleChoices;
    // Use this for initialization
    void Start () {
        arrows = GetComponentsInChildren<Hover>();
        for (int i = 0; i < arrows.Length; i++)
        {
            if (i < MultiplayerManager.multiplayerSingleton.minPlayers)
            {
                arrows[i].GetComponent<Image>().color =
                    i == 0 ? Color.blue : Color.red;
            }
            else
            {
                arrows[i].gameObject.SetActive(false);
            }
        }
    }
    bool isPlaying = false;
    public IEnumerator ChangeBattleChoice(int controlID)
    {
        if (!isPlaying)
        {
            while (Mathf.Abs(Input.GetAxis("Horizontal " + controlID)) == 1)
            {
                isPlaying = true;
                SetArrowPosition((int)arrowPositions[controlID].x + 1, controlID - 1);
                yield return new WaitForSeconds(0.2f);
            }
            isPlaying = false;
        }
    }
    public IEnumerator ChangeTargetChoice(TextMeshProUGUI t, int controlID)
    {
        if (!isPlaying)
        {
            while (Mathf.Abs(Input.GetAxis("Vertical " + controlID)) == 1)
            {
                isPlaying = true;
                Action(t, controlID);
                yield return new WaitForSeconds(0.1f);
            }
            isPlaying = false;
        }
    }
    static Dictionary<int, Vector2> arrowPositions;
    void SetArrowPosition(int position, int playerNumber = 1)
    {
        SetUpArrows();
        if (position >= battleChoices.Length)
        {
            position = 0;
        }
        arrowPositions[playerNumber + 1] = new Vector2(position, arrowPositions[playerNumber + 1].y);
        Action(battleChoices[position].transform, playerNumber);
    }
    public int inventoryPos = -1;
    public static void Action(TextMeshProUGUI target, int playerNumber)
    {
        if (arrowPositions != null && target != null)
        {
            Action(target.transform, playerNumber - 1);
        }
    }
    public static void Action(TargetText target, int playerNumber)
    {
        if (arrowPositions != null)
        {
            Action(target.transform, playerNumber - 1);
        }
    }
    public static void Action(int i, int playerNumber)
    {
        if (arrowPositions != null)
        {
            Action(battleChoices[i].transform, playerNumber - 1);
        }
    }
    static void Action(Transform t, int playerNumber)
    {
      //  Debug.Log(playerNumber + " " + arrows.Length);
        arrows[playerNumber].target = t;
    }
    public static int GetPosition(int playerNumber)
    {
        int result = 0;
        if (arrowPositions != null && playerNumber <= arrowPositions.Count)
        {
            result = (int)arrowPositions[playerNumber].x;
        }
        return result;
    }
    public void SetUpArrows()
    {
        if (arrowPositions == null)
        {
            arrowPositions = new Dictionary<int, Vector2>();
            for (int i = 0; i < MultiplayerManager.multiplayerSingleton.minPlayers; i++)
            {
                arrowPositions.Add(i + 1, -Vector2.up);
            }
        }
        battleChoices = GetComponentsInChildren<Button>();
    }    
}
