using DevExpress.XtraEditors.Controls;
using FIS.Base;
using FIS.Common;
using FIS.Controllers;
using FIS.Entities;
using FIS.Utils;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;


namespace FIS.AppClient.Controls
{
    public partial class ucUploadFile : ucModule
    {
        private List<string> AttachmentFiles = new List<string>();
        private List<string> checkedListFiles = new List<string>();

        public ucUploadFile()
        {
            InitializeComponent();
            Program.FileName = String.Empty;
        }
        public int MessageID { get; set; }

        protected override void BuildButtons()
        {
#if DEBUG
            SetupContextMenu(mainLayout);
            SetupSaveLayout(mainLayout);

#endif
        }

        private void btnFilename_Click(object sender, ButtonPressedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = IMPORTMASTER.ATTACKED_FILE_EXTENSIONS;
            if (Program.strExecMod == "S")
                openFile.Multiselect = false;
            else
                openFile.Multiselect = true;

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                AttachmentFiles.AddRange(openFile.FileNames);
                checkedListBoxControl1.Items.Clear();
                int i = itemChecked;
                foreach (var strFile in AttachmentFiles)
                {
                    checkedListBoxControl1.Items.Add(strFile, true);
                    i++;
                }
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)

        {
            Execute();
        }
        // Ham upload file khong su dung chu ky so
        public override void Execute()
        {
            Program.blCheckFile = false;
            base.Execute();
            var ctrlSA = new SAController();
            try
            {
                File.AppendAllText("LastErrors.log", string.Format("{0}\r\n-------------\r\n", DateTime.Now.ToLongTimeString()));
                checkedListFiles.Clear();
                for (int i = 0; i < checkedListBoxControl1.Items.Count; i++)
                {
                    if (checkedListBoxControl1.GetItemChecked(i) == true)
                    {

                        checkedListFiles.Add(checkedListBoxControl1.GetItemValue(i).ToString());
                    }
                }
                if (checkedListFiles.Count != 0)
                {
                    // Tao file Zip
                    var outPathname = System.Environment.GetEnvironmentVariable("TEMP") + "\\" + RandomString(10, false) + ".zip"; ;
                    FileStream fsOut = File.Create(outPathname);
                    ZipOutputStream zipStream = new ZipOutputStream(fsOut);
                    zipStream.SetLevel(9); //0-9, 9 being the highest level of compression                    
                    foreach (var filename in checkedListFiles)
                    {

                        if (!string.IsNullOrEmpty(filename))
                        {
                            FileInfo fi = new FileInfo(filename);
                            string entryName = System.IO.Path.GetFileName(filename);
                            entryName = ZipEntry.CleanName(entryName);
                            ZipEntry newEntry = new ZipEntry(entryName);
                            newEntry.DateTime = fi.LastWriteTime;

                            newEntry.Size = fi.Length;
                            zipStream.PutNextEntry(newEntry);

                            // Zip the file in buffered chunks                                
                            byte[] buffer = new byte[4096];
                            using (FileStream streamReader = File.OpenRead(filename))
                            {
                                StreamUtils.Copy(streamReader, zipStream, buffer);
                            }
                            zipStream.CloseEntry();
                        }
                    }
                    zipStream.IsStreamOwner = true; // Makes the Close also Close the underlying stream
                    zipStream.Close();
                    var _streamAttr = new FileStream(outPathname, FileMode.Open, FileAccess.Read);
                    prgUploadFile.Value = 0;
                    var fileUpload = new UploadFileStream(_streamAttr);
                    fileUpload.OnUploadStatusChanged += fileUpload_OnUploadStatusChanged;

                    FileUpload upload = new FileUpload();
                    upload.FileName = System.IO.Path.GetFileName(outPathname);
                    upload.UploadStream = fileUpload;
                    upload.SecID = 0;
                    ctrlSA.SaveFile(upload);

                    Program.FileName = Program.FileName + System.IO.Path.GetFileName(outPathname);
                    _streamAttr.Dispose();
                }

                //List<string> listparam = new List<string>();
                //for (int i = 0; i < checkedListBoxControl1.Items.Count; i++)
                //{

                //    if (checkedListBoxControl1.GetItemChecked(i) == false)
                //    {
                //        listparam.Clear();
                //        listparam.Add(checkedListBoxControl1.GetItemValue(i).ToString());                                                
                //    }
                //}
                //FIS.Entities.Session session = new FIS.Entities.Session();
                //ctrlSA.GetCurrentSessionInfo(out session);
                //listparam.Add(session.SessionKey);
                //ctrlSA.ExecuteStoreProcedure("SP_DELFILE_BY_SESSIONSKEY", listparam);

                if (checkedListBoxControl1.Items.Count > 0)
                    Program.blCheckFile = true;

                File.AppendAllText("LastErrors.log", string.Format("{0}\r\n-------------\r\n", DateTime.Now.ToLongTimeString()));
                CloseModule();
            }
            catch (Exception ex)
            {
                //CREATE ERROCODE: 702 - ERR_FILE_IS_NOT_ATTACKED
                File.AppendAllText("LastErrors.log", string.Format("{0}\r\n-------------\r\n", ex.Message));
                Program.FileName = null;
                var cex = ErrorUtils.CreateError(702);
                ShowError(cex);
            }
        }
        //public override void Execute()
        //{
        //    Program.blCheckFile = false;
        //    base.Execute();
        //    var ctrlSA = new SAController();
        //    try
        //    {
        //        File.AppendAllText("LastErrors.log", string.Format("{0}\r\n-------------\r\n", DateTime.Now.ToLongTimeString()));
        //        //edit by TrungTT - 16.04.2012
        //        checkedListFiles.Clear();
        //        for (int i = 0; i < checkedListBoxControl1.Items.Count; i++)
        //        {
        //            if (checkedListBoxControl1.GetItemChecked(i) == true)
        //            {

        //                checkedListFiles.Add(checkedListBoxControl1.GetItemValue(i).ToString());
        //            }
        //        }
        //        if (checkedListFiles.Count != 0)
        //        {
        //            DataContainer container = null;
        //            DataContainer conSinger = null;
        //            List<string> lstParam = new List<string>();
        //            ctrlSA.ExecuteProcedureFillDataset(out container, "SP_CHECKSIGNER", lstParam);
        //            ctrlSA.ExecuteProcedureFillDataset(out conSinger, "SP_GETSIGNER", lstParam);

        //            // Xoa het file trong Deffile trong session 
        //            ctrlSA.ExecuteStoreProcedure("sp_deffile_del_bysession", lstParam);

        //            string IsCert = CONSTANTS.No;

        //            // Tao file Zip
        //            var outPathname = System.Environment.GetEnvironmentVariable("TEMP") + "\\" + RandomString(10, false) + ".zip"; ;
        //            FileStream fsOut = File.Create(outPathname);
        //            ZipOutputStream zipStream = new ZipOutputStream(fsOut);
        //            zipStream.SetLevel(9); //0-9, 9 being the highest level of compression                    

        //            string _filenamesigned;
        //            string _filetemplate = string.Empty;
        //            foreach (var filename in checkedListFiles)
        //            {
        //                _filenamesigned = filename;
        //                _filetemplate = filename;

        //                if (File.Exists(filename) && filename.Length != 0)
        //                {
        //                    // Ky file truoc khi nen
        //                    int result;
        //                    //if (IsCert == CONSTANTS.Yes)
        //                    bool chkCA = false;
        //                    if (container.DataSet.Tables[0].Rows.Count > 0)
        //                    {
        //                        IsCert = container.DataSet.Tables[0].Rows[0][0].ToString();
        //                        if (IsCert == CONSTANTS.Yes)
        //                        {
        //                            string serial = VNPTLIB.CertificateInfos.getCertSerial();
        //                            for (int i = 0; i < conSinger.DataSet.Tables[0].Rows.Count; i++)
        //                            {
        //                                if (conSinger.DataSet.Tables[0].Rows[i]["serial"].ToString() == serial)
        //                                {
        //                                    chkCA = true;
        //                                }
        //                            }
        //                        }
        //                    }
        //                    //else
        //                    //{
        //                    //    FIS.Entities.Session  session = new FIS.Entities.Session();
        //                    //    ctrlSA.GetCurrentSessionInfo(out session);
        //                    //    if (session.Type != CONSTANTS.USER_TYPE_UBCKNN)
        //                    //    {
        //                    //        IsCert = CONSTANTS.Yes;
        //                    //    }

        //                    //}

        //                    if (IsCert == CONSTANTS.Yes)
        //                    {
        //                        if (chkCA)
        //                        {
        //                            // Lay gio tu timestamp server
        //                            string datetime = VNPTLIB.TSA.checkTSA();
        //                            if (datetime == null)
        //                            {
        //                                datetime = DateTime.Now.ToString("dd/MM/yyyy");
        //                            }
        //                            if (Path.GetExtension(_filenamesigned).ToUpper() == ".PDF")
        //                            {
        //                                result = VNPTLIB.SignPDF.SignDetached(filename, datetime);
        //                                if (result == 0)
        //                                {
        //                                    _filenamesigned = VNPTLIB.SignPDF.fileSigned;
        //                                }
        //                                else
        //                                {
        //                                    ShowMessVNPTCA(result, VNPTLIB.SignPDF.errorPDF);
        //                                    Program.FileName = null;
        //                                    return;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                result = VNPTLIB.SignOffice.Sign2k7XLS(filename, datetime);
        //                                if (result == 0)
        //                                {
        //                                    _filenamesigned = filename;
        //                                }
        //                                else
        //                                {
        //                                    ShowMessVNPTCA(result, VNPTLIB.SignOffice.error);
        //                                    Program.FileName = null;
        //                                    return;
        //                                }
        //                            }
        //                        }
        //                        else
        //                        {
        //                            var cex = ErrorUtils.CreateError(10);
        //                            ShowError(cex);
        //                            Program.FileName = null;
        //                            return;
        //                        }
        //                    }

        //                    FileInfo fi = new FileInfo(_filenamesigned);
        //                    //string entryName = System.IO.Path.GetFileName(outPathname);
        //                    string entryName = System.IO.Path.GetFileName(_filenamesigned);
        //                    entryName = ZipEntry.CleanName(entryName);
        //                    ZipEntry newEntry = new ZipEntry(entryName);
        //                    newEntry.DateTime = fi.LastWriteTime;

        //                    newEntry.Size = fi.Length;
        //                    zipStream.PutNextEntry(newEntry);

        //                    // Zip the file in buffered chunks                                
        //                    byte[] buffer = new byte[4096];
        //                    using (FileStream streamReader = File.OpenRead(_filenamesigned))
        //                    {
        //                        StreamUtils.Copy(streamReader, zipStream, buffer);
        //                    }
        //                    zipStream.CloseEntry();
        //                }
        //            }

        //            zipStream.IsStreamOwner = true; // Makes the Close also Close the underlying stream
        //            zipStream.Close();

        //            #region "Tao rieng phan nay de update bieu mau"
        //            FIS.Entities.Session session = new FIS.Entities.Session();
        //            ctrlSA.GetCurrentSessionInfo(out session);
        //            if (session.Username.ToUpper() == CONSTANTS.USER_ADMIN && !string.IsNullOrEmpty(_filetemplate))
        //            {
        //                var _streamAttr = new FileStream(_filetemplate, FileMode.Open, FileAccess.Read);
        //                prgUploadFile.Value = 0;
        //                var fileUpload = new UploadFileStream(_streamAttr);
        //                fileUpload.OnUploadStatusChanged += fileUpload_OnUploadStatusChanged;

        //                FileUpload upload = new FileUpload();
        //                upload.FileName = System.IO.Path.GetFileName(_filetemplate);
        //                upload.UploadStream = fileUpload;
        //                upload.SecID = 0;
        //                ctrlSA.SaveFile(upload);

        //                Program.FileName = Program.FileName + System.IO.Path.GetFileName(_filetemplate) + ",";
        //                _streamAttr.Dispose();
        //            }
        //            else
        //            {
        //                if (File.Exists(outPathname) && outPathname.Length != 0)
        //                {
        //                    var _streamAttr = new FileStream(outPathname, FileMode.Open, FileAccess.Read);
        //                    prgUploadFile.Value = 0;
        //                    var fileUpload = new UploadFileStream(_streamAttr);
        //                    fileUpload.OnUploadStatusChanged += fileUpload_OnUploadStatusChanged;

        //                    FileUpload upload = new FileUpload();
        //                    upload.FileName = System.IO.Path.GetFileName(outPathname);
        //                    upload.UploadStream = fileUpload;
        //                    upload.SecID = 0;
        //                    ctrlSA.SaveFile(upload);
        //                    Program.FileName = Program.FileName + System.IO.Path.GetFileName(outPathname) + ",";
        //                    _streamAttr.Dispose();
        //                }
        //            }
        //            #endregion

        //            // Xoa file Zip
        //            File.Delete(outPathname);
        //        }
        //        List<string> listparam = new List<string>();
        //        for (int i = 0; i < checkedListBoxControl1.Items.Count; i++)
        //        {

        //            if (checkedListBoxControl1.GetItemChecked(i) == false)
        //            {
        //                listparam.Clear();
        //                listparam.Add(checkedListBoxControl1.GetItemValue(i).ToString());
        //                ctrlSA.ExecuteStoreProcedure("SP_DELFILE_BY_SESSIONSKEY", listparam);
        //            }
        //        }

        //        //trungtt - 17.10.2013 - rao lai vi doan code nay lam cho ko multifile duoc
        //        //// Xoa file khong nam trong list
        //        //List<string> values = new List<string>();
        //        //values.Add(Program.FileName);
        //        //ctrlSA.ExecuteStoreProcedure("sp_deffile_dif_del", values);
        //        //// End
        //        //end trungtt

        //        if (checkedListBoxControl1.Items.Count > 0)
        //            Program.blCheckFile = true;

        //        File.AppendAllText("LastErrors.log", string.Format("{0}\r\n-------------\r\n", DateTime.Now.ToLongTimeString()));
        //        CloseModule();
        //        //end TrungTT
        //    }
        //    catch (Exception ex)
        //    {
        //        //CREATE ERROCODE: 702 - ERR_FILE_IS_NOT_ATTACKED
        //        File.AppendAllText("LastErrors.log", string.Format("{0}\r\n-------------\r\n", ex.Message));
        //        Program.FileName = null;
        //        var cex = ErrorUtils.CreateError(702);
        //        ShowError(cex);
        //    }
        //}
        private byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        private string RandomString(int size, bool lowerCase)
        {
            StringBuilder sb = new StringBuilder();
            char c;
            Random rand = new Random();
            for (int i = 0; i < size; i++)
            {
                c = Convert.ToChar(Convert.ToInt32(rand.Next(65, 87)));
                sb.Append(c);
            }
            if (lowerCase)
                return sb.ToString().ToLower();
            return sb.ToString();

        }
        private void fileUpload_OnUploadStatusChanged(object obj, UploadFileStream.UploadStatusArgs e)
        {
            prgUploadFile.Value = (int)(e.Uploaded * 100 / e.Length);
            // if (prgUploadFile.Value == 100) { lblStatus.Visible = false; }
        }
        public byte[] ReadFile(string filePath)
        {
            byte[] buffer = new byte[0];
            FileStream fileStream = null;

            try
            {
                using (SAController ctrlSA = new SAController())
                {
                    List<string> values = new List<string>();
                    values.Add("SYS");
                    values.Add("FILESIZE");
                    DataContainer dc;
                    ctrlSA.ExecuteProcedureFillDataset(out dc, "sp_sysvar_sel_bygrame", values);
                    DataTable dt = dc.DataTable;
                    if (dt.Rows.Count > 0)
                    {
                        fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                        int length = (int)fileStream.Length;  // get file length
                        if (length <= Convert.ToDecimal(dt.Rows[0]["VARVALUE"].ToString()))
                        {
                            buffer = new byte[length];            // create buffer
                            int count;                            // actual number of bytes read
                            int sum = 0;                          // total number of bytes read

                            // read until Read method returns 0 (end of the stream has been reached)
                            while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                                sum += count;  // sum is a buffer offset for next reading
                        }
                        else
                        {
                            var cex = ErrorUtils.CreateError(169);
                            ShowError(cex);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                //ShowError(ex);                
                throw new Exception();
            }
            finally
            {
                fileStream.Close();
            }
            return buffer;
        }
        //trungtt - 19.2.2014 - Read and sign a file
        //public byte[] ReadAndSignFile(string filePath)
        //{
        //    byte[] buffer = new byte[0];
        //    FileStream fileStream = null;
        //    try
        //    {
        //        using (SAController ctrlSA = new SAController())
        //        {
        //            List<string> values = new List<string>();
        //            values.Add("SYS");
        //            values.Add("FILESIZE");
        //            DataContainer dc;
        //            ctrlSA.ExecuteProcedureFillDataset(out dc, "sp_sysvar_sel_bygrame", values);
        //            DataTable dt = dc.DataTable;
        //            if (dt.Rows.Count > 0)
        //            {
        //                fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        //                int length = (int)fileStream.Length;  // get file length
        //                if (length <= Convert.ToDecimal(dt.Rows[0]["VARVALUE"].ToString()))
        //                {
        //                    //Sign file
        //                    string[] listFileSign = new string[1] { filePath };
        //                    string[] outputFile;
        //                    int result = VNPTLIB.ClientInterfaces.SignMultiFile(listFileSign.ToArray(), out outputFile);
        //                    if (result == 0)
        //                    //Read file
        //                    {
        //                        fileStream = new FileStream(outputFile[0].ToString(), FileMode.Open, FileAccess.Read);
        //                        length = (int)fileStream.Length;  // get file length
        //                        buffer = new byte[length];            // create buffer
        //                        int count;                            // actual number of bytes read
        //                        int sum = 0;                          // total number of bytes read

        //                        // read until Read method returns 0 (end of the stream has been reached)
        //                        while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
        //                            sum += count;  // sum is a buffer offset for next reading
        //                    }
        //                    else
        //                    {
        //                        File.AppendAllText("LastErrors.log", string.Format("{0} : {1}-{2}\r\n-------------\r\n", DateTime.Now.ToLongDateString(), result.ToString(), VNPTLIB.ClientInterfaces.ErrorMessage));
        //                    }
        //                }
        //                else
        //                {
        //                    var cex = ErrorUtils.CreateError(169);
        //                    ShowError(cex);
        //                }

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //ShowError(ex);                
        //        throw new Exception();
        //    }
        //    finally
        //    {
        //        fileStream.Close();
        //    }
        //    return buffer;
        //}
        //end trungtt

        private int itemChecked;
        private void ucUploadFile_Load(object sender, EventArgs e)
        {

            //DataContainer container = null;
            //var ctrlSA = new SAController();
            //try
            //{
            //    List<string> listparam;
            //    GetOracleParameterValues(out listparam, "SP_LISTFILE_BY_SESSIONSKEY");
            //    //listparam[0] = Program.StrMessageID;
            //    //listparam.Add(null);
            //    ctrlSA.ExecuteProcedureFillDataset(out container, "SP_LISTFILE_BY_SESSIONSKEY", listparam);

            //    var resultTable = container.DataTable;
            //    int i = 0;
            //    foreach (DataRow rows in resultTable.Rows)
            //    {
            //        checkedListBoxControl1.Items.Add(rows["FILENAME"].ToString());
            //        checkedListBoxControl1.SetItemChecked(i, true);
            //        i++;
            //    }
            //    itemChecked = i;
            //}

            //catch (Exception ex)
            //{
            //    ShowError(ex);
            //}

        }
        private void ShowMessVNPTCA(int ErrCode, string ErrCodeString)
        {
            int iErr = 996;
            switch (ErrCode)
            {
                case 100:
                    iErr = 990;
                    break;
                case 101:
                    iErr = 991;
                    break;
                case 102:
                    iErr = 992;
                    break;
                case 400:
                    iErr = 993;
                    break;
                case 402:
                    iErr = 994;
                    break;
                case 403:
                    iErr = 995;
                    break;
                case 105:
                    iErr = 988;
                    break;
                case 106:
                    iErr = 989;
                    break;
                case 500:
                    iErr = 996;
                    File.AppendAllText("LastErrors.log", string.Format("{0} : {1}-{2}\r\n-------------\r\n", DateTime.Now.ToLongDateString(), "Lỗi 500 : ", ErrCodeString));
                    break;
            }
            var cex = ErrorUtils.CreateError(iErr);
            ShowError(cex);
        }
    }
}

