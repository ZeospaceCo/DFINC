using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.HR.BE;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using ObjList = System.Collections.Generic.List<System.Object>;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Globalization;

namespace Zeospace.DeskPlus.HR.Web.Ws.Content
{
    /// <summary>
    /// ListUserDept의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ListUserDept : Base.BaseWSHR
    {
        #region MOD()

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2019-04-02
        /// 작성목적 : 겸직자 기본부서 추가 및 수정
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddUserDept(string p)
        {
            try
            {
                SetDecryptString(p);

                string userID = GetDecryptParam("userID", "0");
                string deptID = GetDecryptParam("deptID", "0");

                DAL.UserDept dal = new DAL.UserDept();
                dal.ModUserDept(CurrentUserInfo.CompanyID, userID.ToNumber<int>(), deptID.ToNumber<int>(), CurrentUserInfo.UserID);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion
    }
}
