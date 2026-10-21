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

namespace Zeospace.DeskPlus.Account.Web.Ws
{
    /// <summary>
    /// GetAccount의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]    
    [System.Web.Script.Services.ScriptService]
    public class GetAccount : Base.BaseWSAccount
    {
        

        #region 사용자 가져오기

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

        

        /// <summary>
        /// 작성자 : 심재운
        /// 사용자 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetPageList(string p)
        {
            try
            {
                SetDecryptString(p);
                string companyID = GetDecryptParam("companyID", "0");
                string deptID = GetDecryptParam("deptID", "0");
                string codeID = GetDecryptParam("codeID", "0");
                string activeUser = GetDecryptParam("activeUser", "0");
                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("recordPerPage", "0");
                string pageSizePageNo = GetDecryptParam("currentPage", "0");
                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string order = GetDecryptParam("order", "");
                if (order.Length == 0) order = "CompanyID, TitleCodeOrder, DutyCodeOrder, UserDeptOrder desc";

                //검색은 전체에서 검색한다.
                //deptID = (searchValue.Length > 0) ? "0" : deptID;               

                string rtnSetPaging = Utility.SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                string[] dateValue = DateHelper.GetSearchTerm(searchTerm, searchTermValue, PreLangReal);

                DAL.User biz = new DAL.User();

                DataTableReader dtr = biz.GetPageList(companyID.ToNumber<int>(),
                    deptID, codeID, (activeUser == "1") ? true : false,
                    setPageingStr[0], setPageingStr[1], searchField, searchValue, order, dateValue[0], dateValue[1]);

                List<object> listobj = new List<object>();
                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "/DeskplusEIP/FileData/");
                string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.UserInfo info = new BE.UserInfo();
                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.LogInID = dtr["LogInID"].ToString();
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                            info.Mail = dtr["Mail"].ToString();
                            info.MobilePhone = dtr["MobilePhone"].ToString();
                            info.ActiveUser = dtr["ActiveUser"].ToString();

                            info.PhotoImage = baseFileDirectory + info.CompanyID + "/Users/" + info.UserID + "/Picture/photo.jpg";
                            if (!System.IO.File.Exists(Server.MapPath(info.PhotoImage)))
                            {
                                info.PhotoImage = this.BaseImgPath + "/Icon/ico_noimg.gif";
                            }

                            info.Name = this.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                                dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(), PreLang);

                            info.DeptName = this.GetLangName(dtr["DeptBase"].ToString(), dtr["DeptKo"].ToString(), dtr["DeptEn"].ToString(), dtr["DeptJa"].ToString(),
                                dtr["DeptFr"].ToString(), dtr["DeptZhCn"].ToString(), dtr["DeptZhTw"].ToString(), PreLang);

                            info.TitleName = this.GetLangName(dtr["TitleBase"].ToString(), dtr["TitleKo"].ToString(), dtr["TitleEn"].ToString(), dtr["TitleJa"].ToString(),
                                dtr["TitleFr"].ToString(), dtr["TitleZhCn"].ToString(), dtr["TitleZhTw"].ToString(), PreLang);

                            info.DutyName = this.GetLangName(dtr["DutyBase"].ToString(), dtr["DutyKo"].ToString(), dtr["DutyEn"].ToString(), dtr["DutyJa"].ToString(),
                                dtr["DutyFr"].ToString(), dtr["DutyZhCn"].ToString(), dtr["DutyZhTw"].ToString(), PreLang);


                            listobj.Add(info);
                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetPageList : " + ex.Message);
                return null;
            }
        }


        /// <summary>
        /// 사용자 카운트 가져오기
        /// 작성자 : 심재운
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetListCount(string p)
        {
            try
            {
                SetDecryptString(p);
                string companyID = GetDecryptParam("companyID", "0");
                string deptID = GetDecryptParam("deptID", "0");
                string codeID = GetDecryptParam("codeID", "0");
                string activeUser = GetDecryptParam("activeUser", "0");
                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");                
                int icnt = 0;

                string[] dateValue = DateHelper.GetSearchTerm(searchTerm, searchTermValue, PreLangReal);

                DAL.User biz = new DAL.User();
                icnt = biz.GetPageListCount(companyID.ToNumber<int>(),
                    deptID, codeID, (activeUser == "1") ? true : false, searchField, searchValue, dateValue[0], dateValue[1]);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetListCount : " + ex.Message);
                return 0;
            }
        }



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

                    info.StartDate = DateHelper.GetCultureDateChar8(be.StartDate, PreLangReal);
                    info.EndDate = DateHelper.GetCultureDateChar8(be.EndDate, PreLangReal);

                    info.CompanyName = be.CompanyName;
                    info.CareerPeriod = be.CareerPeriod;
                    info.TitleName = be.TitleName;
                    info.JobType = be.JobType;
                    info.Etc = be.Etc;
                    info.Orders = be.Orders;

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

                    info.IsFinishName = this.GetLangName(
                        be.IsFinishBase,
                        be.IsFinishKo, be.IsFinishEn, be.IsFinishJa,
                        be.IsFinishFr, be.IsFinishZhcn,
                        be.IsFinishZhtw, PreLang);

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

                    info.RelName = this.GetLangName(be.RelNameBase, be.RelNameKo, be.RelNameEn, be.RelNameJa, be.RelNameFr, be.RelNameZhcn,
                        be.RelNameZhtw, PreLang);

                    info.Scholar = this.GetLangName(be.ScholarBase, be.ScholarKo, be.ScholarEn, be.ScholarJa, be.ScholarFr, be.ScholarZhcn,
                        be.ScholarZhtw, PreLang);

                    info.Live = this.GetLangName(be.LiveBase, be.LiveKo, be.LiveEn, be.LiveJa, be.LiveFr, be.LiveZhcn,
                        be.LiveZhtw, PreLang);

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
        /// 상벌 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetReward(string p)
        {
            try
            {
                SetDecryptString(p);
                string companyID = GetDecryptParam("companyID", "0");
                string userID = GetDecryptParam("userID", "0");                

                DAL.UserReward biz = new DAL.UserReward();
                List<BE.UserRewardInfo> al = biz.GetUserRewardInfo(companyID.ToNumber<int>(), userID.ToNumber<int>());

                List<object> listobj = new List<object>();

                foreach (BE.UserRewardInfo be in al)
                {
                    BE.UserRewardInfo info = new BE.UserRewardInfo();
                    info.CompanyID = be.CompanyID;
                    info.UserID = be.UserID;
                    info.RewardID = be.RewardID;
                    
                    info.RewardDate = DateHelper.GetCultureDateChar8(be.RewardDate, PreLangReal);
                    info.Gubun = be.Gubun;
                    info.RewardType = be.RewardType;
                    info.Contents = be.Contents;
                    info.Rewarder = be.Rewarder;
                    info.Orders = be.Orders;

                    info.GubunName = this.GetLangName(be.GubunBase,
                        be.GubunKo, be.GubunEn, be.GubunJa,
                        be.GubunFr, be.GubunZhcn,
                        be.GubunZhtw, PreLang);

                    listobj.Add(info);
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetReward : " + ex.Message);
                return null;
            }
        }



        /// <summary>
        /// 작성자 : 심재운
        /// 학력 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetScholar(string p)
        {
            try
            {
                SetDecryptString(p);
                string companyID = GetDecryptParam("companyID", "0");
                string userID = GetDecryptParam("userID", "0");                

                DAL.UserScholar biz = new DAL.UserScholar();
                List<BE.UserScholarInfo> al = biz.GetUserScholarInfo(companyID.ToNumber<int>(), userID.ToNumber<int>());

                List<object> listobj = new List<object>();

                foreach (BE.UserScholarInfo be in al)
                {
                    BE.UserScholarInfo info = new BE.UserScholarInfo();
                    info.CompanyID = be.CompanyID;
                    info.UserID = be.UserID;
                    info.ScholarshipID = be.ScholarshipID;

                    info.StartDate = DateHelper.GetCultureDateChar8(be.StartDate, PreLangReal);
                    info.EndDate = DateHelper.GetCultureDateChar8(be.EndDate, PreLangReal);
                    info.SchoolName = be.SchoolName;
                    info.Degree = be.Degree;

                    info.DegreeName = this.GetLangName(
                        be.DegreeBase,
                        be.DegreeKo, be.DegreeEn,
                        be.DegreeJa,
                        be.DegreeFr, be.DegreeZhcn,
                        be.DegreeZhtw, PreLang);

                    info.Major = be.Major;
                    info.GraduateType = be.GraduateType;

                    info.GraduateTypeName = this.GetLangName(
                        be.GraduateTypeBase,
                        be.GraduateTypeKo, be.GraduateTypeEn,
                        be.GraduateTypeJa,
                        be.GraduateTypeFr, be.GraduateTypeZhcn,
                        be.GraduateTypeZhtw, PreLang);

                    info.Etc = be.Etc;
                    info.Orders = be.Orders;

                    listobj.Add(info);
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetScholar : " + ex.Message);
                return null;
            }
        }

        #endregion

        #region 멀티메일

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

        /// <summary>
        /// 작성자 : 심재운
        /// 멀티 메일 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetMailPageList(string p)
        {
            try
            {
                SetDecryptString(p);
                int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
                int userID = GetDecryptParam("userID", "0").ToNumber<int>();                
                string order = GetDecryptParam("order", "");                

                DAL.UserMailDomain dal = new DAL.UserMailDomain();

                var mailList = dal.GetUserMailDomain(companyID, userID, false);

                List<object> listobj = new List<object>();

                foreach (UserMailDomainInfo be in mailList)
                {
                    BE.UserMailDomainInfo info = new BE.UserMailDomainInfo();
                    info.CompanyID = be.CompanyID;
                    info.UserID = be.UserID;
                    info.UserMailDomainID = be.UserMailDomainID;
                    info.MailID = be.MailID;
                    info.MailDomain = be.MailDomain;
                    info.BaseMail = be.BaseMail;

                    listobj.Add(info);
                }                

                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetMailPageList : " + ex.Message);
                return null;
            }
        }

        #endregion

        #region 임직원조회
        /// <summary>
        /// 작성자 : shine
        /// 임직원 조회 중 트리에서 검색 시 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetTreePageList(string p)
        {
            try 
            {
                SetDecryptString(p);

                string companyID = GetDecryptParam("companyID", "0");
                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                string userID = GetDecryptParam("selownerID", "0");
                string deptID = GetDecryptParam("seldeptID", "0");
                string types = GetDecryptParam("types","");
                string order = GetDecryptParam("order", "");                

                string rtnSetPaging = Utility.SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.User biz = new DAL.User();

                DataTableReader dtr = biz.GetSearchUserDeptPageList(companyID.ToNumber<int>(),
                    userID.ToNumber<int>(), deptID.ToNumber<int>(), setPageingStr[0], setPageingStr[1], types, order);               

                 List<object> listobj = new List<object>();
                 string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "/DeskplusEIP/FileData/");
                 string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");

                 if (dtr != null)
                 {
                     if (dtr.HasRows)
                     {
                         while (dtr.Read())
                         {
                             BE.UserInfo info = new BE.UserInfo();

                             info.UserID = dtr["UserID"].ToString().ToNumber<int>();

                             info.Name = this.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                                dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(), PreLang);

                             info.DeptName = this.GetLangName(dtr["DeptBase"].ToString(), dtr["DeptKo"].ToString(), dtr["DeptEn"].ToString(), dtr["DeptJa"].ToString(),
                                dtr["DeptFr"].ToString(), dtr["DeptZhCn"].ToString(), dtr["DeptZhTw"].ToString(), PreLang);

                             info.TitleName = this.GetLangName(dtr["TitleBase"].ToString(), dtr["TitleKo"].ToString(), dtr["TitleEn"].ToString(), dtr["TitleJa"].ToString(),
                                 dtr["TitleFr"].ToString(), dtr["TitleZhCn"].ToString(), dtr["TitleZhTw"].ToString(), PreLang);

                             info.DutyName = this.GetLangName(dtr["DutyBase"].ToString(), dtr["DutyKo"].ToString(), dtr["DutyEn"].ToString(), dtr["DutyJa"].ToString(),
                                 dtr["DutyFr"].ToString(), dtr["DutyZhCn"].ToString(), dtr["DutyZhTw"].ToString(), PreLang);

                             info.MobilePhone = dtr["MobilePhone"].ToString();
                             info.OfficePhone = dtr["OfficePhone"].ToString();
                             info.Mail = dtr["Mail"].ToString();

                             #region 사진 이미지 설정
                             string photoName = dtr["PhotoImage"].ToString();
                             info.PhotoImage = "";
                             if (!string.IsNullOrEmpty(photoName) && photoName.Length > 0)
                             {
                                 string BaseImgPath = "";
                                 string imgTag = "";

                                 BaseImgPath = baseFileDirectory + "{0}/Users/{1}/Picture/photo.jpg";
                                 BaseImgPath = string.Format(BaseImgPath, CurrentUserInfo.CompanyID, info.UserID);

                                 if (System.IO.File.Exists(Server.MapPath(BaseImgPath)))
                                 {
                                     imgTag = "<img src='{0}' width='52' height='52' />";
                                     imgTag = string.Format(imgTag, BaseImgPath);
                                     info.PhotoImage = imgTag;
                                 }
                             }

                             if (info.PhotoImage.Length == 0)
                                 info.PhotoImage = "<img src='" + this.BaseImgPath + "/Icon/ico_noimg.gif' width='52' height='52' />";
                             #endregion

                             listobj.Add(info);
                         }
                     }
                 }
                 return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetTreePageList : " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 작성자 : shine
        /// 임직원 조회 중 트리에서 검색 시 리스트 카운트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetTreeListCount(string p)
        {
            try 
            { 
                SetDecryptString(p);

                string companyID = GetDecryptParam("companyID", "0");
                string userID = GetDecryptParam("selownerID", "0");
                string deptID = GetDecryptParam("seldeptID", "0");
                string types = GetDecryptParam("types","");

                DAL.User biz = new DAL.User();

                int cnt = 0;

                cnt = biz.GetSearchUserDeptPageListCount(companyID.ToNumber<int>(), userID.ToNumber<int>(), deptID.ToNumber<int>(), types);
                return cnt;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetSearchPageListCount : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : shine
        /// 조건에 따른 임직원 조회시 사용자 정보 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetSearchUserList(string p)
        {
            try 
            { 
                //SetDecryptString(p);

                //string companyID = GetDecryptParam("companyID", "0");
                //string totalCount = GetDecryptParam("totalCount", "0");
                //string pageSize = GetDecryptParam("pageSize", "0");
                //string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                //string name = GetDecryptParam("name", "");
                //string mail = GetDecryptParam("mail", "");
                //string deptName = GetDecryptParam("deptName", "");
                //string telePhone = GetDecryptParam("telePhone", "");
                //string titleID = GetDecryptParam("title", "0");
                //string dutyID = GetDecryptParam("duty", "0");
                //string homeAddress = GetDecryptParam("homeAddress", "");
                //string order = GetDecryptParam("order", "");
                //string andOr = GetDecryptParam("andOr", "");                

                //string rtnSetPaging = Utility.SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                //string[] setPageingStr = rtnSetPaging.Split(',');

                //DAL.User biz = new DAL.User();

                //DataTableReader dtr = biz.GetSearchUserPageList(companyID.ToNumber<int>(),name, mail, deptName, telePhone, 
                //    titleID.ToNumber<int>(), dutyID.ToNumber<int>(), homeAddress, order,andOr, setPageingStr[0], setPageingStr[1]);

                //string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                //string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");

                //List<object> listobj = new List<object>();
                
                //if (dtr != null)
                // {
                //     if (dtr.HasRows)
                //     {
                //         while (dtr.Read())
                //         {
                //             BE.UserInfo info = new BE.UserInfo();

                //             info.UserID = dtr["UserID"].ToString().ToNumber<int>();

                //             info.Name = this.GetLangName(
                //                 dtr["NameBase"].ToString(),
                //                 dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                //                dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(), PreLang);

                //             info.DeptName = this.GetLangName(
                //                 dtr["DeptBase"].ToString(),
                //                 dtr["DeptKo"].ToString(), dtr["DeptEn"].ToString(), dtr["DeptJa"].ToString(),
                //                dtr["DeptFr"].ToString(), dtr["DeptZhCn"].ToString(), dtr["DeptZhTw"].ToString(), PreLang);

                //             info.TitleName = this.GetLangName(
                //                 dtr["TitleBase"].ToString(),
                //                 dtr["TitleKo"].ToString(), dtr["TitleEn"].ToString(), dtr["TitleJa"].ToString(),
                //                 dtr["TitleFr"].ToString(), dtr["TitleZhCn"].ToString(), dtr["TitleZhTw"].ToString(), PreLang);

                //             info.DutyName = this.GetLangName(
                //                 dtr["DutyBase"].ToString(),
                //                 dtr["DutyKo"].ToString(), dtr["DutyEn"].ToString(), dtr["DutyJa"].ToString(),
                //                 dtr["DutyFr"].ToString(), dtr["DutyZhCn"].ToString(), dtr["DutyZhTw"].ToString(), PreLang);

                //             info.MobilePhone = dtr["MobilePhone"].ToString();
                //             info.OfficePhone = dtr["OfficePhone"].ToString();
                //             info.Mail = dtr["Mail"].ToString();

                //             #region 사진 이미지 설정
                //             string photoName = dtr["PhotoImage"].ToString();
                //             info.PhotoImage = "";
                //             if (!string.IsNullOrEmpty(photoName) && photoName.Length > 0)
                //             {
                //                 string BaseImgPath = "";
                //                 string imgTag = "";

                //                 BaseImgPath = baseFileDirectory + "{0}/Users/{1}/Picture/photo.jpg";
                //                 BaseImgPath = string.Format(BaseImgPath, companyID, info.UserID);

                //                 if (System.IO.File.Exists(Server.MapPath(BaseImgPath)))
                //                 {
                //                     imgTag = "<img src='{0}' width='52' height='52' />";
                //                     imgTag = string.Format(imgTag, BaseImgPath);
                //                     info.PhotoImage = imgTag;
                //                 }
                //             }

                //             if (info.PhotoImage.Length == 0)
                //                 info.PhotoImage = "<img src='" + this.BaseImgPath + "/Icon/ico_noimg.gif' width='52' height='52' />";
                //             #endregion

                //             listobj.Add(info);
                //         }
                //     }
                // }
                 return null;
            }
            catch (Exception ex) 
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetSearchUserList : " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 임직원 조건별 검색 시 카운트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetSearchUserchListCount(string p)
        {
            try 
            { 
                //SetDecryptString(p);

                //string companyID = GetDecryptParam("companyID", "0");
                //string name = GetDecryptParam("name", "");
                //string mail = GetDecryptParam("mail", "");
                //string deptName = GetDecryptParam("deptName", "");
                //string telePhone = GetDecryptParam("telePhone", "");
                //string titleID = GetDecryptParam("title", "0");
                //string dutyID = GetDecryptParam("duty", "0");
                //string homeAddress = GetDecryptParam("homeAddress", "");
                //string andOr = GetDecryptParam("andOr", "");

                //int cnt = 0;

                //DAL.User biz = new DAL.User();

                //cnt = biz.GetSearchUserListCount(companyID.ToNumber<int>(), name, mail, deptName, telePhone, titleID.ToNumber<int>(), 
                //    dutyID.ToNumber<int>(), homeAddress, andOr);
                return 0;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetUserSearchListCount : " + ex.Message);
                throw;
            }
        }
        #endregion
    }
}
