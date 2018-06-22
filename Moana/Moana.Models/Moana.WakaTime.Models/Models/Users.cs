using System;

namespace Moana.WakaTime.Models.WebModels
{
    public class Users
    {
        public Guid id { get; set; }
        public DateTime created_at { get; set; }
        public string display_name { get; set; }
        public string email { get; set; }
        public bool email_public { get; set; }
        public string full_name { get; set; }
        public string human_readable_website { get; set; }
        public bool is_hireable { get; set; }
        public DateTime last_heartbeat { get; set; }
        public string last_plugin { get; set; }
        public string last_plugin_name { get; set; }
        public string last_project { get; set; }
        public string location { get; set; }
        public DateTime modified_at { get; set; }
        public string photo { get; set; }
        public bool photo_public { get; set; }
        public string plan { get; set; }
        public string timezone { get; set; }
        public string username { get; set; }
        public string website { get; set; }
    }
}
