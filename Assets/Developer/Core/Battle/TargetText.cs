using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TargetText : MonoBehaviour, IPointerDownHandler
{
    public Color targetedColour, notTargetedColour;
    [HideInInspector]
    public AI ai;
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        Targeter targeter = FindObjectOfType<Targeter>();
        targeter.Target = ai;
    }
    UIColourLerp text;
    public void Color(bool targeted)
    {
        if(text == null)
        {
            text = GetComponent<UIColourLerp>();
        }
        text.end = (targeted) ? targetedColour : notTargetedColour;
    }
}
