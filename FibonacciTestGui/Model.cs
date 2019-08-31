using System;
using System.ComponentModel;
using System.Net.Http;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using FibonacciTestGui.Annotations;

namespace FibonacciTestGui
{
    public class FibonacciModel : INotifyPropertyChanged
    {
        public static string Baseaddress = "http://localhost:1337";
        public static string WebServicePath = "/api/v1/fib";

        private static readonly string BodyResultSuffix = "Fibonacci_{0}: ";
        private readonly HttpClient _httpClient;
        private BigInteger _currentFibonacciResult = BigInteger.Zero;

        public FibonacciModel()
        {
            //https://github.com/dotnet/corefx/issues/28135
            var handler = new WinHttpHandler();
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(Baseaddress);
        }

        public BigInteger CurrentFibonacciResult
        {
            get => _currentFibonacciResult;
            set
            {
                if (value == _currentFibonacciResult) return;
                _currentFibonacciResult = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public async void QueryFibonacciResult(long index)
        {
            try
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(Baseaddress + WebServicePath),
                    Content = new StringContent(index.ToString(), Encoding.UTF8, "text/plain")
                };
                var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                var body = response.Content.ReadAsStringAsync().Result;
                ParseBodyAndUpdateResult(index, body);
            }
            catch (Exception)
            {
                CurrentFibonacciResult = BigInteger.Zero;
            }
        }

        private void ParseBodyAndUpdateResult(long index, string body)
        {
            if (body.StartsWith(string.Format(BodyResultSuffix, index)))
                if (BigInteger.TryParse(body.Substring(string.Format(BodyResultSuffix, index).Length),
                    out var tempresult))
                    CurrentFibonacciResult = tempresult;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}