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
using System.Data;
using Microsoft.SqlServer.Dts.Runtime;
using System.ComponentModel;

namespace TaskUnZip
{
    [DtsTask(
        DisplayName = "TaskUnZip",
        Description = "Compress/DeCompress file Zip for SSIS",
        TaskContact = "TaskUnZip: http://www.codeplex.com/TaskUnZip/",
        IconResource = "TaskUnZip.zip_file.ico",
        UITypeName = "TaskUnZip.TaskUnZipUi, TaskUnZip, Version=1.3.0.1, Culture=neutral, PublicKeyToken=08d5a3e5c5e98dbc"
    )]
    public class TaskUnZip : Task
    {
        public enum Type_Operation
        {
            Compress = 1,
            Decompress = 2,
            Please_Select = 0
        }

        private string _folderSource = null;    //"c:\\tmp\\";
        private string _folderDest = null;      //"c:\\tmp\\";
        private string _fileZip = null;         //"test.zip";
        private Type_Operation _typeOperation = Type_Operation.Please_Select;
        private string _password = null;
        private bool _Recurse = true;
        private string _FileFilter = ".*";
        private string _comment = null;
        private bool _testarchive = false;

        /// <summary>
        /// Password with crypt/decrypt file zip
        /// </summary>
        [CategoryAttribute("UnZip SSIS Parameters")]
        [Description("Password encrypted file zip")]
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        /// <summary>
        /// Folder Source
        /// </summary>
        [CategoryAttribute("UnZip SSIS Parameters")]
        [Description("Source folder path.")]
        public string FolderSource
        {
            get { return _folderSource; }
            set { _folderSource = value.Trim(); }
        }

        /// <summary>
        /// Foder destination
        /// </summary>
        [CategoryAttribute("UnZip SSIS Parameters")]
        [Description("Destination folder path.")]
        public string FolderDestination
        {
            get { return _folderDest; }
            set { _folderDest = value.Trim(); }
        }

        /// <summary>
        /// File zip to proccess
        /// </summary>
        [CategoryAttribute("UnZip SSIS Parameters")]
        [Description("File name zip file to process or use.")]
        public string FileZip
        {
            get { return _fileZip; }
            set { _fileZip = value.Trim(); }
        }

        /// <summary>
        /// Compress or Decompress 
        /// </summary>
        [CategoryAttribute("UnZip SSIS Parameters")]
        [Description("Select type operation.")]
        public Type_Operation TypeOperation
        {
            get { return _typeOperation; }
            set { _typeOperation = value; }
        }

        /// <summary>
        /// Proccess sub folder.
        /// </summary>
        [CategoryAttribute("UnZip SSIS Parameters")]
        [Description("Process all file in subfolders.")]
        public bool RecurseSubFolder
        {
            get { return _Recurse; }
            set { _Recurse = value; }
        }

        /// <summary>
        /// Expression to filter file.
        /// </summary>
        [CategoryAttribute("UnZip SSIS Parameters")]
        [Description("This is a regex filter file. '.*' process all file")]
        public string RegExFileFilter
        {
            get { return _FileFilter; }
            set { _FileFilter = value.Trim(); }
        }

        /// <summary>
        /// Comment to insert zip file
        /// </summary>
        [CategoryAttribute("UnZip SSIS Parameters")]
        [Description("Comment to insert into zip file.")]
        public string Comment
        {
            get { return _comment; }
            set { _comment = value.Trim(); }
        }

        /// <summary>
        /// Test archive after compression
        /// </summary>
        [CategoryAttribute("UnZip SSIS Parameters")]
        [Description("If checked file zip created will be tested.")]
        public bool TestArchive
        {
            get { return _testarchive; }
            set { _testarchive = value; }
        }

        public TaskUnZip()
        { }

        public override DTSExecResult Validate(Connections connections, VariableDispenser variableDispenser, IDTSComponentEvents componentEvents, IDTSLogging log)
        {
            if (_typeOperation == Type_Operation.Please_Select) 
            {
                componentEvents.FireError(1, "UnZip SSIS", "Please select type operation (De/Compress).", null, 0);
                return DTSExecResult.Failure; 
            }

            if (_typeOperation == Type_Operation.Compress)
            {
                if ((string.IsNullOrEmpty(_fileZip)) || (string.IsNullOrEmpty(_folderSource)))
                {
                    componentEvents.FireError(1, "UnZip SSIS", "Please select Source Folder and Zip File.", null, 0);
                    return DTSExecResult.Failure; 
                }
            }
            else
            {
                if ((string.IsNullOrEmpty(_fileZip)) || (string.IsNullOrEmpty(_folderSource)) || (string.IsNullOrEmpty(_folderDest)))
                {
                    componentEvents.FireError(1, "UnZip SSIS", "Please select Source Folder, Zip File and Destination Folder.", null, 0);
                    return DTSExecResult.Failure; 
                }
            }
            return DTSExecResult.Success;
        }

        public override DTSExecResult Execute(Connections connections, VariableDispenser variableDispenser, IDTSComponentEvents componentEvents, IDTSLogging log, object transaction)
        {
            bool b = false;

            componentEvents.FireInformation(1, "UnZip SSIS", "Start 1", null, 0, ref b);

            componentEvents.FireInformation(1, "UnZip SSIS", "Start process file: '" + _folderDest + _fileZip + "'.", null, 0, ref b);

            if (_typeOperation == Type_Operation.Please_Select)
            {
                componentEvents.FireError(1, "UnZip SSIS", "Please select type operation (De/Compress).", null, 0);
                throw new Exception("Please select type operation (De/Compress).");
            }

            ICSharpCode.SharpZipLib.Zip.FastZip fz = new ICSharpCode.SharpZipLib.Zip.FastZip();

            if (!_folderSource.EndsWith("\\"))
            {
                _folderSource = _folderSource + "\\";
            }
            if (!_folderDest.EndsWith("\\"))
            {
                _folderDest = _folderDest + "\\";
            }

            if (!string.IsNullOrEmpty(_password))
            {
                fz.Password = _password;
            }

            try
            {
                if (!System.IO.Directory.Exists(_folderDest))
                {
                    componentEvents.FireInformation(1, "UnZip SSIS", "Create Folder: '" + _folderDest + "'.", null, 0, ref b);
                    System.IO.Directory.CreateDirectory(_folderDest);
                }

                if (_typeOperation == Type_Operation.Compress)
                {
                    fz.CreateZip(_folderDest + _fileZip, _folderSource, _Recurse, _FileFilter);

                    if ((_testarchive) || (!string.IsNullOrEmpty(_comment)))
                    {
                        ICSharpCode.SharpZipLib.Zip.ZipFile zipFile = new ICSharpCode.SharpZipLib.Zip.ZipFile(_folderDest + _fileZip);

                        if (_testarchive)
                        {
                            componentEvents.FireInformation(1, "UnZip SSIS", "Start verify file ZIP (" + _fileZip + ")", null, 0, ref b);
                            if (!zipFile.TestArchive(true))
                            {
                                throw new Exception("Verify file zip: " + _fileZip + " failed.");
                            }
                        }

                        if (!string.IsNullOrEmpty(_comment))
                        {
                            componentEvents.FireInformation(1, "UnZip SSIS", "Set Comment.", null, 0, ref b);
                            zipFile.BeginUpdate();
                            zipFile.SetComment(_comment);
                            zipFile.CommitUpdate();
                        }
                    }
                }
                else
                {
                    if (_testarchive)
                    {
                        ICSharpCode.SharpZipLib.Zip.ZipFile zipFile = new ICSharpCode.SharpZipLib.Zip.ZipFile(_folderSource + _fileZip);

                        componentEvents.FireInformation(1, "UnZip SSIS", "Start verify file ZIP (" + _fileZip + ")", null, 0, ref b);

                        if (!zipFile.TestArchive(true))
                        {
                            throw new Exception("Verify file zip: " + _fileZip + " failed.");
                        }
                    }

                    fz.ExtractZip(_folderSource + _fileZip, _folderDest, _FileFilter);
                }
            }
            catch (Exception ex)
            {
                
                throw;
            }

            componentEvents.FireInformation(1, "UnZip SSIS", "End process file: '" + _folderDest + _fileZip + "'.", null, 0, ref b);

            return DTSExecResult.Success;
        }
    }
}

