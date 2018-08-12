using System;

namespace Server.DataLayer
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DbLengthAttribute : Attribute
    {
        public DbLengthAttribute(int length)
        {
            Length = length;
        }

        public int Length { get; }
    }
}