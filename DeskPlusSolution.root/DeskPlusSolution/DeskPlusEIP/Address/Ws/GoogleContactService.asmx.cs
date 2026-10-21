using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using DotNetOpenAuth.ApplicationBlock;
using DotNetOpenAuth.OAuth;
using System.Configuration;
using System.Xml.Linq;
using System.Data;

namespace Zeospace.DeskPlus.Address.Web.Ws
{
    /// <summary>
    /// GoogleContactService의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class GoogleContactService : Base.BaseWSAddress
    {

        private InMemoryTokenManager TokenManager
        {
            get
            {
                var tokenManager = (InMemoryTokenManager)Application["GoogleTokenManager"];
                if (tokenManager == null)
                {
                    string consumerKey = ConfigurationManager.AppSettings["googleConsumerKey"];
                    string consumerSecret = ConfigurationManager.AppSettings["googleConsumerSecret"];
                    if (!string.IsNullOrEmpty(consumerKey))
                    {
                        tokenManager = new InMemoryTokenManager(consumerKey, consumerSecret);
                        Application["GoogleTokenManager"] = tokenManager;
                    }
                }

                return tokenManager;
            }
        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddGoogleContact(string p)
        {
            try
            {
                SetDecryptString(p);
                string UserName = GetDecryptParam("username", "");
                string UserPwd = GetDecryptParam("userpwd", "");
                string AccessToken = GetDecryptParam("accessToken", "");

                return AddGoogleToCsv(UserName, UserPwd); //modify shimpark 2014.11.21

                //var google = new WebConsumer(GoogleConsumer.ServiceDescription, this.TokenManager);

                //XDocument contactsDocument = GoogleConsumer.GetContacts(google, AccessToken, 5, 1);                

                //if (contactsDocument != null)
                //{
                //    return AddGoogleToCsv(UserName, UserPwd);
                //}

                //return false;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetPageList", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 구글 주소록 가져오기
        /// </summary>
        /// <param name="_userName"></param>
        /// <param name="_userPwd"></param>
        /// <returns></returns>
        private bool AddGoogleToCsv(string _userName, string _userPwd)
        {
            try
            {
                BLL.GoogleContactService biz = new BLL.GoogleContactService(_userName, _userPwd);

                List<BE.CSVInfo> al = new List<BE.CSVInfo>();

                //총개수(500 개 제한)와 입력 시각일(2000 년 부터)
                foreach (BE.GoogleContactInfo contact in biz.GetContact(500, new DateTime(2000, 1, 1)))
                {
                    BE.CSVInfo info = new BE.CSVInfo();
                    info.CompanyID = CurrentUserInfo.CompanyID;
                    info.UserID = CurrentUserInfo.UserID;
                    info.Name = contact.FullName;
                    info.Email = contact.OfficeEmail;
                    info.MobilePhone = contact.MPhone;
                    info.HomePhone = contact.HPhone;
                    info.CompanyPhone = contact.CPhone;
                    info.Company = contact.CompanyName;

                    al.Add(info);
                }

                DAL.CSV dal = new DAL.CSV();
                dal.Add(al);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddGoogleContact", ex);
                return false;
            }
        }
    }
}
