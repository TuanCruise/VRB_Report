using FIS.Base;
using FIS.Common;
using FIS.Controllers;
using FIS.Entities;
using FIS.Utils;
using System.Collections.Generic;
using System.Data;

namespace FIS.AppClient
{
    public partial class frmAddIn : DevExpress.XtraEditors.XtraForm
    {
        public frmAddIn()
        {
            InitializeComponent();
            using (var ctrlSA = new SAController())
            {
                User userInfo = new User();
                ctrlSA.GetSessionUserInfo(out userInfo);
                List<string> users = new List<string>();
                users.Add(userInfo.Username);

                DataContainer dc;
                ctrlSA.ExecuteProcedureFillDataset(out dc, "SP_GET_PASS_BY_USER", users);
                DataTable dt = dc.DataTable;
                if (dt.Rows.Count > 0)
                {
                    string strUserName = ""; //= EnCryptDecryptLibrary.CryptorEngine.Encrypt(userInfo.Username, true);
                    string strPassword = "";//= EnCryptDecryptLibrary.CryptorEngine.Encrypt(dt.Rows[0][0].ToString(), true);
                    string sUrl = SysvarUtils.GetVarValue(SYSVAR.GRNAME_SYS, SYSVAR.VARNAME_LINKNDTNN);
                    sUrl = string.Format(sUrl, strUserName, strPassword);
                    wbNDTNN.Navigate(sUrl);
                }
            }
        }
    }
}