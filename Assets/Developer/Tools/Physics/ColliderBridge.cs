namespace Instruction{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ColliderBridge : MonoBehaviour {
        public InstructionOnTrigger instruction;
        void OnTriggerEnter(Collider c)
        {
            if (instruction != null)
            {
                instruction.OnTriggerEnter(c);
            }
        }
    }
}