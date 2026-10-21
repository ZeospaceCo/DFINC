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
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.Approval.Web.Ws.FormFlow.Receive
{
    /// <summary>
    /// Add의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Add : Base.BaseWSApproval
    {
        #region 현재 결재순서 여부 확인
        /// <summary>
        /// 현재 결재순서 여부 확인
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetIsMyTurn(string p)
        {
            SetDecryptString(p);
            int ParentDocumentID = GetDecryptParam("documentID", "0").ToNumber<int>();
            int Sequence = GetDecryptParam("sequence", "0").ToNumber<int>();
            string AccountTag = GetDecryptParam("accountTag", "");
            int TargetUserID = GetDecryptParam("targetUserID", "0").ToNumber<int>();

            string result = "N";

            DAL.CounterPartUser dal = new DAL.CounterPartUser();
            result = dal.GetIsMyTurn(CurrentUserInfo.CompanyID, ParentDocumentID, Sequence, AccountTag, TargetUserID);

            return result;
        }
        #endregion
    }
}
