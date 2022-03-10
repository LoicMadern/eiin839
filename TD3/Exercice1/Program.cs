using System;
using System.Text.Json;

namespace MyClient
{

    public class Contract
    {
        public string name { get; set; }
        public string commercial_name { get; set; }
        public List<string> cities { get; set; }
        public string country_code { get; set; }
    }



    internal class Program
    {
        static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            string api_key = "6f6bf0904d38bdb4a4a7a13fed947c982798b696";
            HttpResponseMessage response = await client.GetAsync($"https://api.jcdecaux.com/vls/v3/contracts?apiKey={api_key}");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            List<Contract> jsonData = JsonSerializer.Deserialize<List<Contract>>(responseBody);

            foreach (Contract element in jsonData)
            {
                Console.WriteLine($"{element.name} - {element.commercial_name}");
            }
        }
    }
}