

using System.Text.Json;
using System.Text.Json.Serialization;

namespace ClientIncr
{

    public class NewValue
    {
        public int value { get; set; }
    }

    public class ClientIncr
    {

        private static string url = "http://localhost:8080/incr?param1=1";
        static readonly HttpClient client = new HttpClient();

        public static async Task Main()
        {

    
            var response = client.GetAsync(url);
            var responseBody = await response.Result.Content.ReadAsStringAsync();

            NewValue json = JsonSerializer.Deserialize<NewValue>(responseBody);
            Console.WriteLine("new value receive :" + json.value);


        }
}

}
   