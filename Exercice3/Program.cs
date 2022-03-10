using System;
using System.Text.Json;

namespace MyClient
{

    public class Position
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class TotalStands
    {
        public Availabilities availabilities { get; set; }
        public int capacity { get; set; }
    }

    public class Availabilities
    {
        public int bikes { get; set; }
        public int stands { get; set; }
        public int mechanicalBikes { get; set; }
        public int electricalBikes { get; set; }
        public int electricalInternalBatteryBikes { get; set; }
        public int electricalRemovableBatteryBikes { get; set; }

    }

    public class MainStands
    {
        public Availabilities availabilities { get; set; }
        public int capacity { get; set; }
    }

    public class Station
    {
        public int number { get; set; }
        public string contract_name { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public Position position { get; set; }
        public bool banking { get; set; }
        public bool bonus { get; set; }
        public string status { get; set; }

        public string lastUpdate { get; set; }

        public bool connected { get; set; }
        public bool overflow { get; set; }
        public TotalStands totalStands { get; set; }
        public MainStands mainStands { get; set; }

    }



    internal class Program
    {
        static readonly HttpClient client = new HttpClient();



        static async Task Main(string[] args)
        {
            string api_key = "6f6bf0904d38bdb4a4a7a13fed947c982798b696";
            HttpResponseMessage response = await client.GetAsync($"https://api.jcdecaux.com/vls/v3/stations/{args[1]}?contract={args[0]}&apiKey={api_key}");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            Station jsonData = JsonSerializer.Deserialize<Station>(responseBody);

            Console.WriteLine("informations concernant la station " + jsonData.name + "\n");


            Console.WriteLine(jsonData.number + "\n" +
                jsonData.contract_name + "\n" +
                jsonData.name + "\n" +
                jsonData.address + "\n" +
                "Position : \n" +
                jsonData.position.latitude + "\n" +
                jsonData.position.longitude + "\n" +
                jsonData.banking + "\n" +
                jsonData.bonus + "\n" +
                jsonData.status + "\n" +
                jsonData.lastUpdate + "\n" +
                jsonData.connected + "\n" +
                jsonData.overflow + "\n" +
                "Total stands : \n" +
                jsonData.totalStands.availabilities.bikes + "\n" +
                jsonData.totalStands.availabilities.stands + "\n" +
                jsonData.totalStands.availabilities.mechanicalBikes + "\n" +
                jsonData.totalStands.availabilities.electricalBikes + "\n" +
                jsonData.totalStands.availabilities.electricalRemovableBatteryBikes + "\n" +
                jsonData.totalStands.availabilities.electricalInternalBatteryBikes + "\n" +
                jsonData.totalStands.capacity + "\n" +
                "Main stands : \n" +
                jsonData.mainStands.availabilities.bikes + "\n" +
                jsonData.mainStands.availabilities.stands + "\n" +
                jsonData.mainStands.availabilities.mechanicalBikes + "\n" +
                jsonData.mainStands.availabilities.electricalBikes + "\n" +
                jsonData.mainStands.availabilities.electricalRemovableBatteryBikes + "\n" +
                jsonData.mainStands.availabilities.electricalInternalBatteryBikes + "\n" +
                jsonData.mainStands.capacity + "\n");

        }
    }
}
