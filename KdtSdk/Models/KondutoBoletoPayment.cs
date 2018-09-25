using Newtonsoft.Json;
using System;

namespace KdtSdk.Models
{
    public class KondutoBoletoPayment : KondutoPayment
    {
        //Expiration date. YYYY-MM-DD
        [JsonProperty("expiration_date")]
        public string ExpirationDate { get; set; }

        public KondutoBoletoPayment()
        : base(KondutoPaymentType.boleto){ }

        public override bool Equals(Object o){
            if (this == o) return true;
		    if (!(o is KondutoBoletoPayment)) return false;

            var that = o as KondutoBoletoPayment;

            if (!Equals(ExpirationDate, that.ExpirationDate)) return false;

		    return true;
        }

        public override int GetHashCode()
        {
 	         return base.GetHashCode();
        }
    }
}