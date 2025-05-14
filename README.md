




建立 xUnit 測試專案 & 加入對主專案的參考
```bash
dotnet new xunit -n DemoWebApi.Tests
dotnet add DemoWebApi.Tests/ reference DemoWebApi/
```

跑測試
```bash
dotnet test
# 測試輸出詳細資訊
dotnet test -v n
```

覆蓋率
```bash
dotnet add package coverlet.collector
dotnet test --collect:"XPlat Code Coverage"

# 轉可讀報表
dotnet tool install --global dotnet-reportgenerator-globaltool
reportgenerator -reports:"TestResults/**/coverage.cobertura.xml" -targetdir:"coverage-report" -reporttypes:Html
```


透過 IDE測試，需要建立 `.sln 解決方案檔`
```bash
dotnet new sln -n DemoSolution
dotnet sln DemoSolution.sln add DemoWebApi/DemoWebApi.csproj
dotnet sln DemoSolution.sln add DemoWebApi.Tests/DemoWebApi.Tests.csproj
```

根目錄，可以直接指定 sln
```bash
dotnet test DemoSolution.sln
```

清除本地資源 & 重抓
```bash
dotnet nuget locals all --clear
dotnet restore
```