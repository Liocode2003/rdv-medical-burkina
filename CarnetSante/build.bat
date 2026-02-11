@echo off
echo ============================================
echo    CarnetSante - Build Release
echo    Ministere de la Sante - Burkina Faso
echo ============================================
echo.

dotnet restore CarnetSante.sln
if %errorlevel% neq 0 ( echo [ERREUR] Restore failed & pause & exit /b 1 )

dotnet build CarnetSante.sln -c Release
if %errorlevel% neq 0 ( echo [ERREUR] Build failed & pause & exit /b 1 )

echo.
echo ============================================
echo    Build REUSSI !
echo ============================================
echo.
echo Pour publier un executable Windows x64 :
echo   dotnet publish src\CarnetSante.WPF\CarnetSante.WPF.csproj ^
echo     -c Release -r win-x64 --self-contained true ^
echo     -p:PublishSingleFile=true -o publish\
echo.
pause
