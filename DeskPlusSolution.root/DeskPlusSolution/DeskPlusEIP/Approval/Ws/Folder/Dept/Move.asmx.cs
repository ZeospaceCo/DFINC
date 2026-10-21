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

        #region 이동
        /// <summary>        
        /// 작성자 : 심재운
        /// 용도 : 부서별 문서의 특정 폴더 이동하기
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
                int toFolderID = GetDecryptParam("toFolderID", "0").ToNumber<int>();
                string docID = GetDecryptParam("docID", "").TrimEnd(',');

                if (docID.Length > 0 && toFolderID > 0)
                {
                    DAL.PublicDocument dal = new DAL.PublicDocument();
                    dal.Move(CurrentUserInfo.CompanyID, docID, toFolderID);
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
