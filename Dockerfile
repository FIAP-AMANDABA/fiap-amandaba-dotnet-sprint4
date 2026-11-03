FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Amandaba.API/Amandaba.API.csproj ./Amandaba.API/
RUN dotnet restore ./Amandaba.API/Amandaba.API.csproj

COPY Amandaba.API/. ./Amandaba.API/
WORKDIR /src/Amandaba.API
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "Amandaba.API.dll"]