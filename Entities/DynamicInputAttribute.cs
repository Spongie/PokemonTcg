using System;

namespace CardEditor.Views
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DynamicInputAttribute : Attribute
    {
        public DynamicInputAttribute(string displayName) :this(displayName, InputControl.Text, null)
        {

        }

        public DynamicInputAttribute(string displayName, InputControl inputType)
            :this(displayName, inputType, null)
        {

        }

        public DynamicInputAttribute(string displayName, InputControl inputType, Type enumType)
        {
            DisplayName = displayName;
            InputType = inputType;
            EnumType = enumType;
        }

        public string DisplayName { get; set; }
        public InputControl InputType { get; set; }
        public Type EnumType { get; set; }
    }

    public enum InputControl
    {
        Text,
        Boolean,
        Dropdown,
        Grid,
        Ability,
        Dynamic
    }
}
