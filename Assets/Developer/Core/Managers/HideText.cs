namespace ManagerClasses.Text
{
    using UnityEngine;
    using TMPro;
    using System.Collections.Generic;
    using ManagerClasses.Text.Effects;

    //Cant be static because it destroys
    public abstract class HideText : MonoBehaviour
    {
        static Canvas mainCanvas;
        static List<TextMeshPro> disablerText;
        static List<TextMeshPro> disabledTexts;
        static LayerMask minimap;
        public static LayerMask MiniMap
        {
            get
            {
                if (minimap == 0)
                {
                    minimap = LayerMask.NameToLayer("Minimap");
                }
                return minimap;
            } 
        }
        //Disables all texts not on the main canvas, apart from one
        public static bool MakeMainText(List<TextMeshPro> text, bool textOn = true)
        {
            if (mainCanvas == null)
            {
                mainCanvas = GameObject.Find("Main Canvas").GetComponent<Canvas>();
            }
            //Turns off all text except the texts being passed
            if (text != null &&  text.Count > 0) 
            {
                if (disabledTexts == null)
                {
                    disabledTexts = new List<TextMeshPro>();
                }   
                disablerText = text;
                if (FlipTextColour.allTexts == null)
                {
                    Debug.LogError("Include instruction in Script Execution Order");
                }
                else
                {
                    foreach (TextMeshPro t1 in disablerText)
                    {
                        if (t1 != null)
                        {
                            t1.gameObject.SetActive(textOn);
                            foreach (TextMeshPro t in FlipTextColour.allTexts)
                            {
                            //    Debug.Log(t.GetComponentInParent<Canvas>());
                                if (t != null && t1 != t && t.GetComponentInParent<Canvas>() != mainCanvas
                                    && t.gameObject.layer != MiniMap && t.text != t1.text)
                                {
                                    disabledTexts.Add(t);
                                    t.gameObject.SetActive(false);
                                }
                            }
                        }
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool MakeMainText(GameObject g, bool textOn = true)
        {
            TextMeshPro t = g.GetComponent<TextMeshPro>();
            List<TextMeshPro> list = new List<TextMeshPro>();
            list.Add(t);
            return MakeMainText(list, textOn);
        }
        //When an event is completed, it sets the text back on
        public static void EnableAllTexts()
        {
        //    Debug.Log(string.Format("disabling {0} texts", disabledTexts.Count));
            if (disabledTexts != null && disabledTexts.Count > 0)
            {
                //Turns on the temporarily disabled texts
                foreach (TextMeshPro t in disabledTexts)
                {
                    if (t != null)
                    {
                        t.gameObject.SetActive(true);
                    }
                }
                //Gets rid of the main text
         //       Debug.Log(disablerText.Count + "disabler texts");
                foreach (TextMeshPro t1 in disablerText)
                {
                 //   Debug.Log(t1);
                    if (t1 != null && t1.transform.parent.gameObject != null)
                    {
                    //    Debug.Log("removing " + t1.name);
                        if (t1.gameObject.activeSelf)
                        {
                            string s = t1.name;
                            FlipTextColour.DestroyText(s);
                        }
                        else
                        {
                            t1.gameObject.SetActive(true);
                        }
                    }
                }
            }
            //  disablerText.gameObject.SetActive(false);
        }
    }
}
