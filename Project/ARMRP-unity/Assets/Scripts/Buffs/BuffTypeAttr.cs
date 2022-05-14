using System;

[AttributeUsage(AttributeTargets.Method)]
public class BuffTypeAttr : DistributeAttr
{
    private string buffName;

    public override string TypeName
    {
        get { return buffName; }
        set { buffName = value; }
    }
}