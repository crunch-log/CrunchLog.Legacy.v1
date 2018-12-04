using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace Bit0.CrunchLog.Config
{
    public class ThemeOutput
    {
        [JsonExtensionData]
        private readonly IDictionary<String, JToken> _additionalData = new Dictionary<String, JToken>();

        /// <summary>
        /// Note: Only requirered if theme outputType is JSON
        /// </summary>
        [JsonIgnore]
        public DirectoryInfo Data { get; set; }

        [JsonProperty("copy")]
        public IDictionary<String, IEnumerable<String>> Copy { get; set; }

        [JsonProperty("process")]
        public IDictionary<String, String> Process { get; set; }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            var contentKey = (String)_additionalData["content"];
            Data = new DirectoryInfo(contentKey);
        }
    }
}