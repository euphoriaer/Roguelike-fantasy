using System;
using System.Collections.Generic;
using System.Reflection;

[AttributeUsage(AttributeTargets.Method)]
public abstract class DistributeAttr : Attribute
{
    public abstract string TypeName
    {
        get; 
        set;
    }
}

/// <summary>
/// 通用方法分发工具
/// </summary>
/// <typeparam name="T1">方法参数,Action</typeparam>
/// <typeparam name="T2">特性类，给方法打标记的特性</typeparam>
/// <typeparam name="T3">方法所属的Class</typeparam>
public class DistributeUtil<T1, T2, T3>
    where T1 : Delegate
    where T2 : DistributeAttr
{
    private Dictionary<string, T1> Cache;

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj">哪个对象上的方法，静态为null</param>
    /// <param name="bindingFlags">方法类型，public private static等等，多个判断条件用 | 连接</param>
    public DistributeUtil(object? obj)
    {
        Cache = new Dictionary<string, T1>();
        //error 使用类类型对象 取代分发
        var methods = typeof(T3).GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

        foreach (var method in methods)
        {
            foreach (var attribute in method.GetCustomAttributes() as Attribute[])
            {
                if (attribute is T2 bAtt)
                {
                    Cache[bAtt.TypeName] = (T1)method.CreateDelegate(typeof(T1), obj);
                    break;
                }
            }
        }
    }

    public T1 GetMethod(string name)
    {
        return Cache[name];
    }
}