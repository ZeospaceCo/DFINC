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

namespace Zeospace.DeskPlus.Account.Web.Ws.MyInfo.Extend
{
    /// <summary>
    /// EduInfo의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class EduInfo : Base.BaseWSAccount
    {

        /// <summary>
        /// 작성자 : 심재운
        /// 교육 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetEducation(string p)
        {
            try
            {
                this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";			//리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.Ws.MyInfo.Extend.EduInfo).Assembly;	//어셈블리 선언
                this.GetResourceManager();

                SetDecryptString(p);
                string companyID = GetDecryptParam("companyID", "0");
                string userID = GetDecryptParam("userID", "0");

                DAL.UserEducation biz = new DAL.UserEducation();
                List<BE.UserEducationInfo> al = biz.GetUserEducationInfo(companyID.ToNumber<int>(), userID.ToNumber<int>());

                List<object> listobj = new List<object>();

                foreach (BE.UserEducationInfo be in al)
                {
                    BE.UserEducationInfo info = new BE.UserEducationInfo();
                    info.CompanyID = be.CompanyID;
                    info.UserID = be.UserID;
                    info.EducationID = be.EducationID;
                    info.Name = be.Name;
                    info.Point = be.Point;
                    info.Grade = be.Grade;
                    info.Place = be.Place;

                    info.StartDate = DateHelper.GetCultureDateChar8(be.StartDate, PreLangReal);
                    info.EndDate = DateHelper.GetCultureDateChar8(be.EndDate, PreLangReal);

                    info.IsFinish = be.IsFinish;
                    info.IsFinishName = be.IsFinishName;
                    info.Orders = be.Orders;
                    info.IsFinishName = this.GetLangName(be.IsFinishBase,be.IsFinishKo, be.IsFinishEn, be.IsFinishJa,be.IsFinishFr, be.IsFinishZhcn,be.IsFinishZhtw, PreLang);
                    info.BtnDel = resMng.GetString("Admin_UserList_cbsUserDel");

                    listobj.Add(info);
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetEducation : " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 교육 할당하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddEducation(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "").ToNumber<int>();
            int userID = GetDecryptParam("userID", "").ToNumber<int>();
            string name = GetDecryptParam("name", "");
            int point = GetDecryptParam("point", "0").ToNumber<int>();
            string grade = GetDecryptParam("grade", "");
            string place = GetDecryptParam("place", "");
            string startDate = GetDecryptParam("startDate", "");
            string endDate = GetDecryptParam("endDate", "");
            string isFinish = GetDecryptParam("isFinish", "");


            if (name.Length > 0)
            {
                BE.UserEducationInfo be = new BE.UserEducationInfo();
                be.CompanyID = companyID;
                be.UserID = userID;
                be.Name = name;
                be.Point = point;
                be.Grade = grade;
                be.Place = place;
                be.StartDate = DateHelper.GetCultureDate(startDate.Trim(), "yyyyMMdd", PreLangReal);
                be.EndDate = DateHelper.GetCultureDate(endDate.Trim(), "yyyyMMdd", PreLangReal);
                be.IsFinish = isFinish;
                be.WriteUserID = CurrentUserInfo.UserID;

                DAL.UserEducation dal = new DAL.UserEducation();
                dal.Add(be);

                return true;
            }

            return false;

        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 교육사항 삭제하기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelEdu(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "").ToNumber<int>();
            int id = GetDecryptParam("id", "").ToNumber<int>();

            if (companyID > 0 && id > 0)
            {
                DAL.UserEducation dal = new DAL.UserEducation();
                return dal.Del(companyID, id);
            }

            return false;
        }
    }
}
