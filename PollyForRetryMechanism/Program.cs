using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;

namespace PollyForRetryMechanism
{
    static class Program
    {
        static void Main(string[] args)
        {
            // Calling the HttpCaller class & Get method within the class to return an HttpResponseMessage type //
            var httpHandler = HttpCaller.Get(ConfigReader.GetSettings<string>(ConfigKeyConstants.ValidUrl)).ConfigureAwait(false);

            // Deserializing the http response content to an object //
            if (httpHandler.GetAwaiter().GetResult().StatusCode == HttpStatusCode.OK)
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
