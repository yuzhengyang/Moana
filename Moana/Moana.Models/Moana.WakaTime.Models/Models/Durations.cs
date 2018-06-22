using System.Collections.Generic;

namespace Moana.Waka.Toolkit.Models
{
    public class Durations
    {
        public string id { get; set; }
        public List<string> dependencies { get; set; }
        public double duration { get; set; }
        public bool is_debugging { get; set; }
        public string project { get; set; }
        public double time { get; set; }
    }
}
