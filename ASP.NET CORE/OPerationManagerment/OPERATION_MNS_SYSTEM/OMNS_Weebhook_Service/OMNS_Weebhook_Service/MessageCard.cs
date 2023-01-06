using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrmsCopyDataService
{
    public class MessageCard
    {
        public MessageCard()
        {
            sections = new List<Section>();
        }
        [JsonProperty("@type")]
        public string type { get; set; }

        [JsonProperty("@context")]
        public string context { get; set; }
        public string themeColor { get; set; }
        public string summary { get; set; }
        public List<Section> sections { get; set; }
    }



    public class Fact
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Section
    {
        public Section()
        {
            facts = new List<Fact>();
        }
        public string activityTitle { get; set; }
        public string activitySubtitle { get; set; }
        public string activityImage { get; set; }
        public List<Fact> facts { get; set; }
        public bool markdown { get; set; }
        public bool? startGroup { get; set; }
        public string text { get; set; }
    }
}
