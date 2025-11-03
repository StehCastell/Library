@echo off
echo Iniciando LibraryAPI...
start cmd /k "cd src/LibraryAPI && dotnet run"

timeout /t 5

echo Iniciando LibraryWeb...
start cmd /k "cd src/LibraryWeb && dotnet run"
