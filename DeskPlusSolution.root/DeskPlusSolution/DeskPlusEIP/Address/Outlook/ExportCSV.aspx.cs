using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Address.Web.Outlook
{
    /// <summary>
    /// 주소록 내보내기
    /// </summary>
    public partial class ExportCSV : Base.BasePageAddress
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Address.Web.Outlook.ExportCSV).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {   
                SetLanguage();
                LoadData();
            }
        }

        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)


        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {

            }
        }

        /// <summary>
        /// 초기화
        /// </summary>
        private void LoadData()
        {

        }

        #endregion

        #region (함수)  

        /// <summary>
        /// 작성자  : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 헤더
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <returns></returns>
        private string GetHeader()
        {
            string header = string.Empty;
            string tmpHeader = string.Empty;
            string gubun = tbTypes.Text.Trim();


            switch (gubun)
            {
                case "google":   // 구글
                    header = "Name,Given Name,Additional Name,Family Name,Yomi Name,Given Name Yomi,Additional Name Yomi,Family Name Yomi,Name Prefix,Name Suffix,Initials,Nickname,Short Name,Maiden Name,Birthday,Gender,Location,Billing Information,Directory Server,Mileage,Occupation,Hobby,Sensitivity,Priority,Subject,Notes,Group Membership,E-mail 1 - Type,E-mail 1 - Value,Phone 1 - Type,Phone 1 - Value,Phone 2 - Type,Phone 2 - Value,Phone 3 - Type,Phone 3 - Value,Organization 1 - Type,Organization 1 - Name,Organization 1 - Yomi Name,Organization 1 - Title,Organization 1 - Department,Organization 1 - Symbol,Organization 1 - Location,Organization 1 - Job Description";
                    break;
                case "outlook":   // 아웃룩
                    if (PreLang == "ko")
                    {
                        header = "이름,전자 메일 주소,휴대폰,집 전화 번호,회사 기본 전화,회사";
                        foreach (string str in header.Split(',')) tmpHeader += "\"" + str + "\"" + ",";
                        header = tmpHeader.TrimEnd(',');
                    }
                    else
                    {
                        header = "Full Name,E-mail Address,Mobile Phone,Home Phone,Business Phone,Company";
                        foreach (string str in header.Split(',')) tmpHeader += "\"" + str + "\"" + ",";
                        header = tmpHeader.TrimEnd(',');
                    }

                    break;
                case "express":   // 익스프레스
                    if (PreLang == "ko")
                    {
                        header = "전체 이름,전자 메일 주소,휴대폰,집 전화,회사 전화,회사";
                    }
                    else
                    {
                        header = "Full Name,E-mail Address,Mobile Phone,Home Phone,Business Phone,Office";
                    }
                    break;
                case "naver":   // 네이버
                    header = "이름,이메일,전화번호";
                    break;
                case "daum":   // 다음
                    header = "이름,이메일,휴대폰,집전화,회사전화,회사";
                    break;
                default:
                    header = "이름,전자 우편 주소,휴대용 전화,집 전화 번호,회사 기본 전화,회사";
                    foreach (string str in header.Split(',')) tmpHeader += "\"" + str + "\"" + ",";
                    header = tmpHeader.TrimEnd(',');
                    break;

            }

            return header;
        }
      
        /// <summary>
        /// 작성자  : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : csv 생성하기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        private void CreateCSV()
        {
            string gubun = tbTypes.Text.Trim();
            string csvFileName = "AddressBook-" + gubun+ ".csv";

            Response.Clear();
            Response.ContentType = "Application/x-msexcel";
            Response.AddHeader("content-disposition", "attachment; filename=\"" + csvFileName + "\"");
            //이거 해줘야~~~ 한글 안깨짐니더...ㅡ.ㅡ;;
            Response.ContentEncoding = System.Text.UnicodeEncoding.Default;

            //Request.ContentEncoding = System.Text.Encoding.GetEncoding("euc-kr");
            //Response.ContentEncoding = System.Text.Encoding.GetEncoding("euc-kr");
            //Response.HeaderEncoding = System.Text.Encoding.GetEncoding("euc-kr");            

            DAL.CSV dal = new DAL.CSV();
            DataTableReader dtr = dal.GetPrivateAddress(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            Response.Write(GetHeader());
            Response.Write(Environment.NewLine);

            string nameBase = string.Empty;
            string mail = string.Empty;
            string mobilePhone = string.Empty;
            string homePhone = string.Empty;
            string companyPhone = string.Empty;
            string companyBase = string.Empty;

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        nameBase = GetLangName(
                            dtr["NameBase"].ToString().ToNull().ToReverseXSS().Replace("'", "").Replace("\"", "").Replace(",", ""),
                            dtr["NameKo"].ToString().ToNull().ToReverseXSS().Replace("'", "").Replace("\"", "").Replace(",", ""),
                            dtr["NameEn"].ToString().ToNull().ToReverseXSS().Replace("'", "").Replace("\"", "").Replace(",", ""),
                            dtr["NameJa"].ToString().ToNull().ToReverseXSS().Replace("'", "").Replace("\"", "").Replace(",", ""),
                            dtr["NameFr"].ToString().ToNull().ToReverseXSS().Replace("'", "").Replace("\"", "").Replace(",", ""),
                            dtr["NameZhCn"].ToString().ToNull().ToReverseXSS().Replace("'", "").Replace("\"", "").Replace(",", ""),
                            dtr["NameZhTw"].ToString().ToNull().ToReverseXSS().Replace("'", "").Replace("\"", "").Replace(",", ""), PreLang);

                        mail = dtr["Mail"].ToString().ToNull().Replace("'", "").Replace("\"", "").Replace(",", "");
                        mobilePhone = dtr["MobilePhone"].ToString().ToNull().Replace("'", "").Replace("\"", "").Replace(",", "");
                        homePhone = dtr["HomePhone"].ToString().ToNull().Replace("'", "").Replace("\"", "").Replace(",", "");
                        companyPhone = dtr["CompanyPhone"].ToString().ToNull().Replace("'", "").Replace("\"", "").Replace(",", "");

                        companyBase = GetLangName(
                            dtr["CompanyBase"].ToString().ToNull().ToReverseXSS().Replace("'", "").Replace("\"", "").Replace(",", ""),
                            dtr["CompanyKo"].ToString().ToNull().ToReverseXSS().Replace("'", "").Replace("\"", "").Replace(",", ""),
                            dtr["CompanyEn"].ToString().ToNull().ToReverseXSS().Replace("'", "").Replace("\"", "").Replace(",", ""),
                            dtr["CompanyJa"].ToString().ToNull().ToReverseXSS().Replace("'", "").Replace("\"", "").Replace(",", ""),
                            dtr["CompanyFr"].ToString().ToNull().ToReverseXSS().Replace("'", "").Replace("\"", "").Replace(",", ""),
                            dtr["CompanyZhCn"].ToString().ToNull().ToReverseXSS().Replace("'", "").Replace("\"", "").Replace(",", ""),
                            dtr["CompanyZhTw"].ToString().ToNull().ToReverseXSS().Replace("'", "").Replace("\"", "").Replace(",", ""), PreLang);

                        // 아웃룩만
                        if (gubun == "outlook")
                        {
                            Response.Write("\"" + nameBase + "\"" + ",");
                            Response.Write("\"" + mail + "\"" + ",");
                            Response.Write("\"" + mobilePhone + "\"" + ",");
                            Response.Write("\"" + homePhone + "\"" + ",");
                            Response.Write("\"" + companyPhone + "\"" + ",");
                            Response.Write("\"" + companyBase + "\"");
                        }
                        else if (gubun == "google")
                        {
                            //홍길똥2,,,,,,,,,,,,,,,,,,,,,,,,,,,* Work,compahy@naver.com,Home,02-781-122,Mobile,010-8578-1122,Work,02-2108-3081,,제오스페이스,,,,,,
                            string googleFormat = "{0},,,,,,,,,,,,,,,,,,,,,,,,,,,* Work,{1},Home,{2},Mobile,{3},Work,{4},,{5},,,,,,";
                            googleFormat = string.Format(googleFormat, nameBase, mail, homePhone, mobilePhone, companyPhone, companyBase);
                            Response.Write(googleFormat);
                        }
                        else if (gubun == "naver")   //네이버
                        {
                            Response.Write(nameBase + ",");
                            Response.Write(mail + ",");
                            Response.Write(mobilePhone);
                        }
                        else   //익스프레스, 다음
                        {
                            Response.Write(nameBase + ",");
                            Response.Write(mail + ",");
                            Response.Write(mobilePhone + ",");
                            Response.Write(homePhone + ",");
                            Response.Write(companyPhone + ",");
                            Response.Write(companyBase);
                        }

                        Response.Write(Environment.NewLine);
                    }

                    
                }
            }

            Response.End();
        }

        #endregion

        #region (속성)        

        #endregion

        #region (이벤트핸들러)

        /// <summary>
        /// CSV 파일 내보내기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnSubmit_Click(object sender, EventArgs e)
        {
            CreateCSV();
        }

        #endregion
    }
}