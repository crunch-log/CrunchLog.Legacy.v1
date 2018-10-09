using System;

namespace Bit0.CrunchLog.Plugin
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class PluginAttribute : Attribute
    {
        public String Name { get; set; }
    }
}