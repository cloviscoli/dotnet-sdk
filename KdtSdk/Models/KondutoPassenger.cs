using System;
using Newtonsoft.Json;

namespace KdtSdk.Models
{
    public class KondutoPassenger : KondutoModel
    {
        #region Attributes

        [JsonProperty("name", Required = Required.Always)]
        public String Name { get; set; }
        [JsonProperty("document", Required = Required.Always)]
        public String Document { get; set; }

        [JsonProperty("document_type", Required = Required.Always)]
        public String DocumentType { get; set; }

        [JsonProperty("dob", Required = Required.Always)]
        public String Dob { get; set; }

        [JsonProperty("nationality")]
        public String Nationality { get; set; }

        [JsonProperty("frequent_traveler")]
        public bool FrequentTraveler { get; set; }

        [JsonProperty("special_needs")]
        public bool SpecialNeeds { get; set; }

        [JsonProperty("loyalty")]
        public KondutoLoyaltyProgram Loyalty { get; set; }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public KondutoPassenger() {}

        public override bool Equals(object o)
        {
            if (this == o) return true;
            if (!(o is KondutoPassenger)) return false;

            var that = o as KondutoPassenger;

            if (!Equals(Name, that.Name)) return false;
            if (!Equals(Document, that.Document)) return false;
            if (!Equals(DocumentType, that.DocumentType)) return false;
            if (!Equals(Dob, that.Dob)) return false;
            if (!Equals(Nationality, that.Nationality)) return false;
            if (!Equals(FrequentTraveler, that.FrequentTraveler)) return false;
            if (!Equals(SpecialNeeds, that.SpecialNeeds)) return false;
            if (!Equals(Loyalty, that.Loyalty)) return false;

            return true;
        }
    }
}
