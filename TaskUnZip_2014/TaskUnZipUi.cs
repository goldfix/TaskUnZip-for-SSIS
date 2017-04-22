/*
Copyright (c) pietro partescano

Permission is hereby granted, free of charge, to any person obtaining a copy 
of this software and associated documentation files (the "Software"), to deal 
in the Software without restriction, including without limitation the rights 
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
copies of the Software, and to permit persons to whom the Software is furnished 
to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all 
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS 
IN THE SOFTWARE. 
*/

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.SqlServer.Dts.Runtime.Design;
using System.Windows.Forms;

namespace ProgettoMultimedia.TaskUnZip_2014
{
    public class TaskUnZipUi : Microsoft.SqlServer.Dts.Runtime.Design.IDtsTaskUI
    {
        TaskHost taskHost;
        Connections connections;

        #region IDtsTaskUI Members

        public void Delete(System.Windows.Forms.IWin32Window parentWindow)
        {

            return;
        }

        public ContainerControl GetView()
        {
            return new InfoBox();
        }

        public void Initialize(TaskHost taskHost, IServiceProvider serviceProvider)
        {
            //throw new NotImplementedException();
            //System.Windows.Forms.MessageBox.Show("Initialize");

            this.taskHost = taskHost;
            IDtsConnectionService cs = serviceProvider.GetService(typeof(IDtsConnectionService)) as IDtsConnectionService;
            this.connections = cs.GetConnections();

            return;
        }

        public void New(System.Windows.Forms.IWin32Window parentWindow)
        {
            return;
        }

        #endregion
    }
}
