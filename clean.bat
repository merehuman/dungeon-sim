@echo off
REM Clean up build artifacts for .NET projects

REM Remove bin and obj from current directory
if exist bin (
    echo Removing bin ...
    rmdir /s /q bin
)
if exist obj (
    echo Removing obj ...
    rmdir /s /q obj
)

echo Cleanup complete! 