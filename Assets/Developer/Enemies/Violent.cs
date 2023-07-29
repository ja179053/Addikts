using AIAdvanced;

public class Violent : Enemy2
{
    protected override void AddParameters()
    {
        Condition<float> c = new LessThan(2);
        healthParameters.Add(new HealthParameter(c, MakePeace()));
        actions.Add(ValidateMyHP());
        base.AddParameters();
    }
}
