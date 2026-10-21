using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using ObjList = System.Collections.Generic.List<System.Object>;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text;

namespace Zeospace.DeskPlus.Portlet.Web.Ws.Package.SMSNew
{
    /// <summary>
    /// RecentNumber의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class RecentNumber : Base.BaseWSPortlet
    {

        /// <summary>
        /// 최신전화번호 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageList(string p)
        {
            try
            {
                SetDecryptString(p);

                Component.SMS.SuremRecentPhoneInfo be = new Component.SMS.SuremRecentPhoneInfo();
                Component.SMS.RecentPhoneDAL bll = new Component.SMS.RecentPhoneDAL();
                DataSet ds = bll.GetRecentPhone(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
                
                ObjList listobj = new ObjList();

                if (ds == null) return listobj;

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataView dv = ds.Tables[0].DefaultView;

                    for (int i = 0; i < dv.Count; i++)
                    {
                        BE.RecentNumberInfo info = new BE.RecentNumberInfo();
                        
                        info.SuremRecentPhoneID = dv[i]["SuremRecentPhoneID"].ToString().ToNumber<int>();
                        info.CompanyID = CurrentUserInfo.CompanyID;
                        info.UserID = CurrentUserInfo.UserID;
                        info.Name = dv[i]["Name"].ToString();
                        info.PhoneNumber = dv[i]["PhoneNumber"].ToString();
                        info.PhoneNumber1 = dv[i]["PhoneNumber1"].ToString();
                        info.PhoneNumber2 = dv[i]["PhoneNumber2"].ToString();
                        info.PhoneNumber3 = dv[i]["PhoneNumber3"].ToString();

                        listobj.Add(info);
                    }
                }

                return listobj;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetPageList : " + ex.Message);
                return null;
            }
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 최신전화번호 삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Del(string p)
        {
            try
            {

                SetDecryptString(p);

                string id = GetDecryptParam("id", "");

                if (id.Length > 0)
                {   
                    Component.SMS.RecentPhoneDAL bll = new Component.SMS.RecentPhoneDAL();

                    foreach (string val in id.Split(','))
                    {
                        if (val.Length > 0)
                        {
                            Component.SMS.SuremRecentPhoneInfo smsInfo = new Component.SMS.SuremRecentPhoneInfo();
                            smsInfo.CompanyID = CurrentUserInfo.CompanyID;
                            smsInfo.UserID = CurrentUserInfo.UserID;
                            smsInfo.SuremRecentPhoneID = val.ToNumber<int>();

                            bll.Del(smsInfo);
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Del : " + ex.Message);
                return false;
            }
        }

    }
}
