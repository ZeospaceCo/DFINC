using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Account.BE;

namespace Zeospace.DeskPlus.Account.Web.Ws.Admin.User
{
    /// <summary>
    /// Mod의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Mod : Base.BaseWSAccount
    {

        /// <summary>
        /// 목적 : 로그인아이디 중복 체크
        /// 작성자 : 심재운
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool GetLoginCheck(string p)
        {
            try
            {

                SetDecryptString(p);
                int companyID = GetDecryptParam("companyID", "0").ToNumber<int>(); //회사아이디
                string loginID = GetDecryptParam("loginID", "");//로그인 아이디
                string gubun = GetDecryptParam("gubun", "");//구분자 : 추가, 수정
                int cnt = 0;

                if (companyID > 0 && loginID.Length > 0)
                {
                    DAL.User dal = new DAL.User();
                    List<BE.UserInfo> al = dal.GetUserByLoginID(companyID, loginID, false);
                    cnt = al.Count;

                    //수정 모드에서 중복 체크 할 경우,
                    if (gubun == "mod")
                    {
                        foreach (BE.UserInfo info in al)
                        {
                            if (CurrentUserInfo.CompanyID == info.CompanyID
                                && loginID == info.LogInID)
                            {
                                cnt = 0;
                            }
                        }
                    }
                }
                //true 이면 중복값이 존재
                return (cnt > 0) ? true : false;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetLoginCheck : " + ex.Message);
                return false;
            }
        }


        /// <summary>
        /// 목적 : 메일 중복 체크
        /// 작성자 : 심재운
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool GetMailCheck(string p)
        {
            try
            {

                SetDecryptString(p);
                int companyID = GetDecryptParam("companyID", "0").ToNumber<int>(); //회사아이디
                string mailID = GetDecryptParam("mailID", "");//메일 아이디
                string mailDomain = GetDecryptParam("mailDomain", "");//메일 도메인
                string gubun = GetDecryptParam("gubun", "");//구분자 : 추가, 수정
                int cnt = 0;

                if (companyID > 0 && mailID.Length > 0 && mailDomain.Length > 0)
                {
                    DAL.UserMailDomain dal = new DAL.UserMailDomain();
                    List<UserMailDomainInfo> al = dal.GetUserMailDomain(companyID, mailID, mailDomain);
                    cnt = al.Count;

                    //수정 모드에서 중복 체크 할 경우,
                    if (gubun == "mod")
                    {
                        foreach (BE.UserMailDomainInfo info in al)
                        {
                            if (CurrentUserInfo.CompanyID == info.CompanyID
                                && mailID == info.MailID && mailDomain == info.MailDomain)
                            {
                                cnt = 0;
                            }
                        }
                    }
                }
                //true 이면 중복값이 존재
                return (cnt > 0) ? true : false;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetMailCheck : " + ex.Message);
                return false;
            }
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 사진 및 도장 삭제하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelPhotoSeal(string p)
        {
            try
            {
                SetDecryptString(p);
                
                int userID = GetDecryptParam("userID", "0").ToNumber<int>();
                string gubun = GetDecryptParam("gubun", "");

                if (gubun == "") return false;

                DAL.User dal = new DAL.User();
                List<UserInfo> userList = dal.GetUserInfo(CurrentUserInfo.CompanyID, userID, false, false);

                string photoImage = "";
                string photoSeal = "";
                if (userList.Count > 0)
                {
                    photoImage = userList[0].PhotoImage;
                    photoSeal = userList[0].SealImage;
                }

                //사진이나 도장 삭제는 결재에서 이전에 사용할 경우, 문제 발생 할 수 있으므로 db 만 삭제함.
                if (gubun == "photo")
                {
                    photoImage = "";
                }

                if (gubun == "seal")
                {
                    photoSeal = "";
                }

                return dal.ModPhotoSeal(CurrentUserInfo.CompanyID, userID, photoSeal, photoImage);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelPhotoSeal : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelOTP(string p)
        {
            try
            {
                SetDecryptString(p);

                int userID = GetDecryptParam("userID", "0").ToNumber<int>();

                DAL.User dal = new DAL.User();
                return dal.DelOTP(CurrentUserInfo.CompanyID, userID);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelOTP : " + ex.Message);
                throw;
            }
        }
    }
}
