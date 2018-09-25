using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace KdtSdk.Models
{
    public class KondutoOrderResponse : KondutoModel
    {
        /* Attributes */
        [JsonProperty("id", Required = Required.Always)]
        public String Id { get; set; }

        [JsonProperty("visitor"), DefaultValue(null)]
        public String Visitor { get; set; }

        [JsonProperty("score", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Double? Score { get; set; }

        [JsonProperty("recommendation", DefaultValueHandling = DefaultValueHandling.Ignore), DefaultValue(KondutoRecommendation.none)]
        public KondutoRecommendation Recommendation { get; set; }

        [JsonProperty("device", DefaultValueHandling = DefaultValueHandling.Ignore), DefaultValue(null)]
        public KondutoDevice Device { get; set; }

        [JsonProperty("navigation", DefaultValueHandling = DefaultValueHandling.Ignore), DefaultValue(null)]
        public KondutoNavigationInfo NavigationInfo { get; set; }

        [JsonProperty("geolocation", DefaultValueHandling = DefaultValueHandling.Ignore), DefaultValue(null)]
        public KondutoGeolocation Geolocation { get; set; }

        [JsonProperty("timestamp", DefaultValueHandling = DefaultValueHandling.Ignore), DefaultValue(0)]
        public long Timestamp { get; set; }

        /* Constructors */
        public KondutoOrderResponse() { }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="o">object to compare</param>
        /// <returns></returns>
        public override bool Equals(Object o)
        {
            if (this == o) return true;
		    if (!(o is KondutoOrder)) return false;

            var that = o as KondutoOrder;

            if (!Equals(Id, that.Id)) return false;
            if (!Equals(Recommendation, that.Recommendation)) return false;
            if (!Equals(Score, that.Score)) return false;

            if (!Equals(Visitor, that.Visitor)) return false;
            if (!Equals(Geolocation, that.Geolocation)) return false;
            if (!Equals(Timestamp, that.Timestamp)) return false;

            if (!Equals(Device, that.Device)) return false;
            if (!Equals(NavigationInfo, that.NavigationInfo)) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
