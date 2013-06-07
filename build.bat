@echo OFF
msbuild /p:Configuration=Release /t:Rebuild
ECHO "WiFi.exe is in WifiProfiles/bin/Release/"
:: will package any .nuspec files in this folder
cpack 

