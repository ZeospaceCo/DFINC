using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;

namespace Zeospace.DeskPlus.Account.BLL
{
    public class SLOPwdChange
    {
        /// <summary>
        /// 타 제품 패스워드 연동 (ERP)
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_userID">The _user identifier.</param>
        /// <param name="_loginID">The _login identifier.</param>
        /// <param name="_password">The _password.</param>
        /// <returns></returns>
        public bool ModPwdChange(int _companyID, int _userID, string _loginID, string _password)
        {
            if (_loginID.Length > 0 && _password.Length > 0)
            {
                #region ERP 인사 패스워드 변경하기

                string erpUse = AppSettingHelper.GetAppSetting(AppSettingType.ACCOUNT, "ERPUse", "Y");
                if (erpUse == "Y")
                {
                    string erpPwd = _password.ToNull();

                    //erpPwd = HashManager.GetHash(erpPwd);

                    //AppException.WriteAssert("ModPwdChange ERP - erpPwd : " + erpPwd, AppSettingType.ACCOUNT);

                    DAL.SLOPwdChange dalERP = new DAL.SLOPwdChange();
                    dalERP.ModERPUserPW(_companyID, _loginID, erpPwd);
                }

                #endregion ERP 인사 패스워드 변경하기
            }

            return true;
        }
    }
}
