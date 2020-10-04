### Structure

hello-gprc
  - protos : proto files (this folder will move to another repository and references as git-submodule)
  - client : test-client
  - server : test-server
  - tmpGPRC : dotnet gPRC template (for reference purpose)

### Command Memo

```
$ dotnet new grpc
$ dotnet add package Grpc.AspNetCore 
```

Program.cs
```
//set krestel to disable TLS
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

```
$ dotnet new proto -n barcode
```

### Reference Repo

istio dotnet sample: https://github.com/cushind/aspnetcore-istio
kubernetes dotnet sample: https://github.com/jtattermusch/grpc-authentication-kubernetes-examples
dotnet sample: https://github.com/Clement-Jean/grpc-csharp-course
kubernetes golang sample: https://github.com/kelseyhightower/grpc-hello-service

gRPC gateway:  https://github.com/grpc-ecosystem/grpc-gateway