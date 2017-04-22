Ver. 1.4.
---------

 - Step 1: Open your Command line

Manual Installation:

 - De-Compress file zip in your folder (eg: c:\temp\).
 - Open Console (not use Visual Studio or SQL Server Console) "**Run as Administrator**".
 - Select correct folder (eg: cd c:\temp\).
 - Execute ..\Install_SQL_2012.cmd

Manual Un-Installation:

 - De-Compress file zip in your folder (eg: c:\temp\).
 - Open Console (not use Visual Studio or SQL Server Console) "**Run as Administrator**".
 - Select correct folder (eg: cd c:\temp\).
 - Execute ..\Un-Install_SQL_2012.cmd 

----

Ver. 1.3.0.1
---------

Manual Installation:

- Stop** SQL Server Integration Services (HOW TO: Control Panel>Administrative Tools>Services>right-click SQL Server Integration Services>Stop)
- Execute ..\Install_SQL_2008.cmd or ..\Install_SQL_2008_x86.cmd (**Run as Administrator**)
- Open Visual Studio and **Add** in "Control Flow Items" TaskUnZip (select _TaskUnZip_ from list)
- **Start** SQL Server Integration Services (HOW TO: Control Panel>Administrative Tools>Services>right-click SQL Server Integration Services>Start)

Manual Un-Installation:

- Open Visual Studio and **Remove** from "Control Flow Items" TaskUnZip (de-select _TaskUnZip_ from list)
- **Stop** SQL Server Integration Services (HOW TO: Control Panel>Administrative Tools>Services>right-click SQL Server Integration Services>Stop)
- Execute ..\Un-Install_SQL_2008.cmd or ..\Un-Install_SQL_2008_x86.cmd (**Run as Administrator**)
- **Start** SQL Server Integration Services (HOW TO: Control Panel>Administrative Tools>Services>right-click SQL Server Integration Services>Start)

----

Ver. 1.2.x.x
---------

Manual Installation:
- **Stop** SQL Server Integration Services (HOW TO: Control Panel>Administrative Tools>Services>right-click SQL Server Integration Services>Stop)
- Execute ..\Install_SQL_2008.cmd or ..\Install_SQL_2005.cmd
- Open Visual Studio and **Add** in "Control Flow Items" TaskUnZip (select _TaskUnZip_ from list)
- **Start** SQL Server Integration Services (HOW TO: Control Panel>Administrative Tools>Services>right-click SQL Server Integration Services>Start)

Manual Un-Installation:

- Open Visual Studio and **Remove** from "Control Flow Items" TaskUnZip (de-select _TaskUnZip_ from list)
- **Stop** SQL Server Integration Services (HOW TO: Control Panel>Administrative Tools>Services>right-click SQL Server Integration Services>Stop)
- Execute ..\Un-Install_SQL_2008.cmd or ..\Un-Install_SQL_2005.cmd
- **Start** SQL Server Integration Services (HOW TO: Control Panel>Administrative Tools>Services>right-click SQL Server Integration Services>Start)

----

**Ver. 1.0.0.2**

* This version support only zip format.
* This versione support only SQL SERVER 2005.
* For Manual Installation:

- **Stop** SQL Server Integration Services (HOW TO: Control Panel>Administrative Tools>Services>right-click SQL Server Integration Services>Stop)
- Execute ..\TaskUnZip\TaskUnZip\bin\Debug\Install.cmd
- Open Visual Studio and **Add** in "Control Flow Items" TaskUnZip (select _TaskUnZip_ from list)
- **Start** SQL Server Integration Services (HOW TO: Control Panel>Administrative Tools>Services>right-click SQL Server Integration Services>Start)

Manual Un-Installation:

- Open Visual Studio and **Remove** from "Control Flow Items" TaskUnZip (de-select _TaskUnZip_ from list)
- **Stop** SQL Server Integration Services (HOW TO: Control Panel>Administrative Tools>Services>right-click SQL Server Integration Services>Stop)
- Execute ..\TaskUnZip\TaskUnZip\bin\Debug\Un-Install.cmd
- **Start** SQL Server Integration Services (HOW TO: Control Panel>Administrative Tools>Services>right-click SQL Server Integration Services>Start)

MSI Installation:

- **Stop** SQL Server Integration Services (HOW TO: Control Panel>Administrative Tools>Services>right-click SQL Server Integration Services>Stop)
- Execute TaskUnZip_Setup.msi
- Open Visual Studio and **Add** in "Control Flow Items" TaskUnZip (select _TaskUnZip_ from list)
- **Start** SQL Server Integration Services (HOW TO: Control Panel>Administrative Tools>Services>right-click SQL Server Integration Services>Start)

MSI Un-Installation:

- Open Visual Studio and **Remove** in "Control Flow Items" TaskUnZip (de-select _TaskUnZip_ from list)
- **Stop** SQL Server Integration Services (HOW TO: Control Panel>Administrative Tools>Services>right-click SQL Server Integration Services>Stop)
- Uninstall TaskUnZip_Setup.msi
- **Start** SQL Server Integration Services (HOW TO: Control Panel>Administrative Tools>Services>right-click SQL Server Integration Services>Start)
