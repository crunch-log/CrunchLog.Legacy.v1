using System;
using System.Collections.Generic;

namespace Bit0.CrunchLog.TemplateModels
{
    public interface IPostListTemplateModel : ITemplateModel
    {
        String Name { get; set; }
        IEnumerable<PostTemplateModel> Posts { get; set; }
        IEnumerable<IPostListTemplateModel> Pages { get; }
        IEnumerable<PaginationInfo> PaginationInfo { get; set; }
    }
}