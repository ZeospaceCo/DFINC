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

namespace Zeospace.DeskPlus.Address.Web.Ws.Admin.Content
{
    /// <summary>
    /// MoveCopy의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class MoveCopy : Base.BaseWSAddress
    {

        /// <summary>
        /// 작성자 : 심재운
        /// 개인주소록 복사하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool CopyCommon(string p)
        {
            try
            {
                SetDecryptString(p);
                int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>(); //원본 카테고리
                string listID = GetDecryptParam("listID", ""); //주소록
                string types = GetDecryptParam("types", ""); //타입
                int toCategoryID = GetDecryptParam("toCategoryID", "0").ToNumber<int>();//북사할 카테고리

                if (listID.Length > 0 && types.Length > 0 && toCategoryID > 0)
                {
                    DAL.Common dal = new DAL.Common();
                    return dal.AddCopy(CurrentUserInfo.CompanyID, categoryID, toCategoryID, listID, types);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("CopyCommon : " + ex.Message);
                throw;
            }

        }


        /// <summary>        
        /// 작성자 : 심재운
        /// 용도 : 공용 주소록 이동
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool MoveCommon(string p)
        {
            try
            {
                SetDecryptString(p);
                int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>(); //원본 카테고리
                string listID = GetDecryptParam("listID", "");
                string types = GetDecryptParam("types", "");
                int toCategoryID = GetDecryptParam("toCategoryID", "0").ToNumber<int>();//북사할 카테고리

                if (listID.Length > 0 && toCategoryID > 0)
                {
                    DAL.Common dal = new DAL.Common();
                    return dal.ModMove(CurrentUserInfo.CompanyID, categoryID, toCategoryID, listID, types);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("MoveCommon : " + ex.Message);
                throw;
            }

        }
    }
}
