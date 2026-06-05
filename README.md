# Hstar Canvas

Fourth-generation Windows 11 AIGC infinite canvas workstation.

## Direction

- Native shell: WinUI 3 / Windows App SDK / WebView2.
- Canvas UI: React, TypeScript, React Flow.
- Runtime: .NET 8 services for workflow validation, provider adapters, task execution, persistence, and bridge contracts.
- UI language: all frontend interaction text is Simplified Chinese.

## MVP Loop

```text
Prompt Node -> Image Generate Node -> Provider Adapter -> Task Queue -> Output Node -> Asset Library
```

## Layout

```text
src/HstarCanvas.Core        Domain models and validation
src/HstarCanvas.Execution   Runtime execution, task queue, provider adapters
src/HstarCanvas.Persistence SQLite and file-backed stores
src/HstarCanvas.Bridge      WebView2 bridge contracts and routing
src/HstarCanvas.Native      Windows paths and native integration helpers
src/HstarCanvas.Web         React/WebView2 canvas frontend
tests/                      Unit tests
installer/                  Windows installer scripts
docs/                       Product and architecture notes
```

## Commands

```powershell
dotnet build HstarCanvas.sln
dotnet test HstarCanvas.sln
npm.cmd --prefix src/HstarCanvas.Web run build
```

