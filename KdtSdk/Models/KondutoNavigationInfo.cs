using System;
using Newtonsoft.Json;

namespace KdtSdk.Models
{
    public class KondutoNavigationInfo : KondutoModel
    {
        /* all times in minutes */
        [JsonProperty("session_time")]
	    public Double SessionTime { get; set; }

        [JsonProperty("referrer")]
	    public String Referrer { get; set; }

	    [JsonProperty("time_site_1d")]
        public Double TimeOnSiteToday { get; set; }

	    [JsonProperty("new_accounts_1d")]
        public int AccountsCreatedToday { get; set; }

	    [JsonProperty("password_resets_1d")]
        public int PasswordResetsToday { get; set; }

	    [JsonProperty("sales_declined_1d")]
        public int SalesDeclinedToday { get; set; }

	    [JsonProperty("sessions_1d")]
        public int SessionsToday { get; set; }

	    [JsonProperty("time_site_7d")]
        public Double TimeOnSiteSinceLastWeek { get; set; }

	    [JsonProperty("new_accounts_7d")]
        public int AccountsCreatedSinceLastWeek { get; set; }

	    [JsonProperty("time_per_page_7d")]
        public Double TimePerPageSinceLastWeek { get; set; }

	    [JsonProperty("password_resets_7d")]
        public int PasswordResetsSinceLastWeek { get; set; }

	    [JsonProperty("checkout_count_7d")]
        public int CheckoutPageViewsSinceLastWeek { get; set; }

	    [JsonProperty("sales_declined_7d")]
        public int SalesDeclinedSinceLastWeek { get; set; }

	    [JsonProperty("sessions_7d")]
        public int SessionsSinceLastWeek { get; set; }

        [JsonProperty("time_since_last_sale")]
        public Double TimeSinceLastSale { get; set; }

	    public override bool Equals(Object o) 
        {
		    if (this == o) return true;
		    if (!(o is KondutoNavigationInfo)) return false;

            var that = o as KondutoNavigationInfo;

            if (!Equals(AccountsCreatedSinceLastWeek, that.AccountsCreatedSinceLastWeek)) return false;
            if (!Equals(AccountsCreatedToday, that.AccountsCreatedToday)) return false;
            if (!Equals(CheckoutPageViewsSinceLastWeek, that.CheckoutPageViewsSinceLastWeek)) return false;
            if (!Equals(PasswordResetsSinceLastWeek, that.PasswordResetsSinceLastWeek)) return false;
            if (!Equals(PasswordResetsToday, that.PasswordResetsToday)) return false;
            if (!Equals(Referrer, that.Referrer)) return false;
            if (!Equals(SalesDeclinedSinceLastWeek, that.SalesDeclinedSinceLastWeek)) return false;
            if (!Equals(SalesDeclinedToday, that.SalesDeclinedToday)) return false;
            if (!Equals(SessionTime, that.SessionTime)) return false;
            if (!Equals(SessionsSinceLastWeek, that.SessionsSinceLastWeek)) return false;
            if (!Equals(SessionsToday, that.SessionsToday)) return false;
            if (!Equals(TimeOnSiteSinceLastWeek, that.TimeOnSiteSinceLastWeek)) return false;
            if (!Equals(TimeOnSiteToday, that.TimeOnSiteToday)) return false;
            if (!Equals(TimePerPageSinceLastWeek, that.TimePerPageSinceLastWeek)) return false;
            if (!Equals(TimeSinceLastSale, that.TimeSinceLastSale)) return false;

		    return true;
	    }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
