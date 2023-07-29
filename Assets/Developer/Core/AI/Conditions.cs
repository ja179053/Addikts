public class Condition<T>
{
    protected float conditionalB;

    public virtual bool Validate(T t)
    {
        return false;
    }
}
public class LessThan : Condition<float>
{
    public LessThan(float b)
    {
        conditionalB = b;
    }
    public override bool Validate(float a)
    {
        bool b = a < conditionalB;
        //    Debug.Log(a + b.ToString());
        return b;
    }

}
public class MoreThan : Condition<float>
{
    public MoreThan(float b)
    {
        conditionalB = b;
    }
    public override bool Validate(float a)
    {
        bool b = a > conditionalB;
        //    Debug.Log(a + b.ToString());
        return b;
    }

}
