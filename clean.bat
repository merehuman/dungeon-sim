@echo off
REM Clean up build artifacts for .NET projects

REM Remove bin and obj from TestConsole
if exist TestConsole\bin (
    echo Removing TestConsole\bin ...
    rmdir /s /q TestConsole\bin
)
if exist TestConsole\obj (
    echo Removing TestConsole\obj ...
    rmdir /s /q TestConsole\obj
)

echo Cleanup complete! 