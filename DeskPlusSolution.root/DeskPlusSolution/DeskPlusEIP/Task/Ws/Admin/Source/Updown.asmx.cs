using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Zeospace.DeskPlus.Task.Web.Ws.Admin.Source
{
    /// <summary>
    /// Updown의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Updown : Base.BaseWSTask
    {
        #region MOD
        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-05-06
        /// 출처 순서변경
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool SourceOrder(string p)
        {
            try
            {

                SetDecryptString(p);
                string sourceList = GetDecryptParam("sourceList", ""); //대분류 아이디값            
                sourceList = sourceList.TrimEnd(',');

                DAL.Source dal = new DAL.Source();
                return dal.SourceOrder(CurrentUserInfo.CompanyID, sourceList);

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
