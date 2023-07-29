namespace AIAdvanced
{
    using System.Collections;
    public class Parameter
    {
        public Condition<float> condition;
        float parameterA, parameterB;
        public bool disabled;

    }
    public class HealthParameter : Parameter
    {
        public IEnumerator function;
        public HealthParameter(Condition<float> newCondition, IEnumerator action)
        {
            condition = newCondition;
            function = action;
            disabled = false;
        }
    }
    public class MagicParameter : Parameter
    {
        public IEnumerator function;
        public MagicParameter(Condition<float> newCondition, IEnumerator action)
        {
            condition = newCondition;
            function = action;
            disabled = false;
        }
    }
    public class TargetParameter : Parameter
    {
        public AI target;
        public TargetParameter(Condition<float> newCondition, AI action)
        {
            condition = newCondition;
            target = action;
            disabled = false;
        }
    }
}