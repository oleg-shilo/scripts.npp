echo off

set programfiles=%PROGRAMFILES(X86)%

md "..\..\bin\Plugins\NppScripts"
md "..\..\bin\Samples"
md "%programfiles%\Notepad++\plugins\NppScripts"

copy "bin\Release\NppScripts.dll" "%programfiles%\Notepad++\plugins\NppScripts.dll"
copy "bin\Release\NppScripts.xml" "%programfiles%\Notepad++\plugins\NppScripts.xml"
copy "bin\Release\NppScripts\*.dll" "%programfiles%\Notepad++\plugins\NppScripts"
copy  "..\HtmlView\bin\Release\*.exe" "%userprofile%\Documents\NppScripts" 
copy  "..\HtmlView\bin\Release\*.xml" "%userprofile%\Documents\NppScripts" 

echo 2...
copy "bin\release\NppScripts.dll" "..\..\bin\Plugins\NppScripts.dll"
copy "bin\release\NppScripts.xml" "..\..\bin\Plugins\NppScripts.xml"
copy "bin\release\NppScripts\*.dll" "..\..\bin\Plugins\NppScripts"
copy "bin\release\NppScripts\*.exe" "..\..\bin\Plugins\NppScripts"
copy "bin\release\NppScripts\*.xml" "..\..\bin\Plugins\NppScripts"

copy "%userprofile%\Documents\NppScripts\*.cs" "..\..\bin\Samples"
copy "%userprofile%\Documents\NppScripts\HtmlView.exe" "..\..\bin\Samples"
copy "%userprofile%\Documents\NppScripts\HtmlView.xml" "..\..\bin\Samples"

copy "..\..\readme.txt" "..\..\bin\readme.txt"
copy "..\..\license.txt" "..\..\bin\license.txt"

pause