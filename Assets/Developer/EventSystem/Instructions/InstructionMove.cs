namespace Instruction
{
    public class InstructionMove : InstructionAction
    {
        // Use this for initialization
        protected override bool Setup()
        {
            waitTime = 0.1f;
            CameraAI3.moveEventPresent = true;
            return ActionSetup("Move");
        }
    }
}
