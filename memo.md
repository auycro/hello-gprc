```
$ dotnet new grpc
$ dotnet add package Grpc.AspNetCore 
```

Program.cs
```
                    //set krestel to disble TLS
                    webBuilder.ConfigureKestrel(options =>
                    {
                        options.ListenLocalhost(5000, o => o.Protocols = HttpProtocols.Http2);
                    });
                    webBuilder.UseStartup<Startup>();
```

```
$ dotnet sln gRPC.sln add server/server.csproj
$ dotnet sln gRPC.sln add client/client.csproj
```

```
$ dotnet add package Grpc --version 2.32.0
$ dotnet add package Grpc.Tools --version 2.32.0
$ dotnet add package Google.Protobuf --version 3.13.0
```

server.csproj|client.csproj
```
  <ItemGroup>
    <!--Protobuf Include="Protos\greet.proto" GrpcServices="Server"  /-->
    <Protobuf Include="Protos\*.proto" OutputDir="%(RelativePath)models\"  />
  </ItemGroup>
```