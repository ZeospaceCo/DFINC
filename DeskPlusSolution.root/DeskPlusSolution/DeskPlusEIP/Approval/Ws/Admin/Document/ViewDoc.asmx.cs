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


namespace Zeospace.DeskPlus.Approval.Web.Ws.Admin.Document
{
    /// <summary>
    /// ViewDoc 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ViewDoc : Base.BaseWSApproval
    {

        #region 삭제

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-11-13
        /// 작성목적 : 연관되어 있는 완결 문서 삭제하기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelDoc(string p)
        {
            try
            {
                if (IsAdmin == false) return false;

                SetDecryptString(p);
                string docID = GetDecryptParam("docID", "");

                BLL.Document bizDoc = new BLL.Document();
                bizDoc.DelDoc(CurrentUserInfo.CompanyID, docID.ToNumber<int>(), true);

                return true;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }

        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-11-13
        /// 작성목적 : 해당 완결 문서 삭제하기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelDocID(string p)
        {
            try
            {
                if (IsAdmin == false) return false;

                SetDecryptString(p);
                string docID = GetDecryptParam("docID", "");

                BLL.Document bizDoc = new BLL.Document();
                bizDoc.DelDocID(CurrentUserInfo.CompanyID, docID.ToNumber<int>(), true);

                return true;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }

        }

        #endregion

        #region 문서폐기

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-11-26
        /// 작성목적 : 문서 폐기하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DisuseDoc(string p)
        {
            try
            {
                if (IsAdmin == false) return false;

                SetDecryptString(p);
                string docID = GetDecryptParam("docID", "");

                DAL.Document bizDoc = new DAL.Document();

                int tmpCompanyID = (CurrentUserInfo.CompanyID.ToString() + "000").ToNumber<int>();
                return bizDoc.DisuseDoc(CurrentUserInfo.CompanyID, tmpCompanyID, docID.ToNumber<int>(), CurrentUserInfo.UserID);
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
