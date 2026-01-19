# =========================
# BUILD
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiamos el csproj y restauramos
COPY ["BootcampCLT.csproj", "./"]
RUN dotnet restore "BootcampCLT.csproj"

# Copiamos el resto del código
COPY . .
RUN dotnet build "BootcampCLT.csproj" -c Release -o /app/build

# =========================
# PUBLISH
# =========================
FROM build AS publish
RUN dotnet publish "BootcampCLT.csproj" -c Release -o /app/publish /p:UseAppHost=false

# =========================
# RUNTIME
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

EXPOSE 8080

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BootcampCLT.dll"]
