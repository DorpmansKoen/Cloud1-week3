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

        #region Rekenen GET
        [FunctionName("Sum")]
        public static HttpResponseMessage Sum([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "rekenmachine/som/{getal1}/{getal2}")]HttpRequestMessage req, int getal1, int getal2, TraceWriter log)
        {
            int som = getal1 + getal2;
            return req.CreateResponse(HttpStatusCode.OK, "Som is " + som);
        }

        [FunctionName("Divide")]
        public static HttpResponseMessage Divide([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "rekenmachine/divide/{getal1}/{getal2}")]HttpRequestMessage req, int getal1, int getal2, TraceWriter log)
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

        #region Rekenen POST

        [FunctionName("DividePost")]
        public async static Task<HttpResponseMessage> DividePost([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "rekenmachine/dividePost")]HttpRequestMessage req, TraceWriter log)
        {
            var content = await req.Content.ReadAsStringAsync();
            log.Info(content);

            var getallen = JsonConvert.DeserializeObject<Getallen>(content);            

            Result r = new Result();
            r.Quotient = getallen.Getal1 / getallen.Getal2;

            return req.CreateResponse(HttpStatusCode.OK);
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

        /*
        [FunctionName("Drink")]
        public async static Task<HttpResponseMessage> Drink([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Drink")]HttpRequestMessage req, TraceWriter log)
        {
            var json = await req.Content.ReadAsStringAsync();
            Order newOrder = null;
            newOrder = JsonConvert.DeserializeObject<Order>(json);
            
            if(newOrder.MyAge >= newOrder.MyOrder.DrinkingAge)           
                return req.CreateResponse(HttpStatusCode.OK, "You can have your drink");
            else
                return req.CreateResponse(HttpStatusCode.PreconditionFailed, "You can't have your drink");
        }
        */
        [FunctionName("Drink")]
        public async static Task<HttpResponseMessage> Drink([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Drink")]HttpRequestMessage req, TraceWriter log)
        {
            var json = await req.Content.ReadAsStringAsync();
            Order newOrder = null;
            newOrder = JsonConvert.DeserializeObject<Order>(json);

            int customerAge = newOrder.MyAge;
            int drinkingAge = newOrder.MyOrder.DrinkingAge;

            if (customerAge >= drinkingAge && customerAge <= 100)
                return req.CreateResponse(HttpStatusCode.OK, "You can have your drink");
            else if (customerAge <= 0 || customerAge >= 100)
                return req.CreateResponse(HttpStatusCode.NotAcceptable, "Invalid age");
            else if (newOrder.MyOrder == null)
                return req.CreateResponse(HttpStatusCode.NotAcceptable, "No drink has been ordered");
            else
                return req.CreateResponse(HttpStatusCode.PreconditionFailed, "You can't have your drink");
        }
    }
}
