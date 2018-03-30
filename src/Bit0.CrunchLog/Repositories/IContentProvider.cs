using System.Collections.Generic;
using System.IO;

namespace Bit0.CrunchLog.Repositories
{
    public interface IContentProvider
    {
        IDictionary<FileInfo, Content> GetAll();
    }
}