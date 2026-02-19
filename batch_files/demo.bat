@echo off
setlocal
cd /d "%~dp0.."

echo ========================================
echo   Dungeon Sim - Clean, Build, Run
echo ========================================
echo.

echo [1/3] Cleaning...
dotnet clean --nologo -v q
if errorlevel 1 (
    echo Clean failed.
    exit /b 1
)
echo Clean complete.
echo.

echo [2/3] Building...
dotnet build --nologo -v minimal
if errorlevel 1 (
    echo Build failed.
    exit /b 1
)
echo Build complete.
echo.

echo [3/3] Running application...
echo ----------------------------------------
dotnet run --no-build
set RUN_EXIT=%errorlevel%
echo ----------------------------------------
echo.
if %RUN_EXIT% neq 0 (
    echo Application exited with error code %RUN_EXIT%.
    echo Check the output above for exception or missing-file messages.
) else (
    echo Done.
)
echo.
pause
endlocal
