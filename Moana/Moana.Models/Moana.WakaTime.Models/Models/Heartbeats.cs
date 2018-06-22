using System;

namespace Moana.WakaTime.Models.WebModels
{
    public class Heartbeats
    {
        public Guid id { get; set; }
        public string branch { get; set; }
        public string entity { get; set; }
        public string type { get; set; }
        public bool is_debugging { get; set; }
        public bool is_write { get; set; }
        public string language { get; set; }
        public string project { get; set; }
        public double time { get; set; }
    }
}
