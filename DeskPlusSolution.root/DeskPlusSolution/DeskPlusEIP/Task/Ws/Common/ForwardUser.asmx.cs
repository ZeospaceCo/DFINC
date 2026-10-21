using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.Task.BE;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
namespace Zeospace.DeskPlus.Task.Web.Ws.Common
{
    /// <summary>
    /// ForwardUser의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ForwardUser : Base.BaseWSTask
    {

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-22
        /// 작성목적 : 전달하기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddForward(string p)
        {
            try
            {
                SetDecryptString(p);

                int listID = GetDecryptParam("listID", "").ToNumber<int>();
                string userList = GetDecryptParam("userList", "").TrimEnd(',');

                if (listID == 0 || userList.Length == 0) return true;

                BLL.Forward bll = new BLL.Forward();
                return bll.Add(CurrentUserInfo.CompanyID, listID, CurrentUserInfo.UserID, CurrentUserInfo.Name, userList, CurrentUserInfo.PreferredLanguage);

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }
    }
}
