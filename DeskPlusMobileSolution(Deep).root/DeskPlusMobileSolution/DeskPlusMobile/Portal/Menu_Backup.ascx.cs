using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;
using System.Xml;

namespace Zeospace.DeskPlusMobile.Web.Portal
{
    public partial class Menu_Backup : Zeospace.DeskPlusMobile.Web.Helper.MobileBaseControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Portal.Portal).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadAuth();
                LoadCnt();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadAuth()
        {
            string pimsCheck = String.Empty;

            maMail.Visible = false;
            maApproval.Visible = false;
            maNotice.Visible = false;
            maPims.Visible = false;
            maAddress.Visible = false;
            maWebHard.Visible = false;
            maCommunity.Visible = false;
            maMyTalk.Visible = false;
            maTask.Visible = false;
            maRM.Visible = false;
            maAttend.Visible = false;

            if (UserInfo.MenuRoles != null)
            {
                for (int i = 0; i < UserInfo.MenuRoles.Length; i++)
                {
                    if (UserInfo.MenuRoles[i] == "1")
                    {
                        maMail.Visible = true;
                    }

                    if (UserInfo.MenuRoles[i] == "2")
                    {
                        maApproval.Visible = true;
                    }

                    if (UserInfo.MenuRoles[i] == "3")
                    {
                        maNotice.Visible = true;
                    }

                    if (UserInfo.MenuRoles[i] == "4")
                    {
                        maPims.Visible = true;
                        pimsCheck = "Y";
                    }

                    if (UserInfo.MenuRoles[i] == "5")
                    {
                        maAddress.Visible = true;
                    }

                    if (UserInfo.MenuRoles[i] == "6")
                    {
                        maWebHard.Visible = true;
                    }

                    if (UserInfo.MenuRoles[i] == "7")
                    {
                        maCommunity.Visible = true;
                    }

                    if (UserInfo.MenuRoles[i] == "8")
                    {
                        maMyTalk.Visible = true;
                    }

                    if (UserInfo.MenuRoles[i] == "10")
                    {
                        maTask.Visible = true;
                    }

                    if (UserInfo.MenuRoles[i] == "11")
                    {
                        maRM.Visible = true;                        
                    }

                    if (UserInfo.MenuRoles[i] == "12")
                    {
                        maAttend.Visible = true;
                    }                    
                }
            }
        }

        private void LoadCnt()
        {
            try
            {
                #region 메일 건수 (받은 메일함+개인메일함 읽지 않은 메일 건수)
                string useMail = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "CONFIG_USEMAIL");
                if (useMail.ToUpper() == "TRUE")
                {
                    if (maMail.Visible)
                    {
                        string paramMailCount = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Mail, "ParamMailCount");
                        string param = string.Format(paramMailCount, UserInfo.MailDomain, UserInfo.MailID, UserInfo.MailPassword, "basic", "4", "all", "");

                        string mailWebServiceUrl = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Mail, "MailWebServiceUrl");

                        string sXmlData = UrlSendHelper.UsePost(mailWebServiceUrl, param);

                        if (sXmlData != "")
                        {
                            XmlDocument xml = new XmlDocument();
                            xml.LoadXml(sXmlData);

                            string returnCode = xml.SelectSingleNode("/data/returnCode").InnerText;

                            if (returnCode == "0")
                            {
                                //성공                    
                                int mailCount = int.Parse(xml.SelectSingleNode("/data/result").InnerText);

                                if (mailCount > 0)
                                {
                                    MailCnt = "<span>" + mailCount.ToString("#,##0") + "</span>";
                                }
                            }
                        }
                    }
                }

                #endregion

                #region 결재 건수 (결재대상문서,대결결재대상문서 건수)

                if (maApproval.Visible)
                {
                    int appCount = 0;
                    string sqlAppQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_MENUCNT");
                    SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                };
                    parm[0].Value = CompanyID;
                    parm[1].Value = UserID;


                    DataSet dsApp = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sqlAppQuery, "dsList", parm);

                    DataRow[] dr = dsApp.Tables["dsList"].Select("CAT = 'AL'");
                    appCount = dr.Length;

                    dr = dsApp.Tables["dsList"].Select("CAT = 'FL'");
                    appCount += dr.Length;

                    if (appCount > 0)
                    {
                        AppCnt = "<span>" + appCount.ToString("#,##0") + "</span>";
                    }
                }

                #endregion

                #region 게시판 (읽지 않은 건수)

                if (maNotice.Visible)
                {
                    int boaCount = 0;
                    string cIDList = String.Empty;

                    string sqlBoaQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPortal, "SEL_QRY_SYSCode");
                    sqlBoaQuery = String.Format(sqlBoaQuery, CompanyID, "MobileBoard", "SubBoard");
                    DataSet dsBoa = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlBoaQuery, "UserBoard", null);

                    foreach (DataRow drBoa in dsBoa.Tables[0].Rows)
                    {
                        cIDList = drBoa["InterfaceCode"].ToString();
                    }

                    if (cIDList != "")
                    {
                        string[] arrCID = cIDList.Split(',');

                        Zeospace.DeskPlusMobile.Web.Board.Ws.BLL.List biz =
                               new Zeospace.DeskPlusMobile.Web.Board.Ws.BLL.List();

                        for (int i = 0; i < arrCID.Length; i++)
                        {
                            //통합게시판 회사아이디
                            int boaComID = Zeospace.DeskPlusMobile.Web.Board.Ws.BLL.Category.GetCompanyCategoryInfo(Convert.ToInt32(arrCID[i]));
                            boaCount += biz.GetNoReadMenu(UserInfo.CompanyID, boaComID, UserInfo.UserID, UserInfo.DeptID, Convert.ToInt32(arrCID[i]), PreLang);
                        }

                        if (boaCount > 0)
                        {
                            BoaCnt = "<span>" + boaCount.ToString("#,##0") + "</span>";
                        }
                    }
                }

                #endregion

                #region 일정관리 (당일 일정,할일)

                if (maPims.Visible)
                {
                    int pimCount = 0;
                    string pimsType = "";
                    if (UserEnv != null)
                    {
                        pimsType = UserEnv;
                    }
                    else
                    {
                        pimsType = "0|0|0|0|";
                    }

                    string[] pimTypeArr = pimsType.Split('|');


                    DataSet dsSch = new DataSet();
                    SqlParameter[] parm1 = new SqlParameter[]{
                                        new SqlParameter("@CompanyID", SqlDbType.Int)
                                        ,new SqlParameter("@TmpDate", SqlDbType.VarChar,10)
                                        ,new SqlParameter("@UserID", SqlDbType.Int)
                                        ,new SqlParameter("@IsOpen", SqlDbType.Char,1)
            };
                    parm1[0].Value = CompanyID;
                    parm1[1].Value = System.DateTime.Now.Year + "-" + System.DateTime.Now.Month.ToString().PadLeft(2, '0') + "-" + System.DateTime.Now.Day.ToString().PadLeft(2, '0');
                    parm1[2].Value = UserID;
                    parm1[3].Value = "";

                    if (pimsType.IndexOf("1") > -1)
                    {
                        if (pimTypeArr[0] == "1")
                        {
                            //개인일정
                            string proc0 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_SCHEDULEETC0");
                            DataSet ds0 = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc0, "Schedule", parm1);
                            dsSch.Merge(ds0);
                        }

                        if (pimTypeArr[1] == "1")
                        {
                            //부서일정
                            string proc1 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_SCHEDULEETC1");
                            DataSet ds1 = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc1, "Schedule", parm1);
                            dsSch.Merge(ds1);
                        }

                        if (pimTypeArr[2] == "1")
                        {
                            //공유일정
                            string proc2 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_SCHEDULEETC2");
                            DataSet ds2 = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc2, "Schedule", parm1);
                            dsSch.Merge(ds2);
                        }

                        if (pimTypeArr[3] == "1")
                        {
                            //전체일정
                            string proc3 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_SCHEDULEETC3");
                            DataSet ds3 = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc3, "Schedule", parm1);
                            dsSch.Merge(ds3);
                        }
                    }
                    else
                    {
                        string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_SCHEDULEETC");
                        dsSch = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "Schedule", parm1);
                    }

                    pimCount = dsSch.Tables[0].Rows.Count;

                    DataSet dsTodo = new DataSet();
                    SqlParameter[] parm2 = new SqlParameter[]{
                                            new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@year", SqlDbType.Int)
                                            ,new SqlParameter("@month", SqlDbType.Int)
                                            ,new SqlParameter("@day", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                                            ,new SqlParameter("@IsOpen", SqlDbType.Char,1)
                };
                    parm2[0].Value = CompanyID;
                    parm2[1].Value = System.DateTime.Now.Year;
                    parm2[2].Value = System.DateTime.Now.Month;
                    parm2[3].Value = System.DateTime.Now.Day;
                    parm2[4].Value = UserID;
                    parm2[5].Value = "N";

                    if (pimsType.IndexOf("1") > -1)
                    {
                        if (pimTypeArr[0] == "1")
                        {
                            //개인할일
                            string proc0 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_TODOETC0");
                            DataSet ds0 = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc0, "ToDo", parm2);
                            dsTodo.Merge(ds0);
                        }

                        if (pimTypeArr[1] == "1")
                        {
                            //부서할일
                            string proc1 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_TODOETC1");
                            DataSet ds1 = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc1, "ToDo", parm2);
                            dsTodo.Merge(ds1);
                        }

                        if (pimTypeArr[2] == "1")
                        {
                            //공유할일
                            string proc2 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_TODOETC2");
                            DataSet ds2 = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc2, "ToDo", parm2);
                            dsTodo.Merge(ds2);
                        }

                        if (pimTypeArr[3] == "1")
                        {
                            //전체할일
                            string proc3 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_TODOETC3");
                            DataSet ds3 = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc3, "ToDo", parm2);
                            dsTodo.Merge(ds3);
                        }
                    }
                    else
                    {
                        string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_TODOETC");
                        dsTodo = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "ToDo", parm2);
                    }

                    pimCount += dsTodo.Tables[0].Rows.Count;

                    if (pimCount > 0)
                    {
                        PimCnt = "<span>" + pimCount.ToString("#,##0") + "</span>";
                    }
                }

                #endregion

                #region 마이톡 (당일 등록된 글)

                if (maMyTalk.Visible)
                {
                    int mytalkCount = 0;
                    string sqlmyQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLSNS, "SEL_PROC_MyTalkTodayCnt");
                    SqlParameter[] parm3 = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                };
                    parm3[0].Value = CompanyID;
                    parm3[1].Value = UserID;

                    object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sqlmyQuery, parm3);
                    mytalkCount = Convert.ToInt32(obj);

                    if (mytalkCount > 0)
                    {
                        MyTalkCnt = "<span>" + mytalkCount.ToString("#,##0") + "</span>";
                    }
                }

                #endregion

                #region 업무협업 건수 (요청받은 협업+참조받은협업+전달받은 협업 읽지 않은 건수)

                if (maTask.Visible)
                {

                    int taskCount = 0;
                    string sqlTASKQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLTask, "SEL_PROC_MENUCNT");
                    SqlParameter[] parm4 = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                };
                    parm4[0].Value = CompanyID;
                    parm4[1].Value = UserID;


                    DataSet dsTask = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sqlTASKQuery, "dsList", parm4);

                    DataRow[] drTask = dsTask.Tables["dsList"].Select("CAT = 'TTO'");
                    taskCount = drTask.Length;

                    drTask = dsTask.Tables["dsList"].Select("CAT = 'TCCU'");
                    taskCount += drTask.Length;

                    drTask = dsTask.Tables["dsList"].Select("CAT = 'TCCD'");
                    taskCount += drTask.Length;

                    drTask = dsTask.Tables["dsList"].Select("CAT = 'TFO'");
                    taskCount += drTask.Length;

                    if (taskCount > 0)
                    {
                        TaskCnt = "<span>" + taskCount.ToString("#,##0") + "</span>";
                    }
                }

                #endregion

                #region 커뮤니티 건수 (읽지 않은 게시판 글)

                if (maCommunity.Visible)
                {
                    int comCount = 0;

                    string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCommunity, "SEL_QRY_USERCOMMUNITYID");
                    proc = string.Format(proc, CompanyID, UserID);

                    DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, proc, "GetCommunityMenu", null);

                    if (ds != null)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            comCount += LoadComCnt(dr["CommunityID"].ToString(), dr["Grade"].ToString());
                        }
                    }

                    if (comCount > 0)
                    {
                        ComCnt = "<span>" + comCount.ToString("#,##0") + "</span>";
                    }
                }

                #endregion

                #region 자원관리 건수 (승인하지 않은 예약 내역)

                if(maRM.Visible)
                {
                    int rmCount = 0;

                    RM.Ws.Data.Reserve dal = new RM.Ws.Data.Reserve();
                    rmCount = dal.GetUsalbleListCount(CompanyID.ToNumber<int>(), UserID.ToNumber<int>(), "0", "");
                    

                    if (rmCount > 0)
                    {
                        RMCnt = "<span>" + rmCount.ToString("#,##0") + "</span>";
                    }
                }

                #endregion

                #region 알람함 건수 (읽지 않은 알람)
                int nbCount = 0;

                MDM.WS.Data.Notification dalNoti = new MDM.WS.Data.Notification();
                nbCount = dalNoti.GetNotReadListCount(CompanyID, UserID.ToNumber<int>());
                
                if (nbCount > 0)
                {
                    NBCnt = "<span>" + nbCount.ToString("#,##0") + "</span>";
                }                

                #endregion

            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("Menu.ascx : ", ex);
            }
        }

        private int LoadComCnt(string _communityID, string _grade)
        {
            int rtn = 0;

            string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCommunity, "SEL_QRY_MENUCNT");
            proc = string.Format(proc, CompanyID, _communityID, _grade);

            DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, proc, "GetCommunity", null);

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    rtn = dr["Cnt"].ToString().ToNumber<int>();
                }
            }

            return rtn;
        }

        public string MailCnt { get; set; }

        public string AppCnt { get; set; }

        public string BoaCnt { get; set; }

        public string PimCnt { get; set; }

        public string MyTalkCnt { get; set; }

        public string TaskCnt { get; set; }

        public string RMCnt { get; set; }

        public string NBCnt { get; set; }

        public string ComCnt { get; set; }
    }
}