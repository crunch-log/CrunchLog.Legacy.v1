using System;

namespace Bit0.CrunchLog.TemplateModels
{
    public class PaginationInfo
    {
        public Int32 Page { get; set; }
        public Boolean Current { get; set; }

        public override String ToString()
        {
            return $"Page: {Page} | Current: {Current}";
        }
    }
}