using CRM.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CRM.BLL.Services
{
    public class LemlistIntegrationService : ILemlistIntegrationService
    {

        byte[] authenticationBytes = Encoding.ASCII.GetBytes(":5ae614888a4753f07b9833abad6f5b5d"); // <username>:<password>
        public Task AddLeadInCampaign(string contact)
        {
            throw new NotImplementedException();
        }

        public async Task GetAdvertisingCompanies()
        {
            using HttpClient confClient = new HttpClient();
            confClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authenticationBytes));
            var response = await confClient.GetAsync("https://api.lemlist.com/api/campaigns");
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                var companies = Newtonsoft.Json.JsonConvert.DeserializeObject(result);
            }
        }
    }
    public class t
    {
        public string _id { get; set; }
        public string name { get; set; }
    }
}
