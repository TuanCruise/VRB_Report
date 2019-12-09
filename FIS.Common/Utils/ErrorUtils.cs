using System;
using System.ServiceModel;
using FIS.Common;

namespace FIS.Utils
{
    public static class ErrorUtils
    {
        /// <summary>
        /// Tạo lỗi không xác định
        /// </summary>
        /// <returns></returns>
        public static FaultException CreateError(Exception ex)
        {
            return CreateErrorWithSubMessage(ERR_SYSTEM.ERR_SYSTEM_UNKNOWN, ex.ToString());
        }

        /// <summary>
        /// Tạo lỗi với mã lỗi xác định
        /// </summary>
        /// <param name="errorCode">Mã lỗi</param>
        /// <returns></returns>
        public static FaultException CreateError(int errorCode)
        {
            var ex = new FaultException("", new FaultCode(errorCode.ToString()));
            return ex;
        }

        /// <summary>
        /// Tạo lỗi với log dạng Method(Param1, Param2, Param3)
        /// </summary>
        /// <param name="errorCode">Mã lỗi</param>
        /// <param name="methodName">Tên method</param>
        /// <param name="methodParamValues">Các tham số truyền vào method</param>
        /// <returns></returns>
        public static FaultException CreateError(int errorCode, string methodName, params object[] methodParamValues)
        {
#if DEBUG
            var message = methodName + "(";

            var sep = "";
            foreach (object value in methodParamValues)
            {
                message += sep + value;
                sep = ",";
            }
            message += ");";
#else
            var message = "";
#endif

            var ex = new FaultException(message, new FaultCode(errorCode.ToString()));
            return ex;
        }

        /// <summary>
        /// Tạo lỗi xác định với chi tiết lỗi
        /// </summary>
        /// <param name="errorCode">Mã lỗi</param>
        /// <param name="subMessage">Chi tiết lỗi</param>
        /// <returns></returns>
        public static FaultException CreateErrorWithSubMessage(int errorCode, string subMessage)
        {
            var ex = new FaultException(subMessage, new FaultCode(errorCode.ToString()));
            return ex;
        }

        /// <summary>
        /// Tạo lỗi xác định với chi tiết lỗi và Method(Param1, Param2, Param3)
        /// </summary>
        /// <param name="errorCode">Mã lỗi</param>
        /// <param name="subMessage">Chi tiết lỗi</param>
        /// <param name="methodName">Tên Method</param>
        /// <param name="methodParamValues">Các tham số truyền vào method</param>
        /// <returns></returns>
        public static FaultException CreateErrorWithSubMessage(int errorCode, string subMessage, string methodName, params object[] methodParamValues)
        {
#if DEBUG
            var message = methodName + "(";

            var sep = "";
            foreach (object value in methodParamValues)
            {
                message += sep + value;
                sep = ",";
            }
            message += ");\r\n-->";
            message += subMessage;
#else
            var message = subMessage;
#endif

            var ex = new FaultException(message, new FaultCode(errorCode.ToString()));
            return ex;
        }
    }

    public static class ExceptionExtends
    {
        public static int GetErrorCode(this FaultException ex)
        {
            return int.Parse(ex.Code.Name);
        }

        public static string GetErrorMessage(this FaultException ex)
        {
            return ex.Message;
        }
    }
}
