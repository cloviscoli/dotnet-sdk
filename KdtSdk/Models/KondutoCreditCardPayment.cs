using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace KdtSdk.Models
{
    public class KondutoCreditCardPayment : KondutoPayment
    {
        [JsonProperty("bin")]
        public string Bin { get; set; }
        [JsonProperty("last4")]
        public string Last4 { get; set; }
        [JsonProperty("expiration_date")]
        public string ExpirationDate { get; set; }
        [JsonProperty("status"), JsonConverter(typeof(StringEnumConverter))]
        public KondutoCreditCardPaymentStatus Status { get; set; }

        public KondutoCreditCardPayment()
            : base(KondutoPaymentType.credit) { }


        public override bool Equals(object o) 
        {
		    if (this == o) return true;
		    if (!(o is KondutoCreditCardPayment)) return false;

            var that = o as KondutoCreditCardPayment;

            if (!Equals(Bin, that.Bin)) return false;
            if (!Equals(ExpirationDate, that.ExpirationDate)) return false;
            if (!Equals(Last4, that.Last4)) return false;
            if (!Equals(Status, that.Status)) return false;

		    return true;
	    }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}