using System.Data;
using System.Data.SqlClient;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Common.BE;

namespace Zeospace.DeskPlus.Common.DAL
{
    public class SMSSuremDll : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        /// <summary>
        /// 메서드명 :
        /// 작성목적 : sms 발송
        /// 작성자   : 심재운
        /// </summary>
        /// <param name="_smsInfo">The _SMS info.</param>
        /// <returns></returns>
        public bool Transfer(SMSInfo _smsInfo)
        {
            try
            {
                SMSSuremInfo suremInfo = new SMSSuremInfo();

                int companyID = 0;
                int userID = 0;

                if (_smsInfo != null) //신규
                {
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
                }

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

                int cntMember = UtilHelper.CharLen(suremInfo.Member.ToString());
                int cntSendID = UtilHelper.CharLen(companySUREMID);//'사용자ID, 발급받은 아이디
                int cntSendName = UtilHelper.CharLen(suremInfo.SendName);//'발신자명
                int cntRphone1 = UtilHelper.CharLen(suremInfo.Rphone1);//'호출번호, 받을수 있는번호
                int cntRphone2 = UtilHelper.CharLen(suremInfo.Rphone2);
                int cntRphone3 = UtilHelper.CharLen(suremInfo.Rphone3);
                int cntMsg = UtilHelper.CharLen(suremInfo.Msg);//SMS 단보전송
                int cntRdate = UtilHelper.CharLen(suremInfo.Rdate);//'예약일자
                int cntRtime = UtilHelper.CharLen(suremInfo.Rtime);//'예약시간
                int cntSphone1 = UtilHelper.CharLen(suremInfo.Sphone1);//'회신번호
                int cntSphone2 = UtilHelper.CharLen(suremInfo.Sphone2);//'회신번호
                int cntSphone3 = UtilHelper.CharLen(suremInfo.Sphone3);//'회신번호
                int cntCompanyCode = UtilHelper.CharLen(companySUREMCode);//'회사비밀코드
                int cntCompanyName = UtilHelper.CharLen(suremInfo.CompanyName); //'회사이름

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

                interop.SMSCALL.SMSCALLMSG smsCALLMSG = new interop.SMSCALL.SMSCALLMSG();
                string result = smsCALLMSG.WebCallMessageData(sb.ToString());

                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("result : " + result, AppSettingType.COMMON);

                using (SqlConnection cn = new SqlConnection(ConnAlarm(companyID)))
                {
                    SqlCommand cmd = new SqlCommand("SQL_SMSData_Insert", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@InDate", SqlDbType.Char, 8).Value = suremInfo.InDate;
                    cmd.Parameters.Add("@InTime", SqlDbType.Char, 6).Value = suremInfo.InTime;
                    cmd.Parameters.Add("@Member", SqlDbType.Int).Value = suremInfo.Member;
                    cmd.Parameters.Add("@SendId", SqlDbType.VarChar, 16).Value = suremInfo.SendId;
                    cmd.Parameters.Add("@SendName", SqlDbType.VarChar, 32).Value = suremInfo.SendName;
                    cmd.Parameters.Add("@Rphone1", SqlDbType.Char, 4).Value = suremInfo.Rphone1;
                    cmd.Parameters.Add("@Rphone2", SqlDbType.Char, 4).Value = suremInfo.Rphone2;
                    cmd.Parameters.Add("@Rphone3", SqlDbType.Char, 4).Value = suremInfo.Rphone3;
                    cmd.Parameters.Add("@RecvName", SqlDbType.VarChar, 32).Value = suremInfo.RecvName;
                    cmd.Parameters.Add("@Sphone1", SqlDbType.Char, 4).Value = suremInfo.Sphone1;
                    cmd.Parameters.Add("@Sphone2", SqlDbType.Char, 4).Value = suremInfo.Sphone2;
                    cmd.Parameters.Add("@Sphone3", SqlDbType.Char, 4).Value = suremInfo.Sphone3;
                    cmd.Parameters.Add("@Msg", SqlDbType.VarChar, 120).Value = suremInfo.Msg;
                    cmd.Parameters.Add("@Url", SqlDbType.VarChar, 80).Value = suremInfo.Url;
                    cmd.Parameters.Add("@Rdate", SqlDbType.Char, 8).Value = suremInfo.Rdate;
                    cmd.Parameters.Add("@Rtime", SqlDbType.Char, 6).Value = suremInfo.Rtime;
                    cmd.Parameters.Add("@Result", SqlDbType.Char, 1).Value = suremInfo.Result;
                    cmd.Parameters.Add("@Kind", SqlDbType.Char, 1).Value = suremInfo.Kind;
                    cmd.Parameters.Add("@ErrCode", SqlDbType.Int).Value = suremInfo.ErrCode;
                    cmd.Parameters.Add("@RecvTime", SqlDbType.Char, 16).Value = suremInfo.RecvTime;

                    cn.Open();

                    ExecuteNonQuery(cmd);
                }

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }
    }
}