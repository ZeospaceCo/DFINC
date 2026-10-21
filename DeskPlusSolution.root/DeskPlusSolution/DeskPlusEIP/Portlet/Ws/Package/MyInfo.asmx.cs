using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Portlet.Web.Ws.Package
{
    /// <summary>
    /// Opinion의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class MyInfo : Base.BaseWSPortlet
    {
        /// <summary>
        /// 메모 추가
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Add(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);
            
            int companyID = GetDecryptParam("companyID", "").ToNumber<int>();
            int userID = GetDecryptParam("userID", "0").ToNumber<int>();

            try
            {
                //if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
                //{
                //    if (roleList.Length > 0)
                //    {
                //        List<BE.TabRoleInfo> al = new List<BE.TabRoleInfo>();

                //        foreach (string _authValue in roleList.Split(','))
                //        {
                //            BE.TabRoleInfo info = new BE.TabRoleInfo();
                //            info.CompanyID = CurrentUserInfo.CompanyID;
                //            info.TabID = tabID;
                //            info.RoleID = _authValue.ToNumber<int>();

                //            al.Add(info);
                //        }

                //        BLL.TabRole biz = new BLL.TabRole();
                //        biz.AddTx(al);

                //        return true;
                //    }
                //}

                return false;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddTabRole : " + ex);
                return false;
            }
        }



        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2012-05-02
        /// 작성목적 : 메일, 일정, 결재 건수 가져오기 
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<MyInfoEntity> GetMyInfo(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Ws.Package.MyInfo).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion
            

            try
            {
                DAL.MyInfo myInfoDal = new DAL.MyInfo();
                DAL.TodoPopup todoPopupDal = new DAL.TodoPopup();

                #region 일정관리, 전자결재 건수

                DataTableReader dtr = myInfoDal.GetCountList(CurrentUserInfo.CompanyID, CurrentUserInfo.DeptID, CurrentUserInfo.UserID);
                DataTable myInfoTable = new DataTable();
                myInfoTable.Load(dtr);

                int cntWB = myInfoTable.Select("CAT = 'WB'").Length; //오늘의 할일
                int cntWS = todoPopupDal.GetPimScheduleListCnt(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID, DateTime.Now); //오늘의 일정(공유일정 포함)

                int cntPim = cntWB + cntWS;

                //int cntSC = myInfoTable.Select("CAT = 'SC'").Length;  //전자결재 완결
                int cntSCC = myInfoTable.Select("CAT = 'SCC'").Length;  //전자결재 상신문서 
                //int cntSR = myInfoTable.Select("CAT = 'SR'").Length;  //전자결재 상신자반송
                //int cntSRS = myInfoTable.Select("CAT = 'SRS'").Length; //전자결재 결재자반송
                int cntSM = myInfoTable.Select("CAT = 'SM'").Length;  //전자결재 결재대상
                int cntCP = myInfoTable.Select("CAT = 'CP'").Length;  //전자결재 수신문서 요청
                //int cntSCU = myInfoTable.Select("CAT = 'SCU'").Length; //전자결재 참조문서함
                int cntSP = myInfoTable.Select("CAT = 'SP'").Length;  //전자결재 대결자문서
                //int cntEC = myInfoTable.Select("CAT = 'EC'").Length;  //전자결재 시행문 확인
                //int cntES = myInfoTable.Select("CAT = 'ES'").Length;  //전자결재 시행문 직인
                //int cntCR = myInfoTable.Select("CAT = 'CR'").Length;  //전자결재 받은회람
                int cntnoSC = myInfoTable.Select("CAT = 'noSC'").Length;  //전자결재 진행중인문서(상신문서)

                int cntTTO = myInfoTable.Select("CAT = 'TTO'").Length;  //업무협업 (요청받은 협업)
                int cntTCCU = myInfoTable.Select("CAT = 'TCCU'").Length;  //업무협업 (참조받은 협업-개인)
                int cntTCCD = myInfoTable.Select("CAT = 'TCCD'").Length;  //업무협업 (참조받은 협업-부서)
                int cntTFO = myInfoTable.Select("CAT = 'TFO'").Length;  //업무협업 (전달받은 협업)

                int cntTASK = cntTTO + cntTCCU + cntTCCD + cntTFO;
                #endregion

                #region 메일건수
                int cntMail = 0;

                if (!String.IsNullOrEmpty(CurrentUserInfo.MailID))
                {
                    if (!String.IsNullOrEmpty(CurrentUserInfo.MailDomain))
                    {
                        string mailProduct = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailProduct", "");
                        if (mailProduct.ToUpper() == "ZETTA65")
                        {
                            DataTableReader dtrMail = myInfoDal.GetMailCount(CurrentUserInfo.CompanyID, CurrentUserInfo.MailID, CurrentUserInfo.MailDomain);
                            if (dtrMail != null)
                            {
                                if (dtrMail.HasRows)
                                {
                                    while (dtrMail.Read())
                                    {
                                        cntMail = dtrMail["cnt"].ToString().ToNumber<int>();
                                    }
                                }
                            }
                        }
                        else if (mailProduct.ToUpper() == "ZETTA52")
                        {
                            DataTableReader dtrMail = myInfoDal.GetMailCount52(CurrentUserInfo.CompanyID, CurrentUserInfo.MailID);
                            if (dtrMail != null)
                            {
                                if (dtrMail.HasRows)
                                {
                                    while (dtrMail.Read())
                                    {
                                        cntMail = dtrMail["Cnt"].ToString().ToNumber<int>();
                                    }
                                }
                            }
                        }
                        else if (mailProduct.ToUpper().IndexOf("DEEP").Equals(0))
                        {
                            //DEEP 메일건수 연동
                            string mailpw = "";
                            Zeospace.DeskPlus.Account.DAL.User user = new Account.DAL.User();
                            List<Zeospace.DeskPlus.Account.BE.UserInfo> userList = user.GetUserInfo(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, true, true);
                            foreach (Zeospace.DeskPlus.Account.BE.UserInfo userInfo in userList)
                            {
                                mailpw = (userInfo.MailPassword.Equals("")) ? userInfo.Password : userInfo.MailPassword;
                            }

                            Zeospace.DeskPlus.Common.BLL.MailDeepFactory bizMail = new Common.BLL.MailDeepFactory();
                            //받은편지함 안읽은 메일 건수(포틀릿)
                            cntMail = bizMail.GetBasicNoReadCnt(CurrentUserInfo.CompanyID, CurrentUserInfo.MailID, CurrentUserInfo.MailDomain, mailpw);
                        }
                    }
                }

                #endregion

                MyInfoEntity be = new MyInfoEntity();
                
                #region MyInfo용
                be.NewMailStr = cntMail.ToString(); //신규메일
                be.TodayPimSchStr = cntPim.ToString(); //오늘일정 + 오늘할일
                //be.TodayPimDoStr = cntWB.ToString();  //오늘할일                
                be.DocmentStr = cntnoSC.ToString();     //상신문서
                be.ApprovalStr = cntSM.ToString();    //결재문서 - 결재할 문서
                be.DeptSusinStr = cntSP.ToString();   //대결문서
                be.CompleteStr = cntCP.ToString();    //수신문서 - 수신받은 문서
                be.TaskStr = cntTASK.ToString(); // 업무협업
                #endregion

                #region Menu용
                be.NewMailCnt = this.GetResource("NewMail") + " (" + cntMail + ")"; //신규메일
                be.TodayPimSchCnt = this.GetResource("TodaySchedule") + " (" + cntPim + ")"; //오늘일정
                //be.TodayPimDoCnt = this.GetResource("TodayWork") + " (" + cntWB + ")";  //오늘할일                
                be.DocmentCnt = this.GetResource("Saction") + " (" + cntnoSC + ")";     //상신문서
                be.ApprovalCnt = this.GetResource("CompleteDoc") + " (" + cntSM + ")";    //결재문서 - 결재할 문서
                be.DeptSusinCnt = this.GetResource("DraftDoc") + " (" + cntSP + ")";   //대결문서
                be.CompleteCnt = this.GetResource("DeptDoc") + " (" + cntCP + ")";    //수신문서 - 수신받은 문서
                be.TaskCnt = this.GetResource("Task") + " (" + cntTASK + ")";    //업무협업
                #endregion

                List<MyInfoEntity> al = new List<MyInfoEntity>();
                al.Add(be);
                return al;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetMyInfo : " + ex);
                throw;
            }
        }


    }

    /// <summary>
    /// MyInfoEntity
    /// </summary>
    [Serializable]    
    public class MyInfoEntity
    {
        public MyInfoEntity() { }

        public string NewMailCnt { get; set; }
        public string TodayPimSchCnt { get; set; }
        public string TodayPimDoCnt { get; set; }
        public string NewReplyCnt { get; set; }
        public string DocmentCnt { get; set; }
        public string ApprovalCnt { get; set; }
        public string DeptSusinCnt { get; set; }
        public string CompleteCnt { get; set; }
        public string TaskCnt { get; set; }

        public string NewMailStr { get; set; }
        public string TodayPimSchStr { get; set; }
        public string TodayPimDoStr { get; set; }
        public string NewReplyStr { get; set; }
        public string DocmentStr { get; set; }
        public string ApprovalStr { get; set; }
        public string DeptSusinStr { get; set; }
        public string CompleteStr { get; set; }
        public string TaskStr { get; set; }
    }
}
