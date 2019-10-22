echo off

cd ..\src\NppScripts\NppScripts\samples

"C:\Program Files\7-Zip\7z.exe" a samples.zip *
move samples.zip ..\samples.zip

cd ..\..\..\..\bin

rd /s /q NppScripts.x86
rd /s /q NppScripts.x64

md NppScripts.x64\NppScripts\NppScripts
md NppScripts.x86\NppScripts\NppScripts

copy ..\src\NppScripts\bin\Release\CSScriptLibrary.dll     NppScripts.x64\NppScripts\NppScripts\CSScriptLibrary.dll
copy ..\src\NppScripts\bin\Release\CSScriptLibrary.xml     NppScripts.x64\NppScripts\NppScripts\CSScriptLibrary.xml
copy ..\src\NppScripts\bin\Release\NppScripts.asm.dll      NppScripts.x64\NppScripts\NppScripts\NppScripts.asm.dll
copy ..\src\NppScripts\NppScripts\samples.zip              NppScripts.x64\NppScripts\NppScripts\samples.zip
copy ..\src\NppPlugin.Host\bin\x64\Release\NppScripts.dll  NppScripts.x64\NppScripts\NppScripts.dll
copy ..\src\NppPlugin.Host\bin\x64\Release\NppScripts.xml  NppScripts.x64\NppScripts\NppScripts.xml
copy readme.txt                                            NppScripts.x64\readme.txt
copy license.txt                                           NppScripts.x64\license.txt

copy ..\src\NppScripts\bin\Release\CSScriptLibrary.dll     NppScripts.x86\NppScripts\NppScripts\CSScriptLibrary.dll
copy ..\src\NppScripts\bin\Release\CSScriptLibrary.xml     NppScripts.x86\NppScripts\NppScripts\CSScriptLibrary.xml
copy ..\src\NppScripts\bin\Release\NppScripts.asm.dll      NppScripts.x86\NppScripts\NppScripts\NppScripts.asm.dll
copy ..\src\NppScripts\NppScripts\samples.zip              NppScripts.x86\NppScripts\NppScripts\samples.zip
copy ..\src\NppPlugin.Host\bin\x86\Release\NppScripts.dll  NppScripts.x86\NppScripts\NppScripts.dll
copy ..\src\NppPlugin.Host\bin\x86\Release\NppScripts.xml  NppScripts.x86\NppScripts\NppScripts.xml
copy readme.txt                                            NppScripts.x86\readme.txt
copy license.txt                                           NppScripts.x86\license.txt

cd NppScripts.x86
"C:\Program Files\7-Zip\7z.exe" a -t7z NppScripts.x86.zip *
cd ..

cd NppScripts.x64
"C:\Program Files\7-Zip\7z.exe" a -t7z NppScripts.x64.zip *

pause
