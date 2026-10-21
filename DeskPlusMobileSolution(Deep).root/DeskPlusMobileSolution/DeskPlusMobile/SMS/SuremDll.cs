using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;
using System.Text;
using Zeospace.DeskPlusMobile.Web.Account;
using interop.SMSCALL;

namespace Zeospace.DeskPlusMobile.Web.SMS
{
    public class SuremDll
    {

        public bool Transfer(Zeospace.DeskPlusMobile.Web.SMS.SMSInfo _smsInfo)
        {
            return TransferProc(_smsInfo);
        }

        private bool TransferProc(Zeospace.DeskPlusMobile.Web.SMS.SMSInfo _smsComInfo)
        {
            try
            {
                bool rtn = false;
                SMS.SuremInfo suremInfo = new SuremInfo();

                int companyID = 0;
                int userID = 0;

                Account.BLL.User bll = new Zeospace.DeskPlusMobile.Web.Account.BLL.User();
                Account.BE.UserInfo be = new Zeospace.DeskPlusMobile.Web.Account.BE.UserInfo();

                companyID = _smsComInfo.CompanyID;
                userID = _smsComInfo.UserID;
                be = bll.GetUser(companyID, userID);

                suremInfo.InDate = System.DateTime.Now.ToString("yyyyMMdd");
                suremInfo.InTime = System.DateTime.Now.ToString("hhmmss");
                suremInfo.Member = 0;
                suremInfo.SendId = be.LogInID;
                suremInfo.SendName = _smsComInfo.WriteUserName;
                suremInfo.Rphone1 = _smsComInfo.ReceivePhone1.Trim();
                suremInfo.Rphone2 = _smsComInfo.ReceivePhone2.Trim();
                suremInfo.Rphone3 = _smsComInfo.ReceivePhone3.Trim();
                suremInfo.RecvName = "";
                suremInfo.Sphone1 = (string.IsNullOrEmpty(_smsComInfo.ReplyPhone1)) ? "" : _smsComInfo.ReplyPhone1.Trim();
                suremInfo.Sphone2 = (string.IsNullOrEmpty(_smsComInfo.ReplyPhone2)) ? "" : _smsComInfo.ReplyPhone2.Trim();
                suremInfo.Sphone3 = (string.IsNullOrEmpty(_smsComInfo.ReplyPhone3)) ? "" : _smsComInfo.ReplyPhone3.Trim();
                suremInfo.Msg = _smsComInfo.Content.Trim();
                suremInfo.Url = "";
                suremInfo.CompanyName = _smsComInfo.CompanyName;
                suremInfo.DeptID = _smsComInfo.DeptID;
                suremInfo.DeptName = _smsComInfo.DeptName;


                //0 : 즉시, 1: 예약
                if (_smsComInfo.Status.Equals("0"))
                {
                    suremInfo.Rdate = "00000000";
                    suremInfo.Rtime = "000000";
                }
                else
                {
                    suremInfo.Rdate = _smsComInfo.SendDate.ToString("yyyyMMdd");
                    suremInfo.Rtime = _smsComInfo.SendTime.Trim();
                }

                suremInfo.Result = "0"; //대기
                suremInfo.Kind = "S"; //S는 sms 전송
                suremInfo.ErrCode = 0; //error 코드
                suremInfo.RecvTime = ""; // 응답시간


                /*
                 * 1. 일련번호 : 10
                 * 2. 사용자ID : 30
                 * 3. 사용자명 : 16
                 * 4. 호출번호1 : 4
                 * 5. 호출번호2 : 4
                 * 6. 호출번호3 : 4
                 * 7. 메시지 : 80
                 * 8. 예약일자 : 없음
                 * 9. 예약시간 : 없음
                 * 10. 회신번호 :4
                 * 11. 회신번호2 : 4
                 * 12. 회신번호3 : 4
                 * 13. 수신자명 : 12
                 * 14. 회사코드 : 12
                 * 15. 회사명 : 16
                 * 
                 */

                //회사코드 : SUREM 에서 부여한 회사코드
                string companySUREMCode = string.Empty;

                //회사아이디 : SUREM 에서 부여한 회사아이디
                string companySUREMID = string.Empty;

                string smsKeyInfo = "";

                //계열사별 SMS 청구 할 경우
                if (suremInfo.CompanyName.Length > 0)
                {
                    //############ 계열사별 계정으로 한다. (최상위부서명)
                    smsKeyInfo = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, suremInfo.CompanyName);
                }
                else
                {
                    //############ 하나의 회사계정으로 한다.
                    smsKeyInfo = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "DEFAULTACCOUNTKEY");
                }

                if (smsKeyInfo.Length > 0)
                {
                    string[] smsKeyInfoStr = smsKeyInfo.Split(':');
                    companySUREMCode = smsKeyInfoStr[0].ToString();
                    companySUREMID = smsKeyInfoStr[1].ToString();
                }

                int cntMember = StringHelper.CharLen(suremInfo.Member.ToString());
                int cntSendID = StringHelper.CharLen(companySUREMID);//'사용자ID, 발급받은 아이디
                int cntSendName = StringHelper.CharLen(suremInfo.SendName);//'발신자명
                int cntRphone1 = StringHelper.CharLen(suremInfo.Rphone1);//'호출번호, 받을수 있는번호
                int cntRphone2 = StringHelper.CharLen(suremInfo.Rphone2);
                int cntRphone3 = StringHelper.CharLen(suremInfo.Rphone3);
                int cntMsg = StringHelper.CharLen(suremInfo.Msg);//SMS 단보전송
                int cntRdate = StringHelper.CharLen(suremInfo.Rdate);//'예약일자
                int cntRtime = StringHelper.CharLen(suremInfo.Rtime);//'예약시간
                int cntSphone1 = StringHelper.CharLen(suremInfo.Sphone1);//'회신번호
                int cntSphone2 = StringHelper.CharLen(suremInfo.Sphone2);//'회신번호
                int cntSphone3 = StringHelper.CharLen(suremInfo.Sphone3);//'회신번호
                int cntCompanyCode = StringHelper.CharLen(companySUREMCode);//'회사비밀코드
                int cntCompanyName = StringHelper.CharLen(suremInfo.CompanyName); //'회사이름

                Helper.AppException.WriteAssert(AppSettingType.SQLApproval, "log - cntMsg : " + cntMsg);

                StringBuilder sb = new StringBuilder();
                sb.Append(suremInfo.Member.ToString() + "".PadRight(10 - cntMember, ' '));
                sb.Append(companySUREMID + "".PadRight(30 - cntSendID, ' '));
                sb.Append(suremInfo.SendName + "".PadRight(16 - cntSendName, ' '));
                sb.Append(suremInfo.Rphone1 + "".PadRight(4 - cntRphone1, ' '));
                sb.Append(suremInfo.Rphone2 + "".PadRight(4 - cntRphone2, ' '));
                sb.Append(suremInfo.Rphone3 + "".PadRight(4 - cntRphone3, ' '));
                sb.Append(suremInfo.Msg + "".PadRight(80 - cntMsg, ' '));
                sb.Append(suremInfo.Rdate);
                sb.Append(suremInfo.Rtime);
                sb.Append(suremInfo.Sphone1 + "".PadRight(4 - cntSphone1, ' '));
                sb.Append(suremInfo.Sphone2 + "".PadRight(4 - cntSphone2, ' '));
                sb.Append(suremInfo.Sphone3 + "".PadRight(4 - cntSphone3, ' '));
                sb.Append("".PadRight(12, ' '));
                sb.Append(companySUREMCode + "".PadRight(12 - cntCompanyCode, ' '));
                sb.Append(suremInfo.CompanyName + "".PadRight(16 - cntCompanyName, ' '));

                Helper.AppException.WriteAssert(AppSettingType.SQLApproval, "log : " + sb.ToString());

                interop.SMSCALL.SMSCALLMSG smsCALLMSG = new SMSCALLMSG();
                string result = smsCALLMSG.WebCallMessageData(sb.ToString());

                Helper.AppException.WriteAssert(AppSettingType.SQLApproval, "result : " + result);
                //aa.WebCallMessageData();


                SqlParameter[] parms = new SqlParameter[] {
                                                            new SqlParameter("@InDate", SqlDbType.Char,8),
                                                            new SqlParameter("@InTime", SqlDbType.Char,6),
                                                            new SqlParameter("@Member", SqlDbType.Int),
                                                            new SqlParameter("@SendId", SqlDbType.VarChar,16),
                                                            new SqlParameter("@SendName", SqlDbType.VarChar,32),
                                                            new SqlParameter("@Rphone1", SqlDbType.Char,4),
                                                            new SqlParameter("@Rphone2", SqlDbType.Char,4),
                                                            new SqlParameter("@Rphone3", SqlDbType.Char,4),
                                                            new SqlParameter("@RecvName", SqlDbType.VarChar,32),
                                                            new SqlParameter("@Sphone1", SqlDbType.Char,4),
                                                            new SqlParameter("@Sphone2", SqlDbType.Char,4),
                                                            new SqlParameter("@Sphone3", SqlDbType.Char,4),
                                                            new SqlParameter("@Msg", SqlDbType.VarChar,120),
                                                            new SqlParameter("@Url", SqlDbType.VarChar,80),
                                                            new SqlParameter("@Rdate", SqlDbType.Char,8),
                                                            new SqlParameter("@Rtime", SqlDbType.Char,6),
                                                            new SqlParameter("@Result", SqlDbType.Char,1),
                                                            new SqlParameter("@Kind", SqlDbType.Char,1),
                                                            new SqlParameter("@ErrCode", SqlDbType.Int),
                                                            new SqlParameter("@RecvTime", SqlDbType.Char,16)
												          };

                parms[0].Value = suremInfo.InDate;
                parms[1].Value = suremInfo.InTime;
                parms[2].Value = suremInfo.Member;
                parms[3].Value = suremInfo.SendId;
                parms[4].Value = suremInfo.SendName;
                parms[5].Value = suremInfo.Rphone1;
                parms[6].Value = suremInfo.Rphone2;
                parms[7].Value = suremInfo.Rphone3;
                parms[8].Value = suremInfo.RecvName;
                parms[9].Value = suremInfo.Sphone1;
                parms[10].Value = suremInfo.Sphone2;
                parms[11].Value = suremInfo.Sphone3;
                parms[12].Value = suremInfo.Msg;
                parms[13].Value = suremInfo.Url;
                parms[14].Value = suremInfo.Rdate;
                parms[15].Value = suremInfo.Rtime;
                parms[16].Value = suremInfo.Result;
                parms[17].Value = suremInfo.Kind;
                parms[18].Value = suremInfo.ErrCode;
                parms[19].Value = suremInfo.RecvTime;

                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_ALARM, CommandType.StoredProcedure, "SQL_SMSData_Insert", parms);

                rtn = true;

                return rtn;
            }
            catch (SqlException ex)
            {
                string errMsg = AppException.SqlExceptionLog(ex);
                AppException.WriteException("SMS.SuremDll.Transfer() : " + errMsg);
                return false;
            }
        }
    }
}