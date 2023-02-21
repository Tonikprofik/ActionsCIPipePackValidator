#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["PackageValidator.Web/PackageValidator.Web.csproj", "PackageValidator.Web/"]
COPY ["PackageValidator.Core/PackageValidator.Core.csproj", "PackageValidator.Core/"]
RUN dotnet restore "PackageValidator.Web/PackageValidator.Web.csproj"
COPY . .
WORKDIR "/src/PackageValidator.Web"
RUN dotnet build "PackageValidator.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PackageValidator.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PackageValidator.Web.dll"]