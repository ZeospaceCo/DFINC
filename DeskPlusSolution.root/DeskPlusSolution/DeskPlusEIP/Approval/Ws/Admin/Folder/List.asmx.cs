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
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Approval.Web.Ws.Admin.Folder
{
    /// <summary>
    /// List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSApproval
    {
        #region GET
        /// <summary>
        /// 특정폴더 권한리스트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetFolderAuthList(string p)
        {
            if (IsAdmin == false) return null;

            SetDecryptString(p);

            int folderID = GetDecryptParam("folderID", "0").ToNumber<int>();

            DAL.FolderAuthority faDal = new DAL.FolderAuthority();
            DataSet ds = faDal.GetFolderAuthList(CurrentUserInfo.CompanyID, folderID, "dsList");

            List<object> uobj = new List<object>();
            foreach (DataRow dr in ds.Tables["dsList"].Rows)
            {
                BE.FolderAuthorityInfo info = new BE.FolderAuthorityInfo();
                info.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();
                info.AuthName = this.GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(), dr["NameFr"].ToString(), dr["NameZhcn"].ToString(), dr["NameZhtw"].ToString(), PreLang);
                info.FolderAuthID = dr["FolderAuthID"].ToString().ToNumber<int>();

                uobj.Add(info);
            }
            return uobj;
        }
        #endregion

        #region SET
        /// <summary>
        /// 특정폴더 삭제
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetDel(string p)
        {
            if (IsAdmin == false) return "";

            SetDecryptString(p);

            int folderID = GetDecryptParam("folderID", "0").ToNumber<int>();

            //하위폴더
            DAL.Folder fDal = new DAL.Folder();
            List<BE.FolderInfo> list = fDal.GetSubFolderList(CurrentUserInfo.CompanyID, folderID, false);
            string isSub = (list.Count > 0) ? "1" : "0";

            //포함양식
            DAL.Form foDal = new DAL.Form();
            List<BE.FormInfo> folist = foDal.GetFormByFolderID(CurrentUserInfo.CompanyID, folderID);
            string isForm = (folist.Count > 0) ? "1" : "0";

            string rtnval = "F";
            if (isSub.Equals("0") && isForm.Equals("0"))
            {
                rtnval = (fDal.Del(CurrentUserInfo.CompanyID, folderID)) ? "T" : "F";
            }

            rtnval += "-" + isSub + "-" + isForm;       //성공여부-하위폴더여부-소속양식여부
            return rtnval;
        }

        /// <summary>
        /// 권한추가
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddAuthList(string p)
        {
            if (IsAdmin == false) return false;

            SetDecryptString(p);
            int folderID = GetDecryptParam("folderID", "0").ToNumber<int>();
            string deptIDlist = GetDecryptParam("id", "").TrimEnd('|');
            int isSub = GetDecryptParam("isSub", "0").ToNumber<int>();

            BLL.FoldersAuthority faBll = new BLL.FoldersAuthority();

            return faBll.Add(CurrentUserInfo.CompanyID, folderID, deptIDlist, isSub);
        }

        /// <summary>
        /// 권한삭제
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool GetAuthDel(string p)
        {
            if (IsAdmin == false) return false;

            SetDecryptString(p);
            int authID = GetDecryptParam("authID", "0").ToNumber<int>();

            DAL.FolderAuthority faDal = new DAL.FolderAuthority();
            return faDal.Del(CurrentUserInfo.CompanyID, authID);
        }
        #endregion
    }
}
