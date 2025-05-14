# 使用官方 ASP.NET 執行階段映像
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# 使用 SDK 映像進行建構
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 複製 .csproj 並還原相依套件
COPY ["DemoWebApi/DemoWebApi.csproj", "DemoWebApi/"]
RUN dotnet nuget locals all --clear \
    && dotnet restore "DemoWebApi/DemoWebApi.csproj"

# 複製整個專案原始碼並編譯
COPY . .
WORKDIR /src/DemoWebApi
RUN dotnet build "DemoWebApi.csproj" -c Release -o /app/build

# 發佈成精簡可執行內容
FROM build AS publish
RUN dotnet publish "DemoWebApi.csproj" -c Release -o /app/publish

# 使用 ASP.NET 執行環境執行發佈結果
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DemoWebApi.dll"]
