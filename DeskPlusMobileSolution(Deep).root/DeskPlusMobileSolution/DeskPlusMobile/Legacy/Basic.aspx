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
	public string deptCodeID = "";

	public string CompanyID = "";
	public string UserID = "";
	public string DeptID = "";
    public string HRDeptID = ""; //근태용 DeptID
    public string PathID = "";
	public string UserName = "";
	public string Title = "";

    private void Page_Load(object sender, System.EventArgs e)
    {
        if (!IsPostBack)
        {
            CompanyID = UserInfo.CompanyID.ToString();
            UserID = UserInfo.UserID.ToString();
            UserName = UserInfo.Name.ToString();
            DeptID = UserInfo.DeptID.ToString();
            Title = UserInfo.TitleName.ToString();

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
			AppException.WriteException("Basic.aspx GetUserDept errmsg: " + errMsg);
            return "0";
        }
    }

    //DetpCodeID
    private DataSet GetDeptCodeID(int _companyid, int _deptid, int _titleid, int _dutyid)
    {
        try
        {
            DataSet myDataSet = new DataSet();

            string query = "SELECT top 1 DeptCodeID FROM DP_COM_DeptCode WHERE CompanyID = @CompanyID AND DeptID = @DeptID AND TitleID = @TitleID AND DutyID = @DutyID order by DeptCodeID desc";

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
			AppException.WriteException("Basic.aspx GetDeptCodeID errmsg: " + errMsg);
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
			AppException.WriteException("Basic.aspx GetDeptPathID errmsg: " + errMsg);
            return null;
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
			AppException.WriteException("Basic.aspx GetOver errmsg: " + errMsg);
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
			AppException.WriteException("Basic.aspx GetWeekHoli errmsg: " + errMsg);
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
			AppException.WriteException("Basic.aspx CallBackGetPrivateSum errmsg: " + errMsg);
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
			AppException.WriteException("Basic.aspx GetPrivateSum errmsg: " + errMsg);
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
<script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.js"></script>
<script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.common.js"></script>   
<script language="javascript">
    var DOC = parent.ifrmForm.document;
    var section = parent.CustomSection;
    var isZD = true;  // 발신여부 (발신true,수신false)	

    var deptCodeID = "<%= deptCodeID %>";
    var CompanyID = "<%= CompanyID %>";
    var UserID = "<%= UserID %>";
    var DeptID = "<%= DeptID %>";
    var HRDeptID = "<%= HRDeptID %>";
    var PathID = "<%= PathID %>";
    var UserName = "<%= UserName %>";
    var Title = "<%= Title %>";

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

                if ($("#time1", DOC).val() == "0") {
                    $("#time1", DOC).val("18").prop("selected", true);
                }
                if ($("#time2", DOC).val() == "1") {
                    $("#time2", DOC).val("21").prop("selected", true);
                }

                Calculation();

                DOC.getElementById("companyid").innerText = CompanyID;
                DOC.getElementById("userid").innerText = UserID;
                DOC.getElementById("deptid").innerText = DeptID;
                DOC.getElementById("hrdeptid").innerText = HRDeptID;
                DOC.getElementById("pathid").innerText = PathID;
                DOC.getElementById("username").innerText = UserName;
                DOC.getElementById("title").innerText = Title;
                DOC.getElementById("deptcodeid").innerText = deptCodeID;

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
            case "MODCONTENTRECEIVE":
                //결재선 및 본문수정 (수신)
                isZD = false;

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

                if (DOC.getElementById("startdate").innerText == "") {
                    parent.action_popup.alert("날짜를 선택해 주세요.");
                    parent.ResetProcess();
                    return;
                }
                if (DOC.getElementById("BODY_etc_hour").innerText == "") {
                    parent.action_popup.alert("근무시간을 선택해 주세요.");
                    parent.ResetProcess();
                    return;
                }

                if (DOC.getElementById("BODY_etc_customer").value == "") {
                    parent.action_popup.alert("고객명을 입력해 주세요.");
                    parent.ResetProcess();
                    return;
                }

                if (DOC.getElementById("Reason").value == 0) {
                    parent.action_popup.alert("업무내역을 선택해 주세요.");
                    parent.ResetProcess();
                    return;
                }

                if (DOC.getElementById("BODY_etc_ReasonDetail").value == "") {
                    parent.action_popup.alert("상세내역을 입력해 주세요.");
                    parent.ResetProcess();
                    return;
                }

                if (DOC.getElementById("BODY_etc_traPay1").checked == false) {
                    if (DOC.getElementById("BODY_etc_traPay2").checked == false) {
                        parent.action_popup.alert("교통비청구를 체크해 주세요.");
                        parent.ResetProcess();
                        return;
                    }
                }

                if (DOC.getElementById("BODY_etc_traPay2").checked == true) {
                    if (DOC.getElementById("BODY_etc_transportpay").value == "") {
                        parent.action_popup.alert("청구금액을 입력해 주세요.");
                        parent.ResetProcess();
                        return;
                    }
                }

                // 신청일자에 중복 건 수 조회
                var over = GetOver(DOC.getElementById("companyid").innerText, DOC.getElementById("userid").innerText, DOC.all("startdate").innerText, DOC.all("startdate").innerText);
                if (over != "TRUE") {
                    parent.action_popup.alert("선택한 날짜에 중복 신청된 건이 있습니다.");
                    parent.ResetProcess();
                    return;
                }

                // 주간 연장근무시간 조회
                var overtime = CallBackGetPrivateSum(DOC.getElementById("companyid").innerText, DOC.getElementById("userid").innerText, DOC.all("startdate").innerText);
                var tmpTime = parseInt(overtime) + parseInt(DOC.all("hour").innerText);
                if (tmpTime > 12) {
                    var holiHour = GetWeekHoli(DOC.getElementById("companyid").innerText, DOC.getElementById("userid").innerText, DOC.all("startdate").innerText);
                    if (holiHour == "0") {
                        parent.action_popup.alert("선택한 주의 연장 근무 시간 12시간이 초과되었습니다.");
                        parent.ResetProcess();
                        return;
                    }
                    else {
                        if (tmpTime > (12 + parseInt(holiHour))) {
                            parent.action_popup.alert("선택한 주의 연장 근무 시간 12시간이 초과되었습니다.");
                            parent.ResetProcess();
                            return;
                        }
                    }
                }

                var num = DOC.getElementById("BODY_etc_transportpay").value;
                num = num.replace(/,/gi, "");

                DOC.getElementById("istransportpay").innerText = "Y";
                DOC.getElementById("transportpay").innerText = num;
                DOC.getElementById("extrapay").innerText = parseInt(num) + 10000;

                if (DOC.getElementById("BODY_etc_traPay1").checked == true) {
                    DOC.getElementById("istransportpay").innerText = "N";
                    DOC.getElementById("transportpay").innerText = DOC.getElementById("BODY_etc_transportpay").value;
                    DOC.getElementById("extrapay").innerText = "10000";
                }

                DOC.getElementById("duration").innerText = "0"
                DOC.getElementById("customer").innerText = DOC.getElementById("BODY_etc_customer").value;
                DOC.getElementById("reason").innerText = DOC.getElementById("Reason").options[DOC.getElementById("Reason").selectedIndex].text;
                DOC.getElementById("reasondetails").innerText = DOC.getElementById("BODY_etc_ReasonDetail").innerText;

            }
        }


        parent.AddSubmit(); 		//framework func

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

    /**********************************************************************************************
    작성자   : 
    작성일자 : 
    작성목적 : 문서 KeyDownEvent
    **********************************************************************************************/
    function docKeyUpEvent(_event) {
        //tagId = _event.srcElement.id;
        tagId = _event.target.id;
        if (tagId == "") { return; }

        if (tagId == "BODY_etc_transportpay") {
            var num = DOC.getElementById("BODY_etc_transportpay").value;
            num = num.replace(/,/gi, "");

            if (!isNaN(num.trim())) {
                DOC.getElementById("BODY_etc_transportpay").value = AddComma(num);
                DOC.getElementById("transportpay").value = num;
            }
            else {
                parent.action_popup.alert("숫자만 입력하세요.");
                DOC.all("BODY_etc_transportpay").value = "";
                DOC.all("BODY_etc_transportpay").focus();
                return;
            }
        }
    }


    /**********************************************************************************************
    작성자   : 
    작성일자 : 
    작성목적 : 캘린더
    **********************************************************************************************/

    function SelectDateCode() {
        var url = "/DeskPlusMobile/Common/Calendar.aspx";
        parent.PopupWin(305, 370, url, "", "", true);
    }

    function GetDateUseDay(a, hdYear, hdMonth, hdDay) {
        if (hdMonth.length == 1) {
            hdMonth = "0" + hdMonth;
        }
        if (hdDay.length == 1) {
            hdDay = "0" + hdDay;
        }

        var date = hdYear + "년 " + hdMonth + "월 " + hdDay + "일 ";
        var date2 = hdYear + "-" + hdMonth + "-" + hdDay;

        DOC.getElementById("SPAN_STARTDATE").innerHTML = date;
        DOC.getElementById("startdate").innerText = date2;
    }



    function Calculation() {
        var sel1 = DOC.all("time1");
        var sel2 = DOC.all("time2");

        totval = parseInt(sel2.value) - parseInt(sel1.value);
        if (totval < 1) {
            DOC.getElementById("BODY_etc_hour").innerText = "";
            parent.action_popup.alert("시작시간이 종료시간보다 빨라야 합니다.");
        }
        else {
            // 점심시간 1시간 빼기
            if (parseInt(sel2.value) > 12 && parseInt(sel1.value) <= 13) {
                totval = totval - 1;
            }

            DOC.getElementById("BODY_etc_hour").innerText = totval;
            DOC.getElementById("starthour").innerText = sel1.value;
            DOC.getElementById("endhour").innerText = sel2.value;
            DOC.getElementById("hour").innerText = totval;
        }
    }

    function AddComma(num) {
        num = Number(num).toLocaleString('en');
        return num.split('.')[0];
    }

    $("#time1", DOC).change(function () {
        Calculation();
    });

    $("#time2", DOC).change(function () {
        Calculation();
    });
	
	

</script>

</head>
<body leftMargin="0" topMargin="0" scroll="no" onload="init()">
    <form id="Form1" method="post" runat="server">
    </form>
</body>
</html>
