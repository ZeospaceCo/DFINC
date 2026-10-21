using System;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Portlet.Web.Package
{
    public partial class MyAppCount : Base.BaseControlPortlet
    {
        public string ApprovalStatusMenuUrl = "/DeskPlusEIP/Approval/Status/Default.aspx";

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Package.MyAppCount).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        #endregion Page Process (페이지 처리)

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        public void LoadData()
        {
            SetApprovalStatusMenuUrl();
            GetMyAppCount();
        }

        #endregion DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        #region (함수)

        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
            }
        }

        private void SetApprovalStatusMenuUrl()
        {
            Zeospace.DeskPlus.Account.DAL.UserEnvironment dal = new Zeospace.DeskPlus.Account.DAL.UserEnvironment();
            System.Collections.Generic.List<Zeospace.DeskPlus.Account.BE.UserEnvironmentInfo> list = dal.GetUserEnv(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
            if (list != null && list.Count > 0 && "NEW".Equals(list[0].ApprovalStatusMenuType))
            {
                ApprovalStatusMenuUrl = "/DeskPlusEIP/Approval/Status/DefaultAll.aspx";
            }
        }

        public void GetMyAppCount()
        {
            DAL.MyInfo dalMyinfo = new DAL.MyInfo();

            AppnoSCCnt = 0;
            AppCompCnt = 0;
            AppRejectCnt = 0;
            AppRejectOtherCnt = 0;
            AppReqCnt = 0;
            AppRecvReqCnt = 0;
            AppProxyCnt = 0;
            AppEnforceSealCnt = 0;
            AppSCUCnt = 0;
            AppCRCnt = 0;
            PimCnt = 0;
            TASKCnt = 0;

            #region 일정관리, 전자결재 건수

            DAL.MyInfo myInfoDal = new DAL.MyInfo();

            DataTableReader dtr = myInfoDal.GetCountList(CurrentUserInfo.CompanyID, CurrentUserInfo.DeptID, CurrentUserInfo.UserID);
            DataTable myInfoTable = new DataTable();
            myInfoTable.Load(dtr);

            int cntWB = myInfoTable.Select("CAT = 'WB'").Length; //오늘의 할일
            int cntWS = myInfoTable.Select("CAT = 'WS'").Length;  //오늘의 일정

            int cntPim = cntWB + cntWS;

            int cntSC = myInfoTable.Select("CAT = 'SC'").Length;  //전자결재 완결
            //int cntSCC = myInfoTable.Select("CAT = 'SCC'").Length;  //전자결재 상신문서 
            int cntSR = myInfoTable.Select("CAT = 'SR'").Length;  //전자결재 상신자반송
            int cntSRS = myInfoTable.Select("CAT = 'SRS'").Length; //전자결재 결재자반송
            int cntSM = myInfoTable.Select("CAT = 'SM'").Length;  //전자결재 결재대상
            int cntCP = myInfoTable.Select("CAT = 'CP'").Length;  //전자결재 수신문서 요청
            int cntSCU = myInfoTable.Select("CAT = 'SCU'").Length; //전자결재 참조문서함
            int cntSP = myInfoTable.Select("CAT = 'SP'").Length;  //전자결재 대결자문서
            //int cntEC = myInfoTable.Select("CAT = 'EC'").Length;  //전자결재 시행문 확인
            int cntES = myInfoTable.Select("CAT = 'ES'").Length;  //전자결재 시행문 직인
            int cntCR = myInfoTable.Select("CAT = 'CR'").Length;  //전자결재 받은회람
            int cntnoSC = myInfoTable.Select("CAT = 'noSC'").Length;  //전자결재 진행중인문서(상신문서)

            int cntTTO = myInfoTable.Select("CAT = 'TTO'").Length;  //업무협업 (요청받은 협업)
            int cntTCCU = myInfoTable.Select("CAT = 'TCCU'").Length;  //업무협업 (참조받은 협업-개인)
            int cntTCCD = myInfoTable.Select("CAT = 'TCCD'").Length;  //업무협업 (참조받은 협업-부서)
            int cntTFO = myInfoTable.Select("CAT = 'TFO'").Length;  //업무협업 (전달받은 협업)

            int cntTASK = cntTTO + cntTCCU + cntTCCD + cntTFO;

            #endregion

            //웹메일(DEEP)
            WebMailCnt = GetWebMailCount();

            //오늘일정
            PimCnt = cntPim;

            //업무협업
            TASKCnt = cntTASK;

            #region 전자결재

            //결재 진행
            AppnoSCCnt = cntnoSC;

            //결재 완결
            AppCompCnt = cntSC;

            //결재 반송(상신자 반송)
            AppRejectCnt = cntSR;

            //결재 반송(결재자 반송)
            AppRejectOtherCnt = cntSRS;

            //결재 상신 요청
            AppReqCnt = cntSM;

            //결재 수신-상신 요청
            AppRecvReqCnt = cntCP;

            //결재 = 대결
            AppProxyCnt = cntSP;

            //결재 = 참조
            AppSCUCnt = cntSCU;

            //결재 = 받은회람
            AppCRCnt = cntCR;

            #endregion 전자결재

            //시행문 신청리스트
            AppEnforceSealCnt = cntES;

        }

        /// <summary>
        /// 신규 메일 건수 가져오기(안읽은 ㅡㅡ;)
        /// 심재운
        /// </summary>
        /// <returns></returns>
        private int GetWebMailCount()
        {
            int cntMail = 0;

            #region 메일건수

            DAL.MyInfo myInfoDal = new DAL.MyInfo();
            DataTableReader dtrMail = null;
            string mailProduct = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailProduct", "");
            if (mailProduct.ToUpper().IndexOf("ZETTA").Equals(0))
            {
                if (mailProduct.ToUpper() == "ZETTA65")
                {
                    dtrMail = myInfoDal.GetMailCount(CurrentUserInfo.CompanyID, CurrentUserInfo.MailID, CurrentUserInfo.MailDomain);
                }
                else if (mailProduct.ToUpper() == "ZETTA52")
                {
                    dtrMail = myInfoDal.GetMailCount52(CurrentUserInfo.CompanyID, CurrentUserInfo.MailID);
                }

                if (dtrMail.HasRows) while (dtrMail.Read()) cntMail = dtrMail["Cnt"].ToString().ToNumber<int>();
            }
            else if (mailProduct.ToUpper().IndexOf("DEEP").Equals(0))
            {
                //DEEP 메일건수 연동
                string mailpw = "";
                Zeospace.DeskPlus.Account.DAL.User user = new Account.DAL.User();
                var userList = user.GetUserInfo(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, true, true);
                foreach (var userInfo in userList)
                {
                    mailpw = (userInfo.MailPassword.Equals("")) ? userInfo.Password : userInfo.MailPassword;
                }

                Zeospace.DeskPlus.Common.BLL.MailDeepFactory bizMail = new Common.BLL.MailDeepFactory();
                //받은편지함 안읽은 메일 건수(포틀릿)
                cntMail = bizMail.GetBasicNoReadCnt(CurrentUserInfo.CompanyID, CurrentUserInfo.MailID, CurrentUserInfo.MailDomain, mailpw);
            }

            #endregion 메일건수

            return cntMail;
        }

        #endregion (함수)

        #region (프로퍼티)

        //결재진행
        public int AppnoSCCnt { get; set; }

        //결재완결
        public int AppCompCnt { get; set; }

        //결재 반송(상신자 반송)
        public int AppRejectCnt { get; set; }

        //결재 반송(결재자 반송)
        public int AppRejectOtherCnt { get; set; }

        //결재요청
        public int AppReqCnt { get; set; }

        //수신요청
        public int AppRecvReqCnt { get; set; }

        //결재대결
        public int AppProxyCnt { get; set; }

        //결재참조
        public int AppSCUCnt { get; set; }

        //결재받은회람
        public int AppCRCnt { get; set; }

        //시행직인
        public int AppEnforceSealCnt { get; set; }

        //나의할일+나의일정
        public int PimCnt { get; set; }

        //업무협업
        public int TASKCnt { get; set; }
        
        //웹메일
        public int WebMailCnt { get; set; }

        #endregion (프로퍼티)
    }

    /// <summary>
    /// BPM 나의 업무
    /// </summary>
    struct TodayEntity
    {
        public string newWork { get; set; }

        public string savedWork { get; set; }

        public string completedWork { get; set; }

        public string completedProcess { get; set; }
    }
}
