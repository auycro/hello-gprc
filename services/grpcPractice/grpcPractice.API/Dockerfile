FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app
EXPOSE 5001

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish --no-restore -c Release -o out /app

#FROM build as test
#WORKDIR /{unittest project}

# Build runtime image
FROM build AS publish
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "server.dll"]