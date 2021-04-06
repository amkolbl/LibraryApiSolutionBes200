using CacheCow.Client;
using CacheCow.Client.RedisCacheStore;
using System;
using System.Net.Http;

namespace TimeClient
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            var client = ClientExtensions.CreateClient(new RedisStore("localhost:6379"));
            client.BaseAddress = new Uri("http://localhost:1337");

            while (true)
            {
                var response = await client.GetAsync("/time");

                Console.WriteLine(response.Headers.CacheControl.ToString());

                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);

                Console.WriteLine("Hit enter to call it again or type 'done' to quit");
                var q = Console.ReadLine();
                if(q == "done")
                {
                    break;
                }

            }
           
        }
    }
}
