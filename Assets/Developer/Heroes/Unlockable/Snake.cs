public class Snake : Ally2 {
    public override void Interact()
    {
        CurrentTarget = ChooseTarget();
        base.Interact();
    }
}
