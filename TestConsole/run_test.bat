@echo off
echo Building and running Dungeon Simulator Console Test...
echo.

dotnet build
if %ERRORLEVEL% NEQ 0 (
    echo Build failed!
    pause
    exit /b 1
)

echo.
echo Build successful! Running test...
echo.

dotnet run

echo.
echo Test completed. Press any key to exit...
pause >nul 