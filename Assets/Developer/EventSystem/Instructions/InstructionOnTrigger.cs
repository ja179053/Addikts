namespace Instruction
{
    using UnityEngine;
    using TMPro;

    public class InstructionOnTrigger : InstructionAction
    {
        public string function;
        public Collider[] colliders;
        Collider myCollider;
        protected override bool Setup()
        {
            if (colliders == null)
            {
                myCollider = GetComponent<Collider>();
            }
            else
            {
                foreach (Collider c in colliders)
                {
                    ColliderBridge cb = c.gameObject.AddComponent<ColliderBridge>();
                    cb.instruction = this;
                }
            }
            if (myCollider == null)
            {
                colliders = GetComponentsInChildren<Collider>();

                foreach (Collider c in colliders)
                {
                    ColliderBridge cb = c.gameObject.AddComponent<ColliderBridge>();
                    cb.instruction = this;
                }
            }
            //Prevents setup on start
            //This is to prevent Same Time instrctions. One instruction at a time.
            //    if (!ActionSetup(function))
            //    { 
            myFunction = function;
            if (GetInstruction())
            {
                foreach (TextMeshPro t in myInstruction)
                {
                    if (t != null)
                    {
                    //    Debug.Log("turned off " + t.name);
                        t.color = Functions.Invisible(t.color, Visibility.Hide);
                        //Something else turns this gameobject on, probably in the FlipTextColour
                     //   t.gameObject.SetActive(false);
                     //   Destroy(t.gameObject);
                    }
                }
            }
            //      return false;
            //    }
            return true;
        }
        public void OnTriggerEnter(Collider c)
        {
            if (c.gameObject.tag == "Hero")
            {
                foreach (TextMeshPro t in myInstruction)
                {
                    if (t != null)
                    {
                     //   Debug.Log("turned on " + t.name);
                        t.color = Functions.Invisible(t.color, Visibility.Show);
                        t.gameObject.SetActive(true);
                    }
                }
                if (!setup && !ActionSetup(function))
                {
                    Debug.LogWarning("Could not setup the function");
                }
            }
        }
    }
}
