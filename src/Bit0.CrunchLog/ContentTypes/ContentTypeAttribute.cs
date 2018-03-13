using System;

namespace Bit0.CrunchLog.ContentTypes
{
    public class ContentTypeAttribute : Attribute
    {
        public String ContentTypeName { get; }

        public ContentTypeAttribute(String contentTypeName)
        {
            ContentTypeName = contentTypeName;
        }
    }
}