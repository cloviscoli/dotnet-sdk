using Newtonsoft.Json;

namespace KdtSdk.Models
{
    public class KondutoSeller : KondutoModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        
        /// <summary>
        /// YYYY-MM-DD
        /// </summary>
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        /* Constructors */

        public KondutoSeller() { }

        public override bool Equals(object o)
        {
            if (this == o) return true;
            if (!(o is KondutoSeller)) return false;

            var that = o as KondutoSeller;

            if (!Equals(Id, that.Id)) return false;
            if (!Equals(Name, that.Name)) return false;
            if (!Equals(CreatedAt, that.CreatedAt)) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
