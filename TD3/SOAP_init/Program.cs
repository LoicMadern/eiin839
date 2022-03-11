

using System.Text.Json;
using System.Text.Json.Serialization;

namespace SOAP_init
{



    public class Client
    {   

        

        static readonly HttpClient client = new HttpClient();

        public static async Task Main()
        {

            var client = new ServiceReference1.CalculatorSoapClient(ServiceReference1.CalculatorSoapClient.EndpointConfiguration.CalculatorSoap);
            Console.WriteLine(await client.ChannelFactory.CreateChannel().AddAsync(2, 2));
 

        }
    }

}
