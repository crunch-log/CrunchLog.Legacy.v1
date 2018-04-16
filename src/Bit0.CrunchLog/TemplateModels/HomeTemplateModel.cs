using System;
using System.Collections.Generic;
using Bit0.CrunchLog.Config;

namespace Bit0.CrunchLog.TemplateModels
{
    public class HomeTemplateModel : ITemplateModel
    {
        public HomeTemplateModel(CrunchConfig config)
        {
            Config = config;
        }

        public CrunchConfig Config { get; }

        public String Permalink => "/";

        public String Layout => Layouts.Home;

        public IEnumerable<TagTemplateModel> Tags {get; set; }
        public IEnumerable<CategoryTemplateModel> Categories {get; set; }
        public IEnumerable<ArchiveTemplateModel> Archives {get; set; }
        public IEnumerable<PostTemplateModel> Posts {get; set; }
    }
}
