using System;
using Newtonsoft.Json;

namespace KdtSdk.Models
{
    public class KondutoItem : KondutoModel
    {
        [JsonProperty("sku")]
        public String Sku { get; set; }
        [JsonProperty("category")]
        public int Category { get; set; }
        [JsonProperty("name")]
        public String Name { get; set; }
        [JsonProperty("description")]
        public String Description { get; set; }
        [JsonProperty("product_code")]
        public String ProductCode { get; set; }
        [JsonProperty("unit_cost")]
        public Double UnitCost { get; set; }
        [JsonProperty("quantity")]
        public float Quantity { get; set; }
        [JsonProperty("discount")]
        public Double Discount { get; set; }

        /// <summary>
        /// YYYY-MM-DD
        /// </summary>
        [JsonProperty("created_at")]
        public String CreatedAt { get; set; }

        public bool ShouldSerializeDiscount()
        {
            return Discount != 0;
        }

	    /* Constructors */

	    public KondutoItem(){}

	    public override bool Equals(object o) 
        {
		    if (this == o) return true;
		    if (!(o is KondutoItem)) return false;

            var that = o as KondutoItem;

            if (!Equals(Sku, that.Sku)) return false;
            if (!Equals(Category, that.Category)) return false;
            if (!Equals(Name, that.Name)) return false;
            if (!Equals(Description, that.Description)) return false;
            if (!Equals(ProductCode, that.ProductCode)) return false;
            if (!Equals(UnitCost, that.UnitCost)) return false;
            if (!Equals(Quantity, that.Quantity)) return false;
            if (!Equals(Discount, that.Discount)) return false;
            if (!Equals(CreatedAt, that.CreatedAt)) return false;

		    return true;
	    }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
