using System;
using System.Web.Services;

namespace Zeospace.DeskPlus.Report.Web.Ws.Content
{
    /// <summary>
    /// FolderOrder의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class FolderOrder : Base.BaseWSReport
    {
        /// <summary>
        /// 순서변경
        /// 심재운
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
                string sortlist = GetDecryptParam("sortlist", "");
                sortlist = sortlist.TrimEnd(',');

                if (sortlist.Length > 0)
                {
                    DAL.Folder dal = new DAL.Folder();
                    return dal.ModOrder(CurrentUserInfo.CompanyID, sortlist);
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }
    }
}