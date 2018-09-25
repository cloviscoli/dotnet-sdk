using System;
using Newtonsoft.Json;
using KdtSdk.Exceptions;
using KdtSdk.Utils;

namespace KdtSdk.Models
{
    public class KondutoModel
    {
        private string _error;

        public string GetError()
        {
            return _error;
        }

        public String ToJson()
        {
            var s = new JsonSerializerSettings();
            s.MissingMemberHandling = MissingMemberHandling.Ignore;
            s.NullValueHandling = NullValueHandling.Ignore;
            s.MissingMemberHandling = MissingMemberHandling.Ignore;
            s.TypeNameHandling = TypeNameHandling.Auto;
            
            try
            {
                JsonConvert.SerializeObject(this, Formatting.Indented, s);
                return JsonConvert.SerializeObject(this, Formatting.Indented);
            }
            catch (JsonSerializationException e)
            {
                throw new KondutoInvalidEntityException(this);
            }
            catch (Exception e)
            {
                throw new KondutoInvalidEntityException(this);
            }
        }

        public static T FromJson<T>(string jsonObject)
        {
            return JsonConvert.DeserializeObject<T>(jsonObject, new KondutoUtils.PaymentConverter());
        }

        /// <summary>
        /// Validation method 
        /// </summary>
        /// <returns>whether this KondutoModel instance is valid or not.</returns>
        public bool IsValid()
        {
            _error = null;

            try
            {
                JsonConvert.SerializeObject(this, Formatting.Indented);
            }
            catch (Exception e)
            {
                _error = e.Message;
                return false;
            }
            
            return true;
		}
    }
}
