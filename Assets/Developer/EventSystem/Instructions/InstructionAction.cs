public abstract class InstructionAction : FollowInstruction {
    
    protected bool ActionSetup(string function)
    {
        //Sets the function name
        myFunction = function;    
        return base.Setup();
    }
    protected override void EventCompleted()
    {
        base.EventCompleted();
        EnableAllTexts();
        Activation(true);
    }
    //To fix the error you gotta download vs 2015 tools for Unity 2018
    //https://marketplace.visualstudio.com/items?itemName=SebastienLebreton.VisualStudio2015ToolsforUnity
}
