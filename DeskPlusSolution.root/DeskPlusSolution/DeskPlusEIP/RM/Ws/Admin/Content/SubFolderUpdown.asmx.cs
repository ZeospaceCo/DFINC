using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.RM.Web.Ws.Admin.Content
{
    /// <summary>
    /// SubFolderUpdown의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class SubFolderUpdown : Base.BaseWSRM
    {

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 자원폴더 순서 변경하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool SubFolderOrderCategory(string p)
        {
            try
            {
                SetDecryptString(p);
                string sortCatList = GetDecryptParam("sortCatList", ""); //카테고리값
                sortCatList = sortCatList.TrimEnd(',');
                
                if (sortCatList.Length > 0)
                {
                    DAL.Category dal = new DAL.Category();
                    return dal.ModOrderCategory(CurrentUserInfo.CompanyID, sortCatList);
                }

                return false;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("SubFolderOrderCategory : " + ex.Message);
                throw;
            }
        }
    }
}
