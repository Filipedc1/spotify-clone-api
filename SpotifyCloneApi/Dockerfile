#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["SpotifyCloneApi/SpotifyCloneApi.csproj", "SpotifyCloneApi/"]
RUN dotnet restore "SpotifyCloneApi/SpotifyCloneApi.csproj"
COPY . .
WORKDIR "/src/SpotifyCloneApi"
RUN dotnet build "SpotifyCloneApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SpotifyCloneApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SpotifyCloneApi.dll"]