using System;
using System.Collections.Generic;
using System.Linq;
using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.TemplateModels;

namespace Bit0.CrunchLog.Extensions
{
    public static class ContentExtensions
    {
        public static PostTemplateModel GetModel(this Content content, CrunchConfig config)
        {
            return new PostTemplateModel(content, config); 
        }

        public static String GetPagePermaLink(this ContentListItem contentListItem, Int32 page)
        {
            if (page == 1)
            {
                return contentListItem.Permalink;
            }

            if (contentListItem.Permalink.EndsWith("/"))
            {
                return $"{contentListItem.Permalink}{page}";
            }

            return  $"{contentListItem.Permalink}/{page}";
        }

        public static IEnumerable<PostListTemplateModel> GetPages(this ContentListItem contentListItem, CrunchConfig config)
        {
            var totalPages = (Int32) Math.Ceiling(contentListItem.Children.Count() / (Double) config.Pagination.PageSize);

            for (var i = 1; i <= totalPages; i++)
            {
                yield return new PostListTemplateModel(contentListItem, config, i, totalPages);
            }
        }
    }
}
