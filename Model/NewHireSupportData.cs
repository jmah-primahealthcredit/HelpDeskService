using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDeskService.Model
{
    public class NewHireSupportData: SupportData 
    {
        [JsonProperty("JobPosition")]
        public string JobPosition { get; set; } = "";

        [JsonProperty("StartDate")]
        public string StartDate { get; set; } = "";

        [JsonProperty("FirstName")]
        public string FirstName { get; set; } = "";

        [JsonProperty("LastName")]
        public string LastName { get; set; } = "";

        [JsonProperty("EmailAddress")]
        public string EmailAddress { get; set; } = "";

        [JsonProperty("PhoneNumber")]
        public string PhoneNumber { get; set; } = "";

        [JsonProperty("Department")]
        public Department Department { get; set; } = new Department();
    }
}
