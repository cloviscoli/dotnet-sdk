using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using KdtSdk.Exceptions;
using KdtSdk.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KdtSdk
{
    /// <summary>
    ///     Konduto is an HTTP Client for connecting to Konduto's API.
    /// </summary>
    public class Konduto
    {
        public const string Version = "1.0.9";

        public HttpMessageHandler MessageHandler = null;

        private string _apiKey;
        private Uri _endpoint;
        private string _proxyAddress;
        private string _proxyPassword;
        private string _proxyUsername;
        private string _requestBody;
        private string _responseBody;

        private bool _useProxy;

        public Konduto(string apiKey)
        {
            SetApiKey(apiKey);
            _endpoint = new Uri("https://api.konduto.com/v1/");
        }

        /// <summary>
        ///     Konduto's API endpoint (default is https://api.konduto.com/v1/)
        /// </summary>
        /// <param name="endpoint"></param>
        public void SetEndpoint(Uri endpoint)
        {
            _endpoint = endpoint;
        }

        /// <summary>
        ///     sets the merchant secret API key, which is required for Konduto's API authentication.
        /// </summary>
        /// <param name="apiKey"></param>
        public void SetApiKey(string apiKey)
        {
            if (apiKey == null || apiKey.Length != 21)
                throw new ArgumentOutOfRangeException("Illegal API Key: " + apiKey);
            _apiKey = apiKey;
        }

        /// <summary>
        ///     Helper method to debug requests made to Konduto's API.
        /// </summary>
        /// <returns>a String containing API Key, Konduto's API endpoint, request and response bodies.</returns>
        public string Debug()
        {
            var sb = new StringBuilder();
            sb.Append($"API Key: {_apiKey}\n");
            sb.Append($"Endpoint: {_endpoint}\n");
            if (_requestBody != null) sb.Append($"Request body: {_requestBody}\n");
            if (_responseBody != null) sb.Append($"Response body: {_responseBody}\n");
            return sb.ToString();
        }

        /// <summary>
        /// </summary>
        /// <param name="orderId">the order identifier</param>
        /// <returns>[GET] order URI (ENDPOINT/orders/orderId)</returns>
        public Uri KondutoGetOrderUrl(string orderId)
        {
            return new Uri(_endpoint + KondutoGetOrderSuffix(orderId));
        }

        /// <summary>
        /// </summary>
        /// <param name="orderId">the order identifier</param>
        /// <returns>/orders/orderId</returns>
        public string KondutoGetOrderSuffix(string orderId)
        {
            return "orders/" + orderId;
        }

        /// <summary>
        ///     [POST] order URI (ENDPOINT/orders)
        /// </summary>
        /// <returns></returns>
        public Uri KondutoPostOrderUrl()
        {
            return new Uri(_endpoint + KondutoPostOrderUrlSuffix());
        }

        /// <summary>
        ///     [POST] order suffix (/orders)
        /// </summary>
        /// <returns></returns>
        public string KondutoPostOrderUrlSuffix()
        {
            return "orders";
        }

        /// <summary>
        /// </summary>
        /// <param name="orderId">the order identifier</param>
        /// <returns>[PUT] order URI (ENDPOINT/orders/orderId)</returns>
        public Uri KondutoPutOrderUrl(string orderId)
        {
            return new Uri(_endpoint + KondutoPutOrderUrlSuffix(orderId));
        }

        /// <summary>
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns>/orders/orderId</returns>
        public string KondutoPutOrderUrlSuffix(string orderId)
        {
            return "orders/" + orderId;
        }

        /// <summary>
        ///     Set proxy address, send only the address with http://ip, even for https connections
        /// </summary>
        /// <param name="proxyAddress">Proxy address, even for https connections, use http</param>
        /// <param name="proxyUser"></param>
        /// <param name="proxyPassword"></param>
        public void SetProxy(string proxyAddress, string proxyUser, string proxyPassword)
        {
            _proxyUsername = proxyUser;
            _proxyPassword = proxyPassword;
            _proxyAddress = proxyAddress;
            _useProxy = true;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        private HttpClient CreateHttpClient()
        {
            var base64 = Base64Encode(_apiKey);

            HttpClient httpClient;

            if (!_useProxy)
            {
                httpClient = MessageHandler == null ? new HttpClient() : new HttpClient(MessageHandler);
            }
            else
            {
                var httpClientHandler = new HttpClientHandler
                {
                    UseDefaultCredentials = false,
                    Proxy =
                        new WebProxy(_proxyAddress, false, null, new NetworkCredential(_proxyUsername, _proxyPassword)),
                    UseProxy = true
                };

                //httpClient = HttpClientFactory.Create(httpClientHandler);
                httpClient = new HttpClient(httpClientHandler);
            }

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + base64);
            httpClient.DefaultRequestHeaders.Add("X-Requested-With", "Konduto SDK .NET " + Version);
            httpClient.BaseAddress = _endpoint;

            return httpClient;
        }

        /// <summary>
        ///     Queries an order from Konduto's API.
        ///     Syncronous
        ///     @see <a href="http://docs.konduto.com">Konduto API Spec</a>
        /// </summary>
        /// <param name="orderId">the order identifier</param>
        /// <returns>a {@link KondutoOrder} instance</returns>
        /// <exception cref="KondutoHTTPException"></exception>
        /// <exception cref="KondutoUnexpectedAPIResponseException"></exception>
        public KondutoOrder GetOrder(string orderId)
        {
            var client = CreateHttpClient();

            _requestBody = orderId;

            var response = client.GetAsync(KondutoGetOrderSuffix(orderId)).Result;
            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content;

                var responseString = responseContent.ReadAsStringAsync().Result;
                _responseBody = responseString;

                var getResponse = JsonConvert.DeserializeObject<JObject>(responseString);

                KondutoOrder order = null;

                JToken jt;
                if (getResponse.TryGetValue("order", out jt))
                    order = KondutoModel.FromJson<KondutoOrder>(jt.ToString());

                return order;
            }

            throw KondutoHTTPExceptionFactory.buildException((int) response.StatusCode,
                response.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        ///     Sends an order for Konduto and gets it analyzed
        ///     (i.e with recommendation, score, device, geolocation and navigation info).
        ///     @see <a href="http://docs.konduto.com">Konduto API Spec</a>
        /// </summary>
        /// <param name="order">a {@link KondutoOrder} instance</param>
        /// <exception cref="KondutoInvalidEntityException"></exception>
        /// <exception cref="KondutoHTTPException"></exception>
        /// <exception cref="KondutoUnexpectedAPIResponseException"></exception>
        public KondutoOrder Analyze(KondutoOrder order)
        {
            var httpClient = CreateHttpClient();

            var response = httpClient.PostAsync(KondutoPostOrderUrl(),
                new StringContent(order.ToJson(),
                    Encoding.UTF8,
                    "application/json"));

            _requestBody = order.ToJson();

            if (response.Result.IsSuccessStatusCode)
            {
                // by calling. Result you are performing a synchronous call
                var responseContent = response.Result.Content;

                // by calling. Result you are synchronously reading the result
                var responseString = responseContent.ReadAsStringAsync().Result;

                _responseBody = responseString;

                if (order.Analyze)
                    order.MergeKondutoOrderResponse(KondutoModel.FromJson<KondutoAPIFullResponse>(responseString)
                        .Order);

                return order;
            }

            var responseContentError = response.Result.Content != null
                ? response.Result.Content.ReadAsStringAsync().Result
                : "Error with response";
            throw KondutoHTTPExceptionFactory.buildException((int) response.Result.StatusCode,
                responseContentError);
        }

        /// <summary>
        ///     Updates an order status.
        ///     @see <a href="http://docs.konduto.com">Konduto API Spec</a>
        /// </summary>
        /// <param name="orderId">the order to update</param>
        /// <param name="newStatus">the new status (APPROVED, DECLINED or FRAUD)</param>
        /// <param name="comments">
        ///     some comments (an empty String is accepted, although we recommend setting it to at least a
        ///     timestamp)
        /// </param>
        /// <exception cref="KondutoHTTPException"></exception>
        /// <exception cref="KondutoUnexpectedAPIResponseException"></exception>
        public void UpdateOrderStatus(string orderId, KondutoOrderStatus newStatus, string comments)
        {
            var allowed = new HashSet<KondutoOrderStatus>
            {
                KondutoOrderStatus.approved,
                KondutoOrderStatus.declined,
                KondutoOrderStatus.fraud,
                KondutoOrderStatus.canceled,
                KondutoOrderStatus.not_authorized
            };

            if (!allowed.Contains(newStatus)) throw new ArgumentException("Illegal status: " + newStatus);

            if (comments == null) throw new NullReferenceException("Comments cannot be null.");

            var requestBody = new JObject();
            requestBody.Add("status", newStatus.ToString().ToLower());
            requestBody.Add("comments", comments);

            var client = CreateHttpClient();

            var response = client.PutAsync(KondutoPutOrderUrl(orderId),
                new StringContent(
                    requestBody.ToString(),
                    Encoding.UTF8,
                    "application/json"));

            if (response.Result.IsSuccessStatusCode)
            {
                var responseBody = JsonConvert.DeserializeObject<JObject>(
                    response.Result.Content.ReadAsStringAsync().Result);

                _responseBody = responseBody.ToString();

                if (responseBody.TryGetValue("order", out var orderUpdateResponse))
                {
                    var updatedOrder = JsonConvert.DeserializeObject<JObject>(orderUpdateResponse.ToString());

                    if (!updatedOrder.TryGetValue("old_status", out var statusResponse) ||
                        !updatedOrder.TryGetValue("new_status", out statusResponse))
                        throw new KondutoUnexpectedAPIResponseException(responseBody.ToString());
                }
            }
            else
            {
                throw KondutoHTTPExceptionFactory.buildException((int) response.Result.StatusCode,
                    response.Result.Content.ReadAsStringAsync().Result);
            }
        }

        /// <summary>
        ///     Base64 Encode
        /// </summary>
        /// <param name="plainText">Text to encode</param>
        /// <returns></returns>
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        ///     Base64 Decode
        /// </summary>
        /// <param name="base64EncodedData">Text to decode</param>
        /// <returns></returns>
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        private class KondutoAPIFullResponse : KondutoModel
        {
            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("order")]
            public KondutoOrderResponse Order { get; set; }
        }
    }
}