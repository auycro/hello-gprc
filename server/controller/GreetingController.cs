using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Greet;
//using Microsoft.Extensions.Logging;

namespace server.controller
{
    public class GreetingController: Greeter.GreeterBase
    {
        //private readonly ILogger<GreeterService> _logger;
        //public GreeterService(ILogger<GreeterService> logger)
        //{
        //    _logger = logger;
        //}
        public override Task<GreetReply> SayHello(GreetRequest request, ServerCallContext context)
        {
            string result = $"Hello, {request.Greeting.FirstName} {request.Greeting.LastName}!";

            return Task.FromResult(new GreetReply
            {
                Message = result
            });
        }
    }
}