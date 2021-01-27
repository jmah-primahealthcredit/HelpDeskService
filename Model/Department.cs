using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace HelpDeskService.Model
{
    public class Department
    {
        [JsonProperty("DepartmentID")]
        public int DepartmentID { get; set; } = 0;


        [JsonProperty("Name")]
        public string Name{ get; set; } = "";

        [JsonProperty("EmailAddress")]
        public string EmailAddress { get; set; } = "";
    }
}
