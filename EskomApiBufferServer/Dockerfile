
#FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
#WORKDIR /app
##EXPOSE 80
##EXPOSE 443
#
#FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
#WORKDIR /src
#COPY ["EskomApiBufferServer/EskomApiBufferServer.csproj", "EskomApiBufferServer/"]
#COPY ["EskomApiBufferService/EskomApiBufferService.csproj", "EskomApiBufferService/"]
#RUN dotnet restore "EskomApiBufferServer/EskomApiBufferServer.csproj"
#COPY . .
#WORKDIR "/src/EskomApiBufferServer"
#RUN dotnet build "EskomApiBufferServer.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "EskomApiBufferServer.csproj" -c Release -o /app/publish
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "EskomApiBufferServer.dll"]


# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/aspnet:3.1
COPY bin/Release/netcoreapp3.1/publish/ App/
WORKDIR /App
ENTRYPOINT ["dotnet", "EskomApiBufferServer.dll"]