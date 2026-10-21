using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Zeospace.DeskPlus.RM.Web.Ws.Admin.Content
{
    /// <summary>
    /// ListUpDown의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ListUpDown : Base.BaseWSRM
    {
        /// <summary>
        /// 탭 순서 변경하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        
        public bool ModOrder(string p)
        {
            try
            {
                SetDecryptString(p);
                string sortList = GetDecryptParam("sortList", ""); //tab 리스트            
                sortList = sortList.TrimEnd(',');

                if (sortList.Length > 0)
                {
                    DAL.List dal = new DAL.List();
                    return dal.ModOrder(CurrentUserInfo.CompanyID, sortList);
                }

                return false;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModOrder : " + ex.Message);
                throw;
            }
        }
    }
}
