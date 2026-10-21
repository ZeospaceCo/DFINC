using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Account.BE;

namespace Zeospace.DeskPlus.Account.Web.Ws.Admin.User
{
    /// <summary>
    /// LinkageInfo의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class LinkageInfo : Base.BaseWSAccount
    {

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-06-19
        /// 작성목적 : 연동정보 추가, 수정(관리자용)
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddInfo(string p)
        {
            try
            {
                SetDecryptString(p);

                int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
                int userID = GetDecryptParam("userID", "0").ToNumber<int>();

                string deptName = GetDecryptParam("deptName", "");    //전표용부서
                string bankNum = GetDecryptParam("bankNum", "");      //은행계좌번호
                string comPoint = GetDecryptParam("comPoint", "");    //커뮤니티 포인트

                if (companyID < 0 || userID < 0) return false;

                BE.UserMoneyInfo info = new UserMoneyInfo();
                DAL.UserMoney dal = new DAL.UserMoney();

                info.CompanyID = companyID;
                info.UserID = userID;
                info.DeptName = deptName;
                info.BankNum = bankNum;
                info.ComPoint = comPoint;

                dal.Add(info);
                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }

        }
    }
}
