using Newtonsoft.Json;
using System;

namespace Bit0.CrunchLog.Config
{
    public class Pagination
    {
        private const Int32 _defaultPazeSize = 4;
        private Int32 _pageSize = _defaultPazeSize;

        [JsonProperty("pageSize")]
        public Int32 PageSize
        {
            get => _pageSize > 0 ? _pageSize : _defaultPazeSize;
            set => _pageSize = value;
        }
    }
}