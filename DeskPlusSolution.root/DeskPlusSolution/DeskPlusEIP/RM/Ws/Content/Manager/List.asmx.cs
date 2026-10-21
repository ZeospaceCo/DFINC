using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.RM.Web.Ws.Content.Manager
{
    /// <summary>
    /// List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSRM
    {
        #region GET()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2017-03-08
        /// 작성목적 : 리스트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetList(string p)
        {
            this.ResourceName = "Zeospace.DeskPlus.RM.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.RM.Web.Ws.Content.Manager.List).Assembly;	//어셈블리 선언
            this.GetResourceManager();

            try
            {
                SetDecryptString(p);

                DAL.List dal = new DAL.List();
                DataTableReader dtr = dal.GetManagerList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.RM.BE.ListInfo info = new BE.ListInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.CategoryID = dtr["CategoryID"].ToString().ToNumber<int>();
                            info.ListID = dtr["ListID"].ToString().ToNumber<int>();
                            info.FolderName = this.GetLangName(dtr["CNameBase"].ToString(), dtr["CNameKo"].ToString(),
                                                                dtr["CNameEn"].ToString(), dtr["CNameJa"].ToString(), dtr["CNameFr"].ToString(),
                                                                dtr["CNameZhcn"].ToString(), dtr["CNameZhtw"].ToString(), PreLang).ToReverseXSS();
                            info.Title = dtr["Title"].ToString().ToReverseXSS();
                            info.Qty = dtr["Qty"].ToString().ToNumber<int>();
                            info.UsableQty = dtr["UsableQty"].ToString().ToNumber<int>();
                            info.Authority = (dtr["Authority"].ToString() == "0") ? resMng.GetString("No") : resMng.GetString("Yes");                             
                             
                            listobj.Add(info);

                        }
                    }
                }

                return listobj;
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
