@echo off
echo Building and running dungeon-sim...
echo.

dotnet build
if %ERRORLEVEL% NEQ 0 (
    echo Build failed!
    pause
    exit /b 1
)

echo.
echo Build successful! Running dungeon-sim...
echo.

dotnet run

echo.
echo Application completed. Press any key to exit...
pause >nul 