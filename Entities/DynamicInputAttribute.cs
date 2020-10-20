using System;

namespace CardEditor.Views
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DynamicInputAttribute : Attribute
    {
        public DynamicInputAttribute(string displayName)
        {
            DisplayName = displayName;
        }

        public string DisplayName { get; set; }
    }
}
