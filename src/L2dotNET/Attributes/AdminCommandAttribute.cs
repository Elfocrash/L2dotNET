using System;

namespace L2dotNET.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AdminCommandAttribute : Attribute
    {
        public string CommandName { get; set; }
    }
}