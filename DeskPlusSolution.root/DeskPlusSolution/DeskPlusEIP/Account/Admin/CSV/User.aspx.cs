using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.VisualBasic.FileIO;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Account.BE;
using AppExceptions = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException;
using System.Data;

namespace Zeospace.DeskPlus.Account.Web.Admin.CSV
{
    public partial class User : Base.BasePageAccount
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 부서추가
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDept_Click(object sender, EventArgs e)
        {
            if (fuDept.HasFile)
            {
                if (fuDept.PostedFile.ContentLength > 0)
                {
                    string fileName = fuDept.FileName;

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

                        fuDept.SaveAs(Server.MapPath(spoolPath + fileName));

                        SaveDept(Server.MapPath(spoolPath + fileName));
                        
                    }
                    catch (Exception ex)
                    {
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("file upload fail : " + ltDept.Text, ex);
                    }
                }
            }
        }

        /// <summary>
        /// 부서 저장
        /// </summary>
        /// <param name="filePath"></param>
        private void SaveDept(string filePath)
        {
            try
            {
                ltDept.Text = "";

                TextFieldParser ps = new TextFieldParser(filePath,
                    System.Text.Encoding.GetEncoding("euc-kr"));

                int companyIDINT = 0;
                int deptCodeINT = 0;
                int deptNameINT = 0;
                int pDeptCodeINT = 0;

                string companyID = "companyid";
                string deptcode = "deptcode";
                string deptname = "deptname";
                string pDeptCode = "parentdeptcode";

                DAL.Dept dalDept = new DAL.Dept();


                //추가 모듈
                using (ps)
                {
                    int i = 0;
                    ps.TextFieldType = FieldType.Delimited;
                    ps.SetDelimiters(",");
                    
                    while (!ps.EndOfData)
                    {
                        i += 1;

                        string[] row = ps.ReadFields();

                        BE.DeptInfo info = new DeptInfo();
                        string parentDeptCode = "";

                        int j = 0;
                        foreach (string fd in row)
                        {
                            if (fd.Length > 0)
                            {
                                j += 1;
                                string f = fd;

                                if (i == 1 && f == companyID) companyIDINT = j;
                                if (i == 1 && f == deptcode) deptCodeINT = j;
                                if (i == 1 && f == deptname) deptNameINT = j;
                                if (i == 1 && f == pDeptCode) pDeptCodeINT = j;

                                if (i > 1 && companyIDINT == j) info.CompanyID = f.ToNumber<int>();
                                if (i > 1 && deptCodeINT == j) info.DeptCode = f;
                                if (i > 1 && deptNameINT == j) info.NameBase = f;
                                if (i > 1 && pDeptCodeINT == j) parentDeptCode = f;
                            }
                        }

                        //부서코드값이 없으면 나간다.
                        if (info.DeptCode.ToNull().Length > 0)
                        {
                            info.WriteUserID = CurrentUserInfo.UserID;

                            List<BE.DeptInfo> alDept = dalDept.GetDeptListsByDeptCode(info.CompanyID, info.DeptCode);


                            //존재하지 않으면
                            if (alDept.Count == 0)
                            {
                                info.ActiveDept = "1";

                                if (i > 1 && info.DeptCode.Length > 0)
                                {
                                    //상위부서코드 값이 존재할 경우
                                    if (parentDeptCode.Length > 0)
                                    {
                                        //상위부서코드로 상위부서id 값 할당하기
                                        List<BE.DeptInfo> alPDept = dalDept.GetDeptListsByDeptCode(info.CompanyID, parentDeptCode);
                                        if (alPDept.Count > 0)
                                        {
                                            info.ParentDeptID = alPDept[0].DeptID;
                                            dalDept.Add(info);
                                        }
                                        else
                                        {
                                            //최상위 부서로 처리한다.
                                            info.ParentDeptID = 0;
                                            dalDept.Add(info);
                                        }
                                    }
                                    else
                                    {
                                        //최상위 부서일 경우
                                        info.ParentDeptID = 0;
                                        dalDept.Add(info);
                                    }
                                }
                            }
                        }
                    }
                }

                TextFieldParser ps1 = new TextFieldParser(filePath,
                    System.Text.Encoding.GetEncoding("euc-kr"));

                //수정모듈 (childdept 및 path 값을 재정립하기 위해 만든것임)
                using (ps1)
                {
                    int i = 0;
                    ps1.TextFieldType = FieldType.Delimited;
                    ps1.SetDelimiters(",");                    

                    while (!ps1.EndOfData)
                    {
                        i += 1;

                        string[] row = ps1.ReadFields();

                        BE.DeptInfo info = new DeptInfo();
                        string parentDeptCode = "";

                        int j = 0;
                        foreach (string fd in row)
                        {
                            if (fd.Length > 0)
                            {
                                j += 1;
                                string f = fd;

                                if (i == 1 && f == companyID) companyIDINT = j;
                                if (i == 1 && f == deptcode) deptCodeINT = j;
                                if (i == 1 && f == deptname) deptNameINT = j;
                                if (i == 1 && f == pDeptCode) pDeptCodeINT = j;

                                if (i > 1 && companyIDINT == j) info.CompanyID = f.ToNumber<int>();
                                if (i > 1 && deptCodeINT == j) info.DeptCode = f;
                                if (i > 1 && deptNameINT == j) info.NameBase = f;
                                if (i > 1 && pDeptCodeINT == j) parentDeptCode = f;
                            }
                        }
                       
                        if (info.DeptCode.ToNull().Length > 0)
                        {
                            info.WriteUserID = CurrentUserInfo.UserID;

                            List<BE.DeptInfo> alDept = dalDept.GetDeptListsByDeptCode(info.CompanyID, info.DeptCode);

                            //존재한다면, 이미 값이 db 에 들어가 잇으며, update처리하면 된다.
                            if (alDept.Count > 0)
                            {
                                if (i > 1 && info.DeptCode.Length > 0)
                                {
                                    info.DeptID = alDept[0].DeptID;
                                    info.RootDept = alDept[0].RootDept;
                                    info.RealDept = alDept[0].RealDept;
                                    info.DeptOrder = alDept[0].DeptOrder;
                                    info.ActiveDept = alDept[0].ActiveDept;

                                    //상위부서코드 값이 존재할 경우
                                    if (parentDeptCode.Length > 0)
                                    {
                                        //상위부서코드로 상위부서id 값 할당하기
                                        List<BE.DeptInfo> alPDept = dalDept.GetDeptListsByDeptCode(alDept[0].CompanyID, parentDeptCode);
                                        if (alPDept.Count > 0)
                                        {
                                            info.ParentDeptID = alPDept[0].DeptID;
                                            dalDept.Mod(info);
                                        }
                                        else
                                        {
                                            //최상위 부서로 처리한다.
                                            info.ParentDeptID = 0;
                                            dalDept.Mod(info);
                                        }
                                    }
                                    else
                                    {
                                        //최상위 부서일 경우
                                        info.ParentDeptID = 0;
                                        dalDept.Mod(info);
                                    }
                                }
                            }
                        }
                    }

                    //Response.Redirect("InOutStep2.aspx", false);

                    ltDept.Text = "부서 업로드 완료되었습니다.";
                }


                
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("부서", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }
        }

        /// <summary>
        /// 직급추가
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnTitle_Click(object sender, EventArgs e)
        {
            if (fuTitle.HasFile)
            {
                if (fuTitle.PostedFile.ContentLength > 0)
                {
                    string fileName = fuTitle.FileName;

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

                        fuTitle.SaveAs(Server.MapPath(spoolPath + fileName));

                        SaveTitle(Server.MapPath(spoolPath + fileName));

                    }
                    catch (Exception ex)
                    {
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("file upload fail : " + ltDept.Text, ex);
                    }
                }
            }
        }
        
        /// <summary>
        /// 직급 저장
        /// </summary>
        /// <param name="filePath"></param>
        private void SaveTitle(string filePath)
        {
            try
            {
                ltTitle.Text = "";

                TextFieldParser ps = new TextFieldParser(filePath,
                    System.Text.Encoding.GetEncoding("euc-kr"));

                int companyIDINT = 0;
                int titleCodeINT = 0;
                int titleNameINT = 0;                

                string companyID = "companyid";
                string titlecode = "titlecode";
                string titlename = "titlename";

                Zeospace.DeskPlus.Common.DAL.Code dalCode = new DeskPlus.Common.DAL.Code();                

                //추가 모듈
                using (ps)
                {
                    int i = 0;
                    ps.TextFieldType = FieldType.Delimited;
                    ps.SetDelimiters(",");

                    while (!ps.EndOfData)
                    {
                        i += 1;

                        string[] row = ps.ReadFields();

                        Zeospace.DeskPlus.Common.BE.CodeInfo info = new DeskPlus.Common.BE.CodeInfo();                        

                        int j = 0;
                        foreach (string fd in row)
                        {
                            if (fd.Length > 0)
                            {
                                j += 1;
                                string f = fd;

                                if (i == 1 && f == companyID) companyIDINT = j;
                                if (i == 1 && f == titlecode) titleCodeINT = j;
                                if (i == 1 && f == titlename) titleNameINT = j;

                                if (i > 1 && companyIDINT == j) info.CompanyID = f.ToNumber<int>();
                                if (i > 1 && titleCodeINT == j) info.InterfaceCode = f;
                                if (i > 1 && titleNameINT == j) info.NameBase = f;
                            }
                        }
                        
                        if (info.InterfaceCode.ToNull().Length > 0)
                        {
                            info.WriteUserID = CurrentUserInfo.UserID;

                            List<Zeospace.DeskPlus.Common.BE.CodeInfo> alTitle = dalCode.GetCodeByInterfaceID(info.CompanyID, info.InterfaceCode);

                            //존재하지 않으면
                            if (alTitle.Count == 0)
                            {
                                info.ActiveCode = "1";

                                if (i > 1 && info.InterfaceCode.Length > 0)
                                {
                                    info.ParentCodeID = 1; //직급 1; 직책은 2
                                    info.Type = "U";
                                    info.Scope = "T";
                                    dalCode.Add(info);
                                }
                            }
                        }
                    }
                }

                TextFieldParser ps1 = new TextFieldParser(filePath,
                    System.Text.Encoding.GetEncoding("euc-kr"));

                //수정모듈 (childdept 및 path 값을 재정립하기 위해 만든것임)
                using (ps1)
                {
                    int i = 0;
                    ps1.TextFieldType = FieldType.Delimited;
                    ps1.SetDelimiters(",");

                    while (!ps1.EndOfData)
                    {
                        i += 1;

                        string[] row = ps1.ReadFields();

                        Zeospace.DeskPlus.Common.BE.CodeInfo info = new DeskPlus.Common.BE.CodeInfo();

                        int j = 0;
                        foreach (string fd in row)
                        {
                            if (fd.Length > 0)
                            {
                                j += 1;
                                string f = fd;

                                if (i == 1 && f == companyID) companyIDINT = j;
                                if (i == 1 && f == titlecode) titleCodeINT = j;
                                if (i == 1 && f == titlename) titleNameINT = j;

                                if (i > 1 && companyIDINT == j) info.CompanyID = f.ToNumber<int>();
                                if (i > 1 && titleCodeINT == j) info.InterfaceCode = f;
                                if (i > 1 && titleNameINT == j) info.NameBase = f;
                            }
                        }

                        if (info.InterfaceCode.ToNull().Length > 0)
                        {
                            info.WriteUserID = CurrentUserInfo.UserID;

                            List<Zeospace.DeskPlus.Common.BE.CodeInfo> alTitle = dalCode.GetCodeByInterfaceID(info.CompanyID, info.InterfaceCode);

                            //존재하지 않으면
                            if (alTitle.Count > 0)
                            {
                                info.ActiveCode = "1";

                                if (i > 1 && info.InterfaceCode.Length > 0)
                                {
                                    info.CodeID = alTitle[0].CodeID;
                                    info.ParentCodeID = alTitle[0].ParentCodeID;
                                    info.Type = alTitle[0].Type;
                                    info.Scope = alTitle[0].Scope;
                                    
                                    dalCode.Mod(info);
                                }
                            }
                        }
                    }


                }

                this.ltTitle.Text = "직급 업로드 완료되었습니다.";

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("직급", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }
        }


        /// <summary>
        /// 직책추가
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDuty_Click(object sender, EventArgs e)
        {
            if (fuDuty.HasFile)
            {
                if (fuDuty.PostedFile.ContentLength > 0)
                {
                    string fileName = fuDuty.FileName;

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

                        fuDuty.SaveAs(Server.MapPath(spoolPath + fileName));

                        SaveDuty(Server.MapPath(spoolPath + fileName));

                    }
                    catch (Exception ex)
                    {
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("file upload fail : " + ltDept.Text, ex);
                    }
                }
            }
        }


        /// <summary>
        /// 직책 저장
        /// </summary>
        /// <param name="filePath"></param>
        private void SaveDuty(string filePath)
        {
            try
            {
                ltDuty.Text = "";

                TextFieldParser ps = new TextFieldParser(filePath,
                    System.Text.Encoding.GetEncoding("euc-kr"));

                int companyIDINT = 0;
                int dutyCodeINT = 0;
                int dutyNameINT = 0;

                string companyID = "companyid";
                string dutycode = "dutycode";
                string dutyname = "dutyname";

                Zeospace.DeskPlus.Common.DAL.Code dalCode = new DeskPlus.Common.DAL.Code();

                //추가 모듈
                using (ps)
                {
                    int i = 0;
                    ps.TextFieldType = FieldType.Delimited;
                    ps.SetDelimiters(",");

                    while (!ps.EndOfData)
                    {
                        i += 1;

                        string[] row = ps.ReadFields();

                        Zeospace.DeskPlus.Common.BE.CodeInfo info = new DeskPlus.Common.BE.CodeInfo();

                        int j = 0;
                        foreach (string fd in row)
                        {
                            if (fd.Length > 0)
                            {
                                j += 1;
                                string f = fd;

                                if (i == 1 && f == companyID) companyIDINT = j;
                                if (i == 1 && f == dutycode) dutyCodeINT = j;
                                if (i == 1 && f == dutyname) dutyNameINT = j;

                                if (i > 1 && companyIDINT == j) info.CompanyID = f.ToNumber<int>();
                                if (i > 1 && dutyCodeINT == j) info.InterfaceCode = f;
                                if (i > 1 && dutyNameINT == j) info.NameBase = f;
                            }
                        }

                        if (info.InterfaceCode.ToNull().Length > 0)
                        {
                            info.WriteUserID = CurrentUserInfo.UserID;

                            List<Zeospace.DeskPlus.Common.BE.CodeInfo> alTitle = dalCode.GetCodeByInterfaceID(info.CompanyID, info.InterfaceCode);

                            //존재하지 않으면
                            if (alTitle.Count == 0)
                            {
                                info.ActiveCode = "1";

                                if (i > 1 && info.InterfaceCode.Length > 0)
                                {
                                    info.ParentCodeID = 2; //직급 1; 직책은 2
                                    info.Type = "U";
                                    info.Scope = "D";
                                    dalCode.Add(info);
                                }
                            }
                        }
                    }
                }

                TextFieldParser ps1 = new TextFieldParser(filePath,
                    System.Text.Encoding.GetEncoding("euc-kr"));

                //수정모듈 (childdept 및 path 값을 재정립하기 위해 만든것임)
                using (ps1)
                {
                    int i = 0;
                    ps1.TextFieldType = FieldType.Delimited;
                    ps1.SetDelimiters(",");

                    while (!ps1.EndOfData)
                    {
                        i += 1;

                        string[] row = ps1.ReadFields();

                        Zeospace.DeskPlus.Common.BE.CodeInfo info = new DeskPlus.Common.BE.CodeInfo();

                        int j = 0;
                        foreach (string fd in row)
                        {
                            if (fd.Length > 0)
                            {
                                j += 1;
                                string f = fd;

                                if (i == 1 && f == companyID) companyIDINT = j;
                                if (i == 1 && f == dutycode) dutyCodeINT = j;
                                if (i == 1 && f == dutyname) dutyNameINT = j;

                                if (i > 1 && companyIDINT == j) info.CompanyID = f.ToNumber<int>();
                                if (i > 1 && dutyCodeINT == j) info.InterfaceCode = f;
                                if (i > 1 && dutyNameINT == j) info.NameBase = f;
                            }
                        }

                        if (info.InterfaceCode.ToNull().Length > 0)
                        {
                            info.WriteUserID = CurrentUserInfo.UserID;

                            List<Zeospace.DeskPlus.Common.BE.CodeInfo> alTitle = dalCode.GetCodeByInterfaceID(info.CompanyID, info.InterfaceCode);

                            //존재하지 않으면
                            if (alTitle.Count > 0)
                            {
                                info.ActiveCode = "1";

                                if (i > 1 && info.InterfaceCode.Length > 0)
                                {
                                    info.CodeID = alTitle[0].CodeID;
                                    info.ParentCodeID = alTitle[0].ParentCodeID;
                                    info.Type = alTitle[0].Type;
                                    info.Scope = alTitle[0].Scope;

                                    dalCode.Mod(info);
                                }
                            }
                        }
                    }


                }

                this.ltDuty.Text = "직책 업로드 완료되었습니다.";

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("직책", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }
        }

        /// <summary>
        /// 사용자 추가
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUser_Click(object sender, EventArgs e)
        {
            if (fuUser.HasFile)
            {
                if (fuUser.PostedFile.ContentLength > 0)
                {
                    string fileName = fuUser.FileName;

                    if (fileName.ToLower().IndexOf(".csv") == -1)
                    {
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "NoCsv", "CSV 파일을 업로드 해주세요.", "");
                        return;
                    }

                    string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                    string guid = System.Guid.NewGuid().ToString();
                    string spoolPath = baseFileDirectory + "{0}/Users/{1}/Spool/{2}/";
                    spoolPath = string.Format(spoolPath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, guid);

                    System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(Server.MapPath(spoolPath));
                    if (di.Exists == false) di.Create();

                    fuUser.SaveAs(Server.MapPath(spoolPath + fileName));

                    SaveUser(Server.MapPath(spoolPath + fileName));
                    SaveUserDept(Server.MapPath(spoolPath + fileName));
                }
            }
        }


        /// <summary>
        /// 사용자 저장
        /// </summary>
        /// <param name="filePath"></param>
        private void SaveUser(string filePath)
        {
            try
            {

                this.ltUser.Text = "";

                TextFieldParser ps = new TextFieldParser(filePath,
                    System.Text.Encoding.GetEncoding("euc-kr"));

                int companyIDINT = 0;
                int nameINT = 0;
                int employeenumberINT = 0;
                int loginidINT = 0;
                int pwdINT = 0;
                int mailidINT = 0;
                int mailpwdINT = 0;
                int maildomainINT = 0;
                int birthdayINT = 0;
                int addressINT = 0;
                int joindateINT = 0;

                //이름,부서코드,부서명,사번,로그인ID,비밀번호,직급코드,직급명,직책코드,직책명,메일아이디,메일패스워드,메일도메인,생일,주소,입사

                string companyID = "companyid";
                string name = "name";
                string employeenumber = "employeenumber";
                string loginid = "loginid";
                string pwd = "pwd";
                string mailid = "mailid";
                string mailpwd = "mailpwd";
                string maildomain = "maildomain";
                string birthday = "birthday";
                string address = "address";
                string joindate = "joindate";
                
                Zeospace.DeskPlus.Account.DAL.User dalUser = new DAL.User();
                Zeospace.DeskPlus.Account.BLL.User bllUser = new BLL.User();

                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");                

                //추가 모듈
                using (ps)
                {
                    int i = 0;
                    ps.TextFieldType = FieldType.Delimited;
                    ps.SetDelimiters(",");

                    while (!ps.EndOfData)
                    {
                        i += 1;

                        string[] row = ps.ReadFields();

                        Zeospace.DeskPlus.Account.BE.UserInfo info = new UserInfo();

                        int j = 0;
                        foreach (string fd in row)
                        {
                            //if (fd.Length > 0)
                            //{
                                j += 1;
                                string f = fd;

                                if (i == 1 && f == companyID) companyIDINT = j;
                                if (i == 1 && f == name) nameINT = j;
                                if (i == 1 && f == employeenumber) employeenumberINT = j;
                                if (i == 1 && f == loginid) loginidINT = j;
                                if (i == 1 && f == pwd) pwdINT = j;
                                if (i == 1 && f == mailid) mailidINT = j;
                                if (i == 1 && f == mailpwd) mailpwdINT = j;
                                if (i == 1 && f == maildomain) maildomainINT = j;
                                if (i == 1 && f == birthday) birthdayINT = j;
                                if (i == 1 && f == address) addressINT = j;
                                if (i == 1 && f == joindate) joindateINT = j;

                                if (i > 1 && companyIDINT == j) info.CompanyID = f.ToNumber<int>();
                                if (i > 1 && nameINT == j) info.NameBase = f;
                                if (i > 1 && employeenumberINT == j) info.EmployeeNumber = f;
                                if (i > 1 && loginidINT == j) info.LogInID = f;
                                if (i > 1 && pwdINT == j) info.Password = f;
                                if (i > 1 && mailidINT == j) info.MailID = f;
                                if (i > 1 && maildomainINT == j) info.MailDomain = f;
                                if (i > 1 && mailpwdINT == j) info.MailPassword = f;
                                if (i > 1 && birthdayINT == j) info.BirthDay = f;
                                if (i > 1 && addressINT == j) info.HomeAddress1 = f;
                                if (i > 1 && joindateINT == j) info.JoinDate = f;
                            //}
                        }

                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("info.NameBase : " + info.NameBase, AppSettingType.ACCOUNT);

                        if (info.LogInID.ToNull().Length > 0)
                        {
                            info.WriteUserID = CurrentUserInfo.UserID;

                            List<Zeospace.DeskPlus.Account.BE.UserInfo> alUser = dalUser.GetUserByLoginID(info.CompanyID, info.LogInID, false);

                            //존재하지 않으면
                            if (alUser.Count == 0)
                            {
                                info.ActiveUser = "1";
                                info.UserCode = info.LogInID;
                                info.WriteDate = System.DateTime.Now;

                                if (i > 1 && info.LogInID.Length > 0)
                                {
                                    int userID = bllUser.Add(info, null);
                                    FileHelper.AddUserFolder(CurrentUserInfo.CompanyID, userID);
                                }
                            }
                            else
                            {
                                info.UserCode = info.LogInID;
                                info.UserID = alUser[0].UserID;
                                info.NameKo = alUser[0].NameKo;
                                info.NameEn = alUser[0].NameEn;
                                info.NameJa = alUser[0].NameJa;
                                info.NameFr = alUser[0].NameFr;
                                info.NameZhCn = alUser[0].NameZhCn;
                                info.NameZhTw = alUser[0].NameZhTw;
                                info.RegistrationNumber = alUser[0].RegistrationNumber;
                                info.Sex = alUser[0].Sex;
                                info.Lunar = alUser[0].Lunar;

                                #region 다국어날짜형태를 숫자 8자리로 만들기                                
                                info.RetireDate = alUser[0].RetireDate;
                                info.WeddingAnniversary = alUser[0].WeddingAnniversary;
                                #endregion

                                info.Description = alUser[0].Description;
                                info.HomeZipCode = alUser[0].HomeZipCode;
                                info.HomeAddress2 = alUser[0].HomeAddress2;
                                info.BirthZipCode = alUser[0].BirthZipCode;
                                info.BirthAddress1 = alUser[0].BirthAddress1;
                                info.BirthAddress2 = alUser[0].BirthAddress2;
                                info.MobilePhone = alUser[0].MobilePhone;
                                info.HomePhone = alUser[0].HomePhone;
                                info.OfficePhone = alUser[0].OfficePhone;
                                info.ExtensionOfficePhone = alUser[0].ExtensionOfficePhone;
                                info.Fax = alUser[0].Fax;
                                info.PrivateQuota = alUser[0].PrivateQuota;
                                info.SecurityApproval = alUser[0].SecurityApproval;
                                info.SecurityLogIn = alUser[0].SecurityLogIn;
                                info.ActiveUser = alUser[0].ActiveUser;
                                info.PreferredLanguage = alUser[0].PreferredLanguage;

                                if (i > 1 && info.LogInID.Length > 0)
                                {
                                    info.WriteDate = System.DateTime.Now;
                                    bllUser.Mod(info, null);
                                }
                            }
                        }
                    }
                }                

                this.ltUser.Text = "사용자 업로드 완료되었습니다.";

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("사용자", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }
        }



        /// <summary>
        /// 사용자의 부서 및 직급,직책 할당하기
        /// </summary>
        /// <param name="filePath"></param>
        private void SaveUserDept(string filePath)
        {
            try
            {
                this.ltUser.Text = "";

                TextFieldParser ps = new TextFieldParser(filePath,
                    System.Text.Encoding.GetEncoding("euc-kr"));

                int companyIDINT = 0;                
                int deptcodeINT = 0;
                int loginidINT = 0;
                int titlecodeINT = 0;
                int dutycodeINT = 0;

                //이름,부서코드,부서명,사번,로그인ID,비밀번호,직급코드,직급명,직책코드,직책명,메일아이디,메일패스워드,메일도메인,생일,주소,입사

                string companyID = "companyid";                
                string deptcode = "deptcode";                
                string loginid = "loginid";
                string titlecode = "titlecode";
                string dutycode = "dutycode";

                DAL.Dept dalDept = new DAL.Dept();
                Zeospace.DeskPlus.Common.DAL.Code dalCode = new DeskPlus.Common.DAL.Code();
                Zeospace.DeskPlus.Account.DAL.User dalUser = new DAL.User();
                Zeospace.DeskPlus.Account.BLL.UserDept bllUserDept = new BLL.UserDept();

                //추가 모듈
                using (ps)
                {
                    int i = 0;
                    ps.TextFieldType = FieldType.Delimited;
                    ps.SetDelimiters(",");

                    while (!ps.EndOfData)
                    {
                        i += 1;

                        string[] row = ps.ReadFields();

                        List<BE.UserDeptInfo> alList = new List<UserDeptInfo>();                        

                        string iLoginID = "";
                        string iDeptCode = "";
                        string iTitleCode = "";
                        string iDutyCode = "";

                        BE.UserDeptInfo info = new UserDeptInfo();

                        int j = 0;
                        foreach (string fd in row)
                        {
                            //if (fd.Length > 0)
                            //{
                                j += 1;
                                string f = fd;

                                if (i == 1 && f == companyID) companyIDINT = j;
                                if (i == 1 && f == loginid) loginidINT = j;
                                if (i == 1 && f == deptcode) deptcodeINT = j;
                                if (i == 1 && f == titlecode) titlecodeINT = j;
                                if (i == 1 && f == dutycode) dutycodeINT = j;

                                if (i > 1 && companyIDINT == j) info.CompanyID = f.ToNumber<int>();
                                if (i > 1 && loginidINT == j) iLoginID = f;
                                if (i > 1 && deptcodeINT == j) iDeptCode = f;
                                if (i > 1 && titlecodeINT == j) iTitleCode = f;
                                if (i > 1 && dutycodeINT == j) iDutyCode = f;
                            //}
                        }

                        AppExceptions.WriteAssert("SaveUserDept - iLoginID : " + iLoginID, AppSettingType.ACCOUNT);
                        AppExceptions.WriteAssert("SaveUserDept - iDeptCode : " + iDeptCode, AppSettingType.ACCOUNT);
                        AppExceptions.WriteAssert("SaveUserDept - iTitleCode : " + iTitleCode, AppSettingType.ACCOUNT);
                        AppExceptions.WriteAssert("SaveUserDept - iDutyCode : " + iDutyCode, AppSettingType.ACCOUNT);                        


                        //로긴 아이디 값이 존재할 경우
                        if (iLoginID.ToNull().Length > 0)
                        {
                            DataTableReader dtr = dalUser.GetUserByLogin(info.CompanyID, iLoginID);

                            if (dtr != null)
                            {
                                if (dtr.HasRows)
                                {
                                    while (dtr.Read())
                                    {
                                        info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                                    }
                                }
                            }

                            AppExceptions.WriteAssert("SaveUserDept - info.UserID : " + info.UserID, AppSettingType.ACCOUNT);
                            

                            if (iDeptCode.ToNull().Length > 0)
                            {
                                List<BE.DeptInfo> alDept = dalDept.GetDeptListsByDeptCode(info.CompanyID, iDeptCode);
                                if (alDept.Count > 0) info.DeptID = alDept[0].DeptID;

                                AppExceptions.WriteAssert("SaveUserDept - info.DeptID : " + info.DeptID, AppSettingType.ACCOUNT);
                            }

                            

                            if (iTitleCode.ToNull().Length > 0)
                            {
                                List<Zeospace.DeskPlus.Common.BE.CodeInfo> alCode = dalCode.GetCodeByInterfaceID(info.CompanyID, iTitleCode);
                                if (alCode.Count > 0) info.CodeTitleID = alCode[0].CodeID;

                                AppExceptions.WriteAssert("SaveUserDept - info.CodeTitleID : " + info.CodeTitleID, AppSettingType.ACCOUNT);
                            }

                            if (iDutyCode.ToNull().Length > 0)
                            {
                                List<Zeospace.DeskPlus.Common.BE.CodeInfo> alCode = dalCode.GetCodeByInterfaceID(info.CompanyID, iDutyCode);
                                if (alCode.Count > 0) info.CodeDutyID = alCode[0].CodeID;

                                AppExceptions.WriteAssert("SaveUserDept - info.CodeDutyID : " + info.CodeDutyID, AppSettingType.ACCOUNT);
                            }

                            if (info.UserID > 0)
                            {
                                info.WriteUserID = CurrentUserInfo.UserID;
                                info.WriteDate = System.DateTime.Now;
                                info.BaseDept = "1";

                                if (i > 1)
                                {
                                    alList.Add(info);
                                    bllUserDept.AddUserDept(alList);
                                }
                            }
                        }
                    }
                }                

                this.ltUser.Text += "사용자 부서 및 직급(직책) 업로드 완료되었습니다.";

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("사용자", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }
        }
    }
}