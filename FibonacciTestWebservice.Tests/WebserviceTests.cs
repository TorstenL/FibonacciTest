using System;
using System.Net;
using System.Net.Http;
using System.Text;
using NUnit.Framework;

namespace FibonacciTestWebservice.Tests
{
    [TestFixture]
    public class WebserviceTests
    {
        private readonly string requestBaseQuery = "/api/v1/fib";

        [Test]
        public void ErrorEmptyBody()
        {
            using (var httpClient = new HttpClient {BaseAddress = new Uri(Startup.Baseaddress)})
            {
                var result = httpClient.GetAsync(requestBaseQuery).Result;
                Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            }
        }

        [Test]
        public void ErrorWrongContentType()
        {
            using (var httpClient = new HttpClient {BaseAddress = new Uri(Startup.Baseaddress)})
            {
                var request = new HttpRequestMessage(HttpMethod.Get, requestBaseQuery);
                request.Content = new StringContent("5", Encoding.UTF8, "application/xml");
                var result = httpClient.SendAsync(request).Result;
                Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            }
        }
        
        [Test]
        public void RightResult()
        {
            using (var httpClient = new HttpClient {BaseAddress = new Uri(Startup.Baseaddress)})
            {
                var request = new HttpRequestMessage(HttpMethod.Get, requestBaseQuery);
                request.Content = new StringContent("61", Encoding.UTF8, "text/plain");
                var result = httpClient.SendAsync(request).Result;
                var body = result.Content.ReadAsStringAsync().Result;
                Assert.AreEqual(result.StatusCode,HttpStatusCode.OK);
                Assert.AreEqual(body,"Fibonacci_61: 2504730781961");
            }
        }
    }
}