using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using NMCT.Cloud1.MyFirstService.Model;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace NMCT.Cloud1.MyFirstService
{
    public static class HelloFunctions
    {
        [FunctionName("HelloFunctions")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "HttpTriggerCSharp/name/{name}")]HttpRequestMessage req, string name, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            // Fetching the name from the path parameter in the request URL
            return req.CreateResponse(HttpStatusCode.OK, "Hello " + name);
        }





        [FunctionName("AddRegistration")]
        public async static Task<HttpResponseMessage> AddRegistration([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "registrations")]HttpRequestMessage req, TraceWriter log)
        {
            var json = await req.Content.ReadAsStringAsync();
            Person newP = null;
            newP = JsonConvert.DeserializeObject<Person>(json);

            newP.Age = 100;
            // new person naar database schrijven
            return req.CreateResponse(HttpStatusCode.OK, newP);
        }
    }
}
