using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;

namespace Zeospace.DeskPlus.RM.Web.Ws.Admin.Folder
{
    /// <summary>
    /// Tree의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Tree : Base.BaseWSRM
    {

        #region GET()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-07
        /// 작성목적 : 하위폴더 갯수 , 해당 폴더의 자원 갯수 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetSubFolderCount(string p)
        {
            SetDecryptString(p);
            int CategoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();

            DAL.Category dal = new DAL.Category();
            DataTableReader dtr = dal.GetSubFolderCount(CurrentUserInfo.CompanyID, CategoryID);

            string rtnVal = String.Empty;

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        rtnVal += dtr["SubFolder"].ToString().ToNumber<int>() + "^";
                        rtnVal += dtr["listCount"].ToString().ToNumber<int>();
                    }
                }
            }

            return rtnVal;
        }

        #endregion 
    }
}
