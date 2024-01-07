@ECHO OFF

REM The following directory is for .NET 4.0
set DOTNETFX2=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319
set PATH=%PATH%;%DOTNETFX2%

echo Installing EHS_Weebhook_Service.exe Win Service.
echo -------------------------------
InstallUtil "D:\EHS_Weebhook_Service\EHS_Weebhook_Service.exe"
echo -------------------------------
pause
echo Done.