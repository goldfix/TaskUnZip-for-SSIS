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

namespace ProgettoMultimedia.TaskUnZip_2012
{
    [DtsTask(
        DisplayName = "TaskUnZip 2012",
        Description = "Compress/DeCompress file Zip for SSIS",
        TaskContact = "TaskUnZip: https://taskunzip.codeplex.com/",
        IconResource = "ProgettoMultimedia.TaskUnZip_2012.zip_file.ico",
        UITypeName = "ProgettoMultimedia.TaskUnZip_2012.TaskUnZipUi, ProgettoMultimedia.TaskUnZip_2012, Version=1.4.6.0, Culture=neutral, PublicKeyToken=08d5a3e5c5e98dbc"
    )]
    public class TaskUnZip : Task
    {
        public enum Type_Compression
        {
            ZIP = 1,
            TAR = 2,
            TAR_GZ = 4,
            Please_Select = 0
        }

        public enum Type_Operation
        {
            Compress = 1,
            Decompress = 2,
            Please_Select = 0
        }

        public enum Store_Paths
        {
            Please_Select = 0,
            No_Paths = 1,
            Relative_Paths = 2,
            Absolute_Paths = 3            
        }

        private static readonly string DEFAULT_FILTER_FILE = ".*";
        private string _folderSource = null;    //"c:\\tmp\\";
        private string _folderDest = null;      //"c:\\tmp\\";
        private string _fileZip = null;         //"test.zip";
        private Type_Operation _typeOperation = Type_Operation.Please_Select;
        private string _password = null;
        private bool _recurse = true;
        private string _fileFilter = DEFAULT_FILTER_FILE;
        private string _comment = null;
        private bool _testarchive = false;
        private bool _overwriteZipFile = true;
        private Store_Paths _storePaths = Store_Paths.Relative_Paths;
        private Type_Compression _typeCompression = Type_Compression.Please_Select;
        private bool _addRootFolder = false;

        #region Properties

        /// <summary>
        /// Add root folder
        /// </summary>
        [CategoryAttribute("UnZip SSIS Parameters")]
        [Description("Add root folder. Only for 'Relative_Paths' selected. It's valid only for Compression.")]
        [System.ComponentModel.DisplayNameAttribute("Add root folder")]
        public bool AddRootFolder
        {
            get { return _addRootFolder; }
            set { _addRootFolder = value; }
        }

        /// <summary>
        /// Type compresion to use. Supported: ZIP, TAR or TAR/Gz
        /// </summary>
        [CategoryAttribute("UnZip SSIS Parameters")]
        [Description("Type compresion to use. Supported: ZIP, TAR or TAR/Gz")]
        [System.ComponentModel.DisplayNameAttribute("Type Compresion")]
        public Type_Compression TypeCompression
        {
            get { return _typeCompression; }
            set { _typeCompression = value; }
        }


        /// <summary>
        /// Password with crypt/decrypt file zip
        /// </summary>
        [CategoryAttribute("UnZip SSIS Parameters")]
        [Description("Password encrypted file zip")]
        [System.ComponentModel.DisplayNameAttribute("Password")]
        public string Password
        {
            get { return _password; }
            set { _password = value;}
        }

        /// <summary>
        /// Folder Source
        /// </summary>
        [CategoryAttribute("UnZip SSIS Parameters")]
        [Description("Source folder path. It's possible to use variable (eg.: @[User::var_source_folder]).")]
        [System.ComponentModel.DisplayNameAttribute("Source Folder")]
        public string FolderSource
        {
            get { return _folderSource; }
            set { _folderSource = value.Trim(); }
        }

        /// <summary>
        /// Foder destination
        /// </summary>
        [CategoryAttribute("UnZip SSIS Parameters")]
        [Description("Destination folder path. It's possible to use variable (eg.: @[User::var_dest_folder]).")]
        [System.ComponentModel.DisplayNameAttribute("Destination Folder")]
        public string FolderDestination
        {
            get { return _folderDest; }
            set { _folderDest = value.Trim(); }
        }

        /// <summary>
        /// Foder destination
        /// </summary>
        [CategoryAttribute("UnZip SSIS Parameters")]
        [Description("Overwrite destination file. It's valid only for Compression.")]
        [System.ComponentModel.DisplayNameAttribute("Overwrite File Archive")]
        public bool OverwriteZipFile
        {
            get { return _overwriteZipFile; }
            set { _overwriteZipFile = value; }
        }

        /// <summary>
        /// File zip to proccess
        /// </summary>
        [CategoryAttribute("UnZip SSIS Parameters")]
        [Description("Archive file name to process or use. It's possible to use variable (eg.: @[User::var_name_file]).")]
        [System.ComponentModel.DisplayNameAttribute("Archive File Name")]
        public string FileZip
        {
            get { return _fileZip; }
            set { _fileZip = value.Trim(); }
        }

        /// <summary>
        /// Type Store Paths
        /// </summary>
        [CategoryAttribute("UnZip SSIS Parameters")]
        [Description("Select type Store Paths.")]
        [System.ComponentModel.DisplayNameAttribute("Type Store Paths")]
        public Store_Paths StorePaths
        {
            get { return _storePaths; }
            set { _storePaths = value; }
        }

        /// <summary>
        /// Compress or Decompress 
        /// </summary>
        [CategoryAttribute("UnZip SSIS Parameters")]
        [Description("Select type operation.")]
        [System.ComponentModel.DisplayNameAttribute("Operation Type")]
        public Type_Operation TypeOperation
        {
            get { return _typeOperation; }
            set { _typeOperation = value; }
        }

        /// <summary>
        /// Proccess sub folder.
        /// </summary>
        [CategoryAttribute("UnZip SSIS Parameters")]
        [Description("Process all file in subfolders. It's valid only for Compression.")]
        [System.ComponentModel.DisplayNameAttribute("Recurse Sub-Folders")]
        public bool RecurseSubFolder
        {
            get { return _recurse; }
            set { _recurse = value; }
        }

        /// <summary>
        /// Expression to filter file.
        /// </summary>
        [CategoryAttribute("UnZip SSIS Parameters")]
        [Description("This is a regex filter file. '.*' process all file. Default is: '.*'. It's possible to use variable (eg.: @[User::var_filter]).")]
        [System.ComponentModel.DisplayNameAttribute("Filter File")]
        public string RegExFileFilter
        {
            get 
            {
                if (string.IsNullOrWhiteSpace(_fileFilter))
                {
                    return DEFAULT_FILTER_FILE;
                }
                else 
                {
                    return _fileFilter;
                }                
            }
            set { _fileFilter = value.Trim(); }
        }

        /// <summary>
        /// Comment to insert zip file
        /// </summary>
        [CategoryAttribute("UnZip SSIS Parameters")]
        [Description("Comment to insert into zip file. It's valid only for Compression.")]
        [System.ComponentModel.DisplayNameAttribute("Comment File Zip")]
        public string Comment
        {
            get { return _comment; }
            set { _comment = value.Trim(); }
        }

        /// <summary>
        /// Test archive after compression
        /// </summary>
        [CategoryAttribute("UnZip SSIS Parameters")]
        [Description("If checked, file zip created will be tested.")]
        [System.ComponentModel.DisplayNameAttribute("Verify Archive")]
        public bool TestArchive
        {
            get
            { return _testarchive; }
            set { _testarchive = value; }
        }

        #endregion

        public TaskUnZip()
        { }

        public override DTSExecResult Validate(Connections connections, VariableDispenser variableDispenser, IDTSComponentEvents componentEvents, IDTSLogging log)
        {
            if (_typeCompression == Type_Compression.Please_Select)
            {
                componentEvents.FireError(1, "UnZip SSIS", "Please select type compression to use.", null, 0);
                return DTSExecResult.Failure;
            }
            else
            {
                if (_typeCompression == Type_Compression.TAR || _typeCompression == Type_Compression.TAR_GZ)
                {
                    if (!string.IsNullOrWhiteSpace(_password) || !string.IsNullOrWhiteSpace(_comment))
                    {
                        componentEvents.FireWarning(1, "UnZip SSIS", "Archives TAR and TAR/GZ do not support option: 'Comment'.", null, 0);
                    }
                }

                if (_typeCompression == Type_Compression.TAR)
                {
                    if (_testarchive)
                    {
                        componentEvents.FireWarning(1, "UnZip SSIS", "Archives TAR do not support option: 'Verify Archive'.", null, 0);
                    }
                }
            }

            if (_typeOperation == Type_Operation.Please_Select) 
            {
                componentEvents.FireError(1, "UnZip SSIS", "Please select type operation (De/Compress).", null, 0);
                return DTSExecResult.Failure; 
            }

            if (_storePaths == Store_Paths.Please_Select)
            {
                componentEvents.FireError(1, "UnZip SSIS", "Please select type Store Paths (No_Paths / Relative_Paths / Absolute_Paths).", null, 0);
                return DTSExecResult.Failure;
            }

            if (_typeOperation == Type_Operation.Compress || _typeOperation == Type_Operation.Decompress)
            {
                if ((string.IsNullOrEmpty(_fileZip)) || (string.IsNullOrEmpty(_folderSource)) || (string.IsNullOrEmpty(_folderDest)))
                {
                    componentEvents.FireError(1, "UnZip SSIS", "Please select Source Folder, Zip File and Destination Folder.", null, 0);
                    return DTSExecResult.Failure;
                }
            }
            return DTSExecResult.Success;
        }

        private string _CheckVarName(string var_name, VariableDispenser variableDispenser)
        {
            try
            {
                if (!var_name.StartsWith("@["))
                {
                    return var_name;
                }
                else
                {
                    string tmp_var_name = var_name.Replace("@[", "").Replace("]", "");
                    if (variableDispenser.Contains(tmp_var_name))
                    {
                        Variables result_variables = null;
                        variableDispenser.LockForRead(tmp_var_name);
                        variableDispenser.GetVariables(ref result_variables);
                        if (result_variables.Contains(tmp_var_name))
                        {
                            return (string)result_variables[tmp_var_name].Value;
                        }
                        else
                        {
                            return var_name;
                        }
                    }
                    else
                    {
                        return var_name;
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message + " - " + ex.StackTrace;
            }
        }

        #region De/Compress Functions -- TAR / GZ --
        private void _Check_GZ(string pathFileTAR, IDTSComponentEvents componentEvents)
        {
            bool b = false;
            if (_testarchive)
            {
                using (ICSharpCode.SharpZipLib.GZip.GZipInputStream gzip = new ICSharpCode.SharpZipLib.GZip.GZipInputStream(System.IO.File.OpenRead(pathFileTAR)))
                {
                    try
                    {
                        componentEvents.FireInformation(1, "UnZip SSIS", "Start verify file GZ (" + _folderSource + _fileZip + ")", null, 0, ref b);

                        gzip.CopyTo(System.IO.MemoryStream.Null);
                        gzip.Flush();

                        componentEvents.FireInformation(1, "UnZip SSIS", "File ZIP verified GZ (" + _folderSource + _fileZip + ")", null, 0, ref b);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Verify file: " + _fileZip + " failed. (" + ex.Message + ")");
                    }
                    gzip.Close();
                }
            }
        }

        private void _DeCompressGZ(string pathFileTAR, IDTSComponentEvents componentEvents)
        {
            _Check_GZ(pathFileTAR, componentEvents);

            using (ICSharpCode.SharpZipLib.GZip.GZipInputStream gzip = new ICSharpCode.SharpZipLib.GZip.GZipInputStream(System.IO.File.OpenRead(pathFileTAR)))
            {  
                _DeCompressTAR(pathFileTAR, componentEvents, gzip);
                //gzip.Flush();
                //gzip.Close();
            }
        }

        private void _CompressGZ(string pathFileTAR, IDTSComponentEvents componentEvents)
        {
            using (ICSharpCode.SharpZipLib.GZip.GZipOutputStream gzip = new ICSharpCode.SharpZipLib.GZip.GZipOutputStream(System.IO.File.Create(pathFileTAR)))
            {
                gzip.SetLevel(9);
                _CompressTAR(pathFileTAR, componentEvents, gzip);
                //gzip.Flush();
                //gzip.Close();
            }

            _Check_GZ(pathFileTAR, componentEvents);
        }

        private void _DeCompressTAR(string pathFileTAR, IDTSComponentEvents componentEvents, System.IO.Stream stream)
        {
            bool b = false;
            if (stream == null)
            {
                stream = System.IO.File.OpenRead(pathFileTAR);
            }

            using (ICSharpCode.SharpZipLib.Tar.TarInputStream tar = new ICSharpCode.SharpZipLib.Tar.TarInputStream(stream))
            {
                ICSharpCode.SharpZipLib.Tar.TarEntry te = tar.GetNextEntry();

                while (te != null)
                {
                    string fn = te.Name.Replace("/", "\\");
                    System.IO.FileInfo fi = null;

                    if ( (!System.Text.RegularExpressions.Regex.Match(fn, _fileFilter).Success) || (te.IsDirectory && te.Size == 0) )
                    {
                        if (!System.Text.RegularExpressions.Regex.Match(fn, _fileFilter).Success)
                        {
                            componentEvents.FireInformation(1, "UnZip SSIS", _typeCompression.ToString() + ": file " + fn + " doesn't match regex filter '" + _fileFilter + "'", null, 0, ref b); //  Added information display when regex doesn't match (Updated on 2015-12-30 by Nico_FR75)
                        }

                        te = tar.GetNextEntry();
                        continue;
                    }

                    componentEvents.FireInformation(1, "UnZip SSIS", _typeCompression.ToString() + ": De-Compress (with '" + _storePaths.ToString() + "') file: " + fn, null, 0, ref b);

                    if (_storePaths == Store_Paths.Absolute_Paths || _storePaths == Store_Paths.Relative_Paths)
                    {
                        //Absolute / Relative Path
                        fi = new System.IO.FileInfo(_folderDest + fn);
                        if (!System.IO.Directory.Exists(fi.DirectoryName))
                        {
                            System.IO.Directory.CreateDirectory(fi.DirectoryName);
                        }
                    }
                    else if (_storePaths == Store_Paths.No_Paths)
                    {
                        //No Path
                        fi = new System.IO.FileInfo(_folderDest + System.IO.Path.GetFileName(fn));
                    }
                    else
                    {
                        throw new Exception("Please select type Store Paths (No_Paths / Relative_Paths / Absolute_Paths).");
                    }

                    using (System.IO.FileStream fs = new System.IO.FileStream(fi.FullName, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                    {
                        tar.CopyTo(fs);
                        fs.Flush();
                    }

                    te = tar.GetNextEntry();
                }

                tar.Flush();
                tar.Close();
            }
        }

        private void _CompressTAR(string pathFileTAR, IDTSComponentEvents componentEvents, System.IO.Stream stream)
        {
            bool b = false;
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(_folderSource);
            System.IO.FileInfo[] fi_s = di.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            if (stream == null)
            {
                stream = System.IO.File.Create(pathFileTAR);
            }

            using (ICSharpCode.SharpZipLib.Tar.TarOutputStream tar = new ICSharpCode.SharpZipLib.Tar.TarOutputStream(stream))
            {
                foreach (System.IO.FileInfo fi in fi_s)
                {
                    if (!System.Text.RegularExpressions.Regex.Match(fi.FullName, _fileFilter).Success)
                    {
                        componentEvents.FireInformation(1, "UnZip SSIS", _typeCompression.ToString() + ": file " + fi.FullName + " doesn't match regex filter '" + _fileFilter + "'. File not processed.", null, 0, ref b);
                        continue;
                    }

                    componentEvents.FireInformation(1, "UnZip SSIS", _typeCompression.ToString() + ": Compress (with '" + _storePaths.ToString() + "') file: " + fi.FullName, null, 0, ref b);
                    string fileName = fi.FullName;

                    if (_storePaths == Store_Paths.Absolute_Paths)
                    {
                        //Absolute Path
                        fileName = fi.FullName.Replace("\\", "/").Substring(3);
                    }
                    else if (_storePaths == Store_Paths.Relative_Paths)
                    {
                        //Relative Path
                        fileName = fileName.Replace(_folderSource, "").Replace("\\", "/");
                        if (_addRootFolder)
                        {
                            fileName = (di.Name + "/" + fileName).Replace("//", "/");
                        }
                    }
                    else if (_storePaths == Store_Paths.No_Paths)
                    {
                        //No Path
                        fileName = fi.Name;
                    }
                    else
                    {
                        throw new Exception("Please select type Store Paths (No_Paths / Relative_Paths / Absolute_Paths).");
                    }

                    using (System.IO.FileStream fs = new System.IO.FileStream(fi.FullName, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {

                        ICSharpCode.SharpZipLib.Tar.TarEntry te = ICSharpCode.SharpZipLib.Tar.TarEntry.CreateTarEntry(fileName);
                        te.Size = fs.Length;
                        tar.PutNextEntry(te);

                        fs.CopyTo(tar);

                        fs.Flush();
                        tar.Flush();

                        tar.CloseEntry();
                    }
                }

                tar.Flush();
                tar.Close();
            }
        }

        #endregion
        
        #region De/Compress Functions -- ZIP --

        private void _Check_ZIP(string pathFileZip, IDTSComponentEvents componentEvents)
        {
            bool b = false;
            if (_testarchive)
            {
                using (ICSharpCode.SharpZipLib.Zip.ZipInputStream fz = new ICSharpCode.SharpZipLib.Zip.ZipInputStream(System.IO.File.OpenRead(pathFileZip)))
                {
                    if (!string.IsNullOrEmpty(_password))
                    {
                        fz.Password = _password;
                    }

                    try
                    {
                        ICSharpCode.SharpZipLib.Zip.ZipEntry ze = fz.GetNextEntry();
                        componentEvents.FireInformation(1, "UnZip SSIS", "Start verify file ZIP (" + _folderSource + _fileZip + ")", null, 0, ref b);

                        while (ze != null)
                        {
                            componentEvents.FireInformation(1, "UnZip SSIS", "Verifying Entry: " + ze.Name, null, 0, ref b);
                            
                            fz.CopyTo(System.IO.MemoryStream.Null);
                            fz.Flush();

                            fz.CloseEntry();
                            ze = fz.GetNextEntry();
                        }
                        componentEvents.FireInformation(1, "UnZip SSIS", "File ZIP verified ZIP (" + _folderSource + _fileZip + ")", null, 0, ref b);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Verify file: " + _fileZip + " failed. (" + ex.Message + ")");
                    }
                    finally
                    {
                        fz.Close();
                    }
                }
            }
        }        

        private void _DeCompressZIP(string pathFileZip, IDTSComponentEvents componentEvents)
        {
            bool b = false;

            _Check_ZIP(pathFileZip, componentEvents);
            
            using (ICSharpCode.SharpZipLib.Zip.ZipInputStream fz = new ICSharpCode.SharpZipLib.Zip.ZipInputStream(System.IO.File.OpenRead(pathFileZip)))
            {
                if (!string.IsNullOrEmpty(_password))
                {
                    fz.Password = _password;
                }

                ICSharpCode.SharpZipLib.Zip.ZipEntry ze = fz.GetNextEntry();

                while (ze != null)
                {
                    string fn = ze.Name.Replace("/", "\\");
                    System.IO.FileInfo fi = null;

                    if ( (!System.Text.RegularExpressions.Regex.Match(fn, _fileFilter).Success) || (ze.IsDirectory && ze.Size == 0) )
                    {
                        if (!System.Text.RegularExpressions.Regex.Match(fn, _fileFilter).Success)
                        {
                            componentEvents.FireInformation(1, "UnZip SSIS", _typeCompression.ToString() + ": file " + fn + " doesn't match regex filter '" + _fileFilter + "'", null, 0, ref b); //  Added information display when regex doesn't match (Updated on 2015-12-30 by Nico_FR75)
                        }

                        fz.CloseEntry();
                        ze = fz.GetNextEntry();
                        continue;
                    }

                    componentEvents.FireInformation(1, "UnZip SSIS", _typeCompression.ToString() + ": De-Compress (with '" + _storePaths.ToString() + "') file: " + fn, null, 0, ref b);

                    if (_storePaths == Store_Paths.Absolute_Paths || _storePaths == Store_Paths.Relative_Paths)
                    {
                        //Absolute / Relative Path
                        fi = new System.IO.FileInfo(_folderDest + fn);
                        if (!System.IO.Directory.Exists(fi.DirectoryName))
                        {
                            System.IO.Directory.CreateDirectory(fi.DirectoryName);
                        }
                    }
                    else if (_storePaths == Store_Paths.No_Paths)
                    {
                        //No Path
                        fi = new System.IO.FileInfo(_folderDest + System.IO.Path.GetFileName(fn));
                    }
                    else
                    {
                        throw new Exception("Please select type Store Paths (No_Paths / Relative_Paths / Absolute_Paths).");
                    }

                    using (System.IO.FileStream fs = new System.IO.FileStream(fi.FullName, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                    {
                        fz.CopyTo(fs);
                        fs.Flush();
                    }

                    fz.CloseEntry();
                    ze = fz.GetNextEntry();
                }

                fz.Close();
            }
        }

        private void _CompressZIP(string pathFileZip,IDTSComponentEvents componentEvents)
        {
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(_folderSource);
            System.IO.FileInfo[] fi_s = di.GetFiles("*.*", (_recurse ? System.IO.SearchOption.AllDirectories : System.IO.SearchOption.TopDirectoryOnly));
            bool b = false;

            try
            {
                using (ICSharpCode.SharpZipLib.Zip.ZipOutputStream fz = new ICSharpCode.SharpZipLib.Zip.ZipOutputStream(System.IO.File.Create(pathFileZip)))
                {
                    fz.SetLevel(9);

                    if (!string.IsNullOrEmpty(_comment))
                    {
                        componentEvents.FireInformation(1, "UnZip SSIS", "Set Comment.", null, 0, ref b);
                        fz.SetComment(_comment);
                    }

                    if (!string.IsNullOrWhiteSpace(_password))
                    {
                        componentEvents.FireInformation(1, "UnZip SSIS", "Set Password.", null, 0, ref b);
                        fz.Password = _password;
                    }


                    foreach (System.IO.FileInfo fi in fi_s)
                    {
                        if (!System.Text.RegularExpressions.Regex.Match(fi.FullName, _fileFilter).Success)
                        {
                            componentEvents.FireInformation(1, "UnZip SSIS", _typeCompression.ToString() + ": file " + fi.FullName + " doesn't match regex filter '" + _fileFilter + "'. File not processed.", null, 0, ref b);
                            continue;
                        }

                        componentEvents.FireInformation(1, "UnZip SSIS", _typeCompression.ToString() + ": Compress (with '" + _storePaths.ToString() + "') file: " + fi.FullName, null, 0, ref b);
                        
                        string file_name = "";
                        ICSharpCode.SharpZipLib.Zip.ZipEntry ze = null;
                        
                        if (_storePaths == Store_Paths.Absolute_Paths)
                        {
                            //Absolute Path
                            file_name = ICSharpCode.SharpZipLib.Zip.ZipEntry.CleanName(fi.FullName);
                            ze = new ICSharpCode.SharpZipLib.Zip.ZipEntry(file_name);
                        }
                        else if (_storePaths == Store_Paths.Relative_Paths)
                        {
                            //Relative Path
                            ICSharpCode.SharpZipLib.Zip.ZipNameTransform zn = new ICSharpCode.SharpZipLib.Zip.ZipNameTransform(_folderSource);
                            file_name = zn.TransformFile(fi.FullName);
                            if (_addRootFolder)
                            {
                                file_name = (di.Name + "/" + file_name).Replace("//", "/");
                            }
                            ze = new ICSharpCode.SharpZipLib.Zip.ZipEntry(file_name);
                        }
                        else if (_storePaths == Store_Paths.No_Paths)
                        {
                            //No Path
                            file_name = fi.Name;
                            ze = new ICSharpCode.SharpZipLib.Zip.ZipEntry(file_name);
                        }
                        else
                        {
                            throw new Exception("Please select type Store Paths (No_Paths / Relative_Paths / Absolute_Paths).");
                        }

                        using (System.IO.FileStream fs = new System.IO.FileStream(fi.FullName, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                        {
                            ze.Size = fs.Length;
                            fz.PutNextEntry(ze);
                            
                            fs.CopyTo(fz);
                            
                            fs.Flush();
                            fz.Flush();
                            
                            fz.CloseEntry();
                        }
                    }

                    fz.Flush();
                }

                _Check_ZIP(pathFileZip, componentEvents);

            }
            catch (Exception ex)
            {
                componentEvents.FireError(1000, "UnZip SSIS", ex.Message, null, 0);
                throw;
            }
            finally
            {

            }
        }

        #endregion

        public override DTSExecResult Execute(Connections connections, VariableDispenser variableDispenser, IDTSComponentEvents componentEvents, IDTSLogging log, object transaction)
        {
            bool b = false;
            
            _fileZip = _CheckVarName(_fileZip, variableDispenser);
            _folderSource = _CheckVarName(_folderSource, variableDispenser);
            _folderDest = _CheckVarName(_folderDest, variableDispenser);
            _fileFilter = _CheckVarName(_fileFilter, variableDispenser);

            if (_typeOperation == Type_Operation.Please_Select)
            {
                componentEvents.FireError(1, "UnZip SSIS", "Please select type operation (De/Compress).", null, 0);
                throw new Exception("Please select type operation (De/Compress).");
            }

            if (!_folderSource.EndsWith("\\"))
            {
                _folderSource = _folderSource + "\\";
            }
            if (!_folderDest.EndsWith("\\"))
            {
                _folderDest = _folderDest + "\\";
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
                    //COMPRESS --------------->
                    componentEvents.FireInformation(1, "UnZip SSIS", "Start -- " + _typeOperation.ToString() + " -- process file: '" + _folderDest + _fileZip + "'.", null, 0, ref b);    //   (Updated on 2015-12-30 by Nico_FR75)

					if (System.IO.File.Exists(_folderDest + _fileZip) && !this.OverwriteZipFile)
                    {
                        throw new Exception("File zip: '" + _folderDest + _fileZip + "' exists. Delete file or set 'OverwriteZipFile=true'");
                    }

                    if (_typeCompression == Type_Compression.ZIP)
                    {
                        _CompressZIP(_folderDest + _fileZip, componentEvents);
                    }
                    else if (_typeCompression == Type_Compression.TAR)
                    {
                        _CompressTAR(_folderDest + _fileZip, componentEvents, null);
                    }
                    else if (_typeCompression == Type_Compression.TAR_GZ)
                    {
                        _CompressGZ(_folderDest + _fileZip, componentEvents);
                    }
                    else
                    {
                        throw new Exception("Type compression not supported.");
                    }
                }
                else
                {
                    //DE-COMPRESS --------------->
                    componentEvents.FireInformation(1, "UnZip SSIS", "Start -- " + _typeOperation.ToString() + " -- process file: '" + _folderSource + _fileZip + "'.", null, 0, ref b);    //   (Updated on 2015-12-30 by Nico_FR75)
					
					if (_typeCompression == Type_Compression.ZIP)
                    {
                        _DeCompressZIP(_folderSource + _fileZip, componentEvents);
                    }
                    else if (_typeCompression == Type_Compression.TAR)
                    {
                        _DeCompressTAR(_folderSource + _fileZip, componentEvents, null);
                    }
                    else if (_typeCompression == Type_Compression.TAR_GZ)
                    {
                        _DeCompressGZ(_folderSource + _fileZip, componentEvents);
                    }
                    else
                    {
                        throw new Exception("Type compression not supported.");
                    }
                }
            }
            catch (Exception ex)
            {
                componentEvents.FireError(1000, "UnZip SSIS", ex.Message, null, 0);
                return DTSExecResult.Failure;
            }

            componentEvents.FireInformation(1, "UnZip SSIS", "End -- " + _typeOperation.ToString() + " -- process file: '" + _folderDest + _fileZip + "'.", null, 0, ref b);

            return DTSExecResult.Success;
        }
    }
}

