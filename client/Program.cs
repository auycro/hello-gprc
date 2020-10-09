//using Hello;
using Grpc.Core;
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Greet;
using UserCode;
using Google.Protobuf;

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
            
            var barcode_service = new UserCode.BarcodeService.BarcodeServiceClient(channel);
            var request_stream = barcode_service.UploadFiles();

            int MaxChunkSize = 256;
            byte[] chunk = new byte[MaxChunkSize];

            string filename = @"./tmp/lorem.txt";
            int i=0;
            using (FileStream fsIn = File.OpenRead(filename))
            {
                //read in ~1 MB chunks
                int bufferLen = 128; //1048576;
                byte[] buffer = new byte[bufferLen];
                while (fsIn.Read(buffer, 0, bufferLen) > 1)
                {
                    var chunks = new Chunks(){ Content = ByteString.CopyFrom(buffer) };
                    
                    i++;
                    string tmp = System.Text.Encoding.UTF8.GetString(buffer);
                    Console.WriteLine($"${i}:{tmp}");
                    
                    await request_stream.RequestStream.WriteAsync(chunks);
                    Array.Clear(buffer, 0, buffer.Length);
                    //await Task.Delay(1000);
                }
                await request_stream.RequestStream.CompleteAsync();
            }            
            Console.WriteLine("End ClientStreaming");
        }
    }
}
