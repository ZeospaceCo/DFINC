using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.EIP.BE;

namespace Zeospace.DeskPlus.EIP.Web.Admin.Company
{
    public partial class Add : Base.BasePageEIPAdmin
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        private string eventArgs = String.Empty;
        public int CompanyID;
        public int CurrentPage;
        public string RecordPerPage; //게시판 리스트 개수
        public string Order;
        //회사별 administrator 의 roleid 값 가져오기
        public int RoleAdmin = 0;
        #endregion

        #region Page Process (페이지 처리)
        #endregion

        #region Method (함수)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.EIP.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Admin.Company.Add).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                //슈퍼 관리자
                if (!CurrentUserInfo.IsSysAdmin)
                {
                    Response.Redirect("/DeskPlusEIP/");
                    return;
                }

                LoadData();
                SetLanguage();
            }
        }

        /// <summary>
        /// 작성목적   : 초기화 데이터 설정
        /// 작성자     : 
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            if (!CurrentUserInfo.IsSysAdmin) Response.Redirect("/DeskPlusEIP/");

            if (CompanyID > 0)
            {
                //회사별 administrator 의 roleid 값 가져오기
                DAL.Role roleDal = new DAL.Role();
                List<RoleInfo> roleList = roleDal.GetRole(CurrentUserInfo.CompanyID, "admin", "1");
                if (roleList.Count > 0) RoleAdmin = roleList[0].RoleID;

                List<Zeospace.DeskPlus.Common.BE.CompanyInfo> al = new List<DeskPlus.Common.BE.CompanyInfo>();
                Zeospace.DeskPlus.Common.DAL.Company dal = new DeskPlus.Common.DAL.Company();
                al = dal.GetCompany(CompanyID, false);

                if (al.Count > 0)
                {
                    foreach (Zeospace.DeskPlus.Common.BE.CompanyInfo info in al)
                    {
                        ctbCompanyName.Text = info.NameBase.ToReverseXSS();
                        ctbInterfaceCode.Text = info.InterfaceCode.ToReverseXSS();
                        ctbMuliLang.Text = GetMuiltiLang(
                            info.NameKo.ToReverseXSS(),
                            info.NameEn.ToReverseXSS(),
                            info.NameJa.ToReverseXSS(),
                            info.NameFr.ToReverseXSS(),
                            info.NameZhcn.ToReverseXSS(),
                            info.NameZhtw.ToReverseXSS());

                        if (info.EstablishDate.Trim().Length > 0)
                        {
                            ctbEstablishDate.Text = info.EstablishDate.Trim().Substring(0, 4) + "-" + info.EstablishDate.Trim().Substring(4, 2) + "-"
                                + info.EstablishDate.Trim().Substring(6, 2);
                        }

                        ctbInstitutorName.Text = info.InstitutorName.ToReverseXSS();
                        ctbInstitutorEngName.Text = info.InstitutorEnglishName.ToReverseXSS();
                        //ctbInstitutorRegNumber.Text = info.InstitutorRegistrationNumber;
                        ctbHomePage.Text = info.HomePage;
                        ctbZipCode.Text = info.ZipCode;
                        ctbAddress1.Text = info.Address1.ToReverseXSS();
                        ctbAddress2.Text = info.Address2.ToReverseXSS();
                        ctbEnglishAddress.Text = info.EnglishAddress.ToReverseXSS();
                        ctbPhone.Text = info.Phone;
                        ctbFax.Text = info.Fax;
                        cbxActive.Checked = (info.ActiveCompany == "1") ? true : false;
                        ctbCompanyQuota.Text = (info.CompanyQuota < 0) ? "" : info.CompanyQuota.ToString();
                        ctbLicenseCnt.Text = info.LicenseCnt.ToString();
                    }
                }
            }
            else
            {
                GetMultiCompany();
            }
        }


        /// <summary>
        /// 작성목적   : 파라미터 설정
        /// 작성자     : 
        /// </summary>
        private void LoadParam()
        {
            CompanyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            RecordPerPage = GetDecryptParam("recordPerPage", "5");
            Order = GetDecryptParam("order", "");
            Order = (Order.Length == 0) ? "CompanyID ASC" : Order;
        }

        /// <summary>
        /// 작성목적   : 다국어 설정
        /// 작성자     : 
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnList.Text = resMng.GetString("Cancel");
                cbtnMod.Text = (CompanyID > 0) ? resMng.GetString("Save") : resMng.GetString("Save");
                //cbtnPost.Text = "우편번호찾기";
                cbtnComUserAdd.Text = resMng.GetString("Add");
                cbtnComUserDel.Text = resMng.GetString("Del");
                cbtnDel.Text = resMng.GetString("Del");
            }
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 인사 등록하기
        /// </summary>
        /// <returns></returns>
        private int Register()
        {
            Zeospace.DeskPlus.Common.DAL.Company dal = new DeskPlus.Common.DAL.Company();
            Zeospace.DeskPlus.Common.BE.CompanyInfo info = new DeskPlus.Common.BE.CompanyInfo();
            info.CompanyID = 0;
            info.CompanyCode = "";
            info.InterfaceCode = ctbInterfaceCode.Text.Trim();
            info.NameBase = ctbCompanyName.Text;

            string multiName = ctbMuliLang.Text.Trim();            
            if (multiName != "" && multiName.IndexOf(",") != -1)
            {
                info.NameKo = multiName.Split(',')[0];
                info.NameEn = multiName.Split(',')[1];
                info.NameJa = multiName.Split(',')[2];
                info.NameFr = multiName.Split(',')[3];
                info.NameZhcn = multiName.Split(',')[4];
                info.NameZhtw = multiName.Split(',')[5];
            }

            info.EstablishDate = ctbEstablishDate.Text.Replace("-", "");
            info.InstitutorName = ctbInstitutorName.Text;
            info.InstitutorEnglishName = ctbInstitutorEngName.Text;
            //info.InstitutorRegistrationNumber = ctbInstitutorRegNumber.Text;
            info.IndustryClass = "";
            info.BusinessCondition = "";
            info.HomePage = ctbHomePage.Text;
            info.ZipCode = ctbZipCode.Text;
            info.Address1 = ctbAddress1.Text;
            info.Address2 = ctbAddress2.Text;
            info.EnglishAddress = ctbEnglishAddress.Text;
            info.Phone = ctbPhone.Text;
            info.Fax = ctbFax.Text;
            info.CompanyQuota = ctbCompanyQuota.Text.Trim().ToNumber<int>();
            info.ActiveCompany = (cbxActive.Checked) ? "1" : "0";
            info.WriteUserID = CurrentUserInfo.UserID;
            info.DefaultValue = ddlCompany.SelectedValue.ToNumber<int>();
            info.LicenseCnt = GetLicense();

            

            return dal.Add(info);

        }

        /// <summary>
        /// 라이센스 카운트 계산 및 가져오기
        /// </summary>
        /// <returns></returns>
        private int GetLicense()
        {
             var dal = new DeskPlus.Common.DAL.Company();
            int cnt = ctbLicenseCnt.Text.ToNumber<int>(); //관리자가 입력한 라이센스 수

            #region 라이센스 추가 (shimpark)
            var alList = dal.GetCompany(true);
            var zeoLicMng = new Interop.ZeoLicEp.LicenseManagerClass();
            var totLicense = zeoLicMng.GetUsers(4);	// 라이센스 사용자(10명 보너스)  GetUsers(4) <- 4 가 DeskPlusEP 제품번호

            var tot = 0;
            foreach (var comInfo in alList) tot += comInfo.LicenseCnt;
            //라이센스 초과일 경우
            if (tot >= totLicense)
            {
                cnt = 0;
            }
            else if (tot < totLicense)
            {
                //여분이 남았을 경우
                var balance = totLicense - tot; //잔여개수 계산
                //요청한 라이센스수가 잔여수보다 많으면 잔여수로 채운다.
                if (balance < cnt)
                {
                    cnt = balance;
                }
            }
            #endregion

            return cnt;
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 인사 수정하기
        /// </summary>
        /// <returns></returns>
        private bool Modify()
        {
            Zeospace.DeskPlus.Common.DAL.Company dal = new DeskPlus.Common.DAL.Company();
            Zeospace.DeskPlus.Common.BE.CompanyInfo info = new DeskPlus.Common.BE.CompanyInfo();
            info.CompanyID = CompanyID;
            info.CompanyCode = "";
            info.InterfaceCode = ctbInterfaceCode.Text.Trim();
            info.NameBase = ctbCompanyName.Text;

            string multiName = ctbMuliLang.Text.Trim();            

            if (multiName != "" && multiName.IndexOf(",") != -1)
            {
                info.NameKo = multiName.Split(',')[0];
                info.NameEn = multiName.Split(',')[1];
                info.NameJa = multiName.Split(',')[2];
                info.NameFr = multiName.Split(',')[3];
                info.NameZhcn = multiName.Split(',')[4];
                info.NameZhtw = multiName.Split(',')[5];
            }
            
            info.EstablishDate = ctbEstablishDate.Text.Replace("-","");
            info.InstitutorName = ctbInstitutorName.Text;
            info.InstitutorEnglishName = ctbInstitutorEngName.Text;
            //info.InstitutorRegistrationNumber = ctbInstitutorRegNumber.Text;
            info.IndustryClass = "";
            info.BusinessCondition = "";
            info.HomePage = ctbHomePage.Text;
            info.ZipCode = ctbZipCode.Text;
            info.Address1 = ctbAddress1.Text;
            info.Address2 = ctbAddress2.Text;
            info.EnglishAddress = ctbEnglishAddress.Text;
            info.Phone = ctbPhone.Text;
            info.Fax = ctbFax.Text;
            info.CompanyQuota = ctbCompanyQuota.Text.Trim().ToNumber<int>();
            info.ActiveCompany = (cbxActive.Checked) ? "1" : "0";
            info.WriteUserID = CurrentUserInfo.UserID;
            info.LicenseCnt = ctbLicenseCnt.Text.ToNumber<int>();

            return dal.Mod(info);

        }


        /// <summary>
        /// 작성자 : 심재운
        /// 인사 삭제하기
        /// </summary>
        /// <returns></returns>
        private bool Del()
        {
            Zeospace.DeskPlus.Common.DAL.Company dal = new DeskPlus.Common.DAL.Company();
            
            if (CompanyID > 0)
            {
                return dal.Del(CompanyID);
            }

            return false;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 회사 정보 가져오기
        /// </summary>
        private void GetMultiCompany()
        {

            Zeospace.DeskPlus.Common.DAL.Company biz = new Zeospace.DeskPlus.Common.DAL.Company();
            List<Zeospace.DeskPlus.Common.BE.CompanyInfo> al = biz.GetCompany(true);
            ddlCompany.Items.Clear();

            string companyName = "";
            foreach (Zeospace.DeskPlus.Common.BE.CompanyInfo info in al)
            {

                companyName = UtilHelper.GetLangName(
                    info.NameBase,
                    info.NameKo, info.NameEn, info.NameJa,
                    info.NameFr, info.NameZhcn,
                    info.NameZhtw, PreLang).ToReverseXSS();


                ddlCompany.Items.Add(new ListItem(companyName.ToReverseXSS(), info.CompanyID.ToString()));
            }
        }
        #endregion

        #region Event Handler (이벤트 핸들러)

        /// <summary>
        /// 추가
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnRegister_Click(object sender, EventArgs e)
        {

            try
            {
                LoadParam();

                string args = eventArgs;

                switch (args)
                {
                    case "Add":  //추가
                        if (CompanyID == 0)
                        {
                            int companyID = Register();
                            if (companyID > 0)
                            {
                                FileHelper.AddCompanyFolder(companyID);
                            }
                        }
                        break;
                    case "Del":  //삭제
                        if (CompanyID > 0)
                        {
                            Del();                            
                        }
                        break;
                    case "Mod":  //수정
                        if (CompanyID > 0)
                        {
                            Modify();
                        }
                        break;
                }

                Dictionary<string, string> paraDic = new Dictionary<string, string>();

                paraDic.Add("companyID", "0");
                paraDic.Add("currentPage", CurrentPage.ToString());
                paraDic.Add("recordPerPage", RecordPerPage.ToString());
                paraDic.Add("order", Order);
                paraDic.Add("alertMsg", GetResource("GoList"));  //"목록으로 이동합니다."
                paraDic.Add("goURL", "/DeskPlusEIP/EIP/Admin/Company/List.aspx");

                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                Utility.Redirect(encParams);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnRegister_Click", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }
        }


        /// <summary>
        /// 들어오는 포스트백 이벤트를 처리하도록 포스트백을 발생시킨 서버 컨트롤에 알립니다.
        /// </summary>
        /// <param name="sourceControl">포스트백을 발생시킨 ASP.NET 서버 컨트롤입니다.이 컨트롤은 <see cref="T:System.Web.UI.IPostBackEventHandler"/> 인터페이스를 구현해야 합니다.</param>
        /// <param name="eventArgument">포스트백 인수입니다.</param>
        protected override void RaisePostBackEvent(IPostBackEventHandler sourceControl, string eventArgument)
        {
            eventArgs = eventArgument;
            base.RaisePostBackEvent(sourceControl, eventArgument);
        }
        #endregion

        #region Property (프로퍼티)

        #endregion
    }
}