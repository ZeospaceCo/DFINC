using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.VisualBasic.FileIO;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Address.Web.Outlook
{
    /// <summary>
    /// 외부주소록 가져오기
    /// </summary>
    public partial class InOutStep1 : Base.BasePageAddress
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Address.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Address.Web.Outlook.InOutStep1).Assembly;	//어셈블리 선언
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
        /// 작성자 :심재운
        /// csv 임시테이블 저장하기
        /// </summary>
        /// <param name="filePath">The file path.</param>
        private void SaveCSV(string filePath)
        {
            try
            {
                TextFieldParser ps = new TextFieldParser(filePath,
                    System.Text.Encoding.GetEncoding("euc-kr"));

                using (ps)
                {
                    int i = 0;
                    ps.TextFieldType = FieldType.Delimited;
                    ps.SetDelimiters(",");

                    int idxName = 0;
                    int idxLastName = 0;
                    int idxMail = 0;
                    int idxMPhone = 0;
                    int idxHPhone = 0;
                    int idxCPhone = 0;
                    int idxCompany = 0;

                    string csvName = "";
                    string csvLastName = "";
                    string csvMai = "";
                    string csvMPhone = "";
                    string csvHPhone = "";
                    string csvCPhone = "";
                    string csvCompany = "";

                    if (tbTypes.Text == "daum")
                    {
                        csvName = "이름";
                        csvMai = "이메일";
                        csvMPhone = "휴대폰";
                        csvHPhone = "집전화";
                        csvCPhone = "회사전화";
                        csvCompany = "회사";
                    }
                    else if (tbTypes.Text == "naver")
                    {
                        csvName = "이름";
                        csvMai = "이메일";
                        csvMPhone = "전화번호";
                        csvHPhone = "";
                        csvCPhone = "";
                        csvCompany = "";
                    }
                    else if (tbTypes.Text == "google")
                    {
                        csvName = "First Name";
                        csvLastName = "Last Name";
                        csvMai = "E-mail Address";
                        csvMPhone = "Mobile Phone";
                        csvHPhone = "Home Phone";
                        csvCPhone = "Business Phone";
                        csvCompany = "Company";
                    }
                    else if (tbTypes.Text == "outlook")
                    {
                        csvName = "성";
                        csvLastName = "이름";
                        csvMai = "전자 메일 주소";
                        csvMPhone = "휴대폰";
                        csvHPhone = "집 전화 번호";
                        csvCPhone = "회사 기본 전화";
                        csvCompany = "회사";
                    }

                    List<BE.CSVInfo> al = new List<BE.CSVInfo>();

                    while (!ps.EndOfData)
                    {
                        i += 1;

                        string[] row = ps.ReadFields();

                        BE.CSVInfo info = new BE.CSVInfo();
                        info.CompanyID = CurrentUserInfo.CompanyID;
                        info.UserID = CurrentUserInfo.UserID;
                        info.Name = "";

                        int j = 0;
                        foreach (string fd in row)
                        {
                            j += 1;
                            string f = fd;

                            if (i == 1 && f == csvName) idxName = j;
                            if (i == 1 && f == csvLastName) idxLastName = j;
                            if (i == 1 && f == csvMai) idxMail = j;
                            if (i == 1 && f == csvMPhone) idxMPhone = j;
                            if (i == 1 && f == csvHPhone) idxHPhone = j;
                            if (i == 1 && f == csvCPhone) idxCPhone = j;
                            if (i == 1 && f == csvCompany) idxCompany = j;

                            if (i > 1 && idxName == j)
                            {
                                if (f.Length > 0) info.Name = f + info.Name;
                            }

                            if (i > 1 && idxLastName == j)
                            {
                                if (f.Length > 0) info.Name = info.Name + f;
                            }

                            if (i > 1 && idxMail == j) info.Email = f;
                            if (i > 1 && idxMPhone == j) info.MobilePhone = f;
                            if (i > 1 && idxHPhone == j) info.HomePhone = f;
                            if (i > 1 && idxCPhone == j) info.CompanyPhone = f;
                            if (i > 1 && idxCompany == j) info.Company = f;
                        }

                        if (i > 1 && info.Name.Length > 0) al.Add(info);
                    }

                    DAL.CSV biz = new DAL.CSV();
                    biz.Add(al);

                    Response.Redirect("InOutStep2.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("csv upload fail", ex);
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
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "NoCsv", GetResource("NoCSV"), "");
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

                        SaveCSV(Server.MapPath(spoolPath + fileName));
                    }
                    catch (Exception ex)
                    {
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("file upload fail", ex);
                    }
                }
            }
        }

        #endregion
    }
}