using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Zeospace.DeskPlus.MDM.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Web;
using System.Data;

namespace Zeospace.DeskPlus.MDM.BLL
{
    public class Send : Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2020-05-29
        /// 작성목적 : 알림 메시지 보내기 
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_receiveKey"></param>
        /// <param name="_receive"></param>
        /// <returns></returns>
        public bool Add(BE.SendInfo _info, string _receiveKey, string _receive)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {      
                    //알림발송내역 
                    DAL.Send dal = new DAL.Send();
                    int SendID = dal.Add(_info);

                    if (SendID > 0)
                    {
                        if (_info.CategoryID > 0 && _info.ListID > 0)
                        {     
                            //게시판 조회권한으로 세팅 
                            SendAlarmUser(_info, SendID);
                        }
                        else
                        {
                            //알림수신자 & App 설치 사용자 
                            string[] ArrReceiveKey = _receiveKey.TrimEnd(';').Split(';');

                            DAL.Receive rdal = new DAL.Receive();
                            for (int i = 0; i < ArrReceiveKey.Length; i++)
                            {
                                string[] ArrTmp = ArrReceiveKey[i].ToString().Split('_');

                                if (ArrTmp[0] == "M")
                                {
                                    rdal.Add(_info.CompanyID, SendID, ArrTmp[1].ToNumber<int>(), 0, ArrTmp[0]);
                                }
                                else
                                {
                                    rdal.Add(_info.CompanyID, SendID, 0, ArrTmp[1].ToNumber<int>(), ArrTmp[0]);
                                }
                            }

                            //모바일 알림
                            string[] ArrReceive = _receive.TrimEnd(';').Split(';');

                            for (int i = 0; i < ArrReceive.Length; i++)
                            {
                                SendAlarm(_info, ArrReceive[i].ToNumber<int>(), SendID);
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }

                    ts.Complete();
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add", ex);
                return false;
            }
        }

        /// <summary>
        /// 모바일 알림 
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_categoryID"></param>
        /// <param name="_listID"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_sendID"></param>
        /// <returns></returns>
        public bool SendAlarm(BE.SendInfo _info, int _receiverID, int _sendID)
        {
            try
            {
                Zeospace.DeskPlus.Common.BLL.Alarm biz = new Common.BLL.Alarm();

                Zeospace.DeskPlus.Common.BE.AlarmInfo alarmInfo = new Zeospace.DeskPlus.Common.BE.AlarmInfo();
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
                alarmInfo.Subject = _info.Header;
                alarmInfo.Content = "";
                alarmInfo.TargetUrl = _info.TargetURL;
                alarmInfo.TargetMobileUrl = _info.MobileTargetURL;                
                alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
                alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
                alarmInfo.WriteDate = System.DateTime.Now;
                alarmInfo.AlarmType = 4;
                alarmInfo.KindSubject = "Notice";
                alarmInfo.ViewOption = "01";
                alarmInfo.Store = 1;
                alarmInfo.UserState = 0;
                alarmInfo.MsgSendID = _sendID;
                alarmInfo.AlarmUrl = _info.AlarmURL;

                return biz.Add(alarmInfo, Zeospace.DeskPlus.Common.BE.AlarmEnum.SuSoft.ToString());
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("SendAlarm : " + ex);
                throw;
            }
        }

        /// <summary>
        /// 알람발송자 명단 가져오기
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_sendID"></param>
        public void SendAlarmUser(BE.SendInfo _info, int _sendID)
        {
            try
            {
                string sendUserIDs = GetReadUser(_info.CompanyID, _info.CategoryID, true, _sendID);
                Account.DAL.User userBiz = new Account.DAL.User();

                if (!string.IsNullOrEmpty(sendUserIDs))
                {
                    //전체 알람 발송
                    if (sendUserIDs == "all")
                    {
                        Dictionary<int, string> alUser = new Dictionary<int, string>();

                        alUser = userBiz.GetUserIDListInfo(_info.CompanyID, true); //계열사 사용자 가져오기

                        foreach (KeyValuePair<int, string> userID in alUser)
                        {
                            SendAlarm(_info, userID.Key, _sendID);
                        }

                    }
                    else //부서 및 사용자 알람 발송
                    {
                        sendUserIDs = sendUserIDs.TrimEnd(',');

                        List<string> userList = sendUserIDs.Split(',').ToList<string>();
                        IEnumerable<string> userDis = userList.Distinct(); //사용자 중복제거

                        foreach (string _userID in userDis)
                        {
                            SendAlarm(_info, _userID.ToNumber<int>(), _sendID);                           
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("SendAlarmUser : " + ex);
                throw;
            }
        }

        /// <summary>
        /// 특정 게시판 카테고리 읽기 권한자 (all : 전체, 사용자아이디)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_categoryID"></param>
        /// <param name="_activeDept"></param>
        /// <param name="_sendID"></param>
        /// <returns></returns>
        private string GetReadUser(int _companyID, int _categoryID, bool _activeDept, int _sendID)
        {
            try
            {
                Board.DAL.Authority auth = new Board.DAL.Authority();
                Account.BLL.User bllUser = new Account.BLL.User();
                Account.DAL.Dept dalDept = new Account.DAL.Dept();
                DAL.Receive rdal = new DAL.Receive();

                DataTableReader dtr = auth.GetAuthority(_companyID, _categoryID);
                string sendUserIDs = string.Empty;

                if (dtr != null)
                {
                    string type = string.Empty;
                    int ownerID;

                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            type = dtr["type"].ToString();
                            ownerID = dtr["ownerID"].ToString().ToNumber<int>();

                            //회사별
                            if (type == "G" && ownerID == 0)
                            {
                                sendUserIDs = "all";
                                rdal.AddAll(_companyID, _sendID);
                                break;
                            }
                            else if (type == "G" && ownerID > 0) //회사별
                            {
                                var alDeptList = dalDept.GetDeptLists(ownerID, _activeDept);

                                rdal.Add(_companyID, _sendID, 0, ownerID, "G");

                                string deptUserList = "";
                                foreach (Account.BE.DeptInfo dInfo in alDeptList)
                                {
                                    //실제 회사 아이디로 검사
                                    deptUserList = bllUser.GetChildDeptUserList(dInfo.CompanyID, ownerID, _activeDept);
                                    sendUserIDs += (deptUserList.Length > 0) ? deptUserList + "," : deptUserList;
                                }
                            }
                            else if (type == "M" && ownerID > 0) //사용자별
                            {
                                sendUserIDs += ownerID.ToString() + ",";
                                rdal.Add(_companyID, _sendID, ownerID, ownerID, "M");
                            }
                        }
                    }
                }

                return sendUserIDs;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetReadUser : " + ex);
                throw;
            }
        }

    }
}
