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
        #region First Function
        [FunctionName("HelloFunctions")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "HttpTriggerCSharp/name/{name}")]HttpRequestMessage req, string name, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            // Fetching the name from the path parameter in the request URL
            return req.CreateResponse(HttpStatusCode.OK, "Hello " + name);
        }
        #endregion

        #region Test Function
        [FunctionName("TestParameters")]
        public static HttpResponseMessage TestParameters([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "TestParameters/{name}")]HttpRequestMessage req, string name, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            // Fetching the name from the path parameter in the request URL
            return req.CreateResponse(HttpStatusCode.OK, "Hello Again" + name);
        }
        #endregion

        #region Rekenen
        [FunctionName("Sum")]
        public static HttpResponseMessage Sum([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "rekenmachine/som/{getal1}/{getal2}")]HttpRequestMessage req, int getal1, int getal2, TraceWriter log)
        {
            int som = getal1 + getal2;
            return req.CreateResponse(HttpStatusCode.OK, "Som is " + som);
        }

        [FunctionName("Divide")]
        public static HttpResponseMessage Divide([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "rekenmachine/devide/{getal1}/{getal2}")]HttpRequestMessage req, int getal1, int getal2, TraceWriter log)
        {            
            if (getal2 != 0)
            {
                int quotient = getal1 / getal2;
                return req.CreateResponse(HttpStatusCode.OK, "Quotient is " + quotient);
            } else
            {
                return req.CreateResponse(HttpStatusCode.Forbidden);
            }
            
        }
        #endregion

        [FunctionName("History")]
        public static HttpResponseMessage History([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "history")]HttpRequestMessage req, TraceWriter log)
        {
            string from = string.Empty;
            string to = string.Empty;

            foreach (var param in req.GetQueryNameValuePairs())
            {
                if (param.Key.Equals("from"))
                {
                    from = param.Value;
                }
                else
                {
                    to = param.Value;
                }
            }
            string value = $"From {from} To {to}";
            return req.CreateResponse(HttpStatusCode.OK, value);
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
