<%@ Page language="c#" AutoEventWireup="true" Inherits="Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Collections" %>
<%@ Import Namespace="ComponentArt.Web.UI" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="Zeospace.DeskPlusMobile.Web.Helper" %>

<script language="C#" runat="server">
	private void Page_Load(object sender, System.EventArgs e)
	{
	}

    [ComponentArtCallbackMethod]
    public string Set_PayReport(string _companyID, string _docID, string _userid, string _deptcodeid, string _customername, string _startdate, string _enddate, string _starthour, string _endhour, string _duration, string _hour, string _extrapay, string _reason, string _reasondetails, string _deptid, string _pathid)
    {
        try
        {
            if (_userid == "" || string.IsNullOrEmpty(_userid) || _docID == "" || string.IsNullOrEmpty(_docID))
            {
                return null;
            }

            string query = "IF NOT EXISTS (SELECT * FROM DP_PAY_Report WHERE CompanyID = @CompanyID and Userid = @UserID and Documentid = @DocumentID) BEGIN INSERT DP_PAY_Report (CompanyID, DocumentID, UserID, DeptCodeID, DeptID, PathID, Code, CustomerName, StartDate, EndDate, StartHour, EndHour, Duration, Hour, ExtraPay, Reason, ReasonDetails, IsPay, IsTransportPay) VALUES (@CompanyID, @DocumentID, @UserID, @DeptCodeID, @DeptID, @PathID, '2', @CustomerName, @StartDate, @EndDate, @StartHour, @EndHour, @Duration, @Hour, @ExtraPay, @Reason, @ReasonDetails, 'N', 'N') END";

            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnSolution"].ConnectionString))
            {
                using (var cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@CompanyID", _companyID);
					cmd.Parameters.AddWithValue("@DocumentID", _docID);
                    cmd.Parameters.AddWithValue("@UserID", _userid);
					cmd.Parameters.AddWithValue("@DeptCodeID", _deptcodeid);
                    cmd.Parameters.AddWithValue("@DeptID", _deptid);
                    cmd.Parameters.AddWithValue("@PathID", _pathid);
                    cmd.Parameters.AddWithValue("@CustomerName", _customername);
                    cmd.Parameters.AddWithValue("@StartDate", _startdate);
                    cmd.Parameters.AddWithValue("@EndDate", _enddate);  
					cmd.Parameters.AddWithValue("@StartHour", _starthour);                  
					cmd.Parameters.AddWithValue("@EndHour", _endhour);  
					cmd.Parameters.AddWithValue("@Duration", _duration);
                    cmd.Parameters.AddWithValue("@Hour", _hour);        
					cmd.Parameters.AddWithValue("@ExtraPay", _extrapay);
					cmd.Parameters.AddWithValue("@Reason", _reason);
					cmd.Parameters.AddWithValue("@ReasonDetails", _reasondetails);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            return "1";
        }
        catch (SqlException ex)
        {
            string errMsg = AppException.SqlExceptionLog(ex);
            AppException.WriteException("특근 보고서 Set_PayReport() errmsg: " + errMsg);
            return "0";
        }
    }

    // 퇴근시간 자동 등록 된것 중 연장근무 시간에 맞게 다시 수정
    [ComponentArtCallbackMethod]
    public string SetAttendance(string _companyID, string _docID, string _userID, string _startdate, string _enddate)
    {
        try
        {
            if (_companyID == "" || string.IsNullOrEmpty(_companyID) || _userID == "" || string.IsNullOrEmpty(_userID))
            {
                return "0";
            }

            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnSolution"].ConnectionString))
            {
                using (var cmd = new SqlCommand("DP_HR_Attendance_UpdWCTime", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CompanyID", _companyID);
                    cmd.Parameters.AddWithValue("@UserID", _userID);
                    cmd.Parameters.AddWithValue("@StartDate", _startdate);
                    cmd.Parameters.AddWithValue("@EndDate", _enddate);
                    cmd.Parameters.AddWithValue("@DocumentID", _docID);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            return "1";
        }
        catch (SqlException ex)
        {
            string errMsg = AppException.SqlExceptionLog(ex);
            AppException.WriteException("특근 보고서 SetAttendance() errmsg: " + errMsg);
            return "0";
        }
    }

    // 지각, 조퇴 여부 신청 시간에 맞게 다시 수정
    [ComponentArtCallbackMethod]
    public string SetBasicAtt(string _companyID, string _docID, string _userID)
    {
        try
        {
            if (_companyID == "" || string.IsNullOrEmpty(_companyID) || _userID == "" || string.IsNullOrEmpty(_userID) || _docID == "" || string.IsNullOrEmpty(_docID))
            {
                return "0";
            }

            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnSolution"].ConnectionString))
            {
                using (var cmd = new SqlCommand("DP_HR_Attendance_UpdBasicAtt", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CompanyID", _companyID);
                    cmd.Parameters.AddWithValue("@UserID", _userID);
                    cmd.Parameters.AddWithValue("@DocumentID", _docID);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            return "1";
        }
        catch (SqlException ex)
        {
            string errMsg = AppException.SqlExceptionLog(ex);
            AppException.WriteException("특근 보고서 SetBasicAtt() errmsg: " + errMsg);
            return "0";
        }
    }

    // 신청일자에 중복 건 수 조회
    [ComponentArtCallbackMethod]
    public string GetOver(int _companyID, int _userID, string _startDate, string _endDate)
    {
        try
        {
            DataSet myDataSet = new DataSet();

            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnSolution"].ConnectionString))
            {
                SqlCommand mySqlCommand = cn.CreateCommand();
                mySqlCommand.CommandText = "DP_HR_Attendance_SelBasicOver";
                mySqlCommand.CommandType = CommandType.StoredProcedure;

                mySqlCommand.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;                 //회사코드
                mySqlCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;                       //사용자ID
                mySqlCommand.Parameters.Add("@StartDate", SqlDbType.VarChar, 10).Value = _startDate;         //조회시작일
                mySqlCommand.Parameters.Add("@EndDate", SqlDbType.VarChar, 10).Value = _endDate;             //조회종료일

                cn.Open();

                SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter();
                mySqlDataAdapter.SelectCommand = mySqlCommand;
                mySqlDataAdapter.Fill(myDataSet, "DP_HR_Attendance_SelBasicOver");
            }

            if (myDataSet != null)
            {
                if (myDataSet.Tables[0].Rows.Count > 0)
                {
                    return "FALSE";
                }
            }

            return "TRUE";
        }
        catch (SqlException ex)
        {
            string errMsg = AppException.SqlExceptionLog(ex);
            AppException.WriteException("특근 보고서 GetOver() errmsg: " + errMsg);
            return "FALSE";
        }
    }

    // 주간 휴가 시간 조회
    [ComponentArtCallbackMethod]
    public string GetWeekHoli(int _companyid, int _userid, string _date)
    {
        try
        {
            string holiHour = "0";
            DataSet myDataSet = new DataSet();

            DateTime tTemp = new DateTime(_date.Split('-')[0].ToNumber<int>(), _date.Split('-')[1].ToNumber<int>(), _date.Split('-')[2].ToNumber<int>());
            string startDate = GetDateOfMonday(tTemp).ToString("yyyy-MM-dd"); // 특정주의 월요일
            string endDate = GetDateOfSunday(tTemp).ToString("yyyy-MM-dd"); // 특정주의 일요일

            string query = @"
SELECT ISNULL(SUM(Duration), 0) * 8
FROM DP_HOLI_Used WITH(NOLOCK)	
WHERE CompanyID = @CompanyID
AND UserID = @UserID
AND (StartDate <= @EndDate AND EndDate >= @StartDate)
";

            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnSolution"].ConnectionString))
            {
                SqlCommand mySqlCommand = cn.CreateCommand();
                mySqlCommand.CommandText = query;
                mySqlCommand.CommandType = CommandType.Text;
                mySqlCommand.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyid;
                mySqlCommand.Parameters.Add("@Userid", SqlDbType.Int).Value = _userid;
                mySqlCommand.Parameters.Add("@Startdate", SqlDbType.VarChar, 10).Value = startDate;
                mySqlCommand.Parameters.Add("@Enddate", SqlDbType.VarChar, 10).Value = endDate;

                cn.Open();

                SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter();
                mySqlDataAdapter.SelectCommand = mySqlCommand;
                mySqlDataAdapter.Fill(myDataSet, "GetWeekHoli");
            }

            if (myDataSet != null)
            {
                if (myDataSet.Tables[0].Rows.Count > 0)
                {
                    holiHour = myDataSet.Tables[0].Rows[0][0].ToString();
                }
            }

            return holiHour;
        }
        catch (SqlException ex)
        {
            string errMsg = AppException.SqlExceptionLog(ex);
            AppException.WriteException("특근 보고서 GetWeekHoli() errmsg: " + errMsg);
            return "0";
        }
    }

    // 주간 연장근무시간 조회
    [ComponentArtCallbackMethod]
    public string CallBackGetPrivateSum(int _companyID, int _userID, string _date)
    {
        try
        {
            string OverTime = "0";

            DateTime tTemp = new DateTime(_date.Split('-')[0].ToNumber<int>(), _date.Split('-')[1].ToNumber<int>(), _date.Split('-')[2].ToNumber<int>());
            string startDate = GetDateOfMonday(tTemp).ToString("yyyy-MM-dd"); // 특정주의 월요일
            string endDate = GetDateOfSunday(tTemp).ToString("yyyy-MM-dd"); // 특정주의 일요일

            DataSet ds = GetPrivateSum(_companyID, _userID, startDate, endDate);

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    OverTime = (dr["OverTimeCnt"].ToString() == "") ? "0" : dr["OverTimeCnt"].ToString();
                }
            }

            return OverTime;
        }
        catch (SqlException ex)
        {
            string errMsg = AppException.SqlExceptionLog(ex);
            AppException.WriteException("특근 보고서 CallBackGetPrivateSum() errmsg: " + errMsg);
            return "0";
        }
    }

    // 주간 연장근무시간 조회
    private DataSet GetPrivateSum(int _companyID, int _userID, string _startDate, string _endDate)
    {
        try
        {
            DataSet myDataSet = new DataSet();

            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnSolution"].ConnectionString))
            {
                SqlCommand mySqlCommand = cn.CreateCommand();
                mySqlCommand.CommandText = "DP_HR_Attendance_SelPrivateSUM";
                mySqlCommand.CommandType = CommandType.StoredProcedure;

                mySqlCommand.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;                 //회사코드
                mySqlCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;                       //사용자ID
                mySqlCommand.Parameters.Add("@DeptID", SqlDbType.Int).Value = 0;                             //부서코드
                mySqlCommand.Parameters.Add("@PathID", SqlDbType.VarChar, 200).Value = "";                   //부서경로
                mySqlCommand.Parameters.Add("@DeptBase", SqlDbType.NVarChar, 100).Value = "";         //부서명
                mySqlCommand.Parameters.Add("@StartDate", SqlDbType.VarChar, 10).Value = _startDate;         //조회시작일
                mySqlCommand.Parameters.Add("@EndDate", SqlDbType.VarChar, 10).Value = _endDate;             //조회종료일
                mySqlCommand.Parameters.Add("@AttendanceType", SqlDbType.Char, 1).Value = "";
                mySqlCommand.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = "";
                mySqlCommand.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = "";
                mySqlCommand.Parameters.Add("@IsIn", SqlDbType.Int).Value = 1;
                mySqlCommand.Parameters.Add("@MinClosingID", SqlDbType.Int).Value = -1;
                mySqlCommand.Parameters.Add("@MaxClosingID", SqlDbType.Int).Value = -1;

                cn.Open();

                SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter();
                mySqlDataAdapter.SelectCommand = mySqlCommand;
                mySqlDataAdapter.Fill(myDataSet, "DP_HR_Attendance_SelPrivateSUM");
            }

            return myDataSet;
        }
        catch (SqlException ex)
        {
            string errMsg = AppException.SqlExceptionLog(ex);
            AppException.WriteException("특근 보고서 GetPrivateSum() errmsg: " + errMsg);
            return null;
        }
    }

    /// <summary>
    /// 특정주의 월요일
    /// </summary>
    /// <param name="sourceDate"></param>
    /// <returns></returns>
    public DateTime GetDateOfMonday(DateTime sourceDate)
    {
        int today = (int)sourceDate.DayOfWeek;
        return sourceDate.AddDays((double)((today == 0) ? -6 : 1 - today));
    }

    /// <summary>
    /// 특정주의 일요일
    /// </summary>
    /// <param name="sourceDate"></param>
    /// <returns></returns>
    public DateTime GetDateOfSunday(DateTime sourceDate)
    {
        int today = (int)sourceDate.DayOfWeek;
        return sourceDate.AddDays((double)((today == 0) ? 0 : 7 - today));
    }
    
</script>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
<title></title>
<script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
<script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
<script language="javascript">
    var DOC = parent.ifrmForm.document;
    var docID = parent.DocumentID;
    var isLast = parent.IsLast; 				// 최종결재여부 (Y/N)
    var isZQ = parent.IsZQ; 					// 반려가능수신(ZQ) 존재여부 (Y/N)

    var companyid = DOC.all("companyid").innerText;
    var userid = DOC.all("userid").innerText;
    var deptid = DOC.all("deptid").innerText;
    var hrdeptid = DOC.all("hrdeptid").innerText;
    var pathid = DOC.all("pathid").innerText;
    var username = DOC.getElementById("username").innerText;
    var title = DOC.getElementById("title").innerText;

    var deptcodeid = "0";
    if (DOC.all("deptcodeid")) deptcodeid = DOC.all("deptcodeid").innerText;

    var customername = DOC.all("customer").innerText;
    var startdate = DOC.all("startdate").innerText;
    var enddate = DOC.all("enddate").innerText;
    var starthour = DOC.all("starthour").innerText;
    var endhour = DOC.all("endhour").innerText;
    var duration = DOC.all("duration").innerText;
    var hour = DOC.all("hour").innerText;

    var obj = DOC.getElementById("Reason");
    var reason = obj.options[obj.selectedIndex].text;
    //var reason = DOC.all("reason").innerText;
    var reasondetails = DOC.all("reasondetails").innerText;
    var extrapay = DOC.all("extrapay").innerText;


    /**********************************************************************************************
    작성자   : 
    작성일자 : 
    작성목적 : 문서로딩시, 초기화작업
    **********************************************************************************************/
    function init() {

    }

    function onComplete() {
        //최종 결재 시
        if (parent.ApprovalType != "R" && isZQ == "N" && parent.AccountTag == "ZD") {
            if (parent.ApprovalType == "A" || isLast == "Y") {

                // 신청일자에 중복 건 수 조회
                var over = GetOver(companyid, userid, startdate, enddate);
                if (over != "TRUE") {
                    parent.action_popup.alert("선택한 날짜에 중복 신청된 건이 있습니다.");
                    parent.LoadingShow(false);
                    return;
                }

                // 주간 연장근무시간 조회
                var overtime = CallBackGetPrivateSum(companyid, userid, startdate);
                var tmpTime = parseInt(overtime) + parseInt(hour);
                if (tmpTime > 12) {
                    var holiHour = GetWeekHoli(companyid, userid, startdate);
                    if (holiHour == "0") {
                        parent.action_popup.alert("선택한 주의 연장 근무 시간 12시간이 초과되었습니다.");
                        parent.LoadingShow(false);
                        return;
                    }
                    else {
                        if (tmpTime > (12 + parseInt(holiHour))) {
                            parent.action_popup.alert("선택한 주의 연장 근무 시간 12시간이 초과되었습니다.");
                            parent.LoadingShow(false);
                            return;
                        }
                    }
                }

                var rlt = "0";
                rlt = Set_PayReport(companyid, docID, userid, deptcodeid, customername, startdate, enddate, starthour, endhour, duration, hour, extrapay, reason, reasondetails, hrdeptid, pathid);

                if (rlt != "1") {
                    parent.action_popup.alert("최종 결재 시 오류가 발생하였습니다.\n관리자에게 문의하세요.");
                    parent.LoadingShow(false);
                    return;
                }

                // 퇴근시간 자동 등록 된것 중 연장근무 시간에 맞게 다시 수정
                var rltAtt = "0";
                rltAtt = SetAttendance(companyid, docID, userid, startdate, enddate);

                if (rltAtt != "1") {
                    parent.action_popup.alert("최종 결재 시 오류가 발생하였습니다.\n관리자에게 문의하세요.");
                    parent.LoadingShow(false);
                    return;
                }

                /*
                // 지각, 조퇴 여부 신청 시간에 맞게 다시 수정
                var rltBasicAtt = "0";
                rltBasicAtt = SetBasicAtt(companyid, docID, userid);

                if (rltBasicAtt != "1") {
                    parent.action_popup.alert("최종 결재 시 오류가 발생하였습니다.\n관리자에게 문의하세요.");
                    parent.LoadingShow(false);
                    return;
                }
                */
            }
        }

        parent.SubmitCustomCall(parent.ApprovalType); // S(승인), A(전결), R(반송), RC(반송취소)
    } 
</script>
</head>
<body onload="init()">
    <form id="Form1" method="post" runat="server">
    </form>
</body>
</html>
