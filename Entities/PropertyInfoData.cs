using System;
using System.Reflection;
using Entities.Models;

namespace Entities
{
    public class PropertyInfoData : DataModel
    {
        private string typeName;
        private string propertyName;

        public PropertyInfoData()
        {

        }

        public PropertyInfoData(PropertyInfo property)
        {
            TypeName = property.DeclaringType.AssemblyQualifiedName;
            PropertyName = property.Name;
        }

        public string TypeName
        {
            get { return typeName; }
            set
            {
                typeName = value;
                FirePropertyChanged();
            }
        }

        public string PropertyName
        {
            get { return propertyName; }
            set
            {
                propertyName = value;
                FirePropertyChanged();
            }
        }

        public PropertyInfo ToProperty()
        {
            return Type.GetType(TypeName).GetProperty(PropertyName);
        }

        public override bool Equals(object obj)
        {
            var other = obj as PropertyInfoData;

            if (other == null)
            {
                return false;
            }

            return other.PropertyName == PropertyName && other.TypeName == TypeName;
        }

        public override int GetHashCode()
        {
            return new { a = PropertyName, b = TypeName }.GetHashCode();
        }
    }
}
