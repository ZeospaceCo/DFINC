using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Text.RegularExpressions;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Account.Web.Ws
{
    /// <summary>
    /// SetAccount의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class SetAccount : Base.BaseWSAccount
    {
        #region ADD

        #region 사용자

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 사용자의 부서 할당하기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddUserDept(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "").ToNumber<int>();
            int userID = GetDecryptParam("userID", "").ToNumber<int>();
            string deptList = GetDecryptParam("deptList", "");
            string titleDutyList = GetDecryptParam("titleDutyList", "");

            if (deptList.Length > 0)
            {
                string[] deptStr = deptList.Split(',');
                string[] titleDutyStr = titleDutyList.Split('$');

                List<BE.UserDeptInfo> al = new List<BE.UserDeptInfo>();

                for(int i=0; i < deptStr.Length;i++)
                {
                    BE.UserDeptInfo be = new BE.UserDeptInfo();
                    be.CompanyID = companyID;
                    be.UserID = userID;
                    be.DeptID = deptStr[i].ToNumber<int>();
                    be.CodeTitleID = titleDutyStr[i].Split(',')[0].ToNumber<int>();
                    be.CodeDutyID = titleDutyStr[i].Split(',')[1].ToNumber<int>();
                    be.BaseDept = titleDutyStr[i].Split(',')[2];
                    be.WriteUserID = CurrentUserInfo.UserID;

                    al.Add(be);
                }

                BLL.UserDept userDeptBiz = new BLL.UserDept();
                return userDeptBiz.AddUserDept(al);
            }

            return false;

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
        /// 용도 : 상벌 할당하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddReward(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "").ToNumber<int>();
            int userID = GetDecryptParam("userID", "").ToNumber<int>();
            string rewardDate = GetDecryptParam("rewardDate", "");
            string gubun = GetDecryptParam("gubun", "");
            string rewardType = GetDecryptParam("rewardType", "");
            string contents = GetDecryptParam("contents", "");
            string rewarder = GetDecryptParam("rewarder", "");


            if (rewardDate.Length > 0)
            {
                BE.UserRewardInfo be = new BE.UserRewardInfo();
                be.CompanyID = companyID;
                be.UserID = userID;
                be.RewardDate = DateHelper.GetCultureDate(rewardDate.Trim(), "yyyyMMdd", PreLangReal);
                be.Gubun = gubun;
                be.RewardType = rewardType;
                be.Contents = contents;
                be.Rewarder = rewarder;
                be.WriteUserID = CurrentUserInfo.UserID;

                DAL.UserReward dal = new DAL.UserReward();
                dal.Add(be);

                return true;
            }

            return false;

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
        /// 용도 : 학력 할당하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddScholar(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "").ToNumber<int>();
            int userID = GetDecryptParam("userID", "").ToNumber<int>();
            string startDate = GetDecryptParam("startDate", "");
            string endDate = GetDecryptParam("endDate", "");
            string schoolName = GetDecryptParam("schoolName", "");
            string degree = GetDecryptParam("degree", "");
            string major = GetDecryptParam("major", "");
            string graduationType = GetDecryptParam("graduationType", "");
            string etc = GetDecryptParam("etc", "");


            if (schoolName.Length > 0)
            {
                BE.UserScholarInfo be = new BE.UserScholarInfo();
                be.CompanyID = companyID;
                be.UserID = userID;
                be.StartDate = DateHelper.GetCultureDate(startDate.Trim(), "yyyyMMdd", PreLangReal);
                be.EndDate = DateHelper.GetCultureDate(endDate.Trim(), "yyyyMMdd", PreLangReal);
                be.SchoolName = schoolName;
                be.Degree = degree;
                be.Major = major;
                be.GraduateType = graduationType;
                be.Etc = etc;
                be.WriteUserID = CurrentUserInfo.UserID;

                DAL.UserScholar dal = new DAL.UserScholar();
                dal.Add(be);

                return true;
            }

            return false;

        }

        #endregion

        #region 도메인

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 멀티메일 추가
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddMultiMail(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "").ToNumber<int>();
            int userID = GetDecryptParam("userID", "").ToNumber<int>();
            string mailID = GetDecryptParam("mailID", "");
            string mailDomain = GetDecryptParam("mailDomain", "");
            string mailPwd = GetDecryptParam("mailPwd", "");

            if (mailID.Length > 0 && mailDomain.Length > 0 && mailPwd.Length > 0)
            {
                BE.UserMailDomainInfo be = new BE.UserMailDomainInfo();
                be.CompanyID = companyID;
                be.UserID = userID;
                be.MailID = mailID;
                be.MailDomain = mailDomain;
                be.MailPassword = 
                be.BaseMail = "0";
                be.MailPassword = mailPwd;

                DAL.UserMailDomain dal = new DAL.UserMailDomain();
                dal.Add(be);

                return true;
            }

            return false;
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 멀티메일 추가
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModMailBase(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "").ToNumber<int>();
            int userID = GetDecryptParam("userID", "").ToNumber<int>();
            int userMailDomainID = GetDecryptParam("userMailDomainID", "0").ToNumber<int>();

            if (userID > 0 && userMailDomainID > 0)
            {   
                DAL.UserMailDomain dal = new DAL.UserMailDomain();                
                dal.ModBase(companyID, userID, userMailDomainID);

                return true;
            }

            return false;
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 멀티메일 삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelMailBase(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "").ToNumber<int>();
            int userID = GetDecryptParam("userID", "").ToNumber<int>();
            int userMailDomainID = GetDecryptParam("userMailDomainID", "0").ToNumber<int>();

            if (userID > 0 && userMailDomainID > 0)
            {
                DAL.UserMailDomain dal = new DAL.UserMailDomain();
                dal.Del(companyID, userID, userMailDomainID);

                return true;
            }

            return false;
        }

        #endregion

        #endregion

        #region MOD

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 게시판 폴더 순서 변경하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModExtendOrder(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            string gubun = GetDecryptParam("gubun", "");
            string sortCatList = GetDecryptParam("sortCatList", ""); //카테고리값            
            sortCatList = sortCatList.TrimEnd(',');

            DAL.User dal = new DAL.User();
            return dal.ModExtendOrder(companyID, sortCatList, gubun);

        }


        /// <summary>        
        /// 작성자 : 심재운
        /// 용도 : 게시판 폴더 순서 변경하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModCodeOrder(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>(); //카테고리값            
            string sortCatList = GetDecryptParam("sortCatList", ""); //카테고리값            
            sortCatList = sortCatList.TrimEnd(',');

            Zeospace.DeskPlus.Common.DAL.Code dal = new DeskPlus.Common.DAL.Code();
            return dal.ModOrder(companyID, sortCatList);

        }



        /// <summary>        
        /// 작성자 : 심재운
        /// 용도 : 부서 순서 변경하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModDeptOrder(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>(); //카테고리값            
            string sortCatList = GetDecryptParam("sortCatList", ""); //카테고리값            
            sortCatList = sortCatList.TrimEnd(',');

            Zeospace.DeskPlus.Account.DAL.Dept dal = new DAL.Dept();
            return dal.ModOrder(companyID, sortCatList);
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 상위 부서 변경하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModParentDeptID(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int deptID = GetDecryptParam("deptID", "0").ToNumber<int>();
            int parentDeptID = GetDecryptParam("parentDeptID", "0").ToNumber<int>();

            Zeospace.DeskPlus.Account.DAL.Dept dal = new DAL.Dept();
            return dal.ModParentDeptID(companyID, deptID, parentDeptID);
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 패스워드 변경하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool PasswordChange(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int userID = GetDecryptParam("userID", "0").ToNumber<int>();
            string password = GetDecryptParam("password", "");
            string mailPassword = GetDecryptParam("mailPassword", "");

            if (password.Length > 0 && mailPassword.Length > 0)
            {
                Account.DAL.User dal = new DAL.User();
                dal.ModPwd(companyID, userID, password, mailPassword);

                return true;
            }

            return false;
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 인사 활성화
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModActive(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "").ToNumber<int>();
            string userList = GetDecryptParam("userList", "");

            if (companyID > 0 && userList.Length > 0)
            {
                DAL.User dal = new DAL.User();

                foreach (string user in userList.Split(','))
                {
                    dal.ModActive(companyID, user.ToNumber<int>());
                }
            }

            return true;
        }

        #endregion

        #region DEL

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 인사 삭제하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelUser(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "").ToNumber<int>();
            string userList = GetDecryptParam("userList", "");

            if (companyID > 0 && userList.Length > 0)
            {
                DAL.User dal = new DAL.User();

                foreach(string user in userList.Split(','))
                {
                    dal.Del(companyID, user.ToNumber<int>());
                }
            }

            return true;
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

            if (companyID >0 && id > 0)
            {
                DAL.UserCareer dal = new DAL.UserCareer();
                return dal.Del(companyID, id);
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


        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 상벌 삭제하기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelReward(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "").ToNumber<int>();
            int id = GetDecryptParam("id", "").ToNumber<int>();

            if (companyID > 0 && id > 0)
            {
                DAL.UserReward dal = new DAL.UserReward();
                return dal.Del(companyID, id);
            }

            return false;
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 학력사항 삭제하기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelScholar(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "").ToNumber<int>();
            int id = GetDecryptParam("id", "").ToNumber<int>();

            if (companyID > 0 && id > 0)
            {
                DAL.UserScholar dal = new DAL.UserScholar();
                return dal.Del(companyID, id);
            }

            return false;
        }

        #endregion
    }
}
