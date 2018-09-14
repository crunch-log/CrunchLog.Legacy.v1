using System;

namespace Bit0.CrunchLog.Template.Models
{
    public class PaginationTemplateModel
    {
        public Int32 Page {get; set; }
        public String Url { get; set; }
        public Boolean IsCurrentPage {get; set;}
    }
}
