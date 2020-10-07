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
        static async Task Main(string[] args)
        {
            Channel channel = new Channel("localhost", 5001, ChannelCredentials.Insecure);

            channel.ConnectAsync().ContinueWith((task) =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                    Console.WriteLine("The client connected successfully");
            });

            //var client = new HelloService.HelloServiceClient(channel);
                        
            //Unary
            //CallUnary(channel);

            //Server streaming
            //await CallServerStreaming(channel);

            //Client streaming
            await CallClientStreaming(channel);
            
            channel.ShutdownAsync().Wait();
            Console.ReadKey();
        }

        public static void CallUnary(Channel channel){
            Console.WriteLine("Start Unary");
            var client_greeter = new Greeter.GreeterClient(channel);
            var request = new GreetRequest() {
                Greeting = new Greeting(){
                    FirstName = "Foo",
                    LastName = "Bar"
                }
            };
            var response = client_greeter.SayHello(request);
            Console.WriteLine(response.Message);
            Console.WriteLine("End Unary");
        }

        public static async Task CallServerStreaming(Channel channel){
            Console.WriteLine("Start ServerStreaming");
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
            Console.WriteLine("End ServerStreaming");
        }

        public static async Task CallClientStreaming(Channel channel){
            Console.WriteLine("Start ClientStreaming");
            
            //var request = new () { Greeting = greeting };
            var barcode_service = new UserCode.BarcodeService.BarcodeServiceClient(channel);
            var request_stream = barcode_service.UploadFiles();

            for (var i = 0; i < 3; i++)
            {
                //await request_stream.RequestStream.WriteAsync(new Chunks { Content =  });
            }
            await request_stream.RequestStream.CompleteAsync();
            //await request_stream.RequestStream();

            Console.WriteLine("End ClientStreaming");
        }
    }
}
