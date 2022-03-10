using System;
using System.Text.Json;
using System.Device.Location;


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
            GeoCoordinate posGPS;

            try
            {
                posGPS = new GeoCoordinate(Double.Parse(args[1]), Double.Parse(args[2]));
                string api_key = "6f6bf0904d38bdb4a4a7a13fed947c982798b696";
                HttpResponseMessage response = await client.GetAsync($"https://api.jcdecaux.com/vls/v3/stations?contract={args[0]}&apiKey={api_key}");

                try
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    List<Station> jsonData = JsonSerializer.Deserialize<List<Station>>(responseBody);

                    Station closestStation = null;
                    double bestDistance = -1;

                    foreach (Station element in jsonData)
                    {
                        GeoCoordinate station_coordinate = new GeoCoordinate(element.position.latitude, element.position.longitude);
                        if (bestDistance < 0 || station_coordinate.GetDistanceTo(posGPS) < bestDistance)
                        {
                            bestDistance = station_coordinate.GetDistanceTo(posGPS);
                            closestStation = element;
                        }
                    }

                    Console.WriteLine($"The closest station is {closestStation.name} at {Math.Round(bestDistance)}meters (GPS coordinates : ({closestStation.position.latitude}, {closestStation.position.longitude})).");
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Error ");
                }
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Error when parsing an argument to a double : please make sure the format is correct.");
            }
        }
    }
}