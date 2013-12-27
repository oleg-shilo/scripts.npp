echo off 

del CSScriptNpp*.msi
del CSScriptNpp*.zip
del CSScriptNpp*.7z

cd Samples
"C:\Program Files\7-Zip\7z.exe" a -x!package.txt ..\Plugins\NppScripts\samples.zip *

cd ..
"C:\Program Files\7-Zip\7z.exe" a -x!package.txt NppScripts.zip *.txt Plugins
"C:\Program Files\7-Zip\7z.exe" a -t7z NppScripts.7z *.txt Plugins

cscs /l setup
cscs /l package

REM copy NppScripts*.7z ..\..\..\..\..\Dropbox\Public\CS-S_NPP
copy NppScripts*.zip ..\..\..\..\..\Dropbox\Public\CS-S_NPP
copy NppScripts*.msi ..\..\..\..\..\Dropbox\Public\CS-S_NPP

pause