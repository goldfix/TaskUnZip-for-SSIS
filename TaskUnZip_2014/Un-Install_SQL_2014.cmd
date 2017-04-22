net stop "MsDtsServer120"
gacutil.exe /u "ICSharpCode.SharpZipLib"
gacutil.exe /u ProgettoMultimedia.TaskUnZip_2014

del "%ProgramFiles%\Microsoft SQL Server\120\DTS\Tasks\ProgettoMultimedia.TaskUnZip_2014.*"
del "%ProgramFiles%\Microsoft SQL Server\120\DTS\Tasks\ICSharpCode.SharpZipLib.dll"

del "%ProgramFiles% (x86)\Microsoft SQL Server\120\DTS\Tasks\ProgettoMultimedia.TaskUnZip_2014.*"
del "%ProgramFiles% (x86)\Microsoft SQL Server\120\DTS\Tasks\ICSharpCode.SharpZipLib.dll"

net start "MsDtsServer120"
pause