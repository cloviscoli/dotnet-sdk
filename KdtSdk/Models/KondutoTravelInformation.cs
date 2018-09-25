using System;
using Newtonsoft.Json;

namespace KdtSdk.Models
{
    public class KondutoTravelInformation : KondutoModel
    {
        [JsonProperty("origin_city")]
        public String OriginCity { get; set; }
        [JsonProperty("destination_city")]
        public String DestinationCity { get; set; }

        [JsonProperty("origin_airport")]
        public String OriginAirport { get; set; }
        [JsonProperty("destination_airport")]
        public String DestinationAirport { get; set; }

        [JsonProperty("date", Required = Required.Always)]
        public String Date { get; set; }
        [JsonProperty("number_of_connections")]
        public int NumberOfConnections { get; set; }
        [JsonProperty("class")]
        public String Class { get; set; }
        [JsonProperty("fare_basis")]
        public String FareBasis { get; set; }

        public KondutoTravelInformation() { }

        public override bool Equals(object o)
        {
            if (this == o) return true;
            if (!(o is KondutoTravelInformation)) return false;

            var that = o as KondutoTravelInformation;

            // required
            if (!Equals(OriginAirport, that.OriginAirport)) return false;
            if (!Equals(DestinationAirport, that.DestinationAirport)) return false;
            if (!Equals(OriginCity, that.OriginCity)) return false;
            if (!Equals(DestinationCity, that.DestinationCity)) return false;
            if (!Equals(Date, that.Date)) return false;
            if (!Equals(NumberOfConnections, that.NumberOfConnections)) return false;
            if (!Equals(Class, that.Class)) return false;
            if (!Equals(FareBasis, that.FareBasis)) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
