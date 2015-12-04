echo off

set programfiles=%PROGRAMFILES(X86)%

md "%programfiles%\Notepad++\plugins\NppScripts"

copy "bin\Debug\NppScripts.dll" "%programfiles%\Notepad++\plugins\NppScripts.dll"
copy "bin\Debug\NppScripts.xml" "%programfiles%\Notepad++\plugins\NppScripts.xml"
copy "bin\Debug\NppScripts\*.dll" "%programfiles%\Notepad++\plugins\NppScripts"

copy  "..\HtmlView\bin\Debug\*.exe" "%userprofile%\Documents\NppScripts" 
copy  "..\HtmlView\bin\Debug\*.xml" "%userprofile%\Documents\NppScripts" 

pause