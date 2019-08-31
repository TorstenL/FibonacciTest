using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using WebApplication1.Controllers;

namespace FibonacciTestWebservice.OutputFormatter
{
    public class FibonacciOutputFormatter : TextOutputFormatter
    {
        public FibonacciOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/plain"));
            SupportedEncodings.Add(Encoding.UTF8);
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context,
            Encoding selectedEncoding)
        {
            var response = context.HttpContext.Response;
            if (context.ObjectType == typeof(BigInteger) && response.StatusCode == (int) HttpStatusCode.OK &&
                response.Headers.ContainsKey(FibonacciController.FibonacciRequestedTag))
            {
                await response.WriteAsync(
                    $"Fibonacci_{response.Headers[FibonacciController.FibonacciRequestedTag]}: {context.Object}");
                return;
            }

            await response.WriteAsync(context.Object.ToString());
        }
    }
}