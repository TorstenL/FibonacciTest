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
        private BigInteger _currentFibonacciResult = 1;
        private readonly HttpClient _httpClient;
        public static string Baseaddress = "http://localhost:5000";
        public static string WebServicePath = "/api/v1/fib";

        public FibonacciModel()
        {
            //https://github.com/dotnet/corefx/issues/28135
            var handler = new WinHttpHandler();
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress =new Uri(Baseaddress);
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

        public async void QueryFibonacciResult(long index)
        {
            try
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(Baseaddress + WebServicePath),
                    Content = new StringContent(index.ToString(), Encoding.UTF8, "text/plain"),
                };
                var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                var body = response.Content.ReadAsStringAsync().Result;
                ParseBodyAndUpdateResult(index,body);
            }
            catch (Exception e)
            {
                CurrentFibonacciResult = BigInteger.Zero;
            }
        }

        private void ParseBodyAndUpdateResult(long index, string body)
        {
            if (body.StartsWith($"Fibonacci_{index}: "))
            {
                if (BigInteger.TryParse(body.Substring($"Fibonacci_{index}: ".Length),
                    out BigInteger tempresult))
                {
                    CurrentFibonacciResult = tempresult;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}