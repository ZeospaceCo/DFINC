using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Visit.Web.Ws.Content
{
    /// <summary>
    /// Add의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Add : Base.BaseWSVisit
    {

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-12
        /// 작성목적 : 기본 공유자 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetShareList(string p)
        {
            this.ResourceName = "Zeospace.DeskPlus.Visit.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Visit.Web.Ws.Content.Add).Assembly;	//어셈블리 선언
            this.GetResourceManager();

            try
            {
                SetDecryptString(p);
                int reserveID = GetDecryptParam("reserveID", "0").ToNumber<int>();

                DAL.Share dal = new DAL.Share();

                DataTableReader dtr = dal.GetShare(CurrentUserInfo.CompanyID);

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.ShareInfo info = new BE.ShareInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.Title = this.GetLangName(
                                       dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                       dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                       dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                       dtr["NameZhtw"].ToString().ToNull(), PreLang);

                            if (dtr["Type"].ToString() == "M")
                            {
                                info.Gubun = resMng.GetString("ShareType2");
                            }
                            else
                            {
                                info.Gubun = resMng.GetString("ShareType1");
                            }

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
    }
}
