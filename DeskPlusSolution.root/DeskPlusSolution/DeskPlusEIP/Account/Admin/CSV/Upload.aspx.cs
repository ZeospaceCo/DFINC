using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.VisualBasic.FileIO;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Account.BE;

namespace Zeospace.DeskPlus.Account.Web.Admin.CSV
{
    public partial class Upload : Base.BasePageAccount
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.Admin.CSV.Upload).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                #region 리소스 처리
                this.GetResourceManager();
                if (!this.resMng.Equals(null))
                {

                }
                #endregion
            }
        }

        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)
        #endregion

        #region (함수)

        /// <summary>
        /// 학력 저장
        /// </summary>
        /// <param name="filePath"></param>
        private void SaveScholar(string filePath)
        {
            try
            {
                ltlMessage.Text = "";

                TextFieldParser ps = new TextFieldParser(filePath,
                    System.Text.Encoding.GetEncoding("euc-kr"));

                using (ps)
                {
                    int i = 0;
                    ps.TextFieldType = FieldType.Delimited;
                    ps.SetDelimiters(",");

                    //CompanyID,LoginID,StartDate,EndDate,SchoolName,Degree,Major,GraduateType,Etc

                    int companyIDINT = 0;
                    int loginIDINT = 0;
                    int startDateINT = 0;
                    int endDateINT = 0;
                    int schoolNameINT = 0;
                    int degreeINT = 0;
                    int majorINT = 0;
                    int graduateTypeINT = 0;
                    int etcINT = 0;

                    string companyID = "CompanyID";
                    string loginID = "LoginID";
                    string startDate = "StartDate";
                    string endDate = "EndDate";
                    string schoolName = "SchoolName";
                    string degree = "Degree";
                    string major = "Major";
                    string graduateType = "GraduateType";
                    string etc = "Etc";

                    DAL.UserScholar dal = new DAL.UserScholar();
                    DAL.User dalUser = new DAL.User();

                    while (!ps.EndOfData)
                    {
                        i += 1;

                        string[] row = ps.ReadFields();

                        BE.UserScholarInfo info = new BE.UserScholarInfo();

                        int j = 0;
                        foreach (string fd in row)
                        {
                            j += 1;
                            string f = fd;

                            if (i == 1 && f == companyID) companyIDINT = j;
                            if (i == 1 && f == loginID) loginIDINT = j;
                            if (i == 1 && f == startDate) startDateINT = j;
                            if (i == 1 && f == endDate) endDateINT = j;
                            if (i == 1 && f == schoolName) schoolNameINT = j;
                            if (i == 1 && f == degree) degreeINT = j;
                            if (i == 1 && f == major) majorINT = j;
                            if (i == 1 && f == graduateType) graduateTypeINT = j;
                            if (i == 1 && f == etc) etcINT = j;

                            if (i > 1 && companyIDINT == j) info.CompanyID = f.ToNumber<int>();
                            if (i > 1 && loginIDINT == j) info.LoginID = f;
                            if (i > 1 && startDateINT == j) info.StartDate = f;
                            if (i > 1 && endDateINT == j) info.EndDate = f;
                            if (i > 1 && schoolNameINT == j) info.SchoolName = f;
                            if (i > 1 && degreeINT == j) info.Degree = f;
                            if (i > 1 && majorINT == j) info.Major = f;
                            if (i > 1 && graduateTypeINT == j) info.GraduateType = f;
                            if (i > 1 && etcINT == j) info.Etc = f;
                        }

                        if (i > 1 && info.SchoolName.Length > 0)
                        {
                            List<UserInfo> alUser = dalUser.GetUserByLoginIDEmpID(info.CompanyID, info.LoginID, true);
                            if (alUser.Count > 0)
                            {
                                info.UserID = alUser[0].UserID;
                                if (info.UserID > 0) dal.Add(info);
                            }
                        }
                    }

                    //Response.Redirect("InOutStep2.aspx", false);

                    ltlMessage.Text = "Scholar 업로드 완료되었습니다.";
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("SaveScholar", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }
        }


        /// <summary>
        /// 상벌 저장
        /// </summary>
        /// <param name="filePath"></param>
        private void SaveReward(string filePath)
        {
            try
            {
                ltlMessage.Text = "";

                TextFieldParser ps = new TextFieldParser(filePath,
                    System.Text.Encoding.GetEncoding("euc-kr"));

                using (ps)
                {
                    int i = 0;
                    ps.TextFieldType = FieldType.Delimited;
                    ps.SetDelimiters(",");

                    //CompanyID,LoginID,RewardDate,Gubun,RewardType,Contents,Rewarder

                    int companyIDINT = 0;
                    int loginIDINT = 0;
                    int rewardDateINT = 0;
                    int gubunINT = 0;
                    int rewardTypeINT = 0;
                    int contentsINT = 0;
                    int rewarderINT = 0;

                    string companyID = "CompanyID";
                    string loginID = "LoginID";
                    string rewardDate = "RewardDate";
                    string gubun = "Gubun";
                    string rewardType = "RewardType";
                    string contents = "Contents";
                    string rewarder = "Rewarder";

                    DAL.UserReward dal = new DAL.UserReward();
                    DAL.User dalUser = new DAL.User();

                    while (!ps.EndOfData)
                    {
                        i += 1;

                        string[] row = ps.ReadFields();

                        BE.UserRewardInfo info = new BE.UserRewardInfo();

                        int j = 0;
                        foreach (string fd in row)
                        {
                            j += 1;
                            string f = fd;

                            if (i == 1 && f == companyID) companyIDINT = j;
                            if (i == 1 && f == loginID) loginIDINT = j;
                            if (i == 1 && f == rewardDate) rewardDateINT = j;
                            if (i == 1 && f == gubun) gubunINT = j;
                            if (i == 1 && f == rewardType) rewardTypeINT = j;
                            if (i == 1 && f == contents) contentsINT = j;
                            if (i == 1 && f == rewarder) rewarderINT = j;

                            if (i > 1 && companyIDINT == j) info.CompanyID = f.ToNumber<int>();
                            if (i > 1 && loginIDINT == j) info.LoginID = f;
                            if (i > 1 && rewardDateINT == j) info.RewardDate = f;
                            if (i > 1 && gubunINT == j) info.Gubun = f;
                            if (i > 1 && rewardTypeINT == j) info.RewardType = f;
                            if (i > 1 && contentsINT == j) info.Contents = f;
                            if (i > 1 && rewarderINT == j) info.Rewarder = f;
                        }

                        if (i > 1 && info.Rewarder.Length > 0)
                        {
                            List<UserInfo> alUser = dalUser.GetUserByLoginIDEmpID(info.CompanyID, info.LoginID, true);
                            if (alUser.Count > 0)
                            {
                                info.UserID = alUser[0].UserID;
                                if (info.UserID > 0) dal.Add(info);
                            }
                        }
                    }

                    //Response.Redirect("InOutStep2.aspx", false);

                    ltlMessage.Text = "Reward 업로드 완료되었습니다.";
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("SaveReward", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }
        }


        /// <summary>
        /// 가족 저장
        /// </summary>
        /// <param name="filePath"></param>
        private void SaveFamily(string filePath)
        {
            try
            {
                ltlMessage.Text = "";

                TextFieldParser ps = new TextFieldParser(filePath,
                    System.Text.Encoding.GetEncoding("euc-kr"));

                using (ps)
                {
                    int i = 0;
                    ps.TextFieldType = FieldType.Delimited;
                    ps.SetDelimiters(",");

                    //CompanyID,LoginID,Relationship,Name,Birthday,Job,Scholarship,IsWithLive

                    int companyIDINT = 0;
                    int loginIDINT = 0;
                    int relationshipINT = 0;
                    int nameINT = 0;
                    int birthdayINT = 0;
                    int jobINT = 0;
                    int scholarshipINT = 0;
                    int isWithLiveINT = 0;

                    string companyID = "CompanyID";
                    string loginID = "LoginID";
                    string relationship = "Relationship";
                    string name = "Name";
                    string birthday = "Birthday";
                    string job = "Job";
                    string scholarship = "Scholarship";
                    string isWithLive = "IsWithLive";

                    DAL.UserFamily dal = new DAL.UserFamily();
                    DAL.User dalUser = new DAL.User();

                    while (!ps.EndOfData)
                    {
                        i += 1;

                        string[] row = ps.ReadFields();

                        BE.UserFamilyInfo info = new BE.UserFamilyInfo();

                        int j = 0;
                        foreach (string fd in row)
                        {
                            j += 1;
                            string f = fd;

                            if (i == 1 && f == companyID) companyIDINT = j;
                            if (i == 1 && f == loginID) loginIDINT = j;
                            if (i == 1 && f == relationship) relationshipINT = j;
                            if (i == 1 && f == name) nameINT = j;
                            if (i == 1 && f == birthday) birthdayINT = j;
                            if (i == 1 && f == job) jobINT = j;
                            if (i == 1 && f == scholarship) scholarshipINT = j;
                            if (i == 1 && f == isWithLive) isWithLiveINT = j;

                            if (i > 1 && companyIDINT == j) info.CompanyID = f.ToNumber<int>();
                            if (i > 1 && loginIDINT == j) info.LoginID = f;
                            if (i > 1 && relationshipINT == j) info.Relationship = f;
                            if (i > 1 && nameINT == j) info.Name = f;
                            if (i > 1 && birthdayINT == j) info.Birthday = f;
                            if (i > 1 && jobINT == j) info.Job = f;
                            if (i > 1 && scholarshipINT == j) info.Scholarship = f;
                            if (i > 1 && isWithLiveINT == j) info.IsWithLive = f;
                        }

                        if (i > 1 && info.Name.Length > 0)
                        {
                            List<UserInfo> alUser = dalUser.GetUserByLoginIDEmpID(info.CompanyID, info.LoginID, true);
                            if (alUser.Count > 0)
                            {
                                info.UserID = alUser[0].UserID;
                                if (info.UserID > 0) dal.Add(info);
                            }
                        }
                    }

                    //Response.Redirect("InOutStep2.aspx", false);

                    ltlMessage.Text = "Family 업로드 완료되었습니다.";
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("SaveFamily", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }
        }
        

        /// <summary>
        /// 경력 저장하기
        /// </summary>
        /// <param name="filePath"></param>
        private void SaveCareer(string filePath)
        {
            try
            {
                ltlMessage.Text = "";

                TextFieldParser ps = new TextFieldParser(filePath,
                    System.Text.Encoding.GetEncoding("euc-kr"));

                using (ps)
                {
                    int i = 0;
                    ps.TextFieldType = FieldType.Delimited;
                    ps.SetDelimiters(",");

                    int companyIDINT = 0;
                    int loginIDINT = 0;
                    int startDateINT = 0;
                    int endDateINT = 0;
                    int companyNameINT = 0;
                    int careerPeriodINT = 0;
                    int titleNameINT = 0;
                    int jobTypeINT = 0;
                    int etcINT = 0;

                    string companyID = "CompanyID";
                    string loginID = "LoginID";
                    string startDate = "StartDate";
                    string endDate = "EndDate";
                    string companyName = "CompanyName";
                    string careerPeriod = "CareerPeriod";
                    string titleName = "TitleName";
                    string jobType = "JobType";
                    string etc = "Etc";

                    
                    DAL.UserCareer dal = new DAL.UserCareer();
                    DAL.User dalUser = new DAL.User();

                    while (!ps.EndOfData)
                    {
                        i += 1;

                        string[] row = ps.ReadFields();

                        BE.UserCareerInfo info = new BE.UserCareerInfo();                                            

                        int j = 0;
                        foreach (string fd in row)
                        {
                            j += 1;
                            string f = fd;

                            if (i == 1 && f == companyID) companyIDINT = j;
                            if (i == 1 && f == loginID) loginIDINT = j;
                            if (i == 1 && f == startDate) startDateINT = j;
                            if (i == 1 && f == endDate) endDateINT = j;
                            if (i == 1 && f == companyName) companyNameINT = j;
                            if (i == 1 && f == careerPeriod) careerPeriodINT = j;
                            if (i == 1 && f == titleName) titleNameINT = j;
                            if (i == 1 && f == jobType) jobTypeINT = j;
                            if (i == 1 && f == etc) etcINT = j;

                            if (i > 1 && companyIDINT == j) info.CompanyID = f.ToNumber<int>();
                            if (i > 1 && loginIDINT == j) info.LoginID = f;
                            if (i > 1 && startDateINT == j) info.StartDate = f;
                            if (i > 1 && endDateINT == j) info.EndDate = f;
                            if (i > 1 && companyNameINT == j) info.CompanyName = f;
                            if (i > 1 && careerPeriodINT == j) info.CareerPeriod = f;
                            if (i > 1 && titleNameINT == j) info.TitleName = f;
                            if (i > 1 && jobTypeINT == j) info.JobType = f;
                            if (i > 1 && etcINT == j) info.Etc = f;
                        }

                        if (i > 1 && info.CompanyName.Length > 0)
                        {
                            List<UserInfo> alUser = dalUser.GetUserByLoginIDEmpID(info.CompanyID, info.LoginID, true);
                            if (alUser.Count > 0)
                            {
                                info.UserID = alUser[0].UserID;
                                if (info.UserID > 0) dal.Add(info);
                            }
                        }
                    }

                    //Response.Redirect("InOutStep2.aspx", false);

                    ltlMessage.Text = "Career 업로드 완료되었습니다.";
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("SaveCareer", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }
        }

        #endregion

        #region (속성)

        #endregion

        #region (이벤트핸들러)

        /// <summary>
        /// 작성자 : 심재운
        /// CSV 임시 테이블에 저장하기
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lbtnSubmit_Click(object sender, System.EventArgs e)
        {
            if (fuFile.HasFile)
            {
                if (fuFile.PostedFile.ContentLength > 0)
                {
                    string fileName = fuFile.FileName;

                    if (fileName.ToLower().IndexOf(".csv") == -1)
                    {
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "NoCsv", "CSV 파일을 업로드 해주세요.", "");
                        return;
                    }

                    try
                    {
                        string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                        string guid = System.Guid.NewGuid().ToString();
                        string spoolPath = baseFileDirectory + "{0}/Users/{1}/Spool/{2}/";
                        spoolPath = string.Format(spoolPath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, guid);

                        System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(Server.MapPath(spoolPath));
                        if (di.Exists == false) di.Create();

                        fuFile.SaveAs(Server.MapPath(spoolPath + fileName));


                        switch(tbTypes.Text)
                        {
                            case "Career":
                                SaveCareer(Server.MapPath(spoolPath + fileName));
                                break;
                            case "Family":
                                SaveFamily(Server.MapPath(spoolPath + fileName));
                                break;
                            case "Scholar":
                                SaveScholar(Server.MapPath(spoolPath + fileName));
                                break;
                            case "Reward":
                                SaveReward(Server.MapPath(spoolPath + fileName));
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("file upload fail : " + tbTypes.Text, ex);
                    }
                }
            }
        }

        #endregion
    }
}