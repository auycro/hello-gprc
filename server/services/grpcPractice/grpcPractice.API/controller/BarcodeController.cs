using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Grpc.Core;
using Google.Protobuf;
using UserCode;

namespace server.controller
{
    public class BarcodeController: BarcodeService.BarcodeServiceBase
    {
        
        public override Task<Barcode> GetBarcode(Filter request, ServerCallContext context)
        {
            string query = $"{request.Query}";
            Console.WriteLine($"The server received the request : {query}");

            return Task.FromResult(new Barcode
            {
                User = "190299282",
                ServiceType = "7eleven",
                BarcodeNum = "109293283723"
            });
        }

        public override async Task GetBarcodes(Filter request, IServerStreamWriter<Barcode> responseStream, ServerCallContext context)
        {
            string query = $"{request.Query}";
            Console.WriteLine($"The server received the request : {query}");

            Random generator = new Random();

            foreach (int i in Enumerable.Range(1, 10))
            {
                String random_barcode = generator.Next(0, 99999).ToString("D5");

                var result = new Barcode(){
                    User = "190299282",
                    ServiceType = "7eleven",
                    BarcodeNum = random_barcode 
                };

                await responseStream.WriteAsync(result);
            }
        }

        public override async Task<Result> UploadFiles(Grpc.Core.IAsyncStreamReader<Chunks> requestStream, Grpc.Core.ServerCallContext context)
        {
            Result result = new Result(){
                StatusCode = Status.StatusCode.Unknown,
                Message = "Unknown"
            };
            //Path SERVER_BASE_PATH = Paths.get("uploads/");
            string filename = Path.Combine(@"./tmp/",$"{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}-{Path.GetRandomFileName()}");
            byte[] resultByte = null;
            while (await requestStream.MoveNext())
            {
                byte[] fileByte = requestStream.Current.Content.ToByteArray();
                fileByte = server.utilities.ByteArrayExtensions.Trim(fileByte);
                //string tmp = System.Text.Encoding.UTF8.GetString(fileByte);
                //Console.WriteLine($"{tmp}");
                //Console.WriteLine($"context:{context.ToString()}");
                using (var stream = new FileStream(filename, FileMode.Append))
                {
                    stream.Write(fileByte, 0, fileByte.Length);
                }
            }

            result.StatusCode = Status.StatusCode.Ok;
            result.Message = "Save Complete";
            Console.WriteLine($"{filename} has been saved.");
            return result;
        }

        public override async Task Chat(Grpc.Core.IAsyncStreamReader<ChatMessage> requestStream, 
                                  Grpc.Core.IServerStreamWriter<Google.Protobuf.WellKnownTypes.StringValue> responseStream, 
                                  Grpc.Core.ServerCallContext context)
        {
            var result = new Google.Protobuf.WellKnownTypes.StringValue();
            string[] random_word = {"Hello", "G'day", "Please!", "Okay", "Perfect!", "NVM"};
            Random generator = new Random();
            
            while (await requestStream.MoveNext())
            {
                string response_text = $"{requestStream.Current.Name}, {random_word[generator.Next(0, 5)]}";
                Console.WriteLine($"response_text: {response_text}");
                result.Value = response_text;
                await responseStream.WriteAsync(result);
                //await Task.Delay(2000);
            }
        }
    }
}