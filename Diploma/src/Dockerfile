FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
#EXPOSE 80
#EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["./Diploma.Instance/Diploma.Instance.csproj", "Diploma.Instance/"]
COPY ["./Diploma.DTO/Diploma.DTO.csproj", "Diploma.DTO/"]
COPY ["./Diploma.Domain/Diploma.Domain.csproj", "Diploma.Domain/"]
COPY ["./Diploma.Logic/Diploma.Logic.csproj", "Diploma.Logic/"]
RUN dotnet restore "Diploma.Instance/Diploma.Instance.csproj"
COPY . .
WORKDIR "/src/Diploma.Instance"
RUN dotnet build "Diploma.Instance.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Diploma.Instance.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Diploma.Instance.dll"]
