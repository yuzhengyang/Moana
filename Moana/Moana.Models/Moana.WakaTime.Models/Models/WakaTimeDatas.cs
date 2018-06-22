using System;

namespace Moana.WakaTime.Models.WebModels
{
    public class WakaTimeDatas
    {
        public string ID { get; set; }
        public string Project { get; set; }
        public string Branch { get; set; }
        public string Entity { get; set; }
        public string Type { get; set; }
        public double BeginTime { get; set; }
        public double EndTime { get; set; }
        public double Duration { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Language { get; set; }
        public bool IsDebugging { get; set; }
        public bool IsWrite { get; set; }
    }
}
