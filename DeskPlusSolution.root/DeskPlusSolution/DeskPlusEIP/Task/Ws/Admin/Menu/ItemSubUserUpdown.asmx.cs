using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Zeospace.DeskPlus.Task.Web.Ws.Admin.Menu
{
    /// <summary>
    /// ItemSubUserUpdown의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ItemSubUserUpdown : Base.BaseWSTask
    {
        #region MOD
         /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-04-21
        /// 소분류 지정자 수정
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ItemSubUserOrder(string p)
        {
            try
            {
                SetDecryptString(p);
                string sortSubUserList = GetDecryptParam("sortSubUserList", ""); //소분류 사용자 값
                sortSubUserList = sortSubUserList.TrimEnd(',');

                DAL.ItemSubUser dal = new DAL.ItemSubUser();
                return dal.ItemSubUserOrder(CurrentUserInfo.CompanyID, sortSubUserList);
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
