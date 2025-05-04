Write-Host "`n🛠️  Starting FantasyDatabaseManager Development Stack..." -ForegroundColor Cyan

function Test-Port {
    param ([int]$Port)
    return (Test-NetConnection -ComputerName "localhost" -Port $Port -InformationLevel Quiet)
}

function Start-ServiceIfMissing {
    param (
        [string]$Name,
        [int]$Port,
        [string]$Command,
        [string]$WorkingDir = ""
    )

    if (Test-Port $Port) {
        Write-Host "✅ $Name already running on port $Port" -ForegroundColor Green
    } else {
        Write-Host "🚀 Starting $Name on port $Port..." -ForegroundColor Yellow

        $startCmd = if ($WorkingDir -ne "") {
            "Push-Location '$WorkingDir'; $Command; Pop-Location"
        } else {
            $Command
        }

        Start-Process powershell -ArgumentList "-NoExit", "-ExecutionPolicy Bypass", "-Command", $startCmd
    }
}

# 📦 Backend ASP.NET Core API
Start-ServiceIfMissing -Name "Backend API (.NET)" -Port 5000 `
    -Command "dotnet run --project 'F:\FantasyDatabase\FantasyDatabaseManager\FantasyDB.API' --urls='http://localhost:5000'" `
    -WorkingDir "F:\FantasyDatabase\FantasyDatabaseManager\FantasyDB.API"

# 🌐 Vite Frontend
Start-ServiceIfMissing -Name "Vite Frontend" -Port 56507 `
    -Command "npm run dev" `
    -WorkingDir "F:\FantasyDatabase\FantasyDatabaseManager\creatorapp"

# 🧠 Assistant Python API
Start-ServiceIfMissing -Name "FantasyAssistant API (FastAPI)" -Port 8000 `
    -Command "python assistant_server.py --mode=server" `
    -WorkingDir "F:\FantasyDatabase\FantasyDatabaseManager\FantasyAssistant"

# 🧩 Ollama LLM Server
Start-ServiceIfMissing -Name "Ollama LLM Server" -Port 11434 `
    -Command "'F:\Development\Ollama\ollama.exe' serve" `
    -WorkingDir "F:\Development\Ollama"

Write-Host "`n✅ All services launched or confirmed running." -ForegroundColor Cyan
Write-Host "`nPress any key to exit this status window..." -ForegroundColor DarkGray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
