using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FIS.Entities;
using FIS.FReader;
using FIS.FReader.Entities;
using FIS.FReader.Excute;

namespace FIS.FReader.Process
{

    public class txtController : IDisposable
    {
        private StreamReader MsgReader;
        public static string TAG_DATE = "Report creation date";
        public static string TAG_CURRENCY = "Currency: ";
        public static string TAG_CONTRACTTYPE = "Contract type: ";
        public long Length { get; protected set; }
        public bool EndOfStream
        {
            get
            {
                return MsgReader.EndOfStream;
            }
        }
        public txtController(string FileName)
        {
            MsgReader = new StreamReader(File.Open(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), Encoding.Default);
            Length = MsgReader.BaseStream.Length;
        }
        public void Seek(long Position)
        {
            MsgReader.BaseStream.Seek(Position, SeekOrigin.Begin);

        }
        public DateTime GetDate(string Text)
        {
            string mDate = Text.Replace("Report creation date ", "").Trim();
            return DateTime.ParseExact(mDate, "dd.MM.yyyy HH:mm:ss", null);
        }
        public String GetData(string input, string filter)
        {
            return input.Replace(filter, "").Trim();
        }

        #region IDisposable Members

        public void Dispose()
        {
            try
            {
                MsgReader.Close();
            }
            catch
            {
                MsgReader = null;
            }
        }

        #endregion
    }
}
