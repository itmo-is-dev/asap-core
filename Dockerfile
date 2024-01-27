FROM mcr.microsoft.com/dotnet/sdk:7.0.203 AS build
WORKDIR /source
COPY ./src ./src
COPY ./*.sln .
COPY ./*.props ./
COPY ./.editorconfig .

RUN dotnet restore "src/Itmo.Dev.Asap.Core/Itmo.Dev.Asap.Core.csproj"

FROM build AS publish
WORKDIR "/source/src/Itmo.Dev.Asap.Core"
RUN dotnet publish "Itmo.Dev.Asap.Core.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0.5 AS final
EXPOSE 8012
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Itmo.Dev.Asap.Core.dll"]
