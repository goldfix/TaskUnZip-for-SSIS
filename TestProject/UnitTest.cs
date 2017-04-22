using System;
using System.Data;
using Microsoft.SqlServer.Dts.Runtime;

public class ScriptMain
{

    public void Main()
    {
        int customerCount;
        int maxRecordCount;

        if (Microsoft.SqlServer.Dts.Variables.Contains("CustomerCount") == true && Dts.Variables.Contains("MaxRecordCount") == true)

        {
            customerCount = (int)Dts.Variables["CustomerCount"].Value;
            maxRecordCount = (int)Dts.Variables["MaxRecordCount"].Value;

        }

        if (customerCount > maxRecordCount)
        {
            Dts.TaskResult = (int)ScriptResults.Failure;
        }
        else
        {
            Dts.TaskResult = (int)ScriptResults.Success;
        }

    }

}