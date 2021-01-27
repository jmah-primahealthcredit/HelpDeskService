using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.Lambda.Core;
using Amazon.S3;
using Amazon.S3.Model;
using EmailService;
using HelpDeskService.Model;
using Newtonsoft.Json;

namespace HelpDeskService
{
    public class SupportService
    {
        protected ILambdaContext _context = null;

        protected readonly string _htmlBody = @"<html><body><h1>[SUPPORT_ISSUE]</h1><p>[SUPPORT_DETAILS]</p></body></html>";
        protected readonly string _htmlLineBreak = "<br>";

        protected List<string> _receiverAddresses = new List<string> { "helpdesk@primahealthcredit.com" };

        public SupportService(ILambdaContext context=null)
        {
            _context = context;
        }

        public async Task NotifyHelpDeskAsync(string senderEmailAddress, string subject, string body)
        {
            Email email = new Email(senderEmailAddress, _receiverAddresses, subject, body, true);
            await email.SendAsync();
        }

        public async Task SubmitRequestAsync(SupportData supportData)
        {
            string details = $"Name - {supportData.RequestorName}" + _htmlLineBreak;
            details += $"Email - {supportData.RequestorEmail}" + _htmlLineBreak;
            details += $"Phone - {supportData.RequestorPhone}" + _htmlLineBreak;

            details += $"Description:" + _htmlLineBreak + $"{supportData.Description}" + _htmlLineBreak;
            string htmlBody = _htmlBody.Replace("[SUPPORT_ISSUE]", supportData.Issue);
            htmlBody = htmlBody.Replace("[SUPPORT_DETAILS]", details);          

            await NotifyHelpDeskAsync("helpdesk@primahealthcredit.com", supportData.Issue, htmlBody);
        }

        public async Task<List<SupportIssue>> GetSupportIssuesAsync()
        {
            List<SupportIssue> issues = new List<SupportIssue>();

            try
            {
                RegionEndpoint bucketRegion = RegionEndpoint.USWest2;//region where you store your file

                var client = new AmazonS3Client(bucketRegion);
                GetObjectRequest request = new GetObjectRequest();
                request.BucketName = "phc-helpdesk";
                request.Key="SupportIssues.json";

                GetObjectResponse response = await client.GetObjectAsync(request);
                StreamReader reader = new StreamReader(response.ResponseStream);
                string issuesJson = reader.ReadToEnd();

                if (!string.IsNullOrEmpty(issuesJson))
                {
                    issues = JsonConvert.DeserializeObject<List<SupportIssue>>(issuesJson);
                    issues = issues.OrderBy(x => x.Issue).ToList();
                    issues.Add(new SupportIssue() { Issue = "Other", IssueID = -1 });
                }
            }
            catch (Exception ex)
            {
                LogLine($"Error: GetSupportIssuesAsync - {ex.Message}");
            }

            return issues;
        }

        public async Task<List<Department>> GetDepartmentsAsync()
        {
            List<Department> departments = new List<Department>();

            try
            {
                RegionEndpoint bucketRegion = RegionEndpoint.USWest2;//region where you store your file

                var client = new AmazonS3Client(bucketRegion);
                GetObjectRequest request = new GetObjectRequest();
                request.BucketName = "phc-helpdesk";
                request.Key = "Departments.json";

                GetObjectResponse response = await client.GetObjectAsync(request);
                StreamReader reader = new StreamReader(response.ResponseStream);
                string departmentsJson = reader.ReadToEnd();

                if (!string.IsNullOrEmpty(departmentsJson))
                {
                    departments = JsonConvert.DeserializeObject<List<Department>>(departmentsJson);
                    departments = departments.OrderBy(x => x.Name).ToList();
                }
            }
            catch (Exception ex)
            {
                LogLine($"Error: GetDepartmentsAsync - {ex.Message}");
            }

            return departments;
        }

        protected string OnboardingBodyContent(NewHireSupportData newHireData)
        {
            string details = $"Description:" + _htmlLineBreak;
            details += $"Hiring Manager: {newHireData.RequestorName}" + _htmlLineBreak;
            details += $"Hiring Manager Email: {newHireData.RequestorEmail}" + _htmlLineBreak;
            details += $"Hiring Manager Phone: {newHireData.RequestorPhone}" + _htmlLineBreak;

            details += $"Department: {newHireData.Department.Name}" + _htmlLineBreak;
            details += $"Job Position: {newHireData.JobPosition}" + _htmlLineBreak;
            details += $"Start Date: {newHireData.StartDate}" + _htmlLineBreak;
            details += $"First Name: {newHireData.FirstName}" + _htmlLineBreak;
            details += $"Last Name: {newHireData.LastName}" + _htmlLineBreak;
            details += $"Personal Email: {newHireData.EmailAddress}" + _htmlLineBreak;
            details += $"Personal Mobile: {newHireData.PhoneNumber}" + _htmlLineBreak;

            string htmlBody = _htmlBody.Replace("[SUPPORT_ISSUE]", "Onboard New Hire");
            return htmlBody.Replace("[SUPPORT_DETAILS]", details);
        }

        public async Task NotifyNewHireAsync(NewHireSupportData newHireData)
        {
            string emailBody = OnboardingBodyContent(newHireData);
            await NotifyHelpDeskAsync("helpdesk@primahealthcredit.com", "Onboard New Hire", emailBody);
        }

        protected void LogLine(string line)
        {
            if (_context != null)
            {
                _context.Logger.LogLine(line);
            }
        }
    }
}
