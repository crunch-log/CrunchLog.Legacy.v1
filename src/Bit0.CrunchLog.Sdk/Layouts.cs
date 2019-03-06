using Bit0.CrunchLog.Attributes;
using System;

namespace Bit0.CrunchLog
{
    public enum Layouts
    {
        [String("post")]
        Post,
        [String("page")]
        Page,
        [String("home")]
        Home,
        [String("list")]
        List,
        [String("list")]
        Tag,
        [String("list")]
        Category,
        [String("list")]
        Archive,
        [String("empty")]
        Empty,
        [String("redirect")]
        Redirect,
        [String("list")]
        Author,
        [String("site")]
        Site,
    }
}