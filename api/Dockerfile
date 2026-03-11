# -------- BUILD STAGE --------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY api/LeadForge.sln .
COPY api/LeadForge/*.csproj LeadForge/

RUN dotnet restore LeadForge.sln

COPY api/LeadForge/ LeadForge/

WORKDIR /src/LeadForge

RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false


# -------- RUNTIME STAGE --------
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY --from=build /app/publish .

# Render port
ENV ASPNETCORE_URLS=http://0.0.0.0:10000

EXPOSE 10000

ENTRYPOINT ["dotnet", "LeadForge.dll"]