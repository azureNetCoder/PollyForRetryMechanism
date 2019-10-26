using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Polly;

namespace PollyForRetryMechanism
{
    public static class HttpCaller
    {
        public static async Task<HttpResponseMessage> Get(string Uri)
        {
            var httpRequest = new HttpClient();

            try
            {
                // Wrapping the http call within the executeWaitRetryOnTransientFailure method //
                return await ExecuteWaitRetryOnTransientFailure(() => httpRequest.GetAsync(Uri));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        ///     This method is used to execute retry mechanism using RetryAsync method.
        /// </summary>
        /// <param name="httpResponse"></param>
        /// <returns></returns>
        private static Task<HttpResponseMessage> ExecuteRetryOnTransientFailure(Func<Task<HttpResponseMessage>> httpResponse)
        {
            return Polly.Policy
                        .Handle<HttpRequestException>()
                        .OrResult<HttpResponseMessage>(x=>x.StatusCode != System.Net.HttpStatusCode.OK)
                        .RetryAsync(3,
                                   (ex, a) => Console.WriteLine($"Retry count {a}"))
                        .ExecuteAsync(httpResponse);
        }

        /// <summary>
        ///     This method is used to execute retry mechanism using WaitRetryAsync which configures a certain wait 
        ///     time before retrying the http request
        /// </summary>
        /// <param name="httpResponse"></param>
        /// <returns></returns>
        private static Task<HttpResponseMessage> ExecuteWaitRetryOnTransientFailure(Func<Task<HttpResponseMessage>> httpResponse)
        {
            return Polly.Policy
                        .Handle<HttpRequestException>()
                        .OrResult<HttpResponseMessage>(x => x.StatusCode != System.Net.HttpStatusCode.OK)
                        .WaitAndRetryAsync(3, retryAttempt =>
                                        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                                      (ex, a) => Console.WriteLine($"Retrying after : {a} seconds"))
                        .ExecuteAsync(httpResponse);
        }


    }
}
