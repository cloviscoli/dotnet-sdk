using Newtonsoft.Json;
using System;

namespace KdtSdk.Models
{
    /// <summary>
    /// * Address model.
    /// @see <a href="http://docs.konduto.com">Konduto API Spec</a>
    /// </summary>
    public class KondutoAddress : KondutoModel
    {
        #region Attributes

        [JsonProperty("name")]
        public String Name { get; set; }
        [JsonProperty("address1")]
        public String Address1 { get; set; }
        [JsonProperty("address2")]
        public String Address2 { get; set; }
        [JsonProperty("zip")]
        public String Zip { get; set; }
        [JsonProperty("city")]
        public String City { get; set; }
        [JsonProperty("state")]
        public String State { get; set; }
        [JsonProperty("country")]
        public String Country { get; set; }

        #endregion

	    /// <summary>
	    /// Constructor
	    /// </summary>
	    public KondutoAddress() 
        {
            Name = null;
            Address1 = null;
            Address2 = null;
            Zip = null;
            City = null;
            State = null;
            Country = null;
        }

        public KondutoAddress WithName(string name)
        {
            Name = name;
            return this;
        }

	    public override bool Equals(object o) {
		    if (this == o) return true;

		    if (!(o is KondutoAddress)) return false;

		    var that = o as KondutoAddress;

            if (!Equals(Address1, that.Address1)) return false;
            if (!Equals(Address2, that.Address2)) return false;
            if (!Equals(City, that.City)) return false;

            if (!Equals(Country, that.Country)) return false;
            if (!Equals(Name, that.Name)) return false;
            if (!Equals(State, that.State)) return false;
            if (!Equals(Zip, that.Zip)) return false;

		    return true;
	    }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
