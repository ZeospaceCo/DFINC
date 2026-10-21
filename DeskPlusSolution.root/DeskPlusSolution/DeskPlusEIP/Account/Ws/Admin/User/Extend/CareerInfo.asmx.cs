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

namespace Zeospace.DeskPlus.Account.Web.Ws.Admin.User.Extend
{
    /// <summary>
    /// CareerInfo의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class CareerInfo : Base.BaseWSAccount
    {

        /// <summary>
        /// 작성자 : 심재운
        /// 경력 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetCareer(string p)
        {
            try
            {
                this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";			//리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.Ws.Admin.User.Extend.CareerInfo).Assembly;	//어셈블리 선언
                this.GetResourceManager();

                SetDecryptString(p);
                string companyID = GetDecryptParam("companyID", "0");
                string userID = GetDecryptParam("userID", "0");

                DAL.UserCareer biz = new DAL.UserCareer();
                List<BE.UserCareerInfo> al = biz.GetUserCareerInfo(companyID.ToNumber<int>(), userID.ToNumber<int>());

                List<object> listobj = new List<object>();

                foreach (BE.UserCareerInfo be in al)
                {
                    BE.UserCareerInfo info = new BE.UserCareerInfo();
                    info.CompanyID = be.CompanyID;
                    info.UserID = be.UserID;
                    info.CareerID = be.CareerID;
                    info.StartDate = be.StartDate;
                    info.EndDate = be.EndDate;

                    if (info.StartDate.Length == 8) info.StartDate = DateHelper.GetCultureDateChar8(be.StartDate, PreLangReal);
                    if (info.EndDate.Length == 8) info.EndDate = DateHelper.GetCultureDateChar8(be.EndDate, PreLangReal);

                    info.CompanyName = be.CompanyName;
                    info.CareerPeriod = be.CareerPeriod;
                    info.TitleName = be.TitleName;
                    info.JobType = be.JobType;
                    info.Etc = be.Etc;
                    info.Orders = be.Orders;
                    info.BtnDel = resMng.GetString("Admin_UserList_cbsUserDel");

                    listobj.Add(info);
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetCareer : " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 경력 할당하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddCareer(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "").ToNumber<int>();
            int userID = GetDecryptParam("userID", "").ToNumber<int>();
            string startDate = GetDecryptParam("startDate", "");
            string endDate = GetDecryptParam("endDate", "");
            string companyName = GetDecryptParam("companyName", "");
            string careerPeriod = GetDecryptParam("careerPeriod", "");
            string titleName = GetDecryptParam("titleName", "");
            string jobType = GetDecryptParam("jobType", "");
            string etc = GetDecryptParam("etc", "");


            if (companyName.Length > 0)
            {
                BE.UserCareerInfo be = new BE.UserCareerInfo();
                be.CompanyID = companyID;
                be.UserID = userID;
                be.StartDate = DateHelper.GetCultureDate(startDate.Trim(), "yyyyMMdd", PreLangReal);
                be.EndDate = DateHelper.GetCultureDate(endDate.Trim(), "yyyyMMdd", PreLangReal);
                be.CompanyName = companyName;
                be.CareerPeriod = careerPeriod;
                be.TitleName = titleName;
                be.JobType = jobType;
                be.Etc = etc;
                be.WriteUserID = CurrentUserInfo.UserID;

                DAL.UserCareer dal = new DAL.UserCareer();
                dal.Add(be);

                return true;
            }

            return false;

        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 경력사항 삭제하기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelCareer(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "").ToNumber<int>();
            int id = GetDecryptParam("id", "").ToNumber<int>();

            if (companyID > 0 && id > 0)
            {
                DAL.UserCareer dal = new DAL.UserCareer();
                return dal.Del(companyID, id);
            }

            return false;
        }
    }
}

