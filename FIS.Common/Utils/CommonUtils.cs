﻿using FIS.Common;
using Microsoft.ServiceModel.Samples;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.ServiceModel.Channels;
using System.Text;

namespace FIS.Utils
{
    public static class MatLabUtils
    {
        public static readonly DateTime MinDate = DateTime.Parse("1-Jan-1900");
        public static double DateNum(this DateTime dt)
        {
            return (dt - MinDate).TotalDays + 693964;
        }
    }
    public static class CommonUtils
    {
        public static Binding CreateHttpBinding()
        {
            /*
            var binding = new WSHttpBinding
            {
                MaxBufferPoolSize = int.MaxValue,
                MaxReceivedMessageSize = int.MaxValue,
                ReliableSession = {
                    Enabled = true
                },
                Security =
                {
                    Mode = SecurityMode.None
                },
                ReaderQuotas =
                {
                    MaxArrayLength = int.MaxValue,
                    MaxStringContentLength = int.MaxValue
                }
            };
            binding.ReceiveTimeout = TimeSpan.FromHours(12);
            binding.SendTimeout = TimeSpan.FromHours(12);
            binding.OpenTimeout = TimeSpan.FromHours(12);
            binding.CloseTimeout = TimeSpan.FromHours(12);

            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.ReaderQuotas.MaxArrayLength = int.MaxValue;
            binding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
            binding.Security.Mode = SecurityMode.None;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
             */

            var binMessageElement = new BinaryMessageEncodingBindingElement();
            binMessageElement.ReaderQuotas.MaxArrayLength = int.MaxValue;
            binMessageElement.ReaderQuotas.MaxStringContentLength = int.MaxValue;
            binMessageElement.ReaderQuotas.MaxBytesPerRead = int.MaxValue;
            binMessageElement.ReaderQuotas.MaxDepth = int.MaxValue;
            binMessageElement.ReaderQuotas.MaxNameTableCharCount = int.MaxValue;

            var gzipElement = new GZipMessageEncodingBindingElement
            {
                InnerMessageEncodingBindingElement = binMessageElement
            };

            var reliableElement = new ReliableSessionBindingElement()
            {
                MaxPendingChannels = 16384,
                Ordered = true
            };
            Boolean _configProxy = false;
            if (App.Configs.UseProxy == CONSTANTS.Yes)
            {
                _configProxy = true;
            }

            var httpTransport = new HttpTransportBindingElement
            {
                KeepAliveEnabled = true,
                MaxBufferPoolSize = int.MaxValue,
                MaxBufferSize = int.MaxValue,
                MaxReceivedMessageSize = int.MaxValue,
                UseDefaultWebProxy = _configProxy,
            };


            //tcpTransport.ReaderQuotas.MaxArrayLength = int.MaxValue;
            //tcpTransport.ReaderQuotas.MaxStringContentLength = int.MaxValue;

            var binding = new CustomBinding(reliableElement, gzipElement, httpTransport)
            {
                ReceiveTimeout = TimeSpan.FromHours(12),
                SendTimeout = TimeSpan.FromHours(12),
                OpenTimeout = TimeSpan.FromHours(12),
                CloseTimeout = TimeSpan.FromHours(12)
            };
            //binding.ReliableSession.Enabled = false;


            //binding.Security.Mode = SecurityMode.None;
            //binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.None;

            return binding;

        }
        public static Binding CreateTcpBinding()
        {
            var binMessageElement = new BinaryMessageEncodingBindingElement();
            binMessageElement.ReaderQuotas.MaxArrayLength = int.MaxValue;
            binMessageElement.ReaderQuotas.MaxStringContentLength = int.MaxValue;
            binMessageElement.ReaderQuotas.MaxBytesPerRead = int.MaxValue;
            binMessageElement.ReaderQuotas.MaxDepth = int.MaxValue;

            var gzipElement = new GZipMessageEncodingBindingElement
            {
                InnerMessageEncodingBindingElement = binMessageElement
            };

            var tcpTransport = new TcpTransportBindingElement
            {
                MaxBufferPoolSize = int.MaxValue,
                MaxBufferSize = int.MaxValue,
                MaxReceivedMessageSize = int.MaxValue,
                ChannelInitializationTimeout = TimeSpan.FromHours(12),
                PortSharingEnabled = false,
                ListenBacklog = int.MaxValue,
                MaxPendingConnections = 100,
                MaxPendingAccepts = 32,
            };
            //tcpTransport.ReaderQuotas.MaxArrayLength = int.MaxValue;
            //tcpTransport.ReaderQuotas.MaxStringContentLength = int.MaxValue;

            var binding = new CustomBinding(gzipElement, tcpTransport)
            {
                ReceiveTimeout = TimeSpan.FromHours(12),
                SendTimeout = TimeSpan.FromHours(12),
                OpenTimeout = TimeSpan.FromHours(12),
                CloseTimeout = TimeSpan.FromHours(12),
            };
            //binding.ReliableSession.Enabled = false;


            //binding.Security.Mode = SecurityMode.None;
            //binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.None;

            return binding;
        }

        public static string MD5Standard(byte[] buffer)
        {
            var md5Encrypt = new MD5CryptoServiceProvider();
            var hashData = md5Encrypt.ComputeHash(buffer);

            return hashData.Aggregate("", (current, t) => current + Convert.ToString(t, 16));
        }

        static public string EncodeTo64(byte[] buffers)
        {
            var returnValue = Convert.ToBase64String(buffers);
            return returnValue;
        }

        static public byte[] DecodeFrom64(string encodedData)
        {
            return Convert.FromBase64String(encodedData);
        }

        public static string MD5Standard(string source)
        {
            var utf8Encoding = new UTF8Encoding();
            byte[] buffer = utf8Encoding.GetBytes(source);
            return MD5Standard(buffer);
        }

        public static string PrivateMD5(string source)
        {
            return MD5Standard(string.Format(CONSTANTS.PRIVATE_MD5_KEY, source));
        }

        public static string MakeUTF8String(string value)
        {
            var res = "";
            foreach (var c in value)
            {
                if (c < 255)
                {
                    res = res + c;
                }
                else
                {
                    res = res + "\\" + string.Format("{0:X4}", Convert.ToUInt16(c));
                }
            }

            return res;
        }

        public static void CacheObject(object serializeObject, string fileName)
        {
            Directory.CreateDirectory(new FileInfo(fileName).Directory.FullName);
            var binaryFormater = new BinaryFormatter();
            var stream = File.Open(fileName, FileMode.Create);
            binaryFormater.Serialize(stream, serializeObject);
            stream.Close();
        }

        public static bool IsCached(string fileName)
        {
#if DEBUG
            return false;
#else
            return File.Exists(fileName);
#endif
        }

        public static object GetCache(string fileName)
        {
            var binaryFormater = new BinaryFormatter();
            var stream = File.Open(fileName, FileMode.Open);
            var cacheResult = binaryFormater.Deserialize(stream);
            stream.Close();

            return cacheResult;
        }

        public static string MD5File(string fileName)
        {
            using (var f = File.OpenRead(fileName))
            {
                var buffer = new byte[f.Length];
                f.Read(buffer, 0, buffer.Length);
                return MD5Standard(buffer);
            }
        }
        public static string Encrypt(string toEncrypt, string key, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            //If hashing use get hashcode regards to your key
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //Always release the resources and flush data
                // of the Cryptographic service provide. Best Practice

                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public static string Decrypt(string cipherString, string key, bool useHashing)
        {
            byte[] keyArray;
            //get the byte code of the string

            byte[] toEncryptArray = Convert.FromBase64String(cipherString);
            if (useHashing)
            {
                //if hashing was used get the hash code with regards to your key
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //release any resource held by the MD5CryptoServiceProvider

                hashmd5.Clear();
            }
            else
            {
                //if hashing was not implemented get the byte code of the key
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            }

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes. 
            //We choose ECB(Electronic code Book)

            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(
                                 toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor                
            tdes.Clear();
            //return the Clear decrypted TEXT
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
        public static string EncryptStringBySHA1(string str)
        {
            SHA1 hash = SHA1.Create();
            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] combined = encoder.GetBytes(str);
            hash.ComputeHash(combined);
            string rethash = Convert.ToBase64String(hash.Hash);
            return rethash;
        }
    }
}
