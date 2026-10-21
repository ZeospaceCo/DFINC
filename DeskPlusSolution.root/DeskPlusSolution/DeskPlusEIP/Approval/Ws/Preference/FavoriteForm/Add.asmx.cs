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
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Approval.Web.Ws.Preference.FavoriteForm
{
    /// <summary>
    /// Add의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Add : Base.BaseWSApproval
    {

        #region GET

        /// <summary>
        /// 양식리스트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetList(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";		            //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.Preference.FavoriteForm.Add).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            DAL.FavoriteForm fDal = new DAL.FavoriteForm();
            DataTableReader dtr = fDal.GetFavoriteFormList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
            List<object> uobj = new List<object>();
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.FavoriteFormInfo info = new BE.FavoriteFormInfo();
                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.FavoriteFormID = dtr["FavoriteFormID"].ToString().ToNumber<int>();
                        info.FormID = dtr["FormID"].ToString().ToNumber<int>();
                        info.FormBase = this.GetLangName(
                                        dtr["FormBase"].ToString().ToNull(), dtr["FormKo"].ToString().ToNull(), dtr["FormEn"].ToString().ToNull()
                                        , dtr["FormJa"].ToString().ToNull(), dtr["FormFr"].ToString().ToNull(), dtr["FormZhcn"].ToString().ToNull()
                                        , dtr["FormZhtw"].ToString().ToNull(), PreLang);
                        info.NameBase = this.GetLangName(
                                        dtr["FolderBase"].ToString().ToNull(), dtr["FolderKo"].ToString().ToNull(), dtr["FolderEn"].ToString().ToNull()
                                        , dtr["FolderJa"].ToString().ToNull(), dtr["FolderFr"].ToString().ToNull(), dtr["FolderZhcn"].ToString().ToNull()
                                        , dtr["FolderZhtw"].ToString().ToNull(), PreLang);
                        info.PathNameBase = this.GetLangName(
                                        dtr["PathBase"].ToString().ToNull(), dtr["PathKo"].ToString().ToNull(), dtr["PathEn"].ToString().ToNull()
                                        , dtr["PathJa"].ToString().ToNull(), dtr["PathFr"].ToString().ToNull(), dtr["PathZhcn"].ToString().ToNull()
                                        , dtr["PathZhtw"].ToString().ToNull(), PreLang);

                        uobj.Add(info);
                    }
                }
            }
            return uobj;
        }

        #endregion
    }
}
