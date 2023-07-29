namespace Instruction
{
    using UnityEngine;
    using UnityEngine.Events;

    public class TextOnShop : Convo
    {
        protected override bool Setup()
        {
            myFunction = "Shopped";
            return base.Setup();
        }
    }
}