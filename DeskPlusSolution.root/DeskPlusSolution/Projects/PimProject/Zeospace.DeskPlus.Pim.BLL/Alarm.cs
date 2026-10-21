using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Xml;
using Zeospace.DeskPlus.Common.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Web;

namespace Zeospace.DeskPlus.Pim.BLL
{
    public  class Alarm
    {
        public Alarm()
        {

        }


        #region 알람

         


        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 알람 보내기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-06-24
        /// </summary>
        /// <param name="_listInfo">The _list info.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <param name="_receiverID">The _receiver ID.</param>
        /// <returns></returns>
        public bool SendAlarm(BE.ScheduleInfo _info, int _listID, int _receiverID)
        {
            Zeospace.DeskPlus.Common.BLL.Alarm biz = new Common.BLL.Alarm();

            AlarmInfo alarmInfo = new AlarmInfo();
            alarmInfo.CompanyID = _info.CompanyID;
            alarmInfo.SenderID = _info.UserID;
            alarmInfo.SenderName = _info.UserName;
            alarmInfo.SenderMail = "";
            alarmInfo.SenderPhone = "";
            alarmInfo.ReceiverID = _receiverID;
            alarmInfo.ReceiverName = "";
            alarmInfo.ReceiverMail = "";
            alarmInfo.ReceiverPhone = "";
            alarmInfo.Status = "";
            alarmInfo.SendDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.Subject = _info.Title.ToReverseXSS();
            alarmInfo.Content = _info.AlarmContent.ToReverseXSS();

            string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
            string fullDomain = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
            string alarmTargetURL = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "AlarmTargetURL", "");
            alarmTargetURL = string.Format(alarmTargetURL, baseHttp, fullDomain);

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("CompanyID", _info.CompanyID.ToString());
            paraDic.Add("DisplayID", _info.DisplayID.ToString());

            HttpContext context = HttpContext.Current;
            string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
            alarmTargetURL = alarmTargetURL + "?p=" + encparam;

            alarmInfo.TargetUrl = alarmTargetURL;
            alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.WriteDate = System.DateTime.Now;
            alarmInfo.AlarmType = 1;
            alarmInfo.KindSubject = "Pim";
            alarmInfo.ViewOption = "01";
            alarmInfo.Store = 1;
            alarmInfo.UserState = 0;

            return biz.Add(alarmInfo, AlarmEnum.SuSoft.ToString());
        }


        /// <summary>
        /// Alarms the content template.
        /// </summary>
        /// <param name="_catName">Name of the _cat.</param>
        /// <param name="_subject">The _subject.</param>
        /// <param name="_name">The _name.</param>
        /// <param name="_startTime">The _start time.</param>
        /// <param name="_deptName">Name of the _dept.</param>
        /// <param name="_preLang">The _pre lang.</param>
        /// <returns></returns>
        public string AlarmContentTemplate(string _catName, string _subject, string _name,
            string _startTime, string _deptName, string _preLang)
        {
            string contHtml = "";
            XmlDocument confirmXml = null;

            string xmlPath = FileHelper.GetServerMapPath("/DeskPlusEIP/Common/App_Data/" + _preLang + "/Board_ContentMessengerInfo.xml");
            confirmXml = new XmlDocument();
            confirmXml.Load(xmlPath);
            XmlNode tmpXmlNode = confirmXml.DocumentElement;

            contHtml = (tmpXmlNode.SelectSingleNode("/ContentInfo/ContentHTML") != null) ? System.Web.HttpUtility.HtmlDecode(tmpXmlNode.SelectSingleNode("/ContentInfo/ContentHTML").InnerText) : "";

            contHtml = contHtml.Replace("$SUBJECT$", _subject);          //문서제목
            contHtml = contHtml.Replace("$TIME$", _startTime);    //날짜

            contHtml = contHtml.Replace("$DEPTNAME$", _deptName);    //부서
            contHtml = contHtml.Replace("$CATEGORYTITLE$", _catName);    //카테고리명                
            contHtml = contHtml.Replace("$NAME$", _name);    //이름

            return contHtml;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// SMS 발송하기
        /// </summary>
        /// <param name="_listInfo">The _list info.</param>
      /*  public void SendSMSUser(BE.ListInfo _listInfo)
        {
            string sendUserIDs = GetReadUser(_listInfo.CompanyID, _listInfo.CategoryID, true);

            if (!string.IsNullOrEmpty(sendUserIDs))
            {
                Common.BLL.SMS bll = new Common.BLL.SMS();


                //전체 알람 발송
                if (sendUserIDs == "all")
                {
                    bll.SendSMS(_listInfo.CompanyID, _listInfo.WriteUserID, _listInfo.SMSLoginID,
                        _listInfo.SMSContent, true, _listInfo.SMSMobilePhone);
                }
                else //부서 및 사용자 알람 발송
                {
                    sendUserIDs = sendUserIDs.TrimEnd(',');
                    Account.DAL.User userDal = new Account.DAL.User();
                    List<string> mobileList = userDal.GetMobileByUsers(_listInfo.CompanyID, sendUserIDs, true);
                    //List<string> userList = sendUserIDs.Split(',').ToList<string>();
                    //IEnumerable<string> userDis = userList.Distinct(); //사용자 중복제거

                    foreach (string receiverMobile in mobileList)
                    {
                        //부서 및 사용자 알람 발송
                        bll.SendSMS(_listInfo.CompanyID, _listInfo.WriteUserID, _listInfo.SMSLoginID,
                        _listInfo.SMSContent, false, receiverMobile);
                    }
                }
            }
        }
        */


       
        #endregion
    }
}
