@echo OFF
msbuild /p:Configuration=Release /t:Rebuild
ECHO "WiFi.exe is in WifiProfiles/bin/Release/"
