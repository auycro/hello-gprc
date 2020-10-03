using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            foreach (int i in Enumerable.Range(1, 10))
            {
                await responseStream.WriteAsync(new Barcode() { 
                    User = "190299282",
                    ServiceType = "7eleven",
                    BarcodeNum = "109293283723" 
                });
            }
        } 
        
    }
}