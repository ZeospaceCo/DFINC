using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using Zeospace.DeskPlus.MDM.BE;
using System.Xml;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;


namespace Zeospace.DeskPlus.MDM.Web.Ws.Portal
{
    /// <summary>
    /// List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSMDM
    {
           
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2017-12-17
        /// 작성목적 : 리스트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetList(string p)
        {       
            try
            {
                #region 다국어 리소스 로드/Title 정보
                this.ResourceName = "Zeospace.DeskPlus.MDM.Web.Resource.GlobalString.string";			//리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.MDM.Web.Portal.List).Assembly;	//어셈블리 선언
                #endregion

                SetDecryptString(p);

                DAL.Portal dal = new DAL.Portal();
                DataTableReader dtr = dal.GetList(CurrentUserInfo.CompanyID);

                string tmp1 = "";
                string tmp2 = "";
                string tmp3 = "";

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.PortalInfo info = new BE.PortalInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.PortalID = dtr["PortalID"].ToString().ToNumber<int>();
                            info.GubunID = dtr["GubunID"].ToString().ToNumber<int>();                           
                            info.Orders = dtr["Orders"].ToString().ToNumber<int>();

                            tmp1 = "";
                            tmp2 = "";
                            tmp3 = "";

                            if (info.Orders == 0)
                            {
                                tmp1 = "selected='True'";
                            }
                            else if (info.Orders == 1)
                            {
                                tmp2 = "selected='True'";
                            }
                            else if (info.Orders == 2)
                            {
                                tmp3 = "selected='True'";
                            }

                            string tmpStr = "<select id='cPortalID_" + info.PortalID + "' class='searchselect form-control'>";
                            tmpStr += "<option value='0' " + tmp1 + ">" + GetResource("NoPortlet") + "</option>";
                            tmpStr += "<option value='1' " + tmp2 + ">" + GetResource("Top") + "</option>";
                            tmpStr += "<option value='2' " + tmp3 + ">" + GetResource("Bottom") + "</option></select>";


                            info.OrderStr = tmpStr;
                            info.NameBase = this.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(),
                                                                dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(),
                                                                dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang);

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

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2017-12-18
        /// 작성목적 : 메인구성 저장 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Mod(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            string portalID = GetDecryptParam("portalID", "");
            string orderValue = GetDecryptParam("orderValue", "");

            try
            {
                if (portalID.Length > 0)
                {
                    string[] ArrID = portalID.Split(',');
                    string[] ArrValue = orderValue.Split(',');

                    DAL.Portal dal = new DAL.Portal();
                    for (int i = 0; i < ArrID.Length; i++)
                    {
                        dal.Mod(CurrentUserInfo.CompanyID, ArrID[i].ToNumber<int>(), ArrValue[i].ToNumber<int>());
                    }
                }

                return true;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Mod : " + ex);
                return false;
            }
        }
    }
}
