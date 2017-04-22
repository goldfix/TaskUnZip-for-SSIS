net stop "MsDtsServer110"
gacutil.exe /u "ICSharpCode.SharpZipLib"
gacutil.exe /u ProgettoMultimedia.TaskUnZip_2012

del "%ProgramFiles%\Microsoft SQL Server\110\DTS\Tasks\ProgettoMultimedia.TaskUnZip_2012.*"
del "%ProgramFiles%\Microsoft SQL Server\110\DTS\Tasks\ICSharpCode.SharpZipLib.dll"

del "%ProgramFiles% (x86)\Microsoft SQL Server\110\DTS\Tasks\ProgettoMultimedia.TaskUnZip_2012.*"
del "%ProgramFiles% (x86)\Microsoft SQL Server\110\DTS\Tasks\ICSharpCode.SharpZipLib.dll"

net start "MsDtsServer110"
pause