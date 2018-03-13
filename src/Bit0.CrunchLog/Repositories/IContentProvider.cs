using System.Collections.Generic;
using System.IO;
using Bit0.CrunchLog.ContentTypes;

namespace Bit0.CrunchLog.Repositories
{
    public interface IContentProvider
    {
        IDictionary<FileInfo, TContent> GetAll<TContent>() where TContent : IContent, new();
    }
}