using System;
using Bit0.CrunchLog.Attributes;

namespace Bit0.CrunchLog.Extensions
{
    /// <summary>
    /// String extensions
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Get string value from enum field
        /// </summary>
        /// <param name="s">Enum field value</param>
        /// <returns></returns>
        public static String GetValue(this Enum s)
        {
            return s.GetFieldAttribute<StringAttribute>().Value;
        }
    }
}