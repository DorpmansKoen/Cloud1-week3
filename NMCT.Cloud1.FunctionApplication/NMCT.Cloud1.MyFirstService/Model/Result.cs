using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMCT.Cloud1.MyFirstService.Model
{
    public class Result
    {
        [JsonProperty("quotient")]
        public int Quotient { get; set; }
    }
}
