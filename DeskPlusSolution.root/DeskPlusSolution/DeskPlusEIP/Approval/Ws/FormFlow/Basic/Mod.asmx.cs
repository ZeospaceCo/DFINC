using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections.Specialized;
using System.Text;
using System.Collections;
using System.Threading;
using System.Globalization;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Approval.Web.Ws.FormFlow.Basic
{
    /// <summary>
    /// Mod의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Mod : Base.BaseWSApproval
    {
        #region GET
        #endregion

        #region SET
        #endregion

        #region ModViewing
        /// <summary>
        /// 해당 결재 결재페이지 viewing처리
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string SetViewing(string p)
        {
            try
            {
                SetDecryptString(p);

                int documentID = GetDecryptParam("documentID", "0").ToNumber<int>();
                int sequence = GetDecryptParam("sequence", "0").ToNumber<int>();
                string approvalTag = GetDecryptParam("approvalTag", "");
                string viewing = GetDecryptParam("viewing", "");

                DAL.Sequence seqDal = new DAL.Sequence();
                return seqDal.ModViewing(CurrentUserInfo.CompanyID, documentID, sequence, approvalTag, viewing).ToString();
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 해당 결재 결재페이지 접근가능여부 확인
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetViewing(string p)
        {
            try
            {
                SetDecryptString(p);

                int documentID = GetDecryptParam("documentID", "0").ToNumber<int>();
                int sequence = GetDecryptParam("sequence", "0").ToNumber<int>();
                string approvalTag = GetDecryptParam("approvalTag", "");
                int userID = GetDecryptParam("targetUserID", "0").ToNumber<int>();

                BLL.Sequence seqBll = new BLL.Sequence();
                BE.SequenceInfo info = seqBll.GetViewing(CurrentUserInfo.CompanyID, documentID, sequence, approvalTag, userID);

                string rtnval = "";
                if (info != null)
                {
                    rtnval = (info.IsViewing.Equals("N")) ? "OK" : GetLangName(info.DeptBase, info.DeptKo, info.DeptEn, info.DeptJa, info.DeptFr, info.DeptZhcn, info.DeptZhtw, PreLang) + "/" + GetLangName(info.TitleBase, info.TitleKo, info.TitleEn, info.TitleJa, info.TitleFr, info.TitleZhcn, info.TitleZhtw, PreLang) + "/" + GetLangName(info.NameBase, info.NameKo, info.NameEn, info.NameJa, info.NameFr, info.NameZhcn, info.NameZhtw, PreLang);
                }
                return rtnval;
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
