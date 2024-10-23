# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY . .
RUN dotnet publish -c Release -o /out WebApi/WebApi.csproj


# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

RUN useradd -m nonrootuser

COPY --from=build /out ./

RUN chown -R nonrootuser /app

USER nonrootuser

ENTRYPOINT ["dotnet", "WebApi.dll"]