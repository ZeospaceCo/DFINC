using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.Approval.BE;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using ObjList = System.Collections.Generic.List<System.Object>;


namespace Zeospace.DeskPlus.Approval.Web.Ws.Folder.Dept
{
    /// <summary>
    /// CirMemo의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class CirMemo : Base.BaseWSApproval
    {
        /// <summary>
        /// 작성자 : 심재운
        /// 회람하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddCir(string p)
        {
            SetDecryptString(p);

            string childDocID = GetDecryptParam("childDocID", "");
            string userList = GetDecryptParam("userList", "");//구분$키값 (M$1,M$2)
            string contents = GetDecryptParam("contents", "");
            string menuName = GetDecryptParam("menuName", "");

            if (childDocID.Length > 0 && userList.Length > 0)
            {
                string tmpUserList = "";

                foreach (string userID in userList.Split(',')) tmpUserList += userID.Split('$')[1] + ",";
                tmpUserList = tmpUserList.TrimEnd(',');

                BLL.CirculationDocument cirBiz = new BLL.CirculationDocument();

                cirBiz.Add(CurrentUserInfo.CompanyID, childDocID, CurrentUserInfo.UserID, tmpUserList, contents, menuName);
            
                return true;
            }

            return false;
        }
    }
}
