FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Alterar a variável de ambiente para a nova porta
ENV ASPNETCORE_URLS=http://+:5202
EXPOSE 5202

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src .  

RUN dotnet restore "MotoRental.Messaging/MotoRental.Messaging.csproj"
WORKDIR "/src/MotoRental.Messaging"
RUN dotnet build "MotoRental.Messaging.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MotoRental.Messaging.csproj" -c Release -o /app/publish

FROM base AS final
RUN addgroup --system appuser && adduser --system appuser --ingroup appuser
WORKDIR /app
COPY --from=publish /app/publish .
USER appuser
ENTRYPOINT ["dotnet", "MotoRental.Messaging.dll"]
