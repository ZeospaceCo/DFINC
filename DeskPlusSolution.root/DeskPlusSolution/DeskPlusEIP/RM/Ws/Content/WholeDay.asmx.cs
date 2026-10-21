using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;


namespace Zeospace.DeskPlus.RM.Web.Ws.Content
{
    /// <summary>
    /// WholeDay의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class WholeDay : Base.BaseWSRM
    {
        #region MOD()
        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-02-24
        /// 작성목적 : 승인 / 거부 처리
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int ModUsable(string p)
        {
            SetDecryptString(p);
            string usable = GetDecryptParam("usable", "0");
            int reservationID = GetDecryptParam("reservationID", "0").ToNumber<int>();

            bool rtnVal = false;
            DAL.Reverse dal = new DAL.Reverse();
            rtnVal = dal.Mod(CurrentUserInfo.CompanyID, reservationID, usable);

            if (rtnVal)
            {
                //알람발송
                SendAlarm(reservationID, usable);

                // 대기자원 카운트 가져오기
                DAL.Reverse Rdal = new DAL.Reverse();
                return Rdal.GetPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "N", "0", "", "", "", "");
            }
            else
            {
                return -1;
            }            
        }


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2017-03-10
        /// 작성목적 : 범례 상태값 설정하기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddModUsable(string p)
        {
            try
            {
                SetDecryptString(p);
                string usable = GetDecryptParam("usable", "");

                BE.UsableInfo info = new BE.UsableInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.UserID = CurrentUserInfo.UserID;
                info.Usable = usable;

                DAL.Usable dal = new DAL.Usable();
                dal.AddMod(info);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion

        #region ADD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2017-03-14
        /// 작성목적 : 승인 요청 알람
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool SendRequest(string p)
        {
            try
            {
                SetDecryptString(p);
                int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
                int listID = GetDecryptParam("listID", "0").ToNumber<int>();
                string tmpdate = GetDecryptParam("tmpDate", "0");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");

                DAL.Reverse dal = new DAL.Reverse();
                DataTableReader dtr = dal.GetWholeUsable(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, categoryID, listID, tmpdate, searchField, searchValue);

                BE.ReserveInfo info = new BE.ReserveInfo();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            info.CompanyID = CurrentUserInfo.CompanyID;
                            info.Subject = dtr["Subject"].ToString().ToReverseXSS();
                            info.WriteUserID = CurrentUserInfo.UserID;
                            info.WriteUserName = CurrentUserInfo.Name;
                            info.ModifyUserID = CurrentUserInfo.UserID;
                            info.ModifyUserName = CurrentUserInfo.Name;
                            info.PreLang = PreLang;
                            info.SMSLoginID = CurrentUserInfo.LogInID;
                            info.Mail = CurrentUserInfo.Name + "<" + CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain + ">";
                            info.ListID = dtr["ListID"].ToString().ToNumber<int>();
                            info.Usable = "0";
                            info.StartDate = Convert.ToDateTime(dtr["StartDate"].ToString());
                            info.Title = dtr["Title"].ToString();

                            BLL.Reserve bll = new BLL.Reserve();
                            bll.SendAlarm(info, dtr["ReservationID"].ToString().ToNumber<int>(), info.StartDate, "REQUEST");
                        }
                    }
                }              

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }

        }

        #endregion 

        /// <summary>
        /// 승인or거부 알람발송
        /// </summary>
        /// <param name="_reservationID"></param>
        /// <param name="_usable"></param>
        private void SendAlarm(int _reservationID, string _usable)
        {
            DAL.Reverse dal = new DAL.Reverse();
            BLL.Reserve bll = new BLL.Reserve();
            string Section = (_usable == "1") ? "APPROVE" : "REJECT";


            //예약정보 가져오기
            DataTableReader dtr = dal.GetList(CurrentUserInfo.CompanyID, _reservationID);
            BE.ReserveInfo info;
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        info = new BE.ReserveInfo();
                        info.CompanyID = CurrentUserInfo.CompanyID;
                        info.ReservationID = dtr["ReservationID"].ToString().ToNumber<int>();
                        info.ParentReservationID = dtr["ParentReservationID"].ToString().ToNumber<int>();
                        info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();
                        info.WriteUserName = dtr["WriteUserName"].ToString();
                        info.ModifyUserID = CurrentUserInfo.UserID;
                        info.ModifyUserName = CurrentUserInfo.Name;
                        info.Usable = _usable;
                        info.StartDate = Convert.ToDateTime(dtr["StartDate"].ToString());
                        info.Subject = dtr["Subject"].ToString().ToReverseXSS();
                        info.PreLang = CurrentUserInfo.PreferredLanguage;
                        info.Mail = CurrentUserInfo.Name + "<" + CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain + ">";
                        info.Title = dtr["Title"].ToString();

                        bll.SendAlarm(info, info.ParentReservationID, info.StartDate, Section);
                    }
                }
            }
        }
    }
}
