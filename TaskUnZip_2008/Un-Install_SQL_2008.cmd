net stop "SQL Server Integration Services 10.0"
gacutil.exe /u "ICSharpCode.SharpZipLib"
gacutil.exe /u TaskUnZip
del "%ProgramFiles%\Microsoft SQL Server\100\DTS\Tasks\TaskUnZip.*"
del "%ProgramFiles%\Microsoft SQL Server\100\DTS\Tasks\ICSharpCode.SharpZipLib.dll"
net start "SQL Server Integration Services 10.0"
pause