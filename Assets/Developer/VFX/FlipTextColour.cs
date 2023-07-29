namespace ManagerClasses.Text.Effects
{
    using System.Collections.Generic;
    using UnityEngine;
    using TMPro;
    //Will be separated into flip text function and text manager later
    public class FlipTextColour : MonoBehaviour
    {
        public bool startDark = true;
        static bool dark = true;
        public static List<TextMeshPro> allTexts;
        public static List<TextMeshPro> questTexts;
        public static List<TextMeshProUGUI> canvasTexts;
        // Use this for initialization
        void Start()
        {
            if (questTexts == null)
            {
                questTexts = new List<TextMeshPro>();
            }
            allTexts = new List<TextMeshPro>();
            canvasTexts = new List<TextMeshProUGUI>();
            canvasTexts.AddRange(FindObjectsOfType<TextMeshProUGUI>());
            if (!startDark)
            {
                Flip(dark);
            }
            allTexts.AddRange(FindObjectsOfType<TextMeshPro>());
        }
        public static void ClearQuests()
        {
            questTexts.Clear();
        }
        static bool lastUsed;
        public static void AutoDark()
        {
            lastUsed = dark;
            Flip(false);

        }
        public static void ReturnColour()
        {
            Flip(!lastUsed);
        }
        public static void SetQuestText(TextMeshPro text)
        {
            if (text != null)
            {
                if (allTexts.Contains(text))
                {
                    allTexts.Remove(text);
                }
                questTexts.Add(text);
            //    Debug.Log("setting " + text + " " + questTexts.Count);
                text.gameObject.SetActive(false);
                GameObject g = GameObject.Find(text.name);
                while (g != null)
                {
                    TextMeshPro t = g.GetComponent<TextMeshPro>();
                    if (allTexts.Contains(t))
                    {
                        allTexts.Remove(t);
                    }
                    questTexts.Add(t);
                    g.SetActive(false);
                    g = GameObject.Find(text.name);
                }
                foreach (TextMeshPro t in questTexts)
                {
                    t.gameObject.SetActive(true);
                 //   Debug.Log(t.transform.parent.parent.name + t.name);
                }
            }
        }
        public static List<TextMeshPro> GetQuestTexts(string s)
        {
            List<TextMeshPro> results = new List<TextMeshPro>();
            foreach(TextMeshPro t in questTexts)
            {
                if(t.name == s)
                {
                    results.Add(t);
                }
            }
            return results;
        }
        //quest texts do not flip colour
    static void Flip(bool lightNotDark)
        {
            List<TextMeshPro> textsToRemove = new List<TextMeshPro>();
            foreach (TextMeshPro t in allTexts)
            {
                if (t == null)
                {
                    textsToRemove.Add(t);
                }
                else
                {

                    if (t.transform.parent != null && t.gameObject.layer != HideText.MiniMap && t.transform.parent.name != "Button")
                    {
                        t.color = (lightNotDark) ? Color.white : Color.black;
                    }
                }
            }
            List<TextMeshProUGUI> textsToRemove2 = new List<TextMeshProUGUI>();
            foreach (TextMeshProUGUI t in canvasTexts)
            {
                if (t == null)
                {
                    textsToRemove2.Add(t);
                }
                else
                {

                    if (t.transform.parent != null && t.gameObject.layer != HideText.MiniMap && t.transform.parent.name != "Button")
                    {
                        t.color = (lightNotDark) ? Color.white : Color.black;
                    }
                }
            }
            foreach (TextMeshProUGUI t in textsToRemove2)
            {
                canvasTexts.Remove(t);
            }
            dark = !lightNotDark;
        }
        public static void DestroyText(string s)
        {
            //Check for other instances of the quest text
            List<TextMeshPro> oldQuests = new List<TextMeshPro>();
       //     Debug.Log(questTexts.Count);
            foreach (TextMeshPro t in questTexts)
            {
            //    Debug.Log(t.name + s);
                if (t.name == s)
                {
                    oldQuests.Add(t);
                }
            }
            foreach (TextMeshPro t in oldQuests)
            {
               // Debug.Log(t.text);
                questTexts.Remove(t);
                Destroy(t.transform.gameObject);
            }
        }

        // Update is called once per frame
        void Update()
        {
            //Shouldnt really remove from a list while youre using it
            if (Application.isEditor)
            {
                if (Input.GetButtonDown("Flip Text"))
                {
                    Flip(dark);
                }
            }
        }
    }
}
