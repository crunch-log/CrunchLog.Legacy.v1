using Bit0.CrunchLog.Attributes;
using System;
using System.Security.Cryptography;
using System.Text;

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
        /// <param name="value">Enum field value</param>
        /// <returns></returns>
        public static String GetValue(this Enum value)
        {
            return value.GetFieldAttribute<StringAttribute>().Value;
        }

        /// <summary>
        /// Get SHA1 hash from a string
        /// </summary>
        /// <param name="str">String to hash</param>
        /// <returns>String representation of SHA1 hash</returns>
        public static String GetSha1Hash(this String str)
        {
            return BitConverter.ToString(SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(str))).Replace("-", "");
        }
    }
}