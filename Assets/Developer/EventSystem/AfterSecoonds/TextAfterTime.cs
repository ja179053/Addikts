namespace Instruciton
{
    using System.Collections;
    using UnityEngine;
    //Class for updating 3D text after a set amount of time
    public class TextAfterTime : InstructionAction
    {
        public float time;
        protected override bool Setup()
        {
            StartCoroutine(Wait());
            textOn = false;
            return ActionSetup("WaitTextAfterTime");
        }
        IEnumerator Wait()
        {
            yield return new WaitForSeconds(time);
            Trigger("WaitTextAfterTime");
        }
    }
}
