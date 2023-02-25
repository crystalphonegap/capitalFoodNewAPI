@ECHO OFF

:: This CMD script provides you with your operating system, hardware and network information.

TITLE *****CREATED BY AHMED SHAIKH*****

ECHO Making Build please wait... 
del Publish 2>null
dotnet restore
dotnet build 
ECHO Publishing  please wait... 
dotnet publish -c Release -o Publish/Publish --self-contained false   
