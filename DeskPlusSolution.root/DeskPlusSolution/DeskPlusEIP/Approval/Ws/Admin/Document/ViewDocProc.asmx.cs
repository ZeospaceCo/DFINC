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
    /// ViewDocProc 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ViewDocProc : Base.BaseWSApproval
    {
        #region 삭제

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-11-13
        /// 작성목적 : 진행중인 문서 삭제하기 (groupID)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelDocProcess(string p)
        {
            try
            {
                if (IsAdmin == false) return false;

                SetDecryptString(p);
                string docID = GetDecryptParam("docID", "");

                if (docID.Length > 0)
                {
                    BLL.DocumentProcess biz = new BLL.DocumentProcess();
                    DAL.DocumentProcess dal = new DAL.DocumentProcess();

                    //groupid 로 문서 가져오기
                    List<BE.DocumentProcessInfo> list = dal.GetDocProcessByGroupID(CurrentUserInfo.CompanyID, docID.ToNumber<int>());
                    biz.DelDocProcess(list, true);
                }

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
        /// 작성목적 : 해당 진행중인 문서 삭제하기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelDocIDProcess(string p)
        {
            try
            {
                if (IsAdmin == false) return false;

                SetDecryptString(p);
                string docID = GetDecryptParam("docID", "");

                if (docID.Length > 0)
                {
                    BLL.DocumentProcess biz = new BLL.DocumentProcess();
                    DAL.DocumentProcess dal = new DAL.DocumentProcess();

                    //해당 문서 정보만 가져오기
                    List<BE.DocumentProcessInfo> list = dal.GetDocument(CurrentUserInfo.CompanyID, docID.ToNumber<int>());
                    biz.DelDocProcess(list, true);
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }

        }

        #endregion

        #region 문서열람초기화
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool UpdViewing(string p)
        {
            try
            {
                if (IsAdmin == false) return false;

                SetDecryptString(p);
                int docID = GetDecryptParam("docID", "0").ToNumber<int>();

                bool rtnval = false;
                if (docID > 0)
                {
                    DAL.Sequence dal = new DAL.Sequence();
                    rtnval = dal.ModViewingForce(CurrentUserInfo.CompanyID, docID);
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
