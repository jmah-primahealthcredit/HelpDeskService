using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDeskService.Model
{
    public class SupportData
    {
        [JsonProperty("RequestorEmail")]
        public string RequestorEmail { get; set; } = "";

        [JsonProperty("RequestorName")]
        public string RequestorName { get; set; } = "";

        [JsonProperty("RequestorPhone")]
        public string RequestorPhone { get; set; } = "";

        [JsonProperty("Issue")]
        public string Issue { get; set; } = "";

        [JsonProperty("Description")]
        public string Description { get; set; } = "";
    }
}
