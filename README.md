# .NET 8 WebAPI CI/CD Demo

使用 ASP.NET Core 8 建立 Web API，實現完整的 CI/CD 流程，包含：

- ASP.NET Core 8
- xUnit 單元測試
- Coverlet 覆蓋率統計
- Compose 容器化
- CI/CD：
    - GitHub Actions：執行單元測試、覆蓋率分析、SonarCloud 程式碼品質檢查，自動部署至 Linode 並發送 Slack 通知
    - GitLab CI（內網）：執行單元測試、產出覆蓋率報告、整合內網 SonarQube 掃描並發送 Slack 通知
---

## 建立與加入測試專案

建立 xUnit 測試專案並加入對主專案的參考：

```bash
dotnet new xunit -n DemoWebApi.Tests
dotnet add DemoWebApi.Tests/ reference DemoWebApi/
```

---

## 執行測試


```bash
dotnet test
# 顯示更詳細的測試輸出
dotnet test -v n
```

---

## 覆蓋率統計（Coverlet + ReportGenerator）

安裝 Coverlet 並產生測試覆蓋率報告，最後轉成 HTML 報表：

```bash
# 安裝 Coverlet
dotnet add package coverlet.collector

# 執行測試並產生覆蓋率報告
dotnet test --collect:"XPlat Code Coverage"

# 根目錄
dotnet test --collect:"XPlat Code Coverage" --results-directory "TestResults"

# 安裝報表工具
dotnet tool install --global dotnet-reportgenerator-globaltool

# 產出 HTML 報表
reportgenerator -reports:"TestResults/**/coverage.cobertura.xml" -targetdir:"coverage-report" -reporttypes:Html
```

---

## VSCode 測試整合（需 .sln）

建立解決方案並加入主專案與測試專案，方便在 IDE 統一管理與測試：

```bash
# 建立解決方案
dotnet new sln -n DemoSolution

# 加入主專案與測試專案
dotnet sln DemoSolution.sln add DemoWebApi/DemoWebApi.csproj
dotnet sln DemoSolution.sln add DemoWebApi.Tests/DemoWebApi.Tests.csproj

# 透過 .sln 統一測試
dotnet test DemoSolution.sln
```

---

## Docker 容器化錯誤處理

常見錯誤訊息：

```
Package Microsoft.AspNetCore.OpenApi, version 8.0.6 was not found.
```

解法：清除本地 nuget 快取並重新還原套件

```bash
dotnet nuget locals all --clear
dotnet restore
```

---

## GitHub Secrets 說明（CI/CD 部署）

| Secret 名稱| 說明|
| --------------------- | ------------------------------------------------- |
| LINODE\_HOST          | Linode 主機 IP|
| LINODE\_USERNAME      | Linode 登入使用者|
| LINODE\_SSHKEY        | SSH 私鑰內容|
| LINODE\_PROJECT\_PATH | 專案於 Linode 中部署的資料夾|
| SLACK\_WEBHOOK\_URL   | Slack Webhook|
| SONAR\_TOKEN          | 從 SonarCloud 專案取得的 Token|
| SONAR\_PROJECT\_KEY   | SonarCloud 中的專案金鑰|
| SONAR\_ORG            | SonarCloud 中的組織 ID|

---

## Slack 通知功能

GitHub Actions 會根據以下狀況主動推播通知到 Slack：

* CI/CD 部署成功
* 測試或建置失敗
* SonarCloud 程式碼品質門檻未通過

通知內容包含：

* 專案名稱與分支
* Commit ID（附連結）
* 推送者資訊
* 狀態顏色（綠：成功、紅：失敗）

---

## 程式碼格式檢查（Code Style / Lint）

檢查或自動修正程式碼風格：

```bash
# 僅檢查，不修改
dotnet format DemoSolution.sln --verify-no-changes --severity warn

# 修正縮排、空白、括號等風格問題
dotnet format DemoSolution.sln --severity warn
```

---

## 程式碼安全與品質分析（SonarScanner）

### 前置

* 前往 [https://sonarcloud.io](https://sonarcloud.io) 註冊並登入
* 授權 GitHub，匯入專案
* 關閉 Automatic Analysis（否則與 CI/CD 衝突）
* 在 GitHub Secrets 中設定 SONAR_TOKEN、SONAR_PROJECT_KEY、SONAR_ORG

### 分析流程

```bash
dotnet tool install --global dotnet-sonarscanner
export PATH="$PATH:$HOME/.dotnet/tools"

dotnet sonarscanner begin \
  /k:"專案 KEY" \
  /o:"組織 ID" \
  /d:sonar.token="你的 Token" \
  /d:sonar.host.url="https://sonarcloud.io"

dotnet build DemoSolution.sln

dotnet sonarscanner end /d:sonar.token="你的 Token"
```

分析後至 [SonarCloud Dashboard](https://sonarcloud.io/dashboard) 查看。

