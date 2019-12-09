using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FIS.FReader.Entities
{
    /// <summary>
    /// File Audit
    /// </summary>
    public class FileToRead
    {
        //private string fileGUID;
        //public string FileGUID
        //{
        //    get { return fileGUID; }
        //    set { fileGUID = value; }
        //}

        private string fileHash;
        public string FileHash
        {
            get { return fileHash; }
            set { fileHash = value; }
        }

        private string fileName;

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }
        private DateTime fileCreate;

        public DateTime FileCreate
        {
            get { return fileCreate; }
            set { fileCreate = value; }
        }
        private DateTime fileLastMod;

        public DateTime FileLastMod
        {
            get { return fileLastMod; }
            set { fileLastMod = value; }
        }

        private string fileLocation;

        public string FileLocation
        {
            get { return fileLocation; }
            set { fileLocation = value; }
        }

        private long fileSize;

        public long FileSize
        {
            get { return fileSize; }
            set { fileSize = value; }
        }

        private int fileRow;

        public int FileRow
        {
            get { return fileRow; }
            set { fileRow = value; }
        }

        public void LoadFile(String inputFileName)
        {
            //to read info file here
            FileInfo info = new FileInfo(inputFileName);
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(inputFileName))
                {
                    fileHash = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLower();
                    //duchvmaddclose
                    //stream.Close();
                }
            }
            fileName = inputFileName;
            fileCreate = info.CreationTime;
            fileLastMod = info.LastWriteTime;
            fileLocation = info.DirectoryName;
            fileSize = info.Length;
        }
        
        private string timeProcess;

        /// <summary>
        /// count by milisecond
        /// </summary>
        public string TimeProcess
        {
            get { return timeProcess; }
            set { timeProcess = value; }
        }

        private string fileStatus;

        public string FileStatus
        {
            get { return fileStatus; }
            set { fileStatus = value; }
        }
    }
}
