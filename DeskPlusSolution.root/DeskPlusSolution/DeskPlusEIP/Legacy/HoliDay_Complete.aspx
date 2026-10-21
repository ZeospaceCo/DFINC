<%@ Page language="c#" AutoEventWireup="true"%>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Collections" %>
<%@ Import Namespace="ComponentArt.Web.UI" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper" %>

<script language="C#" runat="server">
	private void Page_Load(object sender, System.EventArgs e)
	{
	}

	[ComponentArtCallbackMethod]
    public string Set_Holi(string _companyID, string _startdate, string _enddate, string _starthalf, string _endhalf, string _duration, string _userid, string _deptcodeid, string _WkItemCd, string _docID, string _reason, string _deptid, string _pathid)
	{
		try
		{
			if(_userid == "" || string.IsNullOrEmpty(_userid) || _docID == "" || string.IsNullOrEmpty(_docID)) 
			{
				return null;
			}

            string query = "IF NOT EXISTS (SELECT * FROM DP_HOLI_Used WHERE CompanyID = @CompanyID and Userid = @UserID and Documentid = @DocumentID) BEGIN INSERT DP_HOLI_Used (CompanyID, UserID, DeptCodeID, DeptID, PathID, StartDate, EndDate, StartHalf, EndHalf, Duration, WkItemCD, DocumentID, ProcessFlag, Reason) VALUES (@CompanyID, @UserID, @DeptCodeID, @DeptID, @PathID, @StartDate, @EndDate, @StartHalf, @EndHalf, @Duration, @WkItemCD, @DocumentID, '0', @Reason) END";

            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnSolution"].ConnectionString))
            {
                using (var cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@CompanyID", _companyID);
                    cmd.Parameters.AddWithValue("@UserID", _userid);
                    cmd.Parameters.AddWithValue("@DeptCodeID", _deptcodeid);
                    cmd.Parameters.AddWithValue("@DeptID", _deptid);
                    cmd.Parameters.AddWithValue("@PathID", _pathid);
                    cmd.Parameters.AddWithValue("@StartDate", _startdate);
                    cmd.Parameters.AddWithValue("@EndDate", _enddate);
                    cmd.Parameters.AddWithValue("@StartHalf", _starthalf);
                    cmd.Parameters.AddWithValue("@EndHalf", _endhalf);
                    cmd.Parameters.AddWithValue("@Duration", _duration);
                    cmd.Parameters.AddWithValue("@WkItemCD", _WkItemCd);
                    cmd.Parameters.AddWithValue("@DocumentID", _docID);
                    cmd.Parameters.AddWithValue("@Reason", Server.UrlDecode(_reason));

                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
			
			return "1";
		}
		catch (SqlException ex)
		{
			string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
			Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
			return "0";
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
            string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
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
            string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
            return null;
        }
    }

    // 퇴근시간 자동 등록 된것 중 오후반차 시간에 맞게 퇴근시간 다시 수정
    [ComponentArtCallbackMethod]
    public string SetAttendanceByHoliday(string _companyID, string _docID, string _userID)
    {
        try
        {
            if (_companyID == "" || string.IsNullOrEmpty(_companyID) || _userID == "" || string.IsNullOrEmpty(_userID) || _docID == "" || string.IsNullOrEmpty(_docID))
            {
                return "0";
            }

            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnSolution"].ConnectionString))
            {
                using (var cmd = new SqlCommand("DP_HR_Attendance_UpdWCTimeByHoliday", cn))
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
            string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
            return "0";
        }
    }

    // 지각, 조퇴 여부 신청 시간에 맞게 다시 수정
    [ComponentArtCallbackMethod]
    public string SetHolidayAtt(string _companyID, string _docID, string _userID)
    {
        try
        {
            if (_companyID == "" || string.IsNullOrEmpty(_companyID) || _userID == "" || string.IsNullOrEmpty(_userID) || _docID == "" || string.IsNullOrEmpty(_docID))
            {
                return "0";
            }

            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnSolution"].ConnectionString))
            {
                using (var cmd = new SqlCommand("DP_HR_Attendance_UpdHolidayAtt", cn))
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
            string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
            return "0";
        }
    }
            	
</script>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
<title></title>
<script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
<script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   

<script language="javascript">
var DOC		= parent.ifrmForm.document;
var docID	= parent.DocumentID ;
var isLast	= parent.IsLast;					// 최종결재여부 (Y/N)
var isZQ	= parent.IsZQ;						// 반려가능수신(ZQ) 존재여부 (Y/N)

var startdate = DOC.all("start_date").innerText;
var enddate = DOC.all("end_date").innerText;
var starthalf = DOC.all("start_half").innerText;
var endhalf = DOC.all("end_half").innerText;
var duration = DOC.all("duration").innerText;
var WkItemCd = DOC.all("WkItemCd").value;
var reason = DOC.all("reason").innerText;

var deptcodeid = "0";
if (DOC.all("deptcodeid")) deptcodeid = DOC.all("deptcodeid").innerText;

var title = DOC.all("title").innerText;

var username = DOC.getElementById("username").innerText;

var companyid = DOC.all("companyid").innerText;
var userid = DOC.all("userid").innerText;
var deptid = DOC.all("deptid").innerText;
var hrdeptid = DOC.all("hrdeptid").innerText;
var pathid = DOC.all("pathid").innerText;
var proxyuserid = DOC.all("proxyuserid").innerText;
var proxydeptid = DOC.all("proxydeptid").innerText;
var proxystartdate = DOC.all("proxystartdate").innerText;
var proxyenddate = DOC.all("proxyenddate").innerText;
var proxyWkItemCd = DOC.all("WkItemCd").value;

var chkAddProxyDate = "-1";

//////////////////////////////////////////////////////////////////////////////////////////////
// PIM-Schedule 필수항목
// var title = DOC.all("title").innerText; //제목
// var reason = DOC.all("reason").innerText; //사유
// var startdate = DOC.all("start_date").innerText; //시작일자
// var enddate = DOC.all("end_date").innerText; //종료일자
// var starthalf = DOC.all("start_half").innerText; //시작반차
// var endhalf = DOC.all("end_half").innerText; //종료반차
// var userid = DOC.all("userid").innerText; //등록자아이디
// var deptid = DOC.all("deptid").innerText; //등록자부서아이디
// var docID	= parent.DocumentID ; //문서번호
var opentype = "1"; // 공개설정 : 개인일정 "0" 공유일정 "1" 전체일정 "2" 부서일정 "3"
//////////////////////////////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////////////////////////////
// PIM-ToDo 필수항목
// var title = DOC.all("title").innerText; //제목
// var reason = DOC.all("reason").innerText; //사유
// var startdate = DOC.all("start_date").innerText; //시작일자
// var enddate = DOC.all("end_date").innerText; //종료일자
//var starthalf = DOC.all("start_half").innerText; //시작반차
//var endhalf = DOC.all("end_half").innerText; //종료반차
// var userid = DOC.all("userid").innerText; //등록자아이디
// var deptid = DOC.all("deptid").innerText; //등록자부서아이디
// var opentype = "1"; // 공개설정 : 개인일정 "0" 공유일정 "1" 전체일정 "2" 부서일정 "3"
// var docID	= parent.DocumentID ; //문서번호
//////////////////////////////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////////////////////////////
// RM 필수항목
// var title = DOC.all("title").innerText; //제목
// var reason = DOC.all("reason").innerText; //사유
// var startdate = DOC.all("start_date").innerText; //시작일자
// var enddate = DOC.all("end_date").innerText; //종료일자
// var starthalf = DOC.all("start_half").innerText; //시작반차
// var endhalf = DOC.all("end_half").innerText; //종료반차
// var userid = DOC.all("userid").innerText; //등록자아이디
// var WkItemCd = DOC.all("WkItemCd").value; //연차구분코드
// var docID	= parent.DocumentID ; //문서번호
var categoryid = "6"; //DP_RM_CATEGORY 설정
var listidnull = "12"; //listid 기본값 설정
//////////////////////////////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////////////////////////////
// Interface 공통항목
var modulesend = "APP"; // 발송모듈
var sendtarget = ""; //수신모듈
//////////////////////////////////////////////////////////////////////////////////////////////

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 문서로딩시, 초기화작업
**********************************************************************************************/
function init()
{
	
}

function onComplete()
{
	//최종 결재 시
	if (parent.ApprovalType != "R" && isZQ == "N" && parent.AccountTag == "ZD")
	{
	    if (parent.ApprovalType == "A" || isLast == "Y") {

	        if (DOC.getElementById("chkProxy").checked == true) {
	            //대결자 기간 중복 체크
	            var chkProxyDate = CallBackGetProxySelByDate(companyid, userid, deptid, proxystartdate, proxyenddate); // 대결자 중복 기간 체크
	            if (chkProxyDate == "FALSE") {
	                alert("선택한 날짜에 이미 대결자가 지정되어 있습니다. \n관리자에게 문의하세요.");
	                return;
	            }

	            //대결자 저장
	            AddProxyData();

	            if (chkAddProxyDate != "0") {
	                alert("대결자 추가 시 오류가 발생되었습니다. \n관리자에게 문의하세요.");
	                return;
	            }
	        }

	        if (DOC.all("Selholitype")) {
	            WkItemCd = DOC.all("Selholitype").value;
	        }

	        var rlt = "0";
	        rlt = Set_Holi(companyid, startdate, enddate, starthalf, endhalf, duration, userid, deptcodeid, WkItemCd, docID, escape(reason), hrdeptid, pathid);

	        if (rlt != "1") {
	            alert("최종 결재 시 오류가 발생하였습니다.\n관리자에게 문의하세요.");
	            return;
	        }

	        // 퇴근시간 자동 등록 된것 중 오후반차 시간에 맞게 퇴근시간 다시 수정
	        var rltAtt = "0";
	        rltAtt = SetAttendanceByHoliday(companyid, docID, userid);

	        if (rltAtt != "1") {
	            alert("최종 결재 시 오류가 발생하였습니다.\n관리자에게 문의하세요.");
	            return;
	        }

            /*
	        // 지각, 조퇴 여부 신청 시간에 맞게 다시 수정
	        var rltHolidayAtt = "0";
	        rltHolidayAtt = SetHolidayAtt(companyid, docID, userid);

	        if (rltHolidayAtt != "1") {
	            alert("최종 결재 시 오류가 발생하였습니다.\n관리자에게 문의하세요.");
	            return;
	        }
            */

	        //오늘의일정 데이터 저장
	        //AddScheduleData();

	        //오늘의할일 데이터 저장
	        AddToDoData();

	        //자원관리 데이터 저장
	        AddRMData();
	        
	    }
	}

	parent.SubmitCustomCall(parent.ApprovalType);	// S(승인), A(전결), R(반송), RC(반송취소)
}

/*****************************
* METHOD : 대결자 데이터 저장
* 작성자 : kkzz52
*****************************/
function AddProxyData() {

    var url = "/DeskPlusEIP/Approval/Ws/Preference/Proxy/Add.asmx/AddProxyUser";

    ParamArray = {
        'companyid': companyid,
        'userid': userid,
        'deptid': deptid,
        'proxyuserid': proxyuserid,
        'proxydeptid': proxydeptid,
        'proxystartdate': proxystartdate,
        'proxyenddate': proxyenddate,
        'WkItemCd': proxyWkItemCd,
        'reason': ""
    };

    var obj = eval("SuccessAddProxyData");

    //배열값을 파라미터형태로 반환
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/*****************************
* METHOD : 대결자 성공시
* 작성자 : kkzz52
*****************************/
SuccessAddProxyData = function (data, status) {
    chkAddProxyDate = data.d;
}

/*****************************
* METHOD : 오늘의일정 데이터 저장
* 작성자 : 이지원
*****************************/
function AddScheduleData() {

    //스케쥴저장
    var url = "/DeskPlusEIP/Pim/Ws/Interface/Add.asmx/AddSchedule";

    //수신모듈
    sendtarget = "PIM-Schedule";

    //파라미터값 배열에 담기
    //제목,사유,시작일자,종료일자,시작반차,종료반차,등록자아이디,등록자부서아이디,공개설정,발송모듈,수신모듈,등록자이름,문서번호
    ParamArray = {
        'Title': title,
        'Reason' : "",
        'StartTime': startdate,
        'EndTime': enddate,
        'StartHalf': starthalf,
        'EndHalf': endhalf,
        'UserID': userid,
        'DeptID': deptid,
        'OpenType': opentype,
        'ModuleSend': modulesend,
        'SendTarget': sendtarget,
        'UserName': username,
        'DOCID': docID
    };

    var obj = eval("SuccessScheduleData");

    //배열값을 파라미터형태로 반환
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);    
}

/*****************************
* METHOD : 오늘의 할일 데이터 저장
* 작성자 : 이지원
*****************************/
function AddToDoData() {

    var url = "/DeskPlusEIP/Pim/Ws/Interface/Add.asmx/AddToDo";

    //수신모듈
    sendtarget = "PIM-ToDo";

    //파라미터값 배열에 담기
    //제목,사유,시작일자,종료일자,등록자아이디,등록자부서아이디,공개설정,발송모듈,수신모듈,등록자이름,문서번호
    ParamArray = {
        'Title': title,
        'Reason': "",
        'StartTime': startdate,
        'EndTime': enddate,
        'StartHalf': starthalf,
        'EndHalf': endhalf,
        'UserID': userid,
        'DeptID': deptid,
        'OpenType': opentype,
        'ModuleSend': modulesend,
        'SendTarget': sendtarget,
        'UserName': username,
        'DOCID': docID
    };

    var obj = eval("SuccessToDoData");

    //배열값을 파라미터형태로 반환
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/*****************************
* METHOD : 자원관리 데이터 저장
* 작성자 : 이지원
*****************************/
function AddRMData() {

    var url = "/DeskPlusEIP/RM/Ws/Interface/Add.asmx/AddRM";

    //수신모듈
    sendtarget = "RM";

    //파라미터값 배열에 담기
    //제목,사유,시작일자,종료일자,등록자아이디,폴더선택,자원기본값,자원선택,발송모듈,수신모듈,등록자이름,문서번호
    ParamArray = {
        'Title': title,
        'Reason': "",
        'StartTime': startdate,
        'EndTime': enddate,
        'StartHalf': starthalf,
        'EndHalf': endhalf,
        'UserID': userid,
        'DeptID': deptid,
        'CategoryID' : categoryid,
        'ListIDNull' : listidnull,
        'ListID' : WkItemCd,
        'ModuleSend': modulesend,
        'SendTarget': sendtarget,
        'UserName': username,
        'DOCID': docID
    };

    var obj = eval("SuccessRMData");

    //배열값을 파라미터형태로 반환
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/*****************************
* METHOD : 오늘의일정 성공시
* 작성자 : 이지원
*****************************/
SuccessScheduleData = function (data, status) {
    if (data.d == "-1") {
        alert("오늘의 일정 저장시 오류가 발생하였습니다. \n관리자에게 문의하세요.");
    }
}

/*****************************
* METHOD : 오늘의할일 성공시
* 작성자 : 이지원
*****************************/
SuccessToDoData = function (data, status) {
    if (data.d == "-1") {
        alert("오늘의 할일 저장시 오류가 발생하였습니다. \n관리자에게 문의하세요.");
    }
}

/*****************************
* METHOD : 자원관리 성공시
* 작성자 : 이지원
*****************************/
SuccessRMData = function (data, status) {
    if (data.d == "-1") {
        alert("자원관리 저장시 오류가 발생하였습니다. \n관리자에게 문의하세요.");
    }
}

</script>
</head>
<body onload="init()">
    <form id="Form1" method="post" runat="server">
    </form>
</body>
</html>
