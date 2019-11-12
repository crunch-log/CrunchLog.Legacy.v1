using Newtonsoft.Json;
using System;

namespace Bit0.CrunchLog.Config
{
    public class Copyright
    {
        [JsonProperty("startYear")]
        public Int32 StartYear { get; set; } = 2018;

        [JsonProperty("owner")]
        public String Owner { get; set; } = "Nullbit";

        public override String ToString() => $"Copyright (c) {StartYear}{(DateTime.UtcNow.Year > StartYear ? $" - {DateTime.UtcNow.Year}" : "")} {Owner}.";
    }
}