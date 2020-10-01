//using Hello;
using Grpc.Core;
using System;
using System.Threading.Tasks;
using Greet;

namespace client
{
    class Program
    {
        const string target = "127.0.0.1:5001";
        static void Main(string[] args)
        {
            Channel channel = new Channel("localhost", 5001, ChannelCredentials.Insecure);

            channel.ConnectAsync().ContinueWith((task) =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                    Console.WriteLine("The client connected successfully");
            });

            //var client = new HelloService.HelloServiceClient(channel);
            var client = new Greeter.GreeterClient(channel);
            
            var request = new GreetRequest() {
                Greeting = new Greeting(){
                    FirstName = "Foo",
                    LastName = "Bar"
                }
            };
            var response = client.SayHello(request);
            Console.WriteLine(response.Message);

            channel.ShutdownAsync().Wait();
            Console.ReadKey();
        }
    }
}
