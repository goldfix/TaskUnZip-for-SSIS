Simple Task for SSIS (SQL Server Integration Services) to manage, compress and decompress file ZIP/TAR/GZ. 

Features:

* Support to ZIP, TAR and TAR / GZ archive.
* Compress single file or entire folders and / or sub folders.
* Decompress all or singles files.
* Encrypt with password zip file.
* Insert Comment to zip file.
* Verify archive file before extraction and after compression.
* Support file ZIP >= 4GB.
* Configure 'Type Store Paths'
* Support to variables
* Support SQL Server 2014 (ver. 1.5.*)
* Support SQL Server 2012 (ver. 1.4.*)
* Support SQL Server 2008 / R2 (x86 and x64) (ver. 1.3.0.1)
* Support SQL Server 2005 (ver. 1.2.0.0)

Small donation is appreciated 

Ver. 1.5.5.0 (for SSIS 2014): 
It's possible to use variables for these attributes:

Source Folder
Destination Folder
File Name
Filter parameter
Rebuild with Visual Studio 2015

Ver. 1.5.2.0 (for SSIS 2014):

Small fix. Now is reported files, that do not match regex filter. Thanks to: Nico_FR75 (https://taskunzip.codeplex.com/workitem/22127)
Other small corrections (labels and messages).
Add Sql Server 2014 support (x86 and x64).
Add setup file (yep!).
Rebuild with Visual Studio 2013
Remove SQL Server 2012 support (use ver. 1.4.6.5).
Ver. 1.4.10.0 (for SSIS 2012) 
It's possible to use variables for these attributes:

Source Folder
Destination Folder
File Name
Filter parameter
Rebuild with Visual Studio 2015

Ver. 1.4.7.0 (for SSIS 2012)

Small fix. Now is reported files, that do not match regex filter. Thanks to: Nico_FR75 (https://taskunzip.codeplex.com/workitem/22127)
Other small corrections (labels and messages).

Ver. 1.4.6.5 (for SSIS 2012):

thanks to Wim: fixed decompression problem, with big file zip with option "Run64Runtime=FALSE" (https://taskunzip.codeplex.com/workitem/21614)
increased speed compression and decompression
decreased memory use with compression, decompression and verify operations
update recommended

Ver. 1.4.6.0

add support for TAR and TAR / GZ
add parameter "Add root folder"
add parameter "Type Store Paths":
add Sql Server 2012 compatibility
No_Paths
Relative_Paths
Absolute_Paths
review: parameters and descriptions
add more information in 'log result'
embedded (and removed) ICSharpCode.SharpZipLib.dll library
other minor fix

Ver. 1.4.0.1

Minor fix batch file.
Rebuild with Visual Studio 2012

Ver. 1.4.0.0

add: SQL SERVER 2012 support.
add option overwrite destination file zip
other minor fix
update batch file
Remove Support version for SQL Server 2008 / R2 (use ver. 1.3.0.1).

Ver. 1.3.0.1

Add: SQL SERVER 2008 and SQL SERVER 2008 R2 support.
Add: installation batch file for x86 e x64 (tnx JohannesHoppe).
Upgrade sample with foreach loop task.
Upgrade SharpZipLib for .NET Framework ver. 0.86.
Upgrade solution to Visual Studio 2010.
Remove SQL Server 2005 support (use ver. 1.2.0.0).

Ver. 1.2.0.0

Add: SQL SERVER 2008 / 2005 support.
Add: recursive compress.*
Add: filter option for extract and compress file.*
Add: Test archive.
Add: Comment archive.
Bug: Correct installation bug.
Minor corrections.
Support creation file > 4GB.
*(Tnx to: Kevin Wendler)


Thanks to:

http://icsharpcode.github.io/SharpZipLib/
Arthur Lindow
Kevin Wendler
JohannesHoppe
joniwho
Ben
Nico_FR75
cois
wsobolewski

pietro partescano
http://donotexists.blogspot.it/
