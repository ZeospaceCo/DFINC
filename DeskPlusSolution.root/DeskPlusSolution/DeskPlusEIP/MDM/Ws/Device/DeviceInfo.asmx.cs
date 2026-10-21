using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.MDM.Web.Ws.Device
{
    /// <summary>
    /// DeviceInfo의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class DeviceInfo : Base.BaseWSMDM
    {
        #region GET()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-02-15
        /// 작성목적 : 사용자의 UUID에 대한 정보 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetListInfo(string p)
        {
            try
            {
                SetDecryptString(p);
                int userID = GetDecryptParam("userID", "0").ToNumber<int>();
                int deptID = GetDecryptParam("deptID", "0").ToNumber<int>();
                int listID = GetDecryptParam("listID", "0").ToNumber<int>();

                DAL.List dal = new DAL.List();

                DataTableReader dtr = dal.GetListInfo(CurrentUserInfo.CompanyID, userID, deptID, listID);

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {   
                            DateTime dt;

                            string requestDateStr = "";

                            if (DateTime.TryParse(dtr["RequestDate"].ToString(), out dt))
                                requestDateStr = DateHelpers.GetCultureDate(dt, CurrentUserInfo.PreferredLanguage, true);
                            else
                                requestDateStr = string.Empty;

                            string approveDateStr = "";

                            if (DateTime.TryParse(dtr["ApproveDate"].ToString(), out dt))
                                approveDateStr = DateHelpers.GetCultureDate(dt, CurrentUserInfo.PreferredLanguage, true);
                            else
                                approveDateStr = string.Empty;

                            BE.ListInfo info = new BE.ListInfo();
                            info.AccessRight = dtr["AccessRight"].ToString();
                            info.MobileNumber = dtr["MobileNumber"].ToString();
                            info.MobileType = dtr["MobileType"].ToString();
                            info.DeviceName = dtr["DeviceName"].ToString();
                            info.RequestDateStr = requestDateStr;
                            info.ApproveDateStr = approveDateStr;
                            info.MailAlarm = dtr["MailAlarm"].ToString();
                            info.EtcAlarm = dtr["EtcAlarm"].ToString();
                            info.LoginType = dtr["LoginType"].ToString();
                            info.ActiveMobile = dtr["ActiveMobile"].ToString();
                            info.Memo = dtr["Memo"].ToString();

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

        #region ADD()
        #endregion

        #region DEL()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-01-18
        /// 작성목적 : 모바일 사용자 정보 삭제
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Del(string p)
        {
            try
            {
                SetDecryptString(p);
                int userID = GetDecryptParam("userID", "0").ToNumber<int>();
                int listID = GetDecryptParam("listID", "0").ToNumber<int>();            

                DAL.List dal = new DAL.List();
                return dal.Del(CurrentUserInfo.CompanyID, userID, listID);

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }

        #endregion

        #region MOD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-01-18
        /// 작성목적 : 모바일 사용자 정보 저장
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Confirm(string p)
        {
            try
            {
                SetDecryptString(p);
                int userID = GetDecryptParam("userID", "0").ToNumber<int>();
                int listID = GetDecryptParam("listID", "0").ToNumber<int>();
                string activeMobile = GetDecryptParam("activeMobile", "0");
                string mailAlarm = GetDecryptParam("mailAlarm", "N");
                string etcAlarm = GetDecryptParam("etcAlarm", "");      
                string loginType = GetDecryptParam("loginType", "0");
                string accessRight = GetDecryptParam("accessRight", "0");
                string memo = GetDecryptParam("memo", "");

                BE.ListInfo info = new BE.ListInfo();
                DAL.List dal = new DAL.List();

                if (listID == 0)
                {
                    info.CompanyID = CurrentUserInfo.CompanyID;
                    info.UserID = userID;
                    info.AccessRight = accessRight;
                    info.ModifyUserID = CurrentUserInfo.UserID;
                    info.ModifyUserName = CurrentUserInfo.Name;

                    return dal.Add(info);
                }
                else
                {
                    info.CompanyID = CurrentUserInfo.CompanyID;
                    info.UserID = userID;
                    info.ListID = listID;
                    info.ActiveMobile = activeMobile;
                    info.MailAlarm = mailAlarm;
                    info.EtcAlarm = etcAlarm;
                    info.LoginType = loginType;
                    info.AccessRight = accessRight;
                    info.Memo = memo;
                    info.ModifyUserID = CurrentUserInfo.UserID;
                    info.ModifyUserName = CurrentUserInfo.Name;

                    return dal.Mod(info);
                }

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-01-18
        /// 작성목적 : 모바일 사용자 정보 저장
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Mod(string p)
        {
            try
            {
                SetDecryptString(p);
                int userID = GetDecryptParam("userID", "0").ToNumber<int>();
                int listID = GetDecryptParam("listID", "0").ToNumber<int>();
                string mailAlarm = GetDecryptParam("mailAlarm", "N");
                string etcAlarm = GetDecryptParam("etcAlarm", "");
                string loginType = GetDecryptParam("loginType", "0");
                string memo = GetDecryptParam("memo", "");

                BE.ListInfo info = new BE.ListInfo();
                DAL.List dal = new DAL.List();

                
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.UserID = userID;
                info.ListID = listID;
                info.MailAlarm = mailAlarm;
                info.EtcAlarm = etcAlarm;
                info.LoginType = loginType;
                info.Memo = memo;
                info.ModifyUserID = CurrentUserInfo.UserID;
                info.ModifyUserName = CurrentUserInfo.Name;

                return dal.ModUser(info);                

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }


        #endregion 

    }
}
