using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDeskService.Model
{
    public class SupportIssue
    {
        [JsonProperty("IssueID")]
        public int IssueID { get; set; } = -1;

        [JsonProperty("Issue")]
        public string Issue { get; set; } = "";
    }
}
