# CarnetSante - Script de publication Windows
# Execute: .\publish.ps1

Write-Host "===========================================`n   CarnetSante - Publication Windows x64`n===========================================" -ForegroundColor Green

$OutputDir = ".\publish"

Write-Host "`n[1] Nettoyage..." -ForegroundColor Cyan
if (Test-Path $OutputDir) { Remove-Item $OutputDir -Recurse -Force }

Write-Host "[2] Compilation + Publication..." -ForegroundColor Cyan
dotnet publish .\src\CarnetSante.WPF\CarnetSante.WPF.csproj `
    -c Release `
    -r win-x64 `
    --self-contained true `
    -p:PublishSingleFile=true `
    -p:IncludeNativeLibrariesForSelfExtract=true `
    -p:EnableCompressionInSingleFile=true `
    -o $OutputDir

if ($LASTEXITCODE -ne 0) {
    Write-Host "`n[ERREUR] Publication echouee." -ForegroundColor Red
    exit 1
}

Write-Host "`n[OK] Publication reussie !" -ForegroundColor Green
Write-Host "Executable : $OutputDir\CarnetSante.exe" -ForegroundColor Yellow
Write-Host "Taille : $(Get-Item "$OutputDir\CarnetSante.exe" | Select-Object -ExpandProperty Length | ForEach-Object {[math]::Round($_ / 1MB, 1)}) MB"
