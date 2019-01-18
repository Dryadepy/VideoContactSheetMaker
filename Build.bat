CD /D VideoContactSheetMaker
dotnet publish -c Release -v q --self-contained -r win-x64 -f netcoreapp3.0 -o "..\Releases\VCSM.Windows-x64"
dotnet publish -c Release -v q --self-contained -r win-x86 -f netcoreapp3.0 -o "..\Releases\VCSM.Windows-x86"
dotnet publish -c Release -v q --self-contained -r linux-x64 -f netcoreapp3.0 -o "..\Releases\VCSM.Linux-x64"
CD /D ..