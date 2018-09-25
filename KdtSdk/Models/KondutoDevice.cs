using System;
using Newtonsoft.Json;

namespace KdtSdk.Models
{
    public class KondutoDevice : KondutoModel
    {
        [JsonProperty("user_id")]
        public String UserId { get; set; }
        [JsonProperty("fingerprint")]
        public String Fingerprint { get; set; }
        [JsonProperty("platform")]
        public String Platform { get; set; }
        [JsonProperty("browser")]
        public String Browser { get; set; }
        [JsonProperty("language")]
        public String Language { get; set; }
        [JsonProperty("timezone")]
        public String Timezone { get; set; }
        [JsonProperty("cookie")]
        public bool Cookie { get; set; }
        [JsonProperty("javascript")]
        public bool Javascript { get; set; }
        [JsonProperty("flash")]
        public bool Flash { get; set; }
        [JsonProperty("ip")]
        public String Ip { get; set; }

	    public KondutoDevice(){}

	    public override bool Equals(object o) 
        {
		    if (this == o) return true;
		    if (!(o is KondutoDevice)) return false;

            var that = o as KondutoDevice;

            if (!Equals(Cookie, that.Cookie)) return false;
            if (!Equals(Flash, that.Flash)) return false;
            if (!Equals(Javascript, that.Javascript)) return false;
            if (!Equals(Browser, that.Browser)) return false;
            if (!Equals(Fingerprint, that.Fingerprint)) return false;
            if (!Equals(Ip, that.Ip)) return false;
            if (!Equals(Language, that.Language)) return false;
            if (!Equals(Platform, that.Platform)) return false;
            if (!Equals(Timezone, that.Timezone)) return false;
            if (!Equals(UserId, that.UserId)) return false;

		    return true;
	    }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}