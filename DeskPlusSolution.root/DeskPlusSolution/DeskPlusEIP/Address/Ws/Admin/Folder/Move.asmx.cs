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
using Zeospace.DeskPlus.Address.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Address.Web.Ws.Admin.Folder
{
    /// <summary>
    /// Move의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Move : Base.BaseWSAddress
    {

        /// <summary>        
        /// 작성자 : 심재운
        /// 용도 : 공용 주소록 이동
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool MoveCommonCategory(string p)
        {
            try
            {
                SetDecryptString(p);
                int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>(); //원본 카테고리            
                int toCategoryID = GetDecryptParam("toCategoryID", "0").ToNumber<int>();//북사할 카테고리

                if (categoryID > 0)
                {
                    DAL.CommonCategory dal = new DAL.CommonCategory();
                    return dal.ModMoveCategory(CurrentUserInfo.CompanyID, categoryID, toCategoryID);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("MoveCommonCategory : " + ex.Message);
                throw;
            }
        }
    }
}
