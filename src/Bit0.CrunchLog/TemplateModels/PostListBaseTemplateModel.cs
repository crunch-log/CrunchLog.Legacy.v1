using System;
using System.Collections.Generic;
using System.Linq;
using Bit0.CrunchLog.Config;

namespace Bit0.CrunchLog.TemplateModels
{
    public abstract class PostListBaseTemplateModel : IPostListTemplateModel
    {
        protected PostListBaseTemplateModel(CrunchConfig config)
        {
            Config = config;
        }

        public String Permalink { get; set; }
        public String Layout { get; protected set; }
        public CrunchConfig Config { get; set; }

        public String Name { get; set; }
        public IEnumerable<PostTemplateModel> Posts { get; set; }

        public IEnumerable<IPostListTemplateModel> Pages
        {
            get
            {
                var posts = Posts.ToList();
                var pageSize = Config.Pagination.PageSize;
                var totalPages = posts.Count / pageSize;

                if (posts.Count % pageSize > 0)
                {
                    totalPages++;
                }

                var pages = Enumerable.Range(1, totalPages).Select(i =>
                {
                    var list = (IPostListTemplateModel)Activator.CreateInstance(GetType(), Config);
                    list.Name = Name;
                    list.Permalink = i == 1 ? Permalink : $"{Permalink}/page/{i}";
                    list.Posts = Posts.Skip((i - 1) * pageSize).Take(pageSize);
                    list.PaginationInfo = Enumerable.Range(1, totalPages)
                        .Select(p => new PaginationInfo
                        {
                            Page = p,
                            Current = i == p
                        });

                    return list;
                });

                return pages;
            }
        }

        public IEnumerable<PaginationInfo> PaginationInfo { get; set; }

        //protected IDictionary<Int32, T> GetPages<T>() where T : IPostListTemplateModel, new()
        //{
        //    var posts = Posts.ToList();
        //    var pageSize = Config.Pagination.PageSize;
        //    var totalPages = (posts.Count / pageSize) + 1;

        //    var pages = Enumerable.Range(1, totalPages).ToDictionary(
        //        k => k,
        //        v =>
        //        {
        //            var list = new T
        //            {
        //                Config = Config,
        //                Name = Name,
        //                Permalink = $"{Permalink}page/{v}",
        //                Posts = Posts.Skip(v * pageSize).Take(pageSize)
        //            };

        //            return list;
        //        });

        //    return null;
        //}

        public override String ToString()
        {
            return $"Link: {Permalink} | Count: {Posts.Count()} | Pages: {Pages.Count()}";
        }
    }
}