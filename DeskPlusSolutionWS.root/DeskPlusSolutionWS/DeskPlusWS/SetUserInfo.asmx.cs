using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using Zeospace.DeskPlus.Common.BE;
using Zeospace.DeskPlus.Common.BLL;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Transactions;
using System.Data.SqlClient;

using System.Net;
using System.IO;
using System.Xml;
using System.Configuration;
using System.Text;

namespace Zeospace.DeskPlus.DeskPlusWS.Web
{
    /// <summary>
    /// SetUserInfo의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    // [System.Web.Script.Services.ScriptService]
    public class SetUserInfo : System.Web.Services.WebService
    {

        #region struct
        public struct USERINFO  // 받는 필드 정의
        {

            public string XSTAT;              // 전송건수
            public string XMSGS;              // 전송건수
            public string XDATS;              // 전송건수
            public string XTIMS;              // 전송건수
            public string XROWS;              // 전송건수

            public USERINFODETAIL[] arrDatas;

        }

        public struct USERINFODETAIL
        {
            public string empCode;        //사번
            public string LoginID;        //로긴 ID
            public string passWd;        //Password
            public string empName;        //이름
            public string empNameEn;        //이름
            public string empNameJa;        //이름
            public string empNameFr;        //이름
            public string empNameZhcn;        //이름
            public string empNameZhtw;        //이름
            public string Gender;        //성별
            public string BirthDay;        //생일
            public string MobilNP;        //핸폰
            public string CurrentAddressM;        //주소1
            public string CurrentZipcode;        //우편번호
            public string DeptCode;        //부서코드
            public string DeptName;        //부서명
            public string JobPosition;        //직급코드
            public string JobPositionName;        //직급명
            public string JobDuty;        //직책코드
            public string JobDutyName;   //직책명
            public string JoinDate;     //입사일
            public string MailID;       //메일 ID
            public string PRIMARYYN;     // 겸직구분
            public string OfficePhone;       // 사내전화번호
            public string ActiveUser;      //퇴사여부 N 사용 Y 퇴사
            public string SORT_NO;       // 순서
            public string Lunar;       // 양음력
        }

        #endregion


        #region LagacySetUserInfo : 인사사용자 내용을 XI 로 부터 받습니다.
        /// <summary>
        ///  공통 필드에 대한 사항을 XI 로 부터 받습니다.
        /// </summary>
        /// <param name="sap_datas"></param>
        /// <returns></returns>
        [WebMethod]
        public void LagacySetUserInfo(USERINFO commonData)
        {
            try
            {
                if (commonData.arrDatas != null)
                {
                    SetPersonManage objcode = new SetPersonManage();

                    Zeospace.DeskPlus.Account.BLL.User user = new Account.BLL.User();
                    Zeospace.DeskPlus.Account.DAL.UserDept userdept = new Account.DAL.UserDept();
                    Zeospace.DeskPlus.Account.DAL.User dal = new Account.DAL.User();

                    using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                    {
                        Account.BE.UserInfo userinfo;
                        Account.BE.UserDeptInfo userdeptinfo;

                        // 임시테이블 삭제
                        objcode.DelTempUser();

                        DataSet dsUserDeptAll = objcode.GetUserDeptAll("1");
                        DataView dvUserDeptAll = dsUserDeptAll.Tables[0].DefaultView;

                        // 전체 사용자 부서 맵핑을 날려준다.
                        objcode.DelUserDept();

                        //DataSet dsUserAll = objcode.IsUserAll("1");
                        //DataView dvUserAll = dsUserAll.Tables[0].DefaultView;

                        DataSet dsUserAll;
                        DataView dvUserAll;

                        DataSet dsDeptAll = objcode.GetDeptAll("1");
                        DataView dvDeptAll = dsDeptAll.Tables[0].DefaultView;

                        DataSet dsCodeAll = objcode.GetCodeAll("1");
                        DataView dvCodeAll = dsCodeAll.Tables[0].DefaultView;

                        string DefaultMailDomain = ConfigurationManager.AppSettings["DefaultMailDomain"];

                        //WsException.WriteException("=================: " + commonData.arrDatas.Length);

                        for (int i = 0; i < commonData.arrDatas.Length; i++)
                        {
                            //겸직 때문에 매번 유저정보 가져와서 비교하도록 처리
                            dsUserAll = objcode.IsUserAll("1");
                            dvUserAll = dsUserAll.Tables[0].DefaultView;

                            // 사용자 정보를 맵핑 한다.

                            // 일단 들어오는 데이터 로그처리
                            WsException.WriteAssert("LagacySetUserInfo empCode: " + commonData.arrDatas[i].empCode, AppSettingType.ACCOUNT);
                            WsException.WriteAssert("LagacySetUserInfo LoginID: " + commonData.arrDatas[i].LoginID, AppSettingType.ACCOUNT);
                            WsException.WriteAssert("LagacySetUserInfo passWd: " + commonData.arrDatas[i].passWd, AppSettingType.ACCOUNT); ;
                            WsException.WriteAssert("LagacySetUserInfo empName: " + commonData.arrDatas[i].empName, AppSettingType.ACCOUNT);
                            WsException.WriteAssert("LagacySetUserInfo Gender: " + commonData.arrDatas[i].Gender, AppSettingType.ACCOUNT);
                            WsException.WriteAssert("LagacySetUserInfo BirthDay: " + commonData.arrDatas[i].BirthDay, AppSettingType.ACCOUNT);
                            WsException.WriteAssert("LagacySetUserInfo MobilNP: " + commonData.arrDatas[i].MobilNP, AppSettingType.ACCOUNT);
                            WsException.WriteAssert("LagacySetUserInfo CurrentAddressM: " + commonData.arrDatas[i].CurrentAddressM, AppSettingType.ACCOUNT);
                            WsException.WriteAssert("LagacySetUserInfo CurrentZipcode: " + commonData.arrDatas[i].CurrentZipcode, AppSettingType.ACCOUNT);
                            WsException.WriteAssert("LagacySetUserInfo DeptCode: " + commonData.arrDatas[i].DeptCode, AppSettingType.ACCOUNT);
                            WsException.WriteAssert("LagacySetUserInfo DeptName: " + commonData.arrDatas[i].DeptName, AppSettingType.ACCOUNT);
                            WsException.WriteAssert("LagacySetUserInfo JobPosition: " + commonData.arrDatas[i].JobPosition, AppSettingType.ACCOUNT);
                            WsException.WriteAssert("LagacySetUserInfo JobPositionName: " + commonData.arrDatas[i].JobPositionName, AppSettingType.ACCOUNT);
                            WsException.WriteAssert("LagacySetUserInfo JobDuty: " + commonData.arrDatas[i].JobDuty, AppSettingType.ACCOUNT);
                            WsException.WriteAssert("LagacySetUserInfo JobDutyName: " + commonData.arrDatas[i].JobDutyName, AppSettingType.ACCOUNT);
                            WsException.WriteAssert("LagacySetUserInfo MailID: " + commonData.arrDatas[i].MailID, AppSettingType.ACCOUNT);
                            WsException.WriteAssert("LagacySetUserInfo PRIMARYYN: " + commonData.arrDatas[i].PRIMARYYN, AppSettingType.ACCOUNT);
                            WsException.WriteAssert("LagacySetUserInfo OfficePhone: " + commonData.arrDatas[i].OfficePhone, AppSettingType.ACCOUNT);
                            //WsException.WriteAssert("LagacySetUserInfo ActiveUser: " + commonData.arrDatas[i].ActiveUser, AppSettingType.ACCOUNT);

                            // 임시테이블 interfacecode 입력 
                            objcode.SetTempUser(commonData.arrDatas[i].empCode);

                            WsException.WriteAssert("SetUserInfo.asmx 1 : " + DateTime.Now.ToString(), AppSettingType.ACCOUNT);

                            int userid = 0;

                            // 있는지 여부 확인
                            //userid = objcode.IsUser(commonData.arrDatas[i].LoginID);                       

                            // 있는지 여부 확인 (UserID 필터링)
                            dvUserAll.RowFilter = "usercode = '" + commonData.arrDatas[i].empCode + "'";
                            if (dvUserAll.Count > 0)
                            {
                                userid = Convert.ToInt32(dvUserAll[0]["userid"]);
                            }

                            userinfo = new Account.BE.UserInfo();
                            userinfo.UserID = userid;

                            if (commonData.arrDatas[i].ActiveUser == "N")
                                userinfo.ActiveUser = "1";
                            else
                                userinfo.ActiveUser = "0";

                            userinfo.BirthAddress1 = "";
                            userinfo.BirthAddress2 = "";
                            userinfo.BirthDay = commonData.arrDatas[i].BirthDay.Replace("-", "");
                            userinfo.BirthZipCode = "";// 본적지 우편번호 없음.                        
                            userinfo.CompanyID = 1;
                            userinfo.Description = ""; // 사용하지 않은 필드들 우선 여기에 둘수 있으나 우선 "" 처리
                            userinfo.EmployeeNumber = commonData.arrDatas[i].empCode;
                            //회사 내선번호 구한다. 
                            //string exOfficePhone = string.Empty;
                            //if (commonData.arrDatas[i].ZCOMTELNO != null && commonData.arrDatas[i].ZCOMTELNO != "" && commonData.arrDatas[i].ZCOMTELNO.Length > 3)
                            //{
                            //    exOfficePhone = commonData.arrDatas[i].ZCOMTELNO.Substring(commonData.arrDatas[i].ZCOMTELNO.Length - 4, 4);
                            //}

                            userinfo.ExtensionOfficePhone = ""; //commonData.arrDatas[i].OfficePhone; //회사내선번호
                            userinfo.Fax = "";
                            userinfo.HomeAddress1 = commonData.arrDatas[i].CurrentAddressM;
                            userinfo.HomeAddress2 = ""; // 주소없음
                            userinfo.HomePhone = ""; //commonData.arrDatas[i].ZHOMETELNO;
                            userinfo.HomeZipCode = commonData.arrDatas[i].CurrentZipcode;
                            userinfo.IpPhone = "";
                            userinfo.JoinDate = commonData.arrDatas[i].JoinDate.Replace("-", "");
                            userinfo.LogInID = commonData.arrDatas[i].LoginID;
                            userinfo.Lunar = ""; //commonData.arrDatas[i].Lunar;

                            if (commonData.arrDatas[i].MailID != "")
                            {
                                if (commonData.arrDatas[i].MailID.IndexOf('@') != -1)
                                {
                                    string[] strMail = commonData.arrDatas[i].MailID.Split('@');
                                    userinfo.MailID = strMail[0].ToString().Trim();
                                    userinfo.MailDomain = strMail[1].ToString().Trim();
                                }
                                else
                                {
                                    userinfo.MailID = commonData.arrDatas[i].MailID;
                                    userinfo.MailDomain = ConfigurationManager.AppSettings["DefaultMailDomain"];
                                }

                                userinfo.AuthEmail = userinfo.MailID + "@" + userinfo.MailDomain;
                            }

                            userinfo.MailPassword = commonData.arrDatas[i].passWd;  //commonData.arrDatas[i].LoginID + "_" + commonData.arrDatas[i].empCode;

                            userinfo.MailQuota = 0;
                            userinfo.MobilePhone = commonData.arrDatas[i].MobilNP;
                            userinfo.NameBase = commonData.arrDatas[i].empName;
                            userinfo.NameKo = commonData.arrDatas[i].empName;
                            userinfo.NameEn = commonData.arrDatas[i].empNameEn;   // 영어 이름
                            userinfo.NameJa = commonData.arrDatas[i].empNameJa;   // 일본어 이름
                            userinfo.NameFr = commonData.arrDatas[i].empNameFr;   // 프랑스어 이름                     
                            userinfo.NameZhCn = commonData.arrDatas[i].empNameZhcn; // 중국어 이름 
                            userinfo.NameZhTw = commonData.arrDatas[i].empNameZhtw; // 대만어 이름
                            userinfo.OfficePhone = commonData.arrDatas[i].OfficePhone;
                            userinfo.Password = commonData.arrDatas[i].passWd;  //commonData.arrDatas[i].LoginID + "_" + commonData.arrDatas[i].empCode;                        
                            userinfo.PhotoImage = "";
                            userinfo.PrivateQuota = 0;
                            userinfo.RegistrationNumber = "";// commonData.arrDatas[i].REGNO;
                            userinfo.RetireDate = "";
                            userinfo.SealImage = "";
                            userinfo.SecurityApproval = "";
                            userinfo.SecurityLogIn = "";
                            userinfo.Sex = commonData.arrDatas[i].Gender;
                            userinfo.UserCode = commonData.arrDatas[i].empCode;
                            userinfo.PreferredLanguage = ConfigurationManager.AppSettings["DefaultLanguage"];
                            userinfo.OTPYN = "N";

                            int SORT_NO = -1;
                            if (commonData.arrDatas[i].SORT_NO != "")
                            {
                                SORT_NO = Convert.ToInt32(commonData.arrDatas[i].SORT_NO); //사용자 순서
                            }

                            int dPDeptID = -1;			// 조직 내부 코드
                            int dPCodeTitleID = -1;		// 직급 내부 코드
                            //int dPCodeDutyID = -1;		// 직책 내부 코드
                            //int dPCodeHochingID = -1;   // 호칭 내부 코드

                            //userinfo.ERPUserYN = "Y";

                            //WsException.WriteException("=================: " + userid);
                            if (userid == 0)
                            {
                                // insert
                                int dpuserID = user.Add(userinfo, null);

                                //dPDeptID = objcode.GetDeptID("1",commonData.arrDatas[i].DeptCode);
                                //dPCodeTitleID = objcode.GetCodeID("1",commonData.arrDatas[i].JobPosition);
                                //dPCodeDutyID = objcode.GetCodeID("1", commonData.arrDatas[i].ZZJIKCHK);
                                //dPCodeHochingID = objcode.GetCodeID("1", commonData.arrDatas[i].ZZTITEL);

                                // 사용자 DeptID 필터링
                                dvDeptAll.RowFilter = "deptcode = '" + commonData.arrDatas[i].DeptCode + "'";
                                if (dvDeptAll.Count > 0)
                                {
                                    dPDeptID = Convert.ToInt32(dvDeptAll[0]["deptid"]);
                                }

                                // 사용자 CodeTitleID 필터링
                                dvCodeAll.RowFilter = "interfacecode = '" + commonData.arrDatas[i].JobPosition + "'";
                                if (dvCodeAll.Count > 0)
                                {
                                    dPCodeTitleID = Convert.ToInt32(dvCodeAll[0]["codeid"]);
                                }

                                userdeptinfo = new Account.BE.UserDeptInfo();

                                if (commonData.arrDatas[i].PRIMARYYN == "N") //겸직
                                {
                                    userdeptinfo.BaseDept = "0";
                                }
                                else
                                {
                                    userdeptinfo.BaseDept = "1";
                                }

                                // 사용자 CodeDutyID 필터링
                                dvCodeAll.RowFilter = "interfacecode = '" + commonData.arrDatas[i].JobDuty + "'";
                                if (dvCodeAll.Count > 0)
                                {
                                    userdeptinfo.CodeDutyID = Convert.ToInt32(dvCodeAll[0]["codeid"]);
                                }

                                userdeptinfo.CodeTitleID = dPCodeTitleID;
                                userdeptinfo.CompanyID = 1;
                                userdeptinfo.DeptID = dPDeptID;
                                userdeptinfo.UserID = dpuserID;
                                userdeptinfo.WriteUserID = 0;

                                // userdeptinfo.Hoching = dPCodeHochingID;

                                // 사용자부서테이블을 날려주고 시작한다.
                                //objcode.DelUserDept(dpuserID, userdeptinfo.BaseDept);

                                //userdept.AddUserDept(userdeptinfo);
                                objcode.AddUserDept(userdeptinfo);

                                if (SORT_NO > -1)
                                {
                                    objcode.ModUserDeptOrder(userid, dPDeptID, SORT_NO);
                                }

                                // 인장등록처리
                                //objcode.SetSignPerson(dpuserID.ToString(), commonData.arrDatas[i].empName, "1");

                            }
                            else
                            {
                                //사용자 정보 가져와서 핸드폰번호, 사내전화번호 셋팅
                                DataSet dsUser = objcode.GetUserInfo(1, userid);
                                foreach (DataRow dr in dsUser.Tables[0].Rows)
                                {
                                    userinfo.ExtensionOfficePhone = dr["ExtensionOfficePhone"].ToString();
                                    //userinfo.MobilePhone = dr["MobilePhone"].ToString();
                                    userinfo.HomePhone = dr["HomePhone"].ToString();
                                    userinfo.OfficePhone = dr["OfficePhone"].ToString();
                                    userinfo.Fax = dr["Fax"].ToString();
                                    userinfo.Description = dr["Description"].ToString();
                                    userinfo.OTPYN = dr["OTPYN"].ToString();
                                }

                                WsException.WriteAssert("SetUserInfo.asmx 2 : " + DateTime.Now.ToString(), AppSettingType.ACCOUNT);

                                user.Mod(userinfo, null);

                                WsException.WriteAssert("SetUserInfo.asmx 3 : " + DateTime.Now.ToString(), AppSettingType.ACCOUNT);

                                // update
                                //dPDeptID = objcode.GetDeptID("1", commonData.arrDatas[i].DeptCode);
                                //dPCodeTitleID = objcode.GetCodeID("1", commonData.arrDatas[i].JobPosition);
                                //dPCodeDutyID = objcode.GetCodeID("1", commonData.arrDatas[i].ZZJIKCHK);
                                //dPCodeHochingID = objcode.GetCodeID("1", commonData.arrDatas[i].ZZTITEL);

                                dvDeptAll.RowFilter = "deptcode = '" + commonData.arrDatas[i].DeptCode + "'";
                                if (dvDeptAll.Count > 0)
                                {
                                    dPDeptID = Convert.ToInt32(dvDeptAll[0]["deptid"]);
                                }

                                // 사용자 CodeTitleID 필터링
                                dvCodeAll.RowFilter = "interfacecode = '" + commonData.arrDatas[i].JobPosition + "'";
                                //dvCodeAll.RowFilter = "interfacecode = '" + commonData.arrDatas[i].JobPosition + "'";
                                if (dvCodeAll.Count > 0)
                                {
                                    dPCodeTitleID = Convert.ToInt32(dvCodeAll[0]["codeid"]);
                                }

                                userdeptinfo = new Account.BE.UserDeptInfo();

                                if (commonData.arrDatas[i].PRIMARYYN == "N") //겸직
                                {
                                    userdeptinfo.BaseDept = "0";
                                }
                                else
                                {
                                    userdeptinfo.BaseDept = "1";
                                }

                                // 사용자 CodeDutyID 필터링
                                dvCodeAll.RowFilter = "interfacecode = '" + commonData.arrDatas[i].JobDuty + "'";
                                //dvCodeAll.RowFilter = "interfacecode = '" + commonData.arrDatas[i].JobDuty + "'";
                                if (dvCodeAll.Count > 0)
                                {
                                    userdeptinfo.CodeDutyID = Convert.ToInt32(dvCodeAll[0]["codeid"]);
                                }

                                // 호봉
                                dvUserDeptAll.RowFilter = "UserID = " + userid + " AND DeptID = " + dPDeptID;
                                if (dvUserDeptAll.Count > 0)
                                {
                                    userdeptinfo.UserDeptOrder = Convert.ToInt32(dvUserDeptAll[0]["UserDeptOrder"]);
                                }

                                userdeptinfo.CodeTitleID = dPCodeTitleID;
                                userdeptinfo.CompanyID = 1;
                                userdeptinfo.DeptID = dPDeptID;
                                userdeptinfo.UserID = userid;
                                userdeptinfo.WriteUserID = 0;
                                //userdeptinfo.Hoching = dPCodeHochingID;

                                //userdeptinfo.UserDeptOrder = objcode.GetUserDeptOrder(userid); //사용자의 직급 순서

                                // 사용자부서테이블을 날려주고 시작한다.
                                //objcode.DelUserDept(userid, userdeptinfo.BaseDept);

                                WsException.WriteAssert("SetUserInfo.asmx 4 : " + DateTime.Now.ToString(), AppSettingType.ACCOUNT);

                                //userdept.AddUserDept(userdeptinfo);
                                objcode.AddUserDept(userdeptinfo);

                                WsException.WriteAssert("SetUserInfo.asmx 5 : " + DateTime.Now.ToString(), AppSettingType.ACCOUNT);

                                if (SORT_NO > -1)
                                {
                                    objcode.ModUserDeptOrder(userid, dPDeptID, SORT_NO);
                                }
                                else
                                {
                                    objcode.ModUserDeptOrder(userid, dPDeptID, userdeptinfo.UserDeptOrder);
                                }

                                WsException.WriteAssert("SetUserInfo.asmx 6 : " + DateTime.Now.ToString(), AppSettingType.ACCOUNT);
                            }
                        }


                        // 임시테이블중 실테이블에 없는 데이터는 삭제 
                        objcode.DelModifyValueUser();

                        ts.Complete();
                    }

                    //사용자 개인폴더 생성
                    var userInfoList = dal.GetUserInfo(1, false);
                    foreach (var info in userInfoList)
                    {
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.FileHelper.AddUserFolder(info.CompanyID, info.UserID);
                    }

                    /*
                    // 사진 등록
                    DataSet ds = objcode.GetUserInfo();

                    int UserID;
                    string UserEmp;

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            UserID = Convert.ToInt32(ds.Tables[0].Rows[i]["UserID"]);
                            UserEmp = ds.Tables[0].Rows[i]["EmployeeNumber"].ToString();

                            if (DownloadImg(UserID, UserEmp))
                            {
                                objcode.setSealUpdate(UserID, UserEmp);
                            }
                        }
                    }
                    */  
                }

            }
            catch (Exception ex)
            {
                WsException.WriteException("LagacySetUserInfo.asmx[SetUserInfo] : " + ex);
            }
        }

        private bool DownloadImg(int _userID, string _empNum)
        {
            try
            {

                string uri = "http://118.129.144.85/InsaPhoto/" + _empNum + ".jpg";

                string UserPath = "/DeskPlusEIP/FileData/1/users/{0}";
                UserPath = string.Format(UserPath, _userID);
                string PicturePath = UserPath + "/Picture/" + _empNum + ".jpg";

                string fileName = Server.MapPath(PicturePath);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                bool bImage = response.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase);

                if ((response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Moved || response.StatusCode == HttpStatusCode.Redirect) && bImage)
                {
                    using (Stream inputStream = response.GetResponseStream())
                    using (Stream outputStream = File.OpenWrite(fileName))
                    {
                        byte[] buffer = new byte[4096];
                        int bytesRead;
                        do
                        {
                            bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                            outputStream.Write(buffer, 0, bytesRead);
                        }
                        while (bytesRead != 0);
                    }
                    return true;

                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DownloadImg : " + ex.ToString());
                return false;
            }
        }

        private static string GetSHA256Hash(string _plainText)
        {
            byte[] data = Convert.FromBase64String(_plainText);

            StringBuilder sBuilder = new StringBuilder();

            // 16 진수 만들기
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
        #endregion
    }
}
