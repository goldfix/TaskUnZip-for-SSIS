net stop "SQL Server Integration Services 10.0"
gacutil.exe /u "ICSharpCode.SharpZipLib"
gacutil.exe /u TaskUnZip
copy "ICSharpCode.SharpZipLib.dll" "%ProgramFiles%\Microsoft SQL Server\100\DTS\Tasks\" /Y
copy "TaskUnZip.*" "%ProgramFiles%\Microsoft SQL Server\100\DTS\Tasks\" /Y
gacutil.exe /i "%ProgramFiles%\Microsoft SQL Server\100\DTS\Tasks\ICSharpCode.SharpZipLib.dll"
gacutil.exe /i "%ProgramFiles%\Microsoft SQL Server\100\DTS\Tasks\TaskUnZip.dll"
net start "SQL Server Integration Services 10.0"
pause