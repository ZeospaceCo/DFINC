using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.RM.BE;
using Zeospace.DeskPlus.RM.DAL;

namespace Zeospace.DeskPlus.RM.Web.Ws.Interface
{
    /// <summary>
    /// Add의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Add : Base.BaseWSRM
    {
        #region Get
        #endregion

        #region Set
        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2015-10-07
        /// 작성목적 : 자원관리 저장
        /// </summary>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string AddRM(string p)
        {
            try
            {
                SetDecryptString(p);

                //////////////////////////////////
                //사용 파라미터
                //////////////////////////////////
                string _title = GetDecryptParam("Title", ""); //제목 Title : 연차
                string _reason = GetDecryptParam("Reason", ""); //사유 Reason : 아픔
                string _username = GetDecryptParam("UserName", ""); //등록자이름 UserName : 관리자
                DateTime _starttime = DateTime.Parse(GetDecryptParam("StartTime", DateTime.Now.ToString("yyyy-MM-dd 09:00"))); //시작일자 StartTime : 2015-10-10
                DateTime _endtime = DateTime.Parse(GetDecryptParam("EndTime", _starttime.ToString("yyyy-MM-dd 18:00"))); //종료일자 EndTime : 2015-10-10
                string _starthalf = GetDecryptParam("StartHalf", ""); //시작반차 StartHalf : 0,1
                string _endhalf = GetDecryptParam("EndHalf", ""); //종료반차 EndHalf : 0,1
                int _userid = GetDecryptParam("UserID", "").ToNumber<int>(); //등록자아이디 UserID : 1
                int _deptid = GetDecryptParam("DeptID", "").ToNumber<int>(); //등록자아이디 UserID : 1
                int _categoryid = GetDecryptParam("CategoryID", "1").ToNumber<int>(); //자원폴더 CategoryID : 3
                string _listid = GetDecryptParam("ListID", ""); //리스트아이디 ListID : 1
                int _listidnull = GetDecryptParam("ListIDNull", "0").ToNumber<int>(); // 기본값

                string _modulesend = GetDecryptParam("ModuleSend", "APP"); //발송모듈 ModuleSend : APP
                string _sendtarget = GetDecryptParam("SendTarget", "APP"); //수신모듈 SendTarget : PIM-Schedule2                
                int _docid = GetDecryptParam("DOCID", "").ToNumber<int>(); //문서번호 DOCID : 1                               
                //////////////////////////////////

                //자원관리 저장용
                BLL.Reserve bll = new BLL.Reserve();
                //자원관리 기본정보
                BE.ReserveInfo info = new BE.ReserveInfo();

                info.CompanyID = CurrentUserInfo.CompanyID; //업체아이디 : 필수
                //info.Subject = "[" + _title + "] " + _username + " : " + _reason; //제목 : 필수
                info.Subject = "[" + _title + "] " + _username; //제목 : 필수
                info.UserImg = ""; //구분 : 미사용
                info.Content = ""; //내용 : 미사용

                info.WriteUserID = _userid; //등록자아이디 : 필수
                info.DeptID = _deptid;
                info.WriteUserName = _username; //등록자이름 : 필수
                info.WriteDateYM = FileHelper.GetYearMonth(); //등록년월 : 필수
                info.WriteDate = System.DateTime.Now; //등록일자 : 필수

                info.ModifyUserID = _userid; //수정자아이디 : 필수
                info.ModifyUserName = _username; //수정자이름 : 필수
                info.ModifyDate = System.DateTime.Now; //수정일자 : 필수

                info.PreLang = PreLang; //다국어 : 필수

                info.ListID = _listidnull; //리스트아이디 : 기본값설정

                // 자원리스트 가져오기
                DAL.List dalList = new DAL.List();
                DataTableReader drList = dalList.GetList(CurrentUserInfo.CompanyID, _categoryid);                

                //자원리스트의 연차구분코드값과 자원항목코드값을 비교하여 자원코드값 설정
                while (drList.Read())
                {
                    if (_listid == drList.GetString(3))
                    {
                        info.ListID = drList.GetInt32(2);
                    }
                }
                
                info.Qty = 1; //수량 : 기본값 1

                //사용자 정보 가져오기
                Zeospace.DeskPlus.Account.DAL.User dalUser = new Account.DAL.User();
                var userList = dalUser.GetUserInfo(CurrentUserInfo.CompanyID, info.WriteUserID, true, true);

                foreach (var uInfo in userList)
                {
                    //SMS 용도
                    info.SMSLoginID = CurrentUserInfo.LogInID;
                    //메일 용도
                    if (uInfo.MailID.ToNull().Length > 0 && uInfo.MailDomain.ToNull().Length > 0)
                    {
                        info.Mail = uInfo.MailID + "@" + uInfo.MailDomain;
                    }
                    else
                    {
                        info.Mail = "";
                    }
                }

                info.Usable = "1";

                info.IsGuard = "0";

                info.StartDate = _starttime; //시작시간 : 필수
                info.EndDate = _endtime; //종료시간 : 필수   

                info.StartDate = DateTime.Parse(info.StartDate.ToString("yyyy-MM-dd 09:00")); //시작시간 출근시간 설정 : 필수
                info.EndDate = DateTime.Parse(info.EndDate.ToString("yyyy-MM-dd 18:00")); //종료시간 퇴근시간 설정 : 필수

                //시작반차           
                if (_starthalf == "1" || _endhalf == "1")
                {
                    info.EndDate = DateTime.Parse(info.EndDate.ToString("yyyy-MM-dd 14:00"));
                    //시작반차 14시부터                    
                    //info.Subject = "[오전반차] " + _username + " : " + _reason; //제목 : 필수
                    info.Subject = "[오전반차] " + _username;
                }
                //종료반차
                if (_starthalf == "2" || _endhalf == "2")
                {
                    //종료반차 14시까지                    
                    info.StartDate = DateTime.Parse(info.StartDate.ToString("yyyy-MM-dd 14:00"));
                    //info.Subject = "[오후반차] " + _username + " : " + _reason; //제목 : 필수
                    info.Subject = "[오후반차] " + _username;
                }

                //시작시간이 더 큰 경우
                if (info.StartDate >= info.EndDate)
                {   //종료시간을 시작시간 30분 후로 설정
                    info.EndDate = info.StartDate.AddHours(30);
                }

                TimeSpan gap = info.EndDate.Subtract(info.StartDate);
                info.DifferMinite = gap.TotalMinutes.ToString().ToNumber<int>();

                info.ParentReservationID = 0; //자원관리 부모 아이디 : 기본값설정

                //자원관리 저장
                int reservationID = bll.Add(info, null);

                //로그저장
                Zeospace.DeskPlus.Common.BE.InterfaceInfo InterfaceInfo = new Zeospace.DeskPlus.Common.BE.InterfaceInfo();
                Zeospace.DeskPlus.Common.BLL.Interface InterfaceBLL = new Zeospace.DeskPlus.Common.BLL.Interface();
                InterfaceInfo.CompanyID = CurrentUserInfo.CompanyID; //업체아이디 : 필수
                InterfaceInfo.ModuleSend = _modulesend; //발송모듈 : 필수
                InterfaceInfo.SendTarget = _sendtarget; //수신모듈 : 필수
                InterfaceInfo.SendID = _docid; //전자결재아이디 : 필수
                InterfaceInfo.WriteUserID = _userid; //등록자아이디 : 필수
                InterfaceInfo.WriteUserName = _username; //등록자이름 : 필수                          
                               
                string _rtn = _docid.ToString() + "|" + reservationID.ToString();

                if (InterfaceBLL.AddLogData(InterfaceInfo, reservationID) == -1)
                {
                    _rtn = "-1";
                }

                return _rtn;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return "-1";
            }	
        }
        #endregion
    }
}
