using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Zeospace.DeskPlus.Component.SMS;
using Zeospace.DeskPlus.Base.ApplicationFrameworks;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using interop.SMSCALL;
using Zeospace.DeskPlus.Account.BE;

namespace Zeospace.DeskPlus.Component.SMS
{
    public class SuremDll : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL, ISMS
    {
        /// <summary>
        /// 처리
        /// </summary>
        /// <param name="_smsInfo">The _SMS info.</param>
        /// <returns></returns>
        public bool Transfer(Zeospace.DeskPlus.Component.SMS.SMSInfo _smsInfo)
        {
            try
            {
                bool rtn = false;
                SMS.SuremInfo suremInfo = new SuremInfo();

                int companyID = 0;
                int userID = 0;


                Account.BE.UserInfo userInfo = new Zeospace.DeskPlus.Account.BE.UserInfo();
                Account.DAL.User dalUSer = new Account.DAL.User();

                companyID = _smsInfo.CompanyID;
                userID = _smsInfo.UserID;

                suremInfo.InDate = System.DateTime.Now.ToString("yyyyMMdd");
                suremInfo.InTime = System.DateTime.Now.ToString("hhmmss");
                suremInfo.Member = 0;
                suremInfo.SendId = _smsInfo.LoginID;
                suremInfo.SendName = _smsInfo.WriteUserName;
                suremInfo.Rphone1 = _smsInfo.ReceivePhone1.Trim();
                suremInfo.Rphone2 = _smsInfo.ReceivePhone2.Trim();
                suremInfo.Rphone3 = _smsInfo.ReceivePhone3.Trim();
                suremInfo.RecvName = "";
                suremInfo.Sphone1 = (string.IsNullOrEmpty(_smsInfo.ReplyPhone1)) ? "" : _smsInfo.ReplyPhone1.Trim();
                suremInfo.Sphone2 = (string.IsNullOrEmpty(_smsInfo.ReplyPhone2)) ? "" : _smsInfo.ReplyPhone2.Trim();
                suremInfo.Sphone3 = (string.IsNullOrEmpty(_smsInfo.ReplyPhone3)) ? "" : _smsInfo.ReplyPhone3.Trim();
                suremInfo.Msg = _smsInfo.Content.Trim();
                suremInfo.Url = "";
                suremInfo.CompanyName = _smsInfo.CompanyName;
                suremInfo.DeptID = _smsInfo.DeptID;
                suremInfo.DeptName = _smsInfo.DeptName;


                //0 : 즉시, 1: 예약
                if (_smsInfo.Status.Equals("0"))
                {
                    suremInfo.Rdate = "00000000";
                    suremInfo.Rtime = "000000";
                }
                else
                {
                    suremInfo.Rdate = _smsInfo.SendDate.ToString("yyyyMMdd");
                    suremInfo.Rtime = _smsInfo.SendTime.Trim();
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
                    smsKeyInfo = AppSettingHelper.GetAppSetting(AppSettingType.SMS, suremInfo.CompanyName, "");
                }
                else
                {
                    //############ 하나의 회사계정으로 한다.
                    smsKeyInfo = AppSettingHelper.GetAppSetting(AppSettingType.SMS, "DefaultAccountKey", "");
                }

                if (smsKeyInfo.Length > 0)
                {
                    string[] smsKeyInfoStr = smsKeyInfo.Split(':');
                    companySUREMCode = smsKeyInfoStr[0].ToString();
                    companySUREMID = smsKeyInfoStr[1].ToString();
                }

                int cntMember = CharLen(suremInfo.Member.ToString());
                int cntSendID = CharLen(companySUREMID);//'사용자ID, 발급받은 아이디
                int cntSendName = CharLen(suremInfo.SendName);//'발신자명
                int cntRphone1 = CharLen(suremInfo.Rphone1);//'호출번호, 받을수 있는번호
                int cntRphone2 = CharLen(suremInfo.Rphone2);
                int cntRphone3 = CharLen(suremInfo.Rphone3);
                int cntMsg = CharLen(suremInfo.Msg);//SMS 단보전송
                int cntRdate = CharLen(suremInfo.Rdate);//'예약일자
                int cntRtime = CharLen(suremInfo.Rtime);//'예약시간
                int cntSphone1 = CharLen(suremInfo.Sphone1);//'회신번호
                int cntSphone2 = CharLen(suremInfo.Sphone2);//'회신번호
                int cntSphone3 = CharLen(suremInfo.Sphone3);//'회신번호
                int cntCompanyCode = CharLen(companySUREMCode);//'회사비밀코드
                int cntCompanyName = CharLen(suremInfo.CompanyName); //'회사이름

                //Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("log - cntMsg : " + cntMsg, AppSettingType.Site);

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

                //Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("log : " + sb.ToString(), AppSettingType.Site);

                interop.SMSCALL.SMSCALLMSG smsCALLMSG = new SMSCALLMSG();
                string result = smsCALLMSG.WebCallMessageData(sb.ToString());

                //Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("result : " + result, AppSettingType.Site);
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
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Component.SMS.Transfer(): " + errMsg);
                return false;
            }         
        }


        /// <summary>
        /// 메서드명   : CharLen
        /// 작성목적   : 한글(2byte),숫자(1byte),영어(1byte) 글자수
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-01-31
        /// </summary>
        /// <param name="_value">글내용</param>
        /// <param name="limit">제한 글자수</param>
        /// <returns></returns>
        private int CharLen(String _value)
        {
            int strlen = 0;
            char c;
            StringBuilder sb = new StringBuilder();

            for (int j = 0; j < _value.Length; j++)
            {
                c = _value[j];

                //한글 코드의 시작과 끝 값이다
                if (c < 0xac00 || 0xd7a3 < c)
                {
                    strlen++;
                }
                else
                {
                    strlen += 2;  //한글이다..
                }
            }

            return strlen;
        }
    }
}
