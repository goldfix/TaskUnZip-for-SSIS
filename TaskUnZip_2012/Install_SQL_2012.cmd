net stop "MsDtsServer110"
gacutil.exe /u "ICSharpCode.SharpZipLib"
gacutil.exe /u ProgettoMultimedia.TaskUnZip_2012

del "%ProgramFiles%\Microsoft SQL Server\110\DTS\Tasks\ProgettoMultimedia.TaskUnZip_2012.*"
del "%ProgramFiles%\Microsoft SQL Server\110\DTS\Tasks\ICSharpCode.SharpZipLib.dll"

del "%ProgramFiles% (x86)\Microsoft SQL Server\110\DTS\Tasks\ProgettoMultimedia.TaskUnZip_2012.*"
del "%ProgramFiles% (x86)\Microsoft SQL Server\110\DTS\Tasks\ICSharpCode.SharpZipLib.dll"

copy "ProgettoMultimedia.TaskUnZip_2012.*" "%ProgramFiles%\Microsoft SQL Server\110\DTS\Tasks\" /Y
copy "ProgettoMultimedia.TaskUnZip_2012.*" "%ProgramFiles% (x86)\Microsoft SQL Server\110\DTS\Tasks\" /Y

gacutil.exe /f /i "%ProgramFiles%\Microsoft SQL Server\110\DTS\Tasks\ProgettoMultimedia.TaskUnZip_2012.dll"
net start "MsDtsServer110"
pause


