<%@ Page language="c#" AutoEventWireup="true" %>
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
    public string Set_Holi(string _companyID, string _startdate, string _enddate, string _starthalf, string _endhalf, string _duration, string _userid, string _WkItemCd, string _docID, string _reason)
    {
        try
        {
            if (_userid == "" || string.IsNullOrEmpty(_userid) || _docID == "" || string.IsNullOrEmpty(_docID))
            {
                return "0";
            }

            string query = "IF NOT EXISTS (SELECT * FROM DP_HOLI_Used WHERE CompanyID = @CompanyID and Userid = @UserID and Documentid = @DocumentID) BEGIN INSERT DP_HOLI_Used (CompanyID, UserID, StartDate, EndDate, StartHalf, EndHalf, Duration, WkItemCD, DocumentID, ProcessFlag, Reason) VALUES (@CompanyID, @UserID, @StartDate, @EndDate, @StartHalf, @EndHalf, @Duration, @WkItemCD, @DocumentID, '0', @Reason) END";

            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnSolution"].ConnectionString))
            {
                using (var cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@CompanyID", _companyID);
                    cmd.Parameters.AddWithValue("@UserID", _userid);
                    cmd.Parameters.AddWithValue("@StartDate", _startdate);
                    cmd.Parameters.AddWithValue("@EndDate", _enddate);
                    cmd.Parameters.AddWithValue("@StartHalf", _starthalf);
                    cmd.Parameters.AddWithValue("@EndHalf", _endhalf);
                    cmd.Parameters.AddWithValue("@Duration", _duration);
                    cmd.Parameters.AddWithValue("@WkItemCD", _WkItemCd);
                    cmd.Parameters.AddWithValue("@DocumentID", _docID);
                    cmd.Parameters.AddWithValue("@Reason", _reason);

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
    
    // 전체일정 추가
    [ComponentArtCallbackMethod]
    public string SetSchedule(string _companyID, string _userID, string _startdate, string _enddate, string _starthalf, string _endhalf, string _gubun, string _selstarth, string _selstartm, string _selendh, string _selendm)
    {
        try
        {
            if (_companyID == "" || string.IsNullOrEmpty(_companyID) || _userID == "" || string.IsNullOrEmpty(_userID))
            {
                return "0";
            }

            string sTime = " " + _selstarth + ":" + _selstartm;
            string eTime = " " + _selendh + ":" + _selendm;                           

            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnSolution"].ConnectionString))
            {
                using (var cmd = new SqlCommand("DP_PIM_Schedule_InsHoiday", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CompanyID", _companyID);
                    cmd.Parameters.AddWithValue("@UserID", _userID);
                    cmd.Parameters.AddWithValue("@StartDate", _startdate + sTime);
                    cmd.Parameters.AddWithValue("@EndDate", _enddate + eTime);
                    cmd.Parameters.AddWithValue("@Gubun", _gubun);

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
<script language="javascript">
    var DOC = parent.ifrmForm.document;
    var docID = parent.DocumentID;
    var isLast = parent.IsLast; 				// 최종결재여부 (Y/N)
    var isZQ = parent.IsZQ; 					// 반려가능수신(ZQ) 존재여부 (Y/N)

    var startdate = DOC.all("start_date").innerText;
    var enddate = DOC.all("end_date").innerText;
    var starthalf = DOC.all("start_half").innerText;
    var endhalf = DOC.all("end_half").innerText;
    var duration = DOC.all("duration").innerText;
    var userid = DOC.all("userid").innerText;
    var WkItemCd = DOC.all("WkItemCd").value;
    var reason = DOC.all("reason").innerText;
    var username = DOC.all("username").innerText;
    var selstarth = DOC.getElementById("SelStartH").options[DOC.getElementById("SelStartH").selectedIndex].text;
    var selstartm = DOC.getElementById("SelStartM").options[DOC.getElementById("SelStartM").selectedIndex].text;
    var selendh = DOC.getElementById("SelEndH").options[DOC.getElementById("SelEndH").selectedIndex].text;
    var selendm = DOC.getElementById("SelEndM").options[DOC.getElementById("SelEndM").selectedIndex].text;

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

                var rlt = "0";
                rlt = Set_Holi("1", startdate, enddate, starthalf, endhalf, duration, userid, WkItemCd, docID, reason);

                var gubun = DOC.getElementById("WkItemCd").options[DOC.getElementById("WkItemCd").selectedIndex].text;

                var rlt2 = "0";

                rlt2 = SetSchedule("1", userid, startdate, enddate, starthalf, endhalf, gubun, selstarth, selstartm, selendh, selendm);

                if (rlt != "1" || rlt2 != "1") {
                    alert("최종 결재 시 오류가 발생하였습니다.\n관리자에게 문의하세요.");
                    return;
                }
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
