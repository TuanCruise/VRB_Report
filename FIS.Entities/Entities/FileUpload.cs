using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using FIS.Base;
using System.IO;
using System.ServiceModel;

namespace FIS.Entities
{
    [KnownType(typeof(Stream))]
    [MessageContract]
    public class FileUpload
    {
        [MessageHeader]
        public int SecID { get; set; }
        [MessageHeader]
        public string RptID { get; set; }
        [MessageHeader]
        public string Term { get; set; }
        [MessageHeader]
        public string TermNo { get; set; }
        [MessageHeader]
        public int RYear { get; set; }                
        [MessageHeader]
        public string FileName { get; set; }
        [MessageBodyMember]
        public Stream UploadStream { get; set; }
    }
}
