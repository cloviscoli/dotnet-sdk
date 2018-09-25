using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace KdtSdk.Models
{
    /// <summary>
    /// * Address model.
    /// @see <a href="http://docs.konduto.com">Konduto API Spec</a>
    /// </summary>
    public class KondutoTravel : KondutoModel
    {
        #region Attributes
        [JsonProperty("type", Required = Required.Always), JsonConverter(typeof(StringEnumConverter))]
        public KondutoTravelType TravelType { get; set; }

        [JsonProperty("passengers", Required = Required.Always)]
        public List<KondutoPassenger> Passengers { get; set; }

        [JsonProperty("departure", Required = Required.Always)]
        public KondutoTravelInformation Departure { get; set; }

        [JsonProperty("return")]
        public KondutoTravelInformation Return { get; set; }

        #endregion

        public override bool Equals(object o)
        {
            if (this == o) return true;

            if (!(o is KondutoTravel)) return false;

            var that = o as KondutoTravel;

            if (!Passengers.SequenceEqual(that.Passengers)) return false;
            if (!Equals(Departure, that.Departure)) return false;
            if (!Equals(Return, that.Return)) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}