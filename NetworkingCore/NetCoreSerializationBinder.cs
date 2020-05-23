using Newtonsoft.Json.Serialization;
using System;
using System.Runtime.CompilerServices;

namespace NetworkingCore
{
    public class NetCoreSerializationBinder : DefaultSerializationBinder
    {
        public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            base.BindToName(serializedType, out assemblyName, out typeName);

            if (Attribute.GetCustomAttribute(serializedType, typeof(TypeForwardedFromAttribute), false) is TypeForwardedFromAttribute attr)
                assemblyName = attr.AssemblyFullName;
        }
    }
}
