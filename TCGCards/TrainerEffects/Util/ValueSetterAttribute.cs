using System;

namespace TCGCards.TrainerEffects.Util
{

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ValueSetterAttribute : Attribute
    {
        public ValueSetterAttribute(Type targetType)
        {
            TargetType = targetType;
        }

        public Type TargetType { get; set; }
    }
}
