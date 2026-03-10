FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# kopiujemy solution
COPY api/LeadForge.sln .

# kopiujemy projekt
COPY api/LeadForge/ LeadForge/

RUN dotnet restore LeadForge.sln

RUN dotnet publish LeadForge/LeadForge.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "LeadForge.dll"]