using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Pim.BE;
using Zeospace.DeskPlus.Pim.DAL;

namespace Zeospace.DeskPlus.Pim.Web.Ws.Interface
{
    /// <summary>
    /// Add의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Add : Base.BaseWSPim
    {
        #region Get
        #endregion

        #region Set
        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2015-10-06
        /// 작성목적 : 오늘의일정 저장
        /// </summary>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string AddSchedule(string p)
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
                int _deptid = GetDecryptParam("DeptID", "").ToNumber<int>(); //등록자부서아이디 DeptID : 1
                string _opentype = GetDecryptParam("OpenType", "1"); //공개설정 OpenType : 개인일정 0 공유일정 1 전체일정 2 부서일정 3

                string _modulesend = GetDecryptParam("ModuleSend", "APP"); //발송모듈 ModuleSend : APP
                string _sendtarget = GetDecryptParam("SendTarget", "APP"); //수신모듈 SendTarget : PIM-Schedule2                
                int _docid = GetDecryptParam("DOCID", "").ToNumber<int>(); //문서번호 DOCID : 1
                //////////////////////////////////

                BE.ScheduleInfo info = new ScheduleInfo(); //오늘의일정 저장용 리스트
                info.CompanyID = CurrentUserInfo.CompanyID; //업체아이디 : 필수
                info.ToDoID = 0; //오늘의할일 일련번호 : 미사용
                info.Type = "I"; //일정구분 : 기본값 I
                //info.Title = "[" + _title + "] " + _username + " : " + _reason; //제목 : 필수
                info.Title = "[" + _title + "] " + _username; //제목 : 필수
                info.Content = ""; //내용 : 미사용

                //시간 저장시 근무시간 기준으로 저장
                info.StartTime = _starttime; //시작시간 : 필수
                info.EndTime = _endtime; //종료시간 : 필수                 
                info.StartTime = DateTime.Parse(info.StartTime.ToString("yyyy-MM-dd 09:00")); //시작시간 출근시간 설정 : 필수
                info.EndTime = DateTime.Parse(info.EndTime.ToString("yyyy-MM-dd 18:00")); //종료시간 퇴근시간 설정 : 필수

                //시작반차           
                if (_starthalf == "1" || _endhalf == "1")
                {
                    //시작반차 14시부터
                    info.StartTime = DateTime.Parse(info.StartTime.ToString("yyyy-MM-dd 14:00"));
                    //info.Title = "[오전반차] " + _username + " : " + _reason; //제목 : 필수
                    info.Title = "[오전반차] " + _username; //제목 : 필수
                }
                //종료반차
                if (_starthalf == "2" || _endhalf == "2")
                {
                    //종료반차 14시까지
                    info.EndTime = DateTime.Parse(info.EndTime.ToString("yyyy-MM-dd 14:00"));
                    //info.Title = "[오후반차] " + _username + " : " + _reason; //제목 : 필수
                    info.Title = "[오후반차] " + _username; //제목 : 필수
                }

                //시작시간이 더 큰 경우
                if (info.StartTime >= info.EndTime)
                {   //종료시간을 시작시간 30분 후로 설정
                    info.EndTime = info.StartTime.AddHours(30);
                }
                
                info.Alert = "N"; //알람유무 : 기본값 N               
                info.AlertType = ""; // 알람방법
                info.AlertTargetAddress = ""; // 알람연락처
                info.IsOpen = "Y"; //공개유무 : 기본값 Y
                info.UserID = _userid; //등록자아이디 : 필수
                info.DeptID = _deptid; //등록자부서아이디 : 필수
                info.OpenType = _opentype; //공개범위 : 필수 : 개인일정 "0" 공유일정 "1" 전체일정 "2" 부서일정 "3"
                info.ReservationID = 0; //자원관리 아이디 : 미사용
                info.Lunar = "N"; //음양력설정 : 기본값 N
                info.StatusID = 0; //상태값 : 미사용

                int scheduleid = 0;

                if (_starttime.ToString("yyyy-MM-dd") == _endtime.ToString("yyyy-MM-dd"))
                {
                    //하루일 경우
                    info.Repeat = "N"; //반복유무 : 기본값 N

                    DAL.Schedule biz = new DAL.Schedule(); //오늘의일정 저장용
                    scheduleid = biz.Add(info); //오늘의일정 저장 후 오늘의일정ID 값 리턴

                    BE.DisplayInfo disinfo = new DisplayInfo(); //오늘의일정 Display 저장용 리스트
                    disinfo.CompanyID = CurrentUserInfo.CompanyID; //업체아이디 : 필수
                    disinfo.StartDate = info.StartTime; //시작시간 : 필수
                    disinfo.EndDate = info.EndTime; //종료시간 : 필수
                    //시작시간이 더 큰 경우
                    if (info.StartTime == info.EndTime)
                    {
                        //종료시간을 시작시간 30분 후로 설정
                        disinfo.EndDate = disinfo.EndDate.AddMinutes(30);
                    }
                    disinfo.ScheduleID = scheduleid; //리턴받은 오늘의일정ID : 필수
                    disinfo.UserID = _userid; //등록자아이디 : 필수
                    DAL.Display bizdis = new Display(); //오늘의일정 Display 저장용
                    bizdis.AddDisplay(disinfo); //오늘의일정 Display 저장
                }
                else
                {
                    //반복일정인 경우
                    info.Repeat = "Y"; //반복유무 : 기본값 N

                    scheduleid = AddRepeat(info);
                }

                //로그저장
                Zeospace.DeskPlus.Common.BE.InterfaceInfo InterfaceInfo = new Zeospace.DeskPlus.Common.BE.InterfaceInfo();
                Zeospace.DeskPlus.Common.BLL.Interface InterfaceBLL = new Zeospace.DeskPlus.Common.BLL.Interface();
                InterfaceInfo.CompanyID = CurrentUserInfo.CompanyID; //업체아이디 : 필수
                InterfaceInfo.ModuleSend = _modulesend; //발송모듈 : 필수
                InterfaceInfo.SendTarget = _sendtarget; //수신모듈 : 필수
                InterfaceInfo.SendID = _docid; //전자결재아이디 : 필수
                InterfaceInfo.WriteUserID = _userid; //등록자아이디 : 필수
                InterfaceInfo.WriteUserName = _username; //등록자이름 : 필수
               
                string _rtn = _docid.ToString() + "|" + scheduleid.ToString();

                if (InterfaceBLL.AddLogData(InterfaceInfo, scheduleid) == -1)
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

        private int AddRepeat(Pim.BE.ScheduleInfo _info)
        {
            try
            {
                int scheduleid = 0;

                //오늘의일정 Display 저장용 리스트
                BE.DisplayInfo DisInfo = new BE.DisplayInfo();
                //오늘의일정 반복정보 저장용 리스트
                BE.RepeatScheduleInfo info = new BE.RepeatScheduleInfo();

                //오늘의일정 Display 저장용
                DAL.Display bizDis = new DAL.Display();
                //오늘의일정 반복정보 저장용
                DAL.RepeatSchedule bizRep = new DAL.RepeatSchedule();


                info.CompanyID = _info.CompanyID; //업체아이디 : 필수
                info.RepeatType = "D"; //반복타입 : 기본값 D 일반복
                info.SelectType = "2"; //반복타입 : 기본값 2 평일반복
                info.IntervalCnt = 1; //반복주기 : 기본값 1 1일반복
                info.MaxReptCnt = 0; //최대주기 : 기본값 0 
                info.Option1 = "0"; //주반복시 월요일 : 기본값 0 월
                info.Option2 = "0"; //주반복시 화요일 : 기본값 0 월
                info.Option3 = "0"; //주반복시 수요일 : 기본값 0 월
                info.Option4 = "0"; //주반복시 목요일 : 기본값 0 월
                info.Option5 = "0"; //주반복시 금요일 : 기본값 0 월
                info.Option6 = "0"; //주반복시 토요일 : 기본값 0 월
                info.Option7 = "0"; //주반복시 일요일 : 기본값 0 월

                string strS = _info.StartTime.ToString();
                string strE = _info.EndTime.ToString();

                info.StartDate = DateTime.Parse(strS); //시작시간
                info.EndDate = DateTime.Parse(strE); //종료시간

                //시작일자와 종료일자 사이일자 가져오기
                Biz.RepeatScheduleProc bizRepeatBiz = new Biz.RepeatScheduleProc(info);
                bizRepeatBiz.RepeatDate();
                List<BE.DisplayInfo> listobj = bizRepeatBiz.GetDateInfo();

                //기간이면 실행
                if (listobj.Count > 0)
                {

                    Pim.DAL.Schedule bll = new DAL.Schedule(); //오늘의일정 저장용
                    scheduleid = bll.Add(_info); //오늘의일정 저장 후 오늘의일정ID 값 리턴

                    info.ScheduleID = scheduleid;

                    //오늘의일정 Display 저장용 기간만큼 반복
                    foreach (BE.DisplayInfo _be in listobj)
                    {
                        _be.CompanyID = _info.CompanyID; //업체아이디
                        _be.ScheduleID = scheduleid; //오늘의일정아이디
                        _be.UserID = _info.UserID; //등록자아이디

                        string _strS = _info.StartTime.ToString();
                        string _strE = _info.EndTime.ToString();
                        DateTime _st = DateTime.Parse(_strS);
                        DateTime _et = DateTime.Parse(_strE);

                        if (info.StartDate.ToString("yyyy-MM-dd ") == _be.StartDate.ToString("yyyy-MM-dd "))
                        {
                            _be.StartDate = DateTime.Parse(_be.StartDate.ToString("yyyy-MM-dd ") + info.StartDate.ToString("HH:mm"));
                        }
                        else
                        {
                            _be.StartDate = DateTime.Parse(_be.StartDate.ToString("yyyy-MM-dd ") + "09:00");
                        }

                        if (info.EndDate.ToString("yyyy-MM-dd ") == _be.EndDate.ToString("yyyy-MM-dd "))
                        {
                            _be.EndDate = DateTime.Parse(_be.EndDate.ToString("yyyy-MM-dd ") + info.EndDate.ToString("HH:mm"));
                        }
                        else
                        {
                            _be.EndDate = DateTime.Parse(_be.EndDate.ToString("yyyy-MM-dd ") + "18:00");
                        }


                        bizDis.AddDisplay(_be); //오늘의일정 Display 저장
                    }

                    bizRep.AddRepeatSchedule(info); //오늘의일정 반복정보 저장                    
                }

                return scheduleid;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return -1;
            }            
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2015-10-06
        /// 작성목적 : 오늘의할일 저장
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string AddToDo(string p)
        {
            try
            {
                string _rtn = "1"; ;

                SetDecryptString(p);

                //////////////////////////////////
                //사용 파라미터
                //////////////////////////////////
                string _title = GetDecryptParam("Title", ""); //제목 Title : 연차
                string _reason = GetDecryptParam("Reason", ""); //사유 Reason : 아픔
                string _username = GetDecryptParam("UserName", ""); //등록자이름 UserName : 관리자
                DateTime _starttime = DateTime.Parse(GetDecryptParam("StartTime", DateTime.Now.ToString("yyyy-MM-dd 09:00"))); //시작일자 StartTime : 2015-10-10
                DateTime _endtime = DateTime.Parse(GetDecryptParam("EndTime", _starttime.ToString("yyyy-MM-dd 18:00"))); //종료일자 EndTime : 2014-10-10
                DateTime _timefor = DateTime.Parse(GetDecryptParam("StartTime", DateTime.Now.ToString("yyyy-MM-dd 09:00")));
                string _starthalf = GetDecryptParam("StartHalf", ""); //시작반차 StartHalf : 0,1
                string _endhalf = GetDecryptParam("EndHalf", ""); //종료반차 EndHalf : 0,1

                int _userid = GetDecryptParam("UserID", "").ToNumber<int>(); //등록자아이디 UserID : 1
                int _deptid = GetDecryptParam("DeptID", "").ToNumber<int>(); //등록자부서아이디 DeptID : 1
                int _opentype = GetDecryptParam("OpenType", "1").ToNumber<int>(); //공개설정 OpenType : 개인일정 0 공유일정 1 전체일정 2 부서일정 3

                string _modulesend = GetDecryptParam("ModuleSend", "APP"); //발송모듈 ModuleSend : APP
                string _sendtarget = GetDecryptParam("SendTarget", "APP"); //수신모듈 SendTarget : PIM-Schedule2                
                int _docid = GetDecryptParam("DOCID", "").ToNumber<int>(); //문서번호 DOCID : 1
                //////////////////////////////////


                //사이일자 구하기        
                TimeSpan ts = _endtime - _starttime;
                int differenceInDays = ts.Days;

                for (int i = 0; i <= differenceInDays; i++)
                {
                    _timefor = _starttime.AddDays(i);

                    BE.ToDoInfo info = new ToDoInfo(); //오늘의할일 저장용 리스트
                    info.CompanyID = CurrentUserInfo.CompanyID; //업체아이디 : 필수
                    info.Type = "I"; //일정구분 : 기본값 I
                    info.Status = "W"; //상태 : 기본값 W
                    info.Priority = "1"; //중요도 : 기본값 1

                    //info.Title = "[" + _title + "] " + _username + " : " + _reason; //제목 : 필수
                    info.Title = "[" + _title + "] " + _username; //제목 : 필수

                    if (_timefor.ToString("yyyy-MM-dd") == _starttime.ToString("yyyy-MM-dd"))
                    {
                        //시작반차           
                        if (_starthalf == "1" || _endhalf == "1")
                        {
                            //시작반차 14시부터
                            //info.Title = "[오전반차] " + _username + " : " + _reason; //제목 : 필수
                            info.Title = "[오전반차] " + _username; //제목 : 필수
                        }

                        //종료반차
                        if (_starthalf == "2" || _endhalf == "2")
                        {
                            //종료반차 14시까지
                            //info.Title = "[오후반차] " + _username + " : " + _reason; //제목 : 필수
                            info.Title = "[오후반차] " + _username; //제목 : 필수
                        }
                    }                    

                    info.Content = ""; //내용 : 미사용

                    info.StartDate = _starttime; //시작시간 : 필수
                    info.EndDate = _endtime; //종료시간 : 필수

                    info.StartDate = DateTime.Parse(_timefor.ToString("yyyy-MM-dd 00:00")); //시작시간 출근시간 설정 : 필수
                    info.EndDate = DateTime.Parse(_timefor.ToString("yyyy-MM-dd 23:59")); //종료시간 출근시간 설정 : 필수

                    info.ScheduleYN = "N"; //일정반영유무 : 기본값 N
                    info.IsOpen = "Y"; //공개유무 : 기본값 Y
                    info.UserID = _userid; //등록자아이디 : 필수
                    info.ReservationID = 0; //자원관리 아이디 : 미사용             
                    info.DeptID = _deptid; //등록자부서아이디 : 필수
                    info.OpenType = _opentype; //공개범위 : 기본값 1

                    BLL.ToDo biz = new BLL.ToDo();
                    int iTodo = biz.AddTodo(info);

                    //로그저장
                    Zeospace.DeskPlus.Common.BE.InterfaceInfo InterfaceInfo = new Zeospace.DeskPlus.Common.BE.InterfaceInfo();
                    Zeospace.DeskPlus.Common.BLL.Interface InterfaceBLL = new Zeospace.DeskPlus.Common.BLL.Interface();
                    InterfaceInfo.CompanyID = CurrentUserInfo.CompanyID; //업체아이디 : 필수
                    InterfaceInfo.ModuleSend = _modulesend; ; //발송모듈 : 필수
                    InterfaceInfo.SendTarget = _sendtarget; ; //수신모듈 : 필수
                    InterfaceInfo.SendID = _docid; //전자결재아이디 : 필수
                    InterfaceInfo.WriteUserID = _userid; //등록자아이디 : 필수
                    InterfaceInfo.WriteUserName = _username; //등록자이름 : 필수

                    _rtn = _docid.ToString() + "|" + iTodo.ToString();

                    if (InterfaceBLL.AddLogData(InterfaceInfo, iTodo) == -1)
                    {
                        _rtn = "-1";
                    }
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
