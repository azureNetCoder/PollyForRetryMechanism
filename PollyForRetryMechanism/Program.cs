using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PollyForRetryMechanism
{
    class Program
    {
        static void Main(string[] args)
        {
            var validHttpUrl = "https://jsonplaceholder.typicode.com/comments";
            var inValidHttpUrl = "https://jsonplaceholder.typicode.com/comments1";

            // Calling the HttpCaller class & Get method within the class to return an HttpResponseMessage type //
            var httpHandler = HttpCaller.Get(inValidHttpUrl).ConfigureAwait(false);

            // Deserializing the http response content to an object //
            if (httpHandler.GetAwaiter().GetResult().StatusCode == System.Net.HttpStatusCode.OK)
            {
                var modelObject = JsonConvert.DeserializeObject<List<Model>>(httpHandler.GetAwaiter().GetResult()
                                                                            .Content.ReadAsStringAsync()
                                                                            .ConfigureAwait(false).GetAwaiter().GetResult());

                foreach (var item in modelObject)
                {
                    Console.WriteLine($"Name: {item.name}");
                }
            }
            else
            {
                Console.WriteLine($" Error with status code: {httpHandler.GetAwaiter().GetResult().StatusCode} on request to url : {httpHandler.GetAwaiter().GetResult().RequestMessage.RequestUri}");
            }

            Console.ReadKey();
        }
    }
}
