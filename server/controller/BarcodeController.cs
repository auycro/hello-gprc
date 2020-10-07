using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Grpc.Core;
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
            string filename = $"{Path.GetRandomFileName()}{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}.txt";
            FileStream fs = File.Create(filename);
            byte[] fileByte = null;
            while (await requestStream.MoveNext())
            {
                //fileByte = requestStream.Current.Content.ToByteArray();
                fs.WriteAsync(requestStream.Current.Content.ToByteArray());                
            }
            result.StatusCode = Status.StatusCode.Ok;
            result.Message = "Save Complete";
            Console.WriteLine($"{filename} has been saved.");
            return result;
        }
    }
}