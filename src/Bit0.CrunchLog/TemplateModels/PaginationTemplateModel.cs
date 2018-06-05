using System;
using System.Collections.Generic;
using System.Text;

namespace Bit0.CrunchLog.TemplateModels
{
    public class PaginationTemplateModel
    {
        public Int32 Page {get; set; }
        public String Url { get; set; }
        public Boolean IsCurrentPage {get; set;}
    }
}
