call "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\Tools\VsDevCmd.bat" 
nuget restore Kiva.sln
del /S /F /Q build\*
rmdir /s /q build
mkdir build

del /S /F /Q Kiva-MIDI\bin\x64\Release\*
rmdir /s /q Kiva-MIDI\bin\x64\Release
del /S /F /Q KivaShared\bin\x64\Release\*
rmdir /s /q KivaShared\bin\x64\Release
del /S /F /Q KivaInstaller\bin\x64\Release\*
rmdir /s /q KivaInstaller\bin\x64\Release

MSBuild.exe Kiva.sln /p:Configuration=Release /p:Platform=x64 /m

del /S Kiva-MIDI\bin\x64\Release\lib\*.xml 
del /S Kiva-MIDI\bin\x64\Release\lib\*.pdb

powershell -c Compress-Archive -Path Kiva-MIDI\bin\x64\Release\* -CompressionLevel Optimal -Force -DestinationPath build\KivaPortable.zip
if (-Not (Test-Path "C:\Users\Noobie\Documents\Kivix\build\KivaPortable")) { Expand-Archive "C:\Users\Noobie\Documents\Kivix\build\KivaPortable.zip" "C:\Users\Noobie\Documents\Kivix\build\KivaPortable" } else { Write-Host "Folder already exists. Extraction stopped." }

pause
