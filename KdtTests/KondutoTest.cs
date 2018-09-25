﻿using KdtSdk;
using KdtSdk.Exceptions;
using KdtSdk.Models;
using KdtTests.Factories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;


namespace KdtTests
{
    [TestClass]
    public class KondutoTest
    {
        //static String AUTH_HEADER = "Basic VDczOEQ1MTZGMDlDQUIzQTJDMUVF";
        static String AUTH_HEADER = "VDczOEQ1MTZGMDlDQUIzQTJDMUVF";
        static String API_KEY = "T738D516F09CAB3A2C1EE";

        String ORDER_ID;

        JObject ANALYZE_ORDER_RESPONSE;
        KondutoOrder ORDER_FROM_FILE;

        JObject NOT_ANALYZE_ORDER_RESPONSE;


        int[] HTTP_STATUSES = {
			    (int)HttpStatusCode.Unauthorized, // 401
			    (int)HttpStatusCode.Forbidden, // 403
			    (int)HttpStatusCode.NotFound, // 404
			    422, // 422
			    (int)HttpStatusCode.MethodNotAllowed, // 425
			    429, // 429
			    (int)HttpStatusCode.InternalServerError // 500
	    };

        private Konduto konduto;

        [TestInitialize]
        public void Setup()
        {
            konduto = new Konduto(API_KEY);

            ANALYZE_ORDER_RESPONSE = JsonConvert.DeserializeObject<JObject>(
                Properties.Resources.konduto_order);

            JToken jt;
            ANALYZE_ORDER_RESPONSE.TryGetValue("order", out jt);
            ORDER_FROM_FILE = KondutoModel.FromJson<KondutoOrder>(jt.ToString());
            ORDER_ID = ORDER_FROM_FILE.Id;

            NOT_ANALYZE_ORDER_RESPONSE = JsonConvert.DeserializeObject<JObject>(
                Properties.Resources.konduto_order_not_analyzed);
        }

        //[TestMethod]
        //public void TestProxy()
        //{
        //    Konduto konduto = new Konduto("T738D516F09CAB3A2C1EE");
        //    konduto.SetProxy("192.168.0.14:808", "user1", "1234");

        //    KondutoCustomer Customer = new KondutoCustomer
        //    {
        //        Id = "28372",
        //        Name = "KdtUser",
        //        Email = "developer@example.com"
        //    };

        //    KondutoOrder order = new KondutoOrder
        //    {
        //        Id = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString(),
        //        Visitor = "38a9412f0b01b4dd1762ae424169a3e490d75c7a",
        //        TotalAmount = 100.00,
        //        Customer = Customer,
        //        Analyze=true
        //    };

        //    try
        //    {
        //        konduto.Analyze(order);
        //        Assert.IsTrue(order.Recommendation != KondutoRecommendation.none);
        //    }
        //    catch (KondutoException ex)
        //    {
        //        Assert.Fail("Konduto exception shouldn't happen here.");
        //    }
        //}

        [TestMethod]
        public void GetOrderSuccessfullyTest()
        {
            Konduto kdt = new Konduto(API_KEY);

            var fakeResponseHandler = new FakeResponseHandler();
            var message = new HttpResponseMessage(HttpStatusCode.OK);
            message.Content = new StringContent(ANALYZE_ORDER_RESPONSE.ToString());

            var c = kdt.KondutoGetOrderUrl(ORDER_ID);

            fakeResponseHandler.AddFakeResponse(kdt.KondutoGetOrderUrl(ORDER_ID), message);
            kdt.MessageHandler = fakeResponseHandler;

            var v = kdt.GetOrder(ORDER_ID);

            Assert.IsTrue(ORDER_FROM_FILE.Equals(kdt.GetOrder(ORDER_ID)));
        }

        [TestMethod]
        public void GetOrderErrorTest()
        {
            foreach (int code in HTTP_STATUSES)
            {
                try
                {
                    var fakeResponseHandler = new FakeResponseHandler();

                    var message = new HttpResponseMessage((HttpStatusCode)code);
                    message.Content = new StringContent(ORDER_FROM_FILE.ToJson());

                    fakeResponseHandler.AddFakeResponse(konduto.KondutoGetOrderUrl(ORDER_ID), message);
                    konduto.MessageHandler = fakeResponseHandler;

                    konduto.GetOrder(ORDER_ID);
                    Assert.Fail("Exception expected");
                }
                catch (KondutoHTTPException e)
                {
                    //Ok
                }
                catch (Exception e)
                {
                    Assert.Fail("KondutoHTTPException was expected");
                }
            }
        }

        [TestMethod]
        public void AnalyzeSuccessfullyTest()
        {
            var fakeResponseHandler = new FakeResponseHandler();
            var message = new HttpResponseMessage(HttpStatusCode.OK);

            message.Content = new StringContent(ANALYZE_ORDER_RESPONSE.ToString());

            fakeResponseHandler.AddFakeResponse(konduto.KondutoPostOrderUrl(), message);
            konduto.MessageHandler = fakeResponseHandler;

            KondutoOrder orderToSend = KondutoOrderFactory.basicOrder();
            String s = orderToSend.ToJson();
            KondutoOrder orderResponse = null;

            Assert.IsTrue(orderToSend.Recommendation == KondutoRecommendation.none, "basic order should have no recommendation");
            Assert.IsTrue(orderToSend.Score == null, "basic order should have no score");
            Assert.IsNull(orderToSend.Geolocation, "basic order should have no geolocation");
            Assert.IsNull(orderToSend.Device, "basic order should have no device");
            Assert.IsNull(orderToSend.NavigationInfo, "basic order should have no navigation info");
            Assert.IsTrue(orderToSend.Analyze, "basic order should have analyze set to true");

            try
            {
                orderResponse = konduto.Analyze(orderToSend); // do analyze
            }
            catch (KondutoInvalidEntityException e)
            {
                Assert.Fail("order should be valid");
            }
            catch (KondutoUnexpectedAPIResponseException e)
            {
                Assert.Fail("server should respond with status 200");
            }
            catch (KondutoHTTPException e)
            {
                Assert.Fail("server should respond with status 200");
            }

            Double? actualScore = ORDER_FROM_FILE.Score;
            KondutoRecommendation? actualRecommendation = ORDER_FROM_FILE.Recommendation;
            KondutoGeolocation actualGeolocation = ORDER_FROM_FILE.Geolocation;
            KondutoDevice actualDevice = ORDER_FROM_FILE.Device;
            KondutoNavigationInfo actualNavigationInfo = ORDER_FROM_FILE.NavigationInfo;

            Assert.IsTrue(orderResponse.Geolocation.Equals(actualGeolocation));
            Assert.AreEqual(orderResponse.Recommendation, actualRecommendation);
            Assert.AreEqual(orderResponse.Device, actualDevice);
            Assert.AreEqual(orderResponse.NavigationInfo, actualNavigationInfo);
            Assert.AreEqual(orderResponse.Score, actualScore);
        }

        [TestMethod]
        public void PostIntegrationTest()
        {
            Konduto konduto = new Konduto("T738D516F09CAB3A2C1EE");

            KondutoCustomer Customer = new KondutoCustomer
            {
                Id = "28372",
                Name = "KdtUser",
                Email = "developer@example.com"
            };

            KondutoOrder order = new KondutoOrder
            {
                Id = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString(),
                Visitor = "38a9412f0b01b4dd1762ae424169a3e490d75c7a",
                TotalAmount = 100.00,
                Customer = Customer,
                Analyze = true
            };

            try
            {
                konduto.Analyze(order);
                Assert.IsTrue(order.Recommendation != KondutoRecommendation.none);
            }
            catch (KondutoException ex)
            {
                Assert.Fail("Konduto exception shouldn't happen here.");
            }
        }

        [TestMethod]
        public void FullJsonIntegrationTest()
        {
            Konduto konduto = new Konduto("T738D516F09CAB3A2C1EE");

            KondutoCustomer Customer = new KondutoCustomer
            {
                Id = "28372",
                Name = "KdtUser",
                Email = "developer@example.com",
                TaxId = "613.815.776-10",
                Phone1 = "+559912345678",
                Phone2 = "(11)1234-5678",
                IsNew = false,
                IsVip = false,
                CreatedAt = "2014-12-21"
            };

            KondutoPayment payment = new KondutoCreditCardPayment
            {
                Type = KondutoPaymentType.credit,
                Status = KondutoCreditCardPaymentStatus.approved,
                Bin = "490172",
                Last4 = "0012",
                ExpirationDate = "052026"
            };

            KondutoAddress billing = new KondutoAddress
            {
                Name = "Mark Thompson",
                Address1 = "101 Maple Road",
                Address2 = "Apto 33",
                City = "Mato Grosso",
                State = "CuiabÃ¡",
                Zip = "302798",
                Country = "BR"
            };

            KondutoAddress shipping = new KondutoAddress
            {
                Name = "Mark Thompson",
                Address1 = "101 Maple Road",
                Address2 = "Apto 33",
                City = "Mato Grosso",
                State = "CuiabÃ¡",
                Zip = "302798",
                Country = "BR"
            };

            List<KondutoPayment> payments = new List<KondutoPayment>{
                payment
            };

            KondutoItem item1 = new KondutoItem
            {
                Sku = "9919023",
                ProductCode = "123456789999",
                Category = 100,
                Name = "Xbox One",
                Description = "Xbox One PromoÃ§Ã£o Com 2 Controles",
                UnitCost = 1999.99,
                Quantity = 1,
                CreatedAt = "2014-12-21"
            };

            KondutoItem item2 = new KondutoItem
            {
                Sku = "0017273",
                Category = 201,
                Name = "CD Nirvana Nevermind",
                Description = "CD Nirvana Nevermind",
                UnitCost = 29.90,
                Quantity = 2,
                Discount = 5.00
            };

            KondutoOrder order = new KondutoOrder
            {
                Id = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString(),
                Visitor = "38a9412f0b01b4dd1762ae424169a3e490d75c7a",
                TotalAmount = 100.00,
                ShippingAmount = 6.00,
                TaxAmount = 12.00,
                Ip = "201.27.127.73",
                Currency = "BRL",
                Customer = Customer,
                Payments = payments,
                BillingAddress = billing,
                ShippingAddress = shipping,
                MessagesExchanged = 2,
                PurchasedAt = "2014-12-31T13:00:00Z",
                FirstMessage = "2014-12-31T13:00:00Z",
                ShoppingCart = new List<KondutoItem>{
                    item1,
                    item2
                },
                Analyze = true
            };

            try
            {
                konduto.Analyze(order);
                Assert.IsTrue(order.Recommendation != KondutoRecommendation.none);
            }
            catch (KondutoException ex)
            {
                Assert.Fail("Konduto exception shouldn't happen here.");
            }

            try
            {
                KondutoOrder getOrder = konduto.GetOrder(order.Id);
                Assert.IsNotNull(getOrder);
            }
            catch (KondutoException ex)
            {
                Assert.Fail("Konduto exception shouldn't happen here.");
            }
        }

        [TestMethod]
        public void PostBoletoIntegrationTest()
        {
            Konduto konduto = new Konduto("T738D516F09CAB3A2C1EE");

            KondutoCustomer Customer = new KondutoCustomer
            {
                Id = "28372",
                Name = "KdtUser",
                Email = "developer@example.com"
            };

            KondutoOrder order = new KondutoOrder
            {
                Id = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString(),
                Visitor = "38a9412f0b01b4dd1762ae424169a3e490d75c7a",
                TotalAmount = 100.00,
                Customer = Customer,
                Payments = KondutoPaymentFactory.CreatePayments(),
                Analyze = true
            };

            try
            {
                konduto.Analyze(order);
                Assert.IsTrue(order.Recommendation != KondutoRecommendation.none);
            }
            catch (KondutoException ex)
            {
                Assert.Fail("Konduto exception shouldn't happen here.");
            }
        }

        [TestMethod]
        public void PostNonCreditIntegrationTest()
        {
            Konduto konduto = new Konduto("T738D516F09CAB3A2C1EE");

            KondutoCustomer Customer = new KondutoCustomer
            {
                Id = "28372",
                Name = "KdtUser",
                Email = "developer@example.com"
            };

            KondutoOrder order = new KondutoOrder
            {
                Id = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString(),
                Visitor = "38a9412f0b01b4dd1762ae424169a3e490d75c7a",
                TotalAmount = 100.00,
                Customer = Customer,
                Payments = KondutoPaymentFactory.CreateNonCreditPayments(),
                Analyze = true
            };

            try
            {
                konduto.Analyze(order);
                Assert.IsTrue(order.Recommendation != KondutoRecommendation.none);
            }
            catch (KondutoException ex)
            {
                Assert.Fail("Konduto exception shouldn't happen here.");
            }
        }

        [TestMethod]
        public void GetIntegrationTest()
        {
            Konduto konduto = new Konduto("T738D516F09CAB3A2C1EE");

            try
            {
                KondutoOrder order = konduto.GetOrder("1429744771");
            }
            catch (KondutoException ex)
            {
                Assert.Fail("Konduto exception shouldn't happen here.");
            }
        }

        [TestMethod]
        public void PutIntegrationTest()
        {
            //String id = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
            String id = "1429744771";
            
            Konduto konduto = new Konduto("T738D516F09CAB3A2C1EE");
            
            KondutoCustomer Customer = new KondutoCustomer
            {
                Id = "28372",
                Name = "KdtUser",
                Email = "developer@example.com"
            };

            KondutoOrder order = new KondutoOrder
            {
                Id = id,
                Visitor = "38a9412f0b01b4dd1762ae424169a3e490d75c7a",
                TotalAmount = 100.00,
                Customer = Customer,
                Analyze = true
            };

            try
            {
                konduto.Analyze(order);
                Assert.IsTrue(order.Recommendation != KondutoRecommendation.none);
            }
            catch (KondutoException ex)
            {
                Assert.Fail("Konduto exception shouldn't happen here.");
            }

            try
            {
                konduto.UpdateOrderStatus(id, KondutoOrderStatus.fraud, "Manual Review");
            }
            catch (KondutoException ex)
            {
                Assert.Fail("Konduto exception shouldn't happen here.");
            }

            try
            {
                konduto.UpdateOrderStatus(id, KondutoOrderStatus.not_authorized, "Manual Review");
            }
            catch (KondutoException ex)
            {
                Assert.Fail("Konduto exception shouldn't happen here.");
            }
            catch (Exception e)
            {
                Assert.Fail("Exception: " + e.ToString());
            }

            try
            {
                KondutoOrder updatedOrder = konduto.GetOrder(id);
            }
            catch (KondutoException ex)
            {
                Assert.Fail("Konduto exception shouldn't happen here.");
            }
        }

        [TestMethod]
        public void SendOrderToKondutoButDoNotAnalyzeTest()
        {
            var fakeResponseHandler = new FakeResponseHandler();
            var message = new HttpResponseMessage(HttpStatusCode.OK);
            message.Content = new StringContent(NOT_ANALYZE_ORDER_RESPONSE.ToString());

            fakeResponseHandler.AddFakeResponse(konduto.KondutoPostOrderUrl(), message);
            konduto.MessageHandler = fakeResponseHandler;

            KondutoOrder orderToSend = KondutoOrderFactory.basicOrder();
            orderToSend.Analyze = false;

            Assert.IsFalse(orderToSend.Analyze, "order analyze should be false");

            try
            {
                orderToSend = konduto.Analyze(orderToSend); // do analyze
            }
            catch (KondutoInvalidEntityException e)
            {
                Assert.Fail("order should be valid");
            }
            catch (KondutoHTTPException e)
            {
                Assert.Fail("server should respond with status 200");
            }
            catch (KondutoUnexpectedAPIResponseException e)
            {
                Assert.Fail("server should respond with status 200");
            }

            Assert.IsTrue(orderToSend.Score == null);
            Assert.IsTrue(orderToSend.Recommendation == KondutoRecommendation.none);

        }

        [TestMethod]
        public void AnalyzeInvalidOrderTest()
        {
            var fakeResponseHandler = new FakeResponseHandler();
            var message = new HttpResponseMessage(HttpStatusCode.OK);
            message.Content = new StringContent(ORDER_FROM_FILE.ToJson());

            fakeResponseHandler.AddFakeResponse(konduto.KondutoPostOrderUrl(), message);
            konduto.MessageHandler = fakeResponseHandler;

            KondutoOrder orderToSend = new KondutoOrder();

            try
            {
                orderToSend = konduto.Analyze(orderToSend); // do analyze
                Assert.Fail("KondutoInvalidEntityException should have been thrown");
            }
            catch (KondutoInvalidEntityException e)
            {
                //ok
            }
            catch (KondutoHTTPException e)
            {
                Assert.Fail("Expected KondutoInvalidEntityException, but got KondutoHTTPException");
            }
            catch (KondutoUnexpectedAPIResponseException e)
            {
                Assert.Fail("Expected KondutoInvalidEntityException, but got KondutoHTTPException");
            }
        }

        [TestMethod]
        public void AnalyzeHTTPErrorTest()
        {
            foreach (int code in HTTP_STATUSES)
            {
                try
                {
                    var fakeResponseHandler = new FakeResponseHandler();

                    var message = new HttpResponseMessage((HttpStatusCode)code);
                    message.Content = new StringContent(ORDER_FROM_FILE.ToJson());

                    fakeResponseHandler.AddFakeResponse(konduto.KondutoPostOrderUrl(), message);
                    konduto.MessageHandler = fakeResponseHandler;

                    konduto.Analyze(KondutoOrderFactory.basicOrder());
                    Assert.Fail("Exception expected");
                }
                catch (KondutoHTTPException e)
                {
                    //Ok
                }
                catch (Exception e)
                {
                    Assert.Fail("KondutoHTTPException was expected");
                }
            }
        }

        [TestMethod]
        public void UpdateSuccessfullyTest()
        {
            var fakeResponseHandler = new FakeResponseHandler();
            var message = new HttpResponseMessage(HttpStatusCode.OK);
            message.Content = new StringContent("{\"old_status\":\"review\",\"new_status\":\"approved\"}");

            fakeResponseHandler.AddFakeResponse(konduto.KondutoPutOrderUrl(ORDER_ID), message);
            konduto.MessageHandler = fakeResponseHandler;

            try
            {
                konduto.UpdateOrderStatus(ORDER_ID, KondutoOrderStatus.approved, "no comments");
            }
            catch (KondutoHTTPException e)
            {
                Assert.Fail("order update should have succeeded");
            }
            catch (KondutoUnexpectedAPIResponseException e)
            {
                Assert.Fail("order update should have succeeded");
            }
        }

        [TestMethod]
        public void UpdateHTTPErrorTest()
        {
            foreach (int code in HTTP_STATUSES)
            {
                try
                {
                    var fakeResponseHandler = new FakeResponseHandler();

                    var message = new HttpResponseMessage((HttpStatusCode)code);
                    message.Content = new StringContent("{}");

                    fakeResponseHandler.AddFakeResponse(konduto.KondutoPutOrderUrl(ORDER_ID), message);
                    konduto.MessageHandler = fakeResponseHandler;

                    konduto.Analyze(KondutoOrderFactory.basicOrder());
                    Assert.Fail("Exception expected");
                }
                catch (KondutoHTTPException e)
                {
                    //Ok
                }
                catch (Exception e)
                {
                    Assert.Fail("KondutoHTTPException was expected");
                }
            }
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void InvalidStatusWhenUpdatingTest()
        {
            List<KondutoOrderStatus> forbiddenStatus =
                new List<KondutoOrderStatus>()
                {
                    KondutoOrderStatus.not_analyzed,
                    KondutoOrderStatus.pending
                };

            foreach (KondutoOrderStatus status in forbiddenStatus)
            {
                try
                {
                    konduto.UpdateOrderStatus(ORDER_FROM_FILE.Id, status, "");
                    Assert.Fail("expected KondutoInvalidOrderStatus exception");
                }
                catch (KondutoHTTPException e)
                {
                    Assert.Fail("expected KondutoInvalidOrderStatus exception");
                }
                catch (KondutoUnexpectedAPIResponseException e)
                {
                    Assert.Fail("expected KondutoInvalidOrderStatus exception");
                }
            }
        }

        [TestMethod, ExpectedException(typeof(NullReferenceException))]
        public void NullCommentsWhenUpdatingTest()
        {
            try
            {
                konduto.UpdateOrderStatus(ORDER_FROM_FILE.Id, KondutoOrderStatus.approved, null);
            }
            catch (KondutoHTTPException e)
            {
                Assert.Fail("expected NullPointerException");
            }
            catch (KondutoUnexpectedAPIResponseException e)
            {
                Assert.Fail("expected NullPointerException");
            }
        }

        [TestMethod, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void InvalidApiKeyTest()
        {
            konduto.SetApiKey("invalid key");
        }

        private class FakeResponseHandler : DelegatingHandler
        {
            private readonly Dictionary<Uri, HttpResponseMessage> _FakeResponses = new Dictionary<Uri, HttpResponseMessage>();

            public void AddFakeResponse(Uri uri, HttpResponseMessage responseMessage)
            {
                _FakeResponses.Add(uri, responseMessage);
            }

            protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
            {
                var v = request.Headers.Authorization.Parameter;

                Assert.IsTrue(AUTH_HEADER == request.Headers.Authorization.Parameter, "Failing authorizing request.");

                if (_FakeResponses.ContainsKey(request.RequestUri))
                {
                    return _FakeResponses[request.RequestUri];
                }
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        RequestMessage = request
                    };
                }
            }
        }

    }
}
