using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.WebHard.BE;

namespace Zeospace.DeskPlus.WebHard.Web.Ws.Folder
{
    /// <summary>
    /// Tree의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Tree : Base.BaseWSWebHard
    {

        #region GET()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-10-18
        /// 작성목적 : 하위폴더 갯수 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetSubFolderCount(string p)
        {
            SetDecryptString(p);
            int FolderID = GetDecryptParam("folderID", "0").ToNumber<int>();

            DAL.Folder dal = new DAL.Folder();
            return dal.GetSubFolderCount(CurrentUserInfo.CompanyID, FolderID);
        }


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-01-12
        /// 작성목적 : 폴더별 사용자 정보 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetFolderUser(string p)
        {
            SetDecryptString(p);

            int folderID = GetDecryptParam("folderID", "0").ToNumber<Int32>();
            string types = GetDecryptParam("types", "P");

            DAL.FolderUser dal = new DAL.FolderUser();
            List<FolderUserInfo> al = dal.GetList(CurrentUserInfo.CompanyID, folderID, CurrentUserInfo.UserID, types);

            foreach (BE.FolderUserInfo info in al)
            {
                return info.ListType + ":" + info.PageSize;
            }

            return "0:10";
        }


        #endregion 
    }
}
