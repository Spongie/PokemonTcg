using System;

namespace NetworkingCore
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class IgnoreInServiceAttribute : Attribute
    {
    }
}
