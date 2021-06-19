using System;

namespace PigeonProject
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MethodHeaderAttribute : Attribute
    {
        private readonly string _methodHeader;
        public string Header => _methodHeader;
        public MethodHeaderAttribute(string header) => _methodHeader = header;
    }
}
