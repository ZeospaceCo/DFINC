<%@ Page language="c#" AutoEventWireup="true" Inherits="Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Collections" %>
<%@ Import Namespace="ComponentArt.Web.UI" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Xml" %>
<%@ Import Namespace="Zeospace.DeskPlusMobile.Web.Helper" %>

<script language="C#" runat="server">
    public string janDays1 = "";
	public string holiCount = "";    
	
    public string StartDate = "";
    public string EndDate = "";
    
    public string deptCodeID = "";

    public string CompanyID = "";
    public string UserID = "";
    public string DeptID = "";
    public string HRDeptID = ""; //근태용 DeptID
    public string PathID = "";
    public string UserName = "";
    	
	private void Page_Load(object sender, System.EventArgs e)
	{
		if (!IsPostBack)
		{
           string NowYear = DateTime.Now.Year.ToString();
            string NowMonth = (Convert.ToInt32(DateTime.Now.Month.ToString()) < 10) ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString();
            string NowYM = NowYear + NowMonth;
            
            CompanyID = UserInfo.CompanyID.ToString();
            UserID = UserInfo.UserID.ToString();            
            UserName = UserInfo.Name.ToString();
            DeptID = UserInfo.DeptID.ToString();

            // 근태용 겸직자 기본 부서ID 가져오기
            HRDeptID = GetUserDept(UserInfo.CompanyID, UserInfo.UserID);
            if (HRDeptID == "" || HRDeptID == "0")
            {
                HRDeptID = UserInfo.DeptID.ToString();
            }           

            //DeptCodeID 가져오기
            DataSet dsDeptCode = GetDeptCodeID(UserInfo.CompanyID, UserInfo.DeptID, UserInfo.CodeTitleID, UserInfo.CodeDutyID);
            if (dsDeptCode != null && dsDeptCode.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsDeptCode.Tables[0].Rows.Count; i++)
                {
                    deptCodeID = dsDeptCode.Tables[0].Rows[0][0].ToString();
                }
            }
            else
            {
                deptCodeID = "0";
            }
           
            //부서의 PathID 가져오기
            DataSet dsPathID = GetDeptPathID(UserInfo.CompanyID, HRDeptID.ToNumber<int>());
            if (dsPathID != null && dsPathID.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsPathID.Tables[0].Rows.Count; i++)
                {
                    PathID = dsPathID.Tables[0].Rows[0][0].ToString();
                }
            }
            else
            {
                PathID = "";
            }
            
			//총 연차수 가져오기
            DataSet dsReceive1 = GetReceive(UserID, NowYear, NowYM, "1");					
			if (dsReceive1 != null && dsReceive1.Tables[0].Rows.Count >  0)
			{			
				for (int i = 0; i < dsReceive1.Tables[0].Rows.Count; i++)
				{
                    holiCount = dsReceive1.Tables[0].Rows[0][0].ToString();     				
				}					
			}
			else
			{
                holiCount = "0";
			}

			//남은 연차수 가져오기
            DataSet dsJan1 = GetJanDays(UserID, NowYear, NowYM, "1");					
			if (dsJan1 != null && dsJan1.Tables[0].Rows.Count >  0)
			{			
				for (int i = 0; i < dsJan1.Tables[0].Rows.Count; i++)
				{
					janDays1 = dsJan1.Tables[0].Rows[0][0].ToString();     				
				}					
			}
			else
			{
				janDays1 = "0";
			}		
		}
	}

    // 근태용 겸직자 기본 부서ID 가져오기
    private string GetUserDept(int _companyID, int _userID)
    {
        try
        {
            string iDeptID = "0";

            DataSet myDataSet = new DataSet();

            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnSolution"].ConnectionString))
            {
                SqlCommand mySqlCommand = cn.CreateCommand();
                mySqlCommand.CommandText = "DP_HR_UserDept_SelPageList";
                mySqlCommand.CommandType = CommandType.StoredProcedure;

                mySqlCommand.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;                 //회사코드
                mySqlCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;                       //사용자ID

                cn.Open();

                SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter();
                mySqlDataAdapter.SelectCommand = mySqlCommand;
                mySqlDataAdapter.Fill(myDataSet, "DP_HR_UserDept_SelPageList");
            }

            if (myDataSet != null && myDataSet.Tables[0].Rows.Count > 0)
            {
                iDeptID = myDataSet.Tables[0].Rows[0]["DeptID"].ToString();
            }

            return iDeptID;
        }
        catch (SqlException ex)
        {
            string errMsg = AppException.SqlExceptionLog(ex);
			AppException.WriteException("HoliDay.aspx GetUserDept errmsg: " + errMsg);
            return "0";
        }
    }

    //DetpCodeID
    private DataSet GetDeptCodeID(int _companyid, int _deptid, int _titleid, int _dutyid)
    {
        try
        {
            DataSet myDataSet = new DataSet();

            string query = "SELECT DeptCodeID FROM DP_COM_DeptCode WHERE CompanyID = @CompanyID AND DeptID = @DeptID AND TitleID = @TitleID AND DutyID = @DutyID";

            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnSolution"].ConnectionString))
            {
                SqlCommand mySqlCommand = cn.CreateCommand();
                mySqlCommand.CommandText = query;
                mySqlCommand.CommandType = CommandType.Text;
                mySqlCommand.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyid;
                mySqlCommand.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptid;
                mySqlCommand.Parameters.Add("@TitleID", SqlDbType.Int).Value = _titleid;
                mySqlCommand.Parameters.Add("@DutyID", SqlDbType.Int).Value = _dutyid;

                cn.Open();

                SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter();
                mySqlDataAdapter.SelectCommand = mySqlCommand;
                mySqlDataAdapter.Fill(myDataSet, "DP_COM_DeptCode");
            }

            return myDataSet;
        }
        catch (SqlException ex)
        {
            string errMsg = AppException.SqlExceptionLog(ex);
			AppException.WriteException("HoliDay.aspx GetDeptCodeID errmsg: " + errMsg);
            return null;
        }
    }
   
   //PathID
    private DataSet GetDeptPathID(int _companyid, int _deptid)
    {
        try
        {
            DataSet myDataSet = new DataSet();

            string query = "SELECT PathID FROM DP_ACC_Dept WHERE CompanyID = @CompanyID AND DeptID = @DeptID";

            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnSolution"].ConnectionString))
            {
                SqlCommand mySqlCommand = cn.CreateCommand();
                mySqlCommand.CommandText = query;
                mySqlCommand.CommandType = CommandType.Text;
                mySqlCommand.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyid;
                mySqlCommand.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptid;

                cn.Open();

                SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter();
                mySqlDataAdapter.SelectCommand = mySqlCommand;
                mySqlDataAdapter.Fill(myDataSet, "DP_ACC_Dept");
            }

            return myDataSet;
        }
        catch (SqlException ex)
        {
            string errMsg = AppException.SqlExceptionLog(ex);
			AppException.WriteException("HoliDay.aspx GetDeptPathID errmsg: " + errMsg);
            return null;
        }
    }
	
	//총연차수
    private DataSet GetReceive(string _userid, string _nowYear, string _nowDate, string _type)
	{
		try
		{
			DataSet myDataSet = new DataSet();

            if (_userid == "" || string.IsNullOrEmpty(_userid) || _nowDate == "" || string.IsNullOrEmpty(_nowDate)) 
			{
				return  null;
			}

            string query = "SELECT HoliCount FROM DP_HOLI_Admin WHERE UserID = @UserID AND LEFT(StartDate, 4) = @NowYear AND @NowDate >= StartDate AND @NowDate <= EndDate AND Type = @Type";

			using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnSolution"].ConnectionString))
			{
				SqlCommand mySqlCommand = cn.CreateCommand(); 
				mySqlCommand.CommandText = query; 
				mySqlCommand.CommandType = CommandType.Text; 
				mySqlCommand.Parameters.Add("@UserID", SqlDbType.VarChar, 1000).Value = _userid;
                mySqlCommand.Parameters.Add("@NowYear", SqlDbType.VarChar, 1000).Value = _nowYear;
                mySqlCommand.Parameters.Add("@NowDate", SqlDbType.VarChar, 1000).Value = _nowDate; 
				mySqlCommand.Parameters.Add("@Type", SqlDbType.VarChar, 1000).Value = _type;

				cn.Open(); 

				SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter(); 
				mySqlDataAdapter.SelectCommand = mySqlCommand; 
				mySqlDataAdapter.Fill(myDataSet, "DP_HOLI_Admin"); 				
			}

			return myDataSet;				
		}
		catch (SqlException ex)
		{
			string errMsg = AppException.SqlExceptionLog(ex);
			AppException.WriteException("HoliDay.aspx GetReceive errmsg: " + errMsg);
			return null;
		}			
    }

	//남은연차수
    private DataSet GetJanDays(string _userid, string _nowYear, string _nowDate, string _type)
	{
		try
		{
			DataSet myDataSet = new DataSet();

            if (_userid == "" || string.IsNullOrEmpty(_userid) || _nowDate == "" || string.IsNullOrEmpty(_nowDate)) 
			{
				return  null;
			}

            string query = "SELECT SUM(HoliCount) - ISNULL((SELECT SUM(Duration) FROM DP_HOLI_Used WHERE UserID = DP_HOLI_Admin.UserID AND ProcessFlag = '0' AND WkItemCD = @Type AND LEFT(REPLACE(StartDate, '-', ''), 6) >= DP_HOLI_Admin.StartDate AND LEFT(REPLACE(EndDate, '-', ''), 6) <= DP_HOLI_Admin.EndDate), 0) FROM DP_HOLI_Admin WHERE UserID = @UserID AND LEFT(StartDate, 4) = @NowYear AND @NowDate >= StartDate AND @NowDate <= EndDate AND Type = @Type GROUP BY USERID, StartDate, EndDate";

			using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnSolution"].ConnectionString))
			{
				SqlCommand mySqlCommand = cn.CreateCommand(); 
				mySqlCommand.CommandText = query; 
				mySqlCommand.CommandType = CommandType.Text; 
				mySqlCommand.Parameters.Add("@UserID", SqlDbType.VarChar, 1000).Value = _userid;
                                mySqlCommand.Parameters.Add("@NowYear", SqlDbType.VarChar, 1000).Value = _nowYear;
                mySqlCommand.Parameters.Add("@NowDate", SqlDbType.VarChar, 1000).Value = _nowDate;               
				mySqlCommand.Parameters.Add("@Type", SqlDbType.VarChar, 1000).Value = _type; 

				cn.Open(); 

				SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter(); 
				mySqlDataAdapter.SelectCommand = mySqlCommand; 
				mySqlDataAdapter.Fill(myDataSet, "DP_HOLI_Admin"); 				
			}

			return myDataSet;				
		}
		catch (SqlException ex)
		{
			string errMsg = AppException.SqlExceptionLog(ex);
			AppException.WriteException("HoliDay.aspx GetJanDays errmsg: " + errMsg);
			return null;
		}			
    }

    //지정한 대결자 등록되어있는지 조회
    [ComponentArtCallbackMethod]
    public string CallBackGetProxyUser(string _companyID, string _userID, string _deptID, string _proxyUserID, string _proxyDeptID)
    {
        try
        {
            DataSet ds = GetProxyUser(_companyID, _userID, _deptID, _proxyUserID, _proxyDeptID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return "FALSE";
            }            

            return "TRUE";
        }
        catch (SqlException ex)
        {
            string errMsg = AppException.SqlExceptionLog(ex);
			AppException.WriteException("HoliDay.aspx CallBackGetProxyUser errmsg: " + errMsg);
            return "FALSE";
        }
    }    

    //지정한 대결자 등록되어있는지 조회
    private DataSet GetProxyUser(string _companyID, string _userID, string _deptID, string _proxyUserID, string _proxyDeptID)
    {
        try
        {
            DataSet myDataSet = new DataSet();

            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnSolution"].ConnectionString))
            {
                SqlCommand mySqlCommand = cn.CreateCommand();
                mySqlCommand.CommandText = "DP_APP_Proxy_SelByProxyUser";
                mySqlCommand.CommandType = CommandType.StoredProcedure;

                mySqlCommand.Parameters.Add("@CompanyID", SqlDbType.Int).Value = Int32.Parse(_companyID);
                mySqlCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = Int32.Parse(_userID);
                mySqlCommand.Parameters.Add("@DeptID", SqlDbType.Int).Value = Int32.Parse(_deptID);
                mySqlCommand.Parameters.Add("@ProxyUserID", SqlDbType.Int).Value = Int32.Parse(_proxyUserID);
                mySqlCommand.Parameters.Add("@ProxyDeptID", SqlDbType.Int).Value = Int32.Parse(_proxyDeptID);
                mySqlCommand.Parameters.Add("@ProxyID", SqlDbType.Int).Value = 0;

                cn.Open();

                SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter();
                mySqlDataAdapter.SelectCommand = mySqlCommand;
                mySqlDataAdapter.Fill(myDataSet, "DP_APP_Proxy_SelByProxyUser");
            }

            return myDataSet;
        }
        catch (SqlException ex)
        {
            string errMsg = AppException.SqlExceptionLog(ex);
			AppException.WriteException("HoliDay.aspx GetProxyUser errmsg: " + errMsg);
            return null;
        }
    }

    //대결자 중복 기간 체크 
    [ComponentArtCallbackMethod]
    public string CallBackGetProxySelByDate(string _companyID, string _userID, string _deptID, string _startDateStr, string _endDateStr)
    {
        try
        {
            DataSet ds = GetProxySelByDate(_companyID, _userID, _deptID, _startDateStr, _endDateStr);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return "FALSE";
            }

            return "TRUE";
        }
        catch (SqlException ex)
        {
            string errMsg = AppException.SqlExceptionLog(ex);
			AppException.WriteException("HoliDay.aspx CallBackGetProxySelByDate errmsg: " + errMsg);
            return "FALSE";
        }
    }

    //대결자 중복 기간 체크 
    private DataSet GetProxySelByDate(string _companyID, string _userID, string _deptID, string _startDateStr, string _endDateStr)
    {
        try
        {
            DataSet myDataSet = new DataSet();

            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnSolution"].ConnectionString))
            {
                SqlCommand mySqlCommand = cn.CreateCommand();
                mySqlCommand.CommandText = "DP_APP_Proxy_SelByDate";
                mySqlCommand.CommandType = CommandType.StoredProcedure;

                mySqlCommand.Parameters.Add("@CompanyID", SqlDbType.Int).Value = Int32.Parse(_companyID);
                mySqlCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = Int32.Parse(_userID);
                mySqlCommand.Parameters.Add("@DeptID", SqlDbType.Int).Value = Int32.Parse(_deptID);
                mySqlCommand.Parameters.Add("@ProxyStartDateStr", SqlDbType.VarChar, 20).Value = _startDateStr;
                mySqlCommand.Parameters.Add("@ProxyEndDateStr", SqlDbType.VarChar, 20).Value = _endDateStr;
                mySqlCommand.Parameters.Add("@ProxyID", SqlDbType.Int).Value = 0;

                cn.Open();

                SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter();
                mySqlDataAdapter.SelectCommand = mySqlCommand;
                mySqlDataAdapter.Fill(myDataSet, "DP_APP_Proxy_SelByDate");
            }

            return myDataSet;
        }
        catch (SqlException ex)
        {
            string errMsg = AppException.SqlExceptionLog(ex);
			AppException.WriteException("HoliDay.aspx GetProxySelByDate errmsg: " + errMsg);
            return null;
        }
    }

    [ComponentArtCallbackMethod]
    public string getAppUser(string UserID)
    {
        try
        {
            string strSQL = "";
            string rtnVal = "";
            string ApprovalUser = "0|0|0|0|0|0";

            //결재자 정보
            strSQL = "SELECT UserID, NameBase, DeptID, DeptBase, CodeTitleID, TitleBase, CodeDutyID, DutyBase FROM vw_UserInfo ";
            strSQL += "WHERE UserID=" + UserID;
            DataSet dsUser = SQLHelper.ExecuteDataSet(ConfigurationManager.ConnectionStrings["SQLConnSolution"].ConnectionString, CommandType.Text, strSQL, "DP_APP_Account", null);
            if (dsUser != null && dsUser.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsUser.Tables[0].Rows.Count; i++)
                {
                    ApprovalUser = dsUser.Tables[0].Rows[i]["UserID"].ToString() + "|" + dsUser.Tables[0].Rows[i]["NameBase"].ToString() + "|";
                    ApprovalUser += dsUser.Tables[0].Rows[i]["DeptID"].ToString() + "|" + dsUser.Tables[0].Rows[i]["DeptBase"].ToString() + "|";
                    ApprovalUser += dsUser.Tables[0].Rows[i]["CodeTitleID"].ToString() + "|" + dsUser.Tables[0].Rows[i]["TitleBase"].ToString() + "|";
                    ApprovalUser += dsUser.Tables[0].Rows[i]["CodeDutyID"].ToString() + "|" + dsUser.Tables[0].Rows[i]["DutyBase"].ToString();
                }
            }


            return ApprovalUser;
        }
        catch (SqlException ex)
        {
            string errMsg = AppException.SqlExceptionLog(ex);
			AppException.WriteException("HoliDay.aspx getAppUser errmsg: " + errMsg);
            return "-1";
        }
    }
</script>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
<script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.js"></script>
<script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.common.js"></script>   
<script language="javascript">
    var DOC = parent.ifrmForm.document;
    var section = parent.CustomSection;
    var isZD = true; 				// 발신여부 (발신true,수신false)

    var janDays1 = "<%= janDays1 %>";              //남은연차 수
    var holi_receive1 = "<%= holiCount %>";        //총 연차 수

    var deptCodeID = "<%= deptCodeID %>";

    var CompanyID = "<%= CompanyID %>";
    var UserID = "<%= UserID %>";
    var DeptID = "<%= DeptID %>";
    var HRDeptID = "<%= HRDeptID %>";
    var PathID = "<%= PathID %>";
    var UserName = "<%= UserName %>";

    /**********************************************************************************************
    작성자   : 
    작성일자 : 
    작성목적 : 문서로딩시, 초기화작업
    **********************************************************************************************/
    function init() {
        if (parent.documentID) { return; }

        //문서로딩시, 초기화작업
        switch (section.toUpperCase()) {
            case "ADD":
                //문서상신(발신) & 임시저장
                isZD = true;

                DOC.getElementById("janDays").innerText = janDays1 + "일";
                DOC.getElementById("holi_receive").innerText = holi_receive1 + "일";

                DOC.getElementById("companyid").innerText = CompanyID;
                DOC.getElementById("userid").innerText = UserID;
                DOC.getElementById("deptid").innerText = DeptID;
                DOC.getElementById("hrdeptid").innerText = HRDeptID;
                DOC.getElementById("pathid").innerText = PathID;
                DOC.getElementById("username").innerText = UserName;

                DOC.getElementById("deptcodeid").innerText = deptCodeID;

                DOC.getElementById("tdjandays1").innerText = janDays1;
                DOC.getElementById("tdholireceive1").innerText = holi_receive1;


                break;
            case "ADDRECEIVE":
                // 문서상신(수신)
                isZD = false;
                break;
            case "MOD":
                // 수정후재상신(발신)
                isZD = true;
                break;
            case "MODRECEIVE":
                // 수정후재상신(수신)
                isZD = false;
                break;
            case "MODCONTENT":
                //결재선 및 본문수정 (수발신)
                isZD = (parent.AccountTag.indexOf("ZQ") == 0) ? false : true;

                break;
        }
    }

    //상신전에 반드시 수행되어야 할 부분이 있다면 이부분에서 수행.
    function onComplete() {
        if (isZD = true && (section.toUpperCase() == "ADD" || section.toUpperCase() == "MOD" || section.toUpperCase() == "MODCONTENT")) {
            var rlt = true;
            if (section.toUpperCase() == "MODCONTENT") //결재선 및 본문수정(수신)인 경우 제외
            {
                if (parent.AccountTag.indexOf("ZQ") == 0 || parent.AccountTag.indexOf("ZP") == 0) {
                    rlt = false;
                }
            }

            if (rlt) {

                if (DOC.getElementById("start_date").innerText == "") {
                    parent.action_popup.alert("휴가 기간을 선택해 주세요.");
                    parent.ResetProcess();
                    return;
                }

                if (DOC.getElementById("BODY_etc_Reason").innerText == "") {
                    parent.action_popup.alert("이유를 입력하세요.");
                    DOC.getElementById("BODY_etc_Reason").focus();
                    parent.ResetProcess();
                    return;
                }

                DOC.getElementById("reason").innerText = DOC.getElementById("BODY_etc_Reason").innerText;

                // 잔여일자 체크  (잔여연차-신청연차)
                var chkDuration = parseFloat(DOC.getElementById("tdjandays1").innerText) - parseFloat(DOC.getElementById("duration").innerText);
                if (chkDuration < 0) {
                    parent.action_popup.alert("잔여연차를 초과하였습니다.");
                    parent.ResetProcess();
                    return;
                }

                if (DOC.getElementById("chkProxy").checked == true) {
                    if (trim(DOC.getElementById("UserInfo").innerText) == "") {
                        parent.action_popup.alert("업무 대행자(대결자)를 선택하세요.");
                        parent.ResetProcess();
                        return;
                    }

                    if (DOC.getElementById("proxyuserid").innerText == DOC.getElementById("userid").innerText) {
                        parent.action_popup.alert("본인을 대결자로 지정할 수 없습니다.");
                        parent.ResetProcess();
                        return;
                    }

                    // 대결자 중복 기간 체크
                    var startDateStr = DOC.getElementById("start_date").innerText + " 09:00:00";
                    var endDateStr = DOC.getElementById("end_date").innerText + " 18:00:00";
                    var sHalf = DOC.getElementById("start_half").innerText;
                    var eHalf = DOC.getElementById("end_half").innerText;

                    if (sHalf == "1" || eHalf == "1") { // 오전반차
                        endDateStr = DOC.getElementById("end_date").innerText + " 14:00:00";
                    }
                    if (sHalf == "2" || eHalf == "2") { // 오후반차
                        startDateStr = DOC.getElementById("start_date").innerText + " 14:00:00";
                    }

                    var chkProxyDate = CallBackGetProxySelByDate(parent.CompanyID, DOC.getElementById("userid").innerText, DOC.getElementById("deptid").innerText, startDateStr, endDateStr);
                    if (chkProxyDate == "FALSE") {
                        parent.action_popup.alert("선택한 날짜에 이미 대결자가 지정되어 있습니다.\n대결자관리에서 삭제 후 진행하세요.");
                        parent.ResetProcess();
                        return;
                    }

                    DOC.getElementById("proxystartdate").innerText = startDateStr;
                    DOC.getElementById("proxyenddate").innerText = endDateStr;
                }

                parent.document.getElementById("tbEtc1").value = DOC.getElementById("start_date").innerText + ";;" + DOC.getElementById("end_date").innerText + ";;" + DOC.getElementById("start_half").innerText + ";;" + DOC.getElementById("end_half").innerText + ";;" + DOC.getElementById("duration").innerText + ";;" + DOC.getElementById("userid").innerText + ";;" + DOC.getElementById("WkItemCd").value + ";;" + DOC.getElementById("reason").innerText;

            }
        }

        parent.AddSubmit(); 		//framework func
    }

    function setZCUser(userID) {
        ApprovalUser = ""
        ApprovalUser = getAppUser(userID);
        arrUser = ApprovalUser.split("|");

        if (DOC.all("ZC").innerText.indexOf(arrUser[1]) < 0)
            parent.Common.SetApproval("ZC", arrUser[0], arrUser[1], arrUser[2], arrUser[3], arrUser[4], arrUser[5], arrUser[6], arrUser[7], "M", "0", "");
    }

    function SelectDateCode() {
        var url = "/DeskPlusMobile/Common/CalendarDualDialog.aspx";
        parent.PopupWin(340, 370, url, "", "", true);
    }

    function GetDateUseDay(hdYear1, hdMonth1, hdDay1, hdYear2, hdMonth2, hdDay2, hdStartHalf, hdEndHalf, duration) {

        if (hdMonth1.length == 1) {
            hdMonth1 = "0" + hdMonth1;
        }

        if (hdDay1.length == 1) {
            hdDay1 = "0" + hdDay1;
        }

        if (hdMonth2.length == 1) {
            hdMonth2 = "0" + hdMonth2;
        }

        if (hdDay2.length == 1) {
            hdDay2 = "0" + hdDay2;
        }

        var startDate = hdYear1 + "-" + hdMonth1 + "-" + hdDay1;
        var endDate = hdYear2 + "-" + hdMonth2 + "-" + hdDay2;

        calcu_date(startDate, endDate, hdStartHalf, hdEndHalf, duration)
    }

    //휴일 수 처리
    function calcu_date(p_startDate, p_endDate, p_StartHalf, p_EndHalf, p_duration) {
        var arrStartDate = p_startDate.split("-");
        var startDate = new Date(arrStartDate[0], arrStartDate[1] - 1, arrStartDate[2]); //시작일

        var arrEndDate = p_endDate.split("-");
        var endDate = new Date(arrEndDate[0], arrEndDate[1] - 1, arrEndDate[2]); //종료일	

        //반차 체크를 했으면 0.5 뺀다.
        if (p_duration > 0) {
            if (p_StartHalf == 1 || p_StartHalf == 2) p_duration = minusChk(p_duration);
            if (p_EndHalf == 1 || p_EndHalf == 2) p_duration = minusChk(p_duration);
        }

        // 날짜체크
        var chkDate = CompareDT(p_startDate, p_endDate, "lessequal", "ko");
        if (!chkDate) {
            parent.action_popup.alert("시작일자가 종료일자보다 작아야 합니다.");
            return;
        }

        //시작일과 종료일 표시
        var start_half = "";
        var end_half = "";

        if (p_StartHalf == 1) start_half = "(오전반차)";
        else if (p_StartHalf == 2) start_half = "(오후반차)";

        if (p_EndHalf == 1) end_half = "(오전반차)";
        else if (p_EndHalf == 2) end_half = "(오후반차)";

        var formatStart = startDate.getFullYear() + "년 " + (startDate.getMonth() + 1) + "월 " + startDate.getDate() + "일" + start_half + " " + getDayOfWeek(startDate) + "요일";
        var formatEnd = endDate.getFullYear() + "년 " + (endDate.getMonth() + 1) + "월 " + endDate.getDate() + "일" + end_half + " " + getDayOfWeek(endDate) + "요일";

        DOC.getElementById("SPAN_STARTDATE").innerHTML = formatStart + "부터";
        DOC.getElementById("SPAN_ENDDATE").innerHTML = formatEnd + "까지";

        //연차 일때만 일수 반영
        if (DOC.getElementById("WkItemCd").value == "1") {
            DOC.getElementById("SPAN_DURATION").innerText = p_duration;
            DOC.getElementById("duration").innerText = p_duration;
        }
        else {
            DOC.getElementById("SPAN_DURATION").innerText = "0";
            DOC.getElementById("duration").innerText = "0";
        }

        var obj = DOC.getElementById("WkItemCd");
        var text = obj.options[obj.selectedIndex].text;

        DOC.getElementById("title").innerText = text //제목

        DOC.getElementById("start_date").innerText = p_startDate;  //시작일
        DOC.getElementById("end_date").innerText = p_endDate;      //종료일
        DOC.getElementById("start_half").innerText = p_StartHalf;  //시작일 반차
        DOC.getElementById("end_half").innerText = p_EndHalf;      //종료일 반차		
    }

    //요일 구하기
    function getDayOfWeek(now) {
        var day = now.getDay(); //일요일=0,월요일=1,...,토요일=6
        var week = new Array('일', '월', '화', '수', '목', '금', '토');
        return week[day];
    }

    //반차용
    function minusChk(duration) {
        return duration - 0.5;
    }

    //휴일구분 선택 시 총연차, 남은연차 표시
    function SupportItem() {
        if (section.toUpperCase() == "MODCONTENT") //결재선 및 본문수정(수신)인 경우 제외
        {
            if (parent.AccountTag.indexOf("ZQ") == 0) {
                return;
            }
        }

        if (DOC.getElementById("WkItemCd").value == "1") //연차
        {
            DOC.getElementById("janDays").innerText = DOC.getElementById("tdjandays1").innerText + "일";
            DOC.getElementById("holi_receive").innerText = DOC.getElementById("tdholireceive1").innerText + "일";
        }
        else //기타
        {
            DOC.getElementById("janDays").innerText = "0일";
            DOC.getElementById("holi_receive").innerText = "0일";
        }

        //초기화
        DOC.getElementById("SPAN_STARTDATE").innerHTML = "&nbsp;&nbsp;&nbsp;&nbsp;년 &nbsp;&nbsp;월 &nbsp;&nbsp;일 부터";
        DOC.getElementById("SPAN_ENDDATE").innerHTML = "&nbsp;&nbsp;&nbsp;&nbsp;년 &nbsp;&nbsp;월 &nbsp;&nbsp;일 까지";
        DOC.getElementById("SPAN_DURATION").innerHTML = "&nbsp;&nbsp;";
        DOC.getElementById("duration").innerText = "0";
        DOC.getElementById("start_date").innerText = "";
        DOC.getElementById("end_date").innerText = "";
        DOC.getElementById("start_half").innerText = "";
        DOC.getElementById("end_half").innerText = "";

        if (DOC.getElementById("WkItemCd").value == "3") {
            DOC.getElementById("holi_type").innerHTML = "<SELECT id='Selholitype'><OPTION selected value=31>보수교육</OPTION><OPTION value=32>학회참석</OPTION></SELECT>";
        }
        else if (DOC.getElementById("WkItemCd").value == "4") {
            DOC.getElementById("holi_type").innerHTML = "<SELECT id='Selholitype'><OPTION selected value=41>예비군</OPTION><OPTION value=42>민방위</OPTION></SELECT>";
        }
        else if (DOC.getElementById("WkItemCd").value == "5") {
            var tmpHtml = "<SELECT id='Selholitype'><OPTION selected value=51>본인결혼</OPTION><OPTION value=52>형제자매결혼</OPTION>";
            tmpHtml += "<OPTION value=53>자녀결혼</OPTION><OPTION value=54>배우자사망</OPTION><OPTION value=55>부모사망</OPTION><OPTION value=56>자녀사망</OPTION>";
            tmpHtml += "<OPTION value=57>(외)조부모사망</OPTION><OPTION value=58>형제자매사망</OPTION><OPTION value=59>백숙부사망</OPTION>";
            tmpHtml += "<OPTION value=60>부모칠순</OPTION><OPTION value=61>조부모칠순</OPTION></SELECT>";

            DOC.getElementById("holi_type").innerHTML = tmpHtml;
        }
        else {
            DOC.getElementById("holi_type").innerHTML = "";
        }
    }


    /**********************************************************************************************
    작성자   : 
    작성일자 : 
    작성목적 : 상신이후 호출함수
    **********************************************************************************************/
    function onCompleteAfter(_docID) {
        parent.SubmitAfterCallOK(); //framework func
    }

    /**********************************************************************************************
    작성목적 : 업무대행자 지정하기
    **********************************************************************************************/
    function SelectUserInfo() {
        var url = "/DeskPlusMobile/ETC/GetUserInfo.aspx";
        parent.PopupWin(300, 223, url, "", "", true);
    }

    function GetUserInfo(_userID, _userName, _deptName, _deptID) {
        DOC.getElementById("UserInfo").innerText = _deptName + "\n" + _userName;
        DOC.getElementById("proxyuserid").innerText = _userID;
        DOC.getElementById("proxydeptid").innerText = _deptID;
    }

    function CleanUserInfo() {
        DOC.getElementById("UserInfo").innerText = "";
        DOC.getElementById("proxyuserid").innerText = "";
        DOC.getElementById("proxydeptid").innerText = "";
    }

    /**********************************************************************************************
    작성자   : 
    작성일자 : 
    작성목적 : 문서 ClickEvent
    **********************************************************************************************/
    function docClickEvent(_event) {
        //tagId = _event.srcElement.id;
        tagId = _event.target.id;
        if (tagId == "") { return; }

        if (tagId == "btn_calander") {
            if (section.toUpperCase() == "ADD" || section.toUpperCase() == "MOD" || section.toUpperCase() == "MODCONTENT") {
                if (section.toUpperCase() == "MODCONTENT") {
                    if (parent.AccountTag.indexOf("ZQ") < 0 && parent.AccountTag.indexOf("ZP") < 0) {
                        SelectDateCode();
                    }
                }
                else {
                    SelectDateCode();
                }
            }
        }

        if (tagId == "btn_userInfo") {
            if (section.toUpperCase() == "ADD" || section.toUpperCase() == "MOD" || section.toUpperCase() == "MODCONTENT") {
                if (section.toUpperCase() == "MODCONTENT") {
                    if (parent.AccountTag.indexOf("ZQ") < 0 && parent.AccountTag.indexOf("ZP") < 0) {
                        SelectUserInfo();
                    }
                }
                else {
                    SelectUserInfo();
                }
            }
        }
    }

    /**********************************************************************************************
    작성자   : 
    작성일자 : 
    작성목적 : 문서 KeyDownEvent
    **********************************************************************************************/
    function docKeyDownEvent(_event) {
        tagId = _event.srcElement.id;
        if (tagId == "") { return; }

    }

    function trim(obj) {
        return obj.replace(/(^\s*)|(\s*$)/gi, "");
    }
</script>
</head>
<body leftMargin="0" topMargin="0" scroll="no" onload="init()">
    <form id="Form1" method="post" runat="server">
    </form>
</body>
</html>
