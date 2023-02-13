using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPERATION_MNS.Areas.OpeationMns.Models
{
    public class DataChange
    {
        [JsonProperty("key")]
        public object Key { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }
    }
}
