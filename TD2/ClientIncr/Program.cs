


using System.Text.Json;

namespace ClientIncr
{

    public class NewValue
    {
     

        public int value { get; set; }
    }

     internal class Program
    {

        private static string url = "http://localhost:8080/incr?a=0";
        static readonly HttpClient client = new HttpClient();

        public static async Task Main()
        {

    
            var response = client.GetAsync(url);
            var responseBody = await response.Result.Content.ReadAsStreamAsync();

            NewValue json = JsonSerializer.Deserialize<NewValue>(responseBody);
            Console.WriteLine(json.value);


        }
}

}
   