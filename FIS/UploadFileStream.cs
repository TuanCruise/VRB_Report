using System;
using System.IO;

namespace FIS.AppClient
{
    public class UploadFileStream : Stream
    {
        public class UploadStatusArgs : EventArgs
        {
            public long Length { get; set; }
            public int Uploaded { get; set; }
        }

        private readonly FileStream _stream;
        public delegate void UploadStatusEvent(object obj, UploadStatusArgs e);
        public event UploadStatusEvent OnUploadStatusChanged;

        private readonly UploadStatusArgs _uploadStatus;

        protected virtual void OnOnUploadStatusChanged(object obj, UploadStatusArgs e)
        {
            UploadStatusEvent handler = OnUploadStatusChanged;
            if (handler != null) handler(obj, e);
        }

        public UploadFileStream(FileStream stream)
        {
            _stream = stream;
            _uploadStatus = new UploadStatusArgs();
            _uploadStatus.Length = stream.Length;
        }
        //public UploadFileStream(byte[] _bytes)
        //{
        //    //_stream = stream;
        //    _uploadStatus = new UploadStatusArgs();
        //    _uploadStatus.Length = _bytes.Length;
        //}

        public override void Flush()
        {
            _stream.Flush();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _stream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _stream.SetLength(value);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var totalReaded = _stream.Read(buffer, offset, count);
            _uploadStatus.Uploaded += totalReaded;
            OnOnUploadStatusChanged(this, _uploadStatus);
            return totalReaded;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _stream.Write(buffer, offset, count);
        }

        public override bool CanRead
        {
            get { return _stream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return _stream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return _stream.CanWrite; }
        }

        public override long Length
        {
            get { return _stream.Length; }
        }

        public override long Position
        {
            get { return _stream.Position; }
            set { _stream.Position = value; }
        }
    }
}
