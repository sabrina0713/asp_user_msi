#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1803 AS base
WORKDIR /app
EXPOSE 8566
EXPOSE 44342

FROM microsoft/dotnet:2.1-sdk-nanoserver-1803 AS build
WORKDIR /src
COPY ["WebApplication5/WebApplication5.csproj", "WebApplication5/"]
RUN dotnet restore "WebApplication5/WebApplication5.csproj"
COPY . .
WORKDIR "/src/WebApplication5"
RUN dotnet build "WebApplication5.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "WebApplication5.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WebApplication5.dll"]