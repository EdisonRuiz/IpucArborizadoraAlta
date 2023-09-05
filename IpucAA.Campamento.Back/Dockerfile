#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["IpucAA.Campamento.Back/IpucAA.Campamento.Back.csproj", "IpucAA.Campamento.Back/"]
RUN dotnet restore "IpucAA.Campamento.Back/IpucAA.Campamento.Back.csproj"
COPY . .
WORKDIR "/src/IpucAA.Campamento.Back"
RUN dotnet build "IpucAA.Campamento.Back.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IpucAA.Campamento.Back.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IpucAA.Campamento.Back.dll"]