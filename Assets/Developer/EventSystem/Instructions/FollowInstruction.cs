using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ManagerClasses.Text;
using ManagerClasses.Text.Effects;
using TMPro;
//This class is for the instruction "Events". You add this to a delegate
//When an instruction is done, the delegate is triggered, event is successful
//Remember to add instructions to the Script Execution Order
public abstract class FollowInstruction : EventMain {
    public QuestEnum questType;
    protected float waitTime = 0;
    public List<TextMeshPro> myInstruction;
    public bool textOn = true;
    int InstructionCount
    {
        get
        {
            int i = 0;
            foreach (TextMeshPro tmp in myInstruction)
            {
                if (tmp != null)
                {
                    i++;
                }
            }
            return i;
        }
    }
    protected override bool Setup()
    {
        //Instruction is set up here
        //Cannot share a canvas
        if (myFunction == null || myFunction == string.Empty)
        {
            Debug.LogError("No function for " + name);
        }
        if(InstructionCount == 0)
        {
            myInstruction.Clear();
                GetInstruction();
        } else if (!textOn)
        {
            foreach (TextMeshPro t in myInstruction)
            {
                t.gameObject.SetActive(!t.gameObject.activeSelf);
            }
        }
        StartCoroutine(HideTexts());
        return base.Setup();
    }
    protected bool GetInstruction()
    {
        //  Debug.Log(myInstruction[0]);
        myInstruction = FlipTextColour.GetQuestTexts(myFunction);
        if (myInstruction.Count == 0)
        {
            GameObject g = GameObject.Find((int)questType + myFunction + "Instruction");
            if (g == null)
            {
                Debug.LogError(string.Format("You didn't set up an instruction for {0}. Are you sure it has it has its own canvas?", myFunction));
                return false;
            }
            else
            {
                myInstruction.Add(g.GetComponent<TextMeshPro>());
                FlipTextColour.SetQuestText(myInstruction[0]);
                //  Debug.Log(name + "turning off " + myInstruction.name);
            }
        }
        return true;
    }
    protected void EnableAllTexts()
    {
        HideText.EnableAllTexts();
    }
    protected IEnumerator HideTexts()
    {
        yield return new WaitForSeconds(waitTime);
     //   Debug.Log("trying to hide " + myFunction + waitTime);
        if (!HideText.MakeMainText(myInstruction, textOn))
        {
            Debug.LogWarning("ONE TUTORIAL AT A TIME " + myFunction);
        }
    }
}
