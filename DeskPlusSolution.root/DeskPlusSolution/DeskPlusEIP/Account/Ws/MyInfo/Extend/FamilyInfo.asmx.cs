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
    /// FamilyInfo의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class FamilyInfo : Base.BaseWSAccount
    {

        /// <summary>
        /// 작성자 : 심재운
        /// 교육 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetFamily(string p)
        {
            try
            {
                this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";			//리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.Ws.MyInfo.Extend.FamilyInfo).Assembly;	//어셈블리 선언
                this.GetResourceManager();

                SetDecryptString(p);
                string companyID = GetDecryptParam("companyID", "0");
                string userID = GetDecryptParam("userID", "0");

                DAL.UserFamily biz = new DAL.UserFamily();
                List<BE.UserFamilyInfo> al = biz.GetUserFamilyInfo(companyID.ToNumber<int>(), userID.ToNumber<int>());

                List<object> listobj = new List<object>();

                foreach (BE.UserFamilyInfo be in al)
                {
                    BE.UserFamilyInfo info = new BE.UserFamilyInfo();
                    info.CompanyID = be.CompanyID;
                    info.UserID = be.UserID;
                    info.FamilyID = be.FamilyID;
                    info.Relationship = be.Relationship;
                    info.Name = be.Name;

                    info.Birthday = DateHelper.GetCultureDateChar8(be.Birthday, PreLangReal);
                    info.Job = be.Job;
                    info.Scholarship = be.Scholarship;
                    info.IsWithLive = be.IsWithLive;
                    info.Orders = be.Orders;
                    info.RelName = this.GetLangName(be.RelNameBase, be.RelNameKo, be.RelNameEn, be.RelNameJa, be.RelNameFr, be.RelNameZhcn, be.RelNameZhtw, PreLang);
                    info.Scholar = this.GetLangName(be.ScholarBase, be.ScholarKo, be.ScholarEn, be.ScholarJa, be.ScholarFr, be.ScholarZhcn, be.ScholarZhtw, PreLang);
                    info.Live = this.GetLangName(be.LiveBase, be.LiveKo, be.LiveEn, be.LiveJa, be.LiveFr, be.LiveZhcn, be.LiveZhtw, PreLang);
                    info.BtnDel = resMng.GetString("Admin_UserList_cbsUserDel");

                    listobj.Add(info);
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetFamily : " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 가족관계 할당하기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddFamily(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "").ToNumber<int>();
            int userID = GetDecryptParam("userID", "").ToNumber<int>();
            string relation = GetDecryptParam("relation", "");
            string scholar = GetDecryptParam("scholar", "");
            string withLive = GetDecryptParam("withLive", "");
            string name = GetDecryptParam("name", "");
            string birth = GetDecryptParam("birth", "");
            string job = GetDecryptParam("job", "");


            if (name.Length > 0)
            {
                BE.UserFamilyInfo be = new BE.UserFamilyInfo();
                be.CompanyID = companyID;
                be.UserID = userID;
                be.Relationship = relation;
                be.Scholarship = scholar;
                be.IsWithLive = withLive;
                be.Name = name;
                be.Birthday = DateHelper.GetCultureDate(birth.Trim(), "yyyyMMdd", PreLangReal);
                be.Job = job;
                be.WriteUserID = CurrentUserInfo.CompanyID;

                DAL.UserFamily dal = new DAL.UserFamily();
                dal.Add(be);

                return true;
            }

            return false;

        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 가족관계 삭제하기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelFamily(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "").ToNumber<int>();
            int id = GetDecryptParam("id", "").ToNumber<int>();

            if (companyID > 0 && id > 0)
            {
                DAL.UserFamily dal = new DAL.UserFamily();
                return dal.Del(companyID, id);
            }

            return false;
        }
    }
}
