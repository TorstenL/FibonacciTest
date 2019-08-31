using System;
using System.IO;
using System.Numerics;
using FibonacciTestWebservice;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/v1/fib")]
    [ApiController]
    [Produces("text/plain")]
    public class FibonacciController : ControllerBase
    {
        public const string FibonacciRequestedTag = "FibonacciRequested";

        [HttpGet]
        public ActionResult<BigInteger> Get()
        {
            using (var reader = new StreamReader(Request.Body))
            {
                var body = reader.ReadToEnd();
                switch (Request.ContentType)
                {
                    case "text/plain; charset=utf-8":
                    case "text/plain":
                        if (long.TryParse(body, out var index))
                            try
                            {
                                Response.Headers.Add(FibonacciRequestedTag, index.ToString());
                                return FibonacciCalculator.Calculate(index);
                            }
                            catch (Exception e)
                            {
                                return BadRequest(e.Message);
                            }

                        return BadRequest($"Invalid Index Value: ''{body}''");
                    default:
                        return BadRequest("Only Contenttype text/plain accepted");
                }
            }
        }

        [HttpGet("{index}")]
        public ActionResult<BigInteger> Get(long index)
        {
            try
            {
                Response.Headers.Add(FibonacciRequestedTag, index.ToString());
                return FibonacciCalculator.Calculate(index);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}