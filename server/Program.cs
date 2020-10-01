﻿using System;
using System.IO;
using Grpc.Core;
using server.services;

namespace server
{
    class Program
    {
        const int Port = 5001;
        static void Main(string[] args)
        {
            Server server = null;

            try
            {
                server = new Server()
                {
                    Services = {
                        Greet.Greeter.BindService(new GreetingService()),
                    },
                    Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
                };
                server.Start();
                Console.WriteLine("The server is listening on the port : " + Port);
                Console.ReadKey();
            }
            catch (IOException e)
            {
                Console.WriteLine("The server failed to start : " + e.Message);
                throw;
            }
            finally
            {
                if (server != null)
                    server.ShutdownAsync().Wait();
            }

        }
    }
}
