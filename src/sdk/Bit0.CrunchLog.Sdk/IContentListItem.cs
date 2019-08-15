using System;
using System.Collections.Generic;

namespace Bit0.CrunchLog
{
    public interface IContentListItem : IContentBase
    {
        IEnumerable<IContent> Children { get; set; }
        String Name { get; set; }
    }
}