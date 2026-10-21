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

namespace Zeospace.DeskPlus.Approval.Web.Ws.Folder.Approve
{
    /// <summary>
    /// Move의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Move : Base.BaseWSApproval
    {
        #region Mod()

        #region 이동 - 결재문서함
        /// <summary>        
        /// 작성자 : 심재운
        /// 용도 : 완결 문서의 특정 폴더 이동하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool MoveList(string p)
        {
            try
            {
                SetDecryptString(p);
                string toFolderID = GetDecryptParam("toFolderID", "0");
                string appDocID = GetDecryptParam("appDocID", "").TrimEnd(',');

                if (appDocID.Length > 0 && toFolderID.Length > 0)
                {
                    if (appDocID.IndexOf('_') != -1)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (string tmpDoc in appDocID.Split(','))
                        {
                            sb.Append(tmpDoc.Split('_')[1]);
                            sb.Append(",");
                        }

                        appDocID = sb.ToString().TrimEnd(',');
                    }

                    toFolderID = toFolderID.Replace("ApprovalFolder_", "");
                    DAL.ApprovalDocument dal = new DAL.ApprovalDocument();
                    dal.Move(CurrentUserInfo.CompanyID, appDocID, toFolderID.ToNumber<int>());
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Move : " + ex.Message);
                throw;
            }
        }
        #endregion

        #region 이동 - 반려문서함
        /// <summary>        
        /// 작성자 : 심재운
        /// 용도 : 완결 문서의 특정 폴더 이동하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool MoveListReject(string p)
        {
            try
            {
                SetDecryptString(p);
                string toFolderID = GetDecryptParam("toFolderID", "0");
                string appDocID = GetDecryptParam("appDocID", "").TrimEnd(',');
                //Gubun-DocumentID or Gubun-ApprovalDocumentID

                if (appDocID.Length > 0 && toFolderID.Length > 0)
                {
                    string appIds = "", priIDs = "";

                    //if (appDocID.IndexOf('_') != -1)
                    //{
                    StringBuilder sbApp = new StringBuilder();
                    StringBuilder sbPri = new StringBuilder();
                    foreach (string tmpDoc in appDocID.Split(','))
                    {
                        // AppDocID = "APP-291,PRI-327";

                        if (tmpDoc.Split('-')[0].ToString().ToUpper().Equals("APP"))
                        {
                            sbApp.Append(tmpDoc.Split('-')[1]);
                            sbApp.Append(",");
                        }

                        if (tmpDoc.Split('-')[0].ToString().ToUpper().Equals("PRI"))
                        {
                            sbPri.Append(tmpDoc.Split('-')[1]);
                            sbPri.Append(",");
                        }
                    }

                    appIds = sbApp.ToString().TrimEnd(',');
                    priIDs = sbPri.ToString().TrimEnd(',');
                    //}

                    toFolderID = toFolderID.Replace("RejectFolder_", "");
                    DAL.ApprovalDocument dal = new DAL.ApprovalDocument();
                    dal.Move(CurrentUserInfo.CompanyID, appIds, priIDs, toFolderID.ToNumber<int>());
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Move : " + ex.Message);
                throw;
            }
        }
        #endregion

        #endregion
    }
}
