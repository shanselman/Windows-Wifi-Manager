Windows-Wifi-Manager
====================

Windows 8 doesn't have a UI that lets you see the list of all the WiFi hotspots that you once attached to. That list can be seen with "netsh wlan show profiles" then calling "netsh wlan show profile SOMEAPNAME" on at a time, then reading the details.

It's not usually considered safe to have an auto-connect profile setup to an OPEN hotspot. That means that a bad guy could get you to connect to a hotspot by just making one with the same (or a common name.)

This is a little utility that shows you just the stuff you need to know about your Wifi profiles on Windows.

Also, if you call it with the option /deleteautoopen it will automatically delete any profiles that are BOTH open and set to connect automatically.

The code is garbage, but it made for a fun evening. Comments welcome.

I think I may add "wifi delete NAME" as well. That would be useful. Or you can. ;)

##Possibilities

I have a WifiProfile class that's basic, but it woudln't be hard to put a WinForms wrapper around this thing. It would be a fun challenge to DUPLICATE the Win7 dialog exactly.

##Example Run

````C:\>Wifi.exe
AP-guest                 manual    WPA2PSK
HANSELMAN-N                auto    WPA2PSK
HANSELMAN                  auto    WPA2PSK
HanselSpot                 auto    WPA2PSK
nalen dressingroom         auto    WPA2PSK
Qdoba Free Wifi          manual       open
Scott's iPhone 4s          auto    WPA2PSK
SFO-WiFi                 manual       open
Sunrise_Bagels           manual       open
Wayport_Access           manual       open

No WiFi profiles set to OPEN and AUTO connect were found.
Option: Run with /deleteautoopen to auto delete.
````


