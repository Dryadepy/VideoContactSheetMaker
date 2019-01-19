CD /D VideoContactSheetMaker
dotnet publish -c Release -v q --self-contained -r win-x64 -f netcoreapp3.0 -o "..\Releases\VCSM.Windows-x64"
dotnet publish -c Release -v q --self-contained -r linux-x64 -f netcoreapp3.0 -o "..\Releases\VCSM.Linux-x64"
CD /D ..

@echo off
REM Copy ffmpeg windows binaries
if not exist ".\Releases\ffmpeg\" goto end
if not exist "Releases\VCSM.Windows-x64\bin" mkdir ".\Releases\VCSM.Windows-x64\bin"
xcopy /q /y ".\Releases\ffmpeg\*.*" ".\Releases\VCSM.Windows-x64\bin"

:end