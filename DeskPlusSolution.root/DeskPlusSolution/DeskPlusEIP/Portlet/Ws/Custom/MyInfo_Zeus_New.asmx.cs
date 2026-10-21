using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Data.SqlClient;
using System.Configuration;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base;

namespace Zeospace.DeskPlus.Portlet.Web.Ws.Custom
{
    /// <summary>
    /// Opinion의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class MyInfo_Zeus_New : Base.BaseWSPortlet
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
        public List<MyInfoEntity_New> GetMyInfo(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Ws.Custom.MyInfo_Zeus_New).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion


            try
            {
                DAL.MyInfo myInfoDal = new DAL.MyInfo();

                #region 일정관리, 전자결재 건수

                DataTableReader dtr = myInfoDal.GetCountList(CurrentUserInfo.CompanyID, CurrentUserInfo.DeptID, CurrentUserInfo.UserID);
                DataTable myInfoTable = new DataTable();
                myInfoTable.Load(dtr);

                int cntWB = myInfoTable.Select("CAT = 'WB'").Length; //오늘의 할일
                int cntWS = myInfoTable.Select("CAT = 'WS'").Length;  //오늘의 일정
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
                #endregion

                #region 메일건수
                int cntMail = 0;

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
                #endregion

                MyInfoEntity_New be = new MyInfoEntity_New();

                #region MyInfo용
                be.NewMailStr = cntMail.ToString(); //신규메일
                be.TodayPimSchStr = cntWS.ToString(); //오늘일정
                be.TodayPimDoStr = cntWB.ToString();  //오늘할일
                //be.NewReplyCnt = 53;    //답글
                be.DocmentStr = cntnoSC.ToString();     //상신문서
                be.ApprovalStr = cntSM.ToString();    //결재문서 - 결재할 문서
                be.DeptSusinStr = cntSP.ToString();   //대결문서
                be.CompleteStr = cntCP.ToString();    //수신문서 - 수신받은 문서
                #endregion

                #region Menu용
                be.NewMailCnt = this.GetResource("NewMail") + " (" + cntMail + ")"; //신규메일(3)
                be.TodayPimSchCnt = this.GetResource("TodaySchedule") + " (" + cntWS + ")"; //오늘일정
                be.TodayPimDoCnt = this.GetResource("TodayWork") + " (" + cntWB + ")";  //오늘할일
                //be.NewReplyCnt = 53;    //답글
                be.DocmentCnt = this.GetResource("Saction") + " (" + cntnoSC + ")";     //상신문서
                be.ApprovalCnt = this.GetResource("CompleteDoc") + " (" + cntSM + ")";    //결재문서 - 결재할 문서
                be.DeptSusinCnt = this.GetResource("DraftDoc") + " (" + cntSP + ")";   //대결문서
                be.CompleteCnt = this.GetResource("DeptDoc") + " (" + cntCP + ")";    //수신문서 - 수신받은 문서
                #endregion
                
                
                DataSet ds = GetErpCountList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
                int MBO = 0;
                int VoluntaryService = 0;
                int TotalCheckup = 0;

                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        be.Proposal = dr["Proposal"].ToString().ToNumber<int>();
                        MBO = dr["MBO"].ToString().ToNumber<int>();
                        VoluntaryService = dr["VoluntaryService"].ToString().ToNumber<int>();
                        TotalCheckup = dr["TotalCheckup"].ToString().ToNumber<int>();
                    }

                    be.MBO = MBO == 1 ? this.GetResource("Target") : this.GetResource("Non_Target");
                    be.VoluntaryService = VoluntaryService == 1 ? this.GetResource("Target") : this.GetResource("Non_Target");
                    be.TotalCheckup = TotalCheckup == 1 ? this.GetResource("Target") : this.GetResource("Non_Target");

                }
                
                /*
                be.Proposal = 0 ;
                be.MBO = "2";
                be.VoluntaryService = "3";
                be.TotalCheckup = "4";
               */

                List<MyInfoEntity_New> al = new List<MyInfoEntity_New>();
                al.Add(be);
                return al;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetMyInfo : " + ex);
                throw;
            }
        }


        #region 제우스 추가개발
        public DataSet GetErpCountList(int companyID, int userID)
        {


            try
            {

                SqlConnection mySqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnSolution"].ConnectionString);


                SqlCommand mySqlCommand = new SqlCommand("KMS_Today", mySqlConnection);

                mySqlCommand.CommandType = CommandType.StoredProcedure;

                mySqlCommand.Parameters.Add("@userid", SqlDbType.Int).Value = userID;

                SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter();
                mySqlDataAdapter.SelectCommand = mySqlCommand;


                DataSet myDataSet = new DataSet();


                mySqlConnection.Open();

                mySqlDataAdapter.Fill(myDataSet, "Products");




                mySqlConnection.Close();

                return myDataSet;


            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetErpCountList : " + ex);
                throw;
            }

        } 
        #endregion



    }


    /// <summary>
    /// MyInfoEntity
    /// </summary>
    [Serializable]
    public class MyInfoEntity_New
    {
        public MyInfoEntity_New() { }

        public string NewMailCnt { get; set; }
        public string TodayPimSchCnt { get; set; }
        public string TodayPimDoCnt { get; set; }
        public string NewReplyCnt { get; set; }
        public string DocmentCnt { get; set; }
        public string ApprovalCnt { get; set; }
        public string DeptSusinCnt { get; set; }
        public string CompleteCnt { get; set; }

        public string NewMailStr { get; set; }
        public string TodayPimSchStr { get; set; }
        public string TodayPimDoStr { get; set; }
        public string NewReplyStr { get; set; }
        public string DocmentStr { get; set; }
        public string ApprovalStr { get; set; }
        public string DeptSusinStr { get; set; }
        public string CompleteStr { get; set; }

        public int Proposal { get; set; }
        public string MBO { get; set; }
        public string VoluntaryService { get; set; }
        public string TotalCheckup { get; set; }
       

    }
}
