using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Zeospace.DeskPlus.Task.Web.Ws.Admin.Menu
{
    /// <summary>
    /// ItemUserUpdown의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    //ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ItemUserUpdown : Base.BaseWSTask
    {
        #region Mod
        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-04-21
        /// 대분류 항목 수정
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ItemUserOrder(string p)
        {
            try
            {
                SetDecryptString(p);
                string sortItemList = GetDecryptParam("sortItemList", ""); //대분류 아이디값            
                sortItemList = sortItemList.TrimEnd(',');

                DAL.ItemUser dal = new DAL.ItemUser();
                return dal.ItemUserOrder(CurrentUserInfo.CompanyID, sortItemList);
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
