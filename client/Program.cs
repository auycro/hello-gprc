//using Hello;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Greet;
using UserCode;

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
                        
            //Unary
            CallUnary(channel);

            //Server streaming
            CallServerStreaming(channel);

            channel.ShutdownAsync().Wait();
            Console.ReadKey();
        }

        public static void CallUnary(Channel channel){
            Console.WriteLine("Start CallUnary");
            var client_greeter = new Greeter.GreeterClient(channel);
            var request = new GreetRequest() {
                Greeting = new Greeting(){
                    FirstName = "Foo",
                    LastName = "Bar"
                }
            };
            var response = client_greeter.SayHello(request);
            Console.WriteLine(response.Message);
            Console.WriteLine("End CallUnary");
        }

        public static async void CallServerStreaming(Channel channel){
            Console.WriteLine("Start CallServerStreaming");
            var filter = new UserCode.Filter()
            {
                Query = "{user_id:'10938432'}"
            };

            var barcode_service = new UserCode.BarcodeService.BarcodeServiceClient(channel);
            var response = barcode_service.GetBarcodes(filter);

            int i = 0;
            while (await response.ResponseStream.MoveNext())
            {
                i = i+1;
                Barcode barcode = response.ResponseStream.Current;
                Console.WriteLine($"{i}: {barcode.BarcodeNum}");
                await Task.Delay(200);
            }
            Console.WriteLine("End CallServerStreaming");
        }
    }
}
