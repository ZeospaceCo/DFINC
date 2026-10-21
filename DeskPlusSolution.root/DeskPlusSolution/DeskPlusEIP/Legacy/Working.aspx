<%@ Page language="c#" AutoEventWireup="true" Inherits="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BasePage" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Collections" %>
<%@ Import Namespace="ComponentArt.Web.UI" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Xml" %>
<%@ Import Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper" %>

<script language="C#" runat="server">
    public string janDays1 = "";
	public string holiCount = "";    
	public string UserID = "";
    public string StartDate = "";
    public string EndDate = "";
    public string NowYM = "";
		
	private void Page_Load(object sender, System.EventArgs e)
	{
		if (!IsPostBack)
		{
            string NowYear = DateTime.Now.Year.ToString();
            string NowMonth = (Convert.ToInt32(DateTime.Now.Month.ToString()) < 10) ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString();
            NowYM = NowYear + NowMonth;
                        
			UserID = CurrentUserInfo.UserID.ToString();

			//총 연차수 가져오기
            DataSet dsReceive1 = GetReceive(UserID, NowYM, "1");					
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
            DataSet dsJan1 = GetJanDays(UserID, NowYM, "1");					
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
	
	//총연차수
    private DataSet GetReceive(string _userid, string _nowDate, string _type)
	{
		try
		{
			DataSet myDataSet = new DataSet();

            if (_userid == "" || string.IsNullOrEmpty(_userid) || _nowDate == "" || string.IsNullOrEmpty(_nowDate)) 
			{
				return  null;
			}

            string query = "SELECT HoliCount FROM DP_HOLI_Admin WHERE UserID = @UserID AND @NowDate >= StartDate AND @NowDate <= EndDate AND Type = @Type";

			using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnSolution"].ConnectionString))
			{
				SqlCommand mySqlCommand = cn.CreateCommand(); 
				mySqlCommand.CommandText = query; 
				mySqlCommand.CommandType = CommandType.Text; 
				mySqlCommand.Parameters.Add("@UserID", SqlDbType.VarChar, 1000).Value = _userid;
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
			string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
			Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
			return null;
		}			
    }

	//남은연차수
    private DataSet GetJanDays(string _userid, string _nowDate, string _type)
	{
		try
		{
			DataSet myDataSet = new DataSet();

            if (_userid == "" || string.IsNullOrEmpty(_userid) || _nowDate == "" || string.IsNullOrEmpty(_nowDate)) 
			{
				return  null;
			}

            string query = "SELECT SUM(HoliCount) - ISNULL((SELECT SUM(Duration) FROM DP_HOLI_Used WHERE UserID = DP_HOLI_Admin.UserID AND ProcessFlag = '0' AND WkItemCD = @Type AND LEFT(REPLACE(StartDate, '-', ''), 6) >= DP_HOLI_Admin.StartDate AND LEFT(REPLACE(EndDate, '-', ''), 6) <= DP_HOLI_Admin.EndDate), 0) FROM DP_HOLI_Admin WHERE UserID = @UserID AND @NowDate >= StartDate AND @NowDate <= EndDate AND Type = @Type GROUP BY USERID, StartDate, EndDate";		

			using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnSolution"].ConnectionString))
			{
				SqlCommand mySqlCommand = cn.CreateCommand(); 
				mySqlCommand.CommandText = query; 
				mySqlCommand.CommandType = CommandType.Text; 
				mySqlCommand.Parameters.Add("@UserID", SqlDbType.VarChar, 1000).Value = _userid;
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
			string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
			Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
			return null;
		}			
    }

    [ComponentArtCallbackMethod]
    public string GetEtcUserInfo()
    {
        try
        {
            string Result = "";

            DataSet myDataSet = new DataSet();

            string query = "select * from DP_ADD_Com where companyid = 1 and CategoryID = 1 order by CommonID";

            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnSolution"].ConnectionString))
            {
                SqlCommand mySqlCommand = cn.CreateCommand();
                mySqlCommand.CommandText = query;
                mySqlCommand.CommandType = CommandType.Text;

                cn.Open();

                SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter();
                mySqlDataAdapter.SelectCommand = mySqlCommand;
                mySqlDataAdapter.Fill(myDataSet, "DP_ADD_Com");
            }

            if (myDataSet != null && myDataSet.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < myDataSet.Tables[0].Rows.Count; i++)
                {
                    if (Result == "")
                    {
                        Result = myDataSet.Tables[0].Rows[i]["CommonID"].ToString() + "," + myDataSet.Tables[0].Rows[i]["NameBase"].ToString();
                    }
                    else
                    {
                        Result += "|" + myDataSet.Tables[0].Rows[i]["CommonID"].ToString() + "," + myDataSet.Tables[0].Rows[i]["NameBase"].ToString();
                    }
                }
            }

            return Result;
        }
        catch (SqlException ex)
        {
            string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
            return "NULL";
        }
    }

    //총연차수
    [ComponentArtCallbackMethod]
    public string GetEtcReceive(string _userid, string _nowDate, string _type)
    {
        try
        {
            string Result = "0";
            
            DataSet myDataSet = new DataSet();

            if (_userid == "" || string.IsNullOrEmpty(_userid) || _nowDate == "" || string.IsNullOrEmpty(_nowDate))
            {
                return Result;
            }

            string query = "SELECT HoliCount FROM DP_HOLI_Admin WHERE UserID = @UserID AND @NowDate >= StartDate AND @NowDate <= EndDate AND Type = @Type";

            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnSolution"].ConnectionString))
            {
                SqlCommand mySqlCommand = cn.CreateCommand();
                mySqlCommand.CommandText = query;
                mySqlCommand.CommandType = CommandType.Text;
                mySqlCommand.Parameters.Add("@UserID", SqlDbType.VarChar, 1000).Value = _userid;
                mySqlCommand.Parameters.Add("@NowDate", SqlDbType.VarChar, 1000).Value = _nowDate;
                mySqlCommand.Parameters.Add("@Type", SqlDbType.VarChar, 1000).Value = _type;

                cn.Open();

                SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter();
                mySqlDataAdapter.SelectCommand = mySqlCommand;
                mySqlDataAdapter.Fill(myDataSet, "DP_HOLI_Admin");
            }

            if (myDataSet != null && myDataSet.Tables[0].Rows.Count > 0)
            {
                Result = myDataSet.Tables[0].Rows[0][0].ToString();
            }

            return Result;
        }
        catch (SqlException ex)
        {
            string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
            return "0";
        }
    }

    //남은연차수
    [ComponentArtCallbackMethod]
    public string GetEtcJanDays(string _userid, string _nowDate, string _type)
    {
        try
        {
            string Result = "0";
            
            DataSet myDataSet = new DataSet();

            if (_userid == "" || string.IsNullOrEmpty(_userid) || _nowDate == "" || string.IsNullOrEmpty(_nowDate))
            {
                return Result;
            }

            string query = "SELECT SUM(HoliCount) - ISNULL((SELECT SUM(Duration) FROM DP_HOLI_Used WHERE UserID = DP_HOLI_Admin.UserID AND ProcessFlag = '0' AND WkItemCD = @Type AND LEFT(REPLACE(StartDate, '-', ''), 6) >= DP_HOLI_Admin.StartDate AND LEFT(REPLACE(EndDate, '-', ''), 6) <= DP_HOLI_Admin.EndDate), 0) FROM DP_HOLI_Admin WHERE UserID = @UserID AND @NowDate >= StartDate AND @NowDate <= EndDate AND Type = @Type GROUP BY USERID, StartDate, EndDate";

            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnSolution"].ConnectionString))
            {
                SqlCommand mySqlCommand = cn.CreateCommand();
                mySqlCommand.CommandText = query;
                mySqlCommand.CommandType = CommandType.Text;
                mySqlCommand.Parameters.Add("@UserID", SqlDbType.VarChar, 1000).Value = _userid;
                mySqlCommand.Parameters.Add("@NowDate", SqlDbType.VarChar, 1000).Value = _nowDate;
                mySqlCommand.Parameters.Add("@Type", SqlDbType.VarChar, 1000).Value = _type;

                cn.Open();

                SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter();
                mySqlDataAdapter.SelectCommand = mySqlCommand;
                mySqlDataAdapter.Fill(myDataSet, "DP_HOLI_Admin");
            }

            if (myDataSet != null && myDataSet.Tables[0].Rows.Count > 0)
            {
                Result = myDataSet.Tables[0].Rows[0][0].ToString();
            }

            return Result;
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
<script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
<script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
<script language="javascript">
    var DOC = parent.ifrmForm.document;
    var section = parent.CustomSection;
    var isZD = true; 				// 발신여부 (발신true,수신false)

    var userID = "<%= UserID %>";                  //유저아이디
    var janDays1 = "<%= janDays1 %>";              //남은연차 수
    var holi_receive1 = "<%= holiCount %>";        //총 연차 수
    var NowYM = "<%= NowYM %>";


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

                //DOC.getElementById("janDays").innerText = janDays1 + "일";
                //DOC.getElementById("holi_receive").innerText = holi_receive1 + "일";

                DOC.getElementById("janDays").innerText = "0일";
                DOC.getElementById("holi_receive").innerText = "0일";

                DOC.getElementById("userid").innerText = userID; //휴가자
                DOC.getElementById("accountuserid").innerText = userID; //상신자

                //DOC.getElementById("tdjandays1").innerText = janDays1;
                //DOC.getElementById("tdholireceive1").innerText = holi_receive1;

                DOC.getElementById("tdjandays1").innerText = "0";
                DOC.getElementById("tdholireceive1").innerText = "0";
                // 대리자 바인딩
                ProxyBind();

                //시간 바인딩
                TimeBind();

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

    // 대리자 바인딩
    function ProxyBind() {

        var arrInfo = GetEtcUserInfo().split("|");

        var new_option = document.createElement("option");
        new_option.text = "";
        new_option.value = "0";
        new_option.selected = true;
        DOC.getElementById("SelProxy").options.add(new_option, 0);

        for (i = 0; i < arrInfo.length; i++) {
            var temp = arrInfo[i].split(",");

            new_option = document.createElement("option");
            new_option.text = temp[1];
            new_option.value = temp[0];
            DOC.getElementById("SelProxy").options.add(new_option, i + 1);
        }
    }

    // 시간 바인딩
    function TimeBind() {

        var arrInfoH = "00,01,02,03,04,05,06,07,08,09,10,11,12,13,14,15,16,17,18,19,20,21,22,23";
        var arrInfoM = "00,30";

        var arrInfoHs = arrInfoH.split(",");
        var arrInfoMs = arrInfoM.split(",");

        DOC.getElementById("SelStartH").options[0] = new Option("", 0);
        DOC.getElementById("SelEndH").options[0] = new Option("", 0);
        DOC.getElementById("SelStartM").options[0] = new Option("", 0);
        DOC.getElementById("SelEndM").options[0] = new Option("", 0);

        for (i = 0; i < arrInfoHs.length; i++) {
            DOC.getElementById("SelStartH").options[i + 1] = new Option(arrInfoHs[i], i + 1);
            DOC.getElementById("SelEndH").options[i + 1] = new Option(arrInfoHs[i], i + 1);
        }

        for (i = 0; i < arrInfoMs.length; i++) {

            DOC.getElementById("SelStartM").options[i + 1] = new Option(arrInfoMs[i], i + 1);
            DOC.getElementById("SelEndM").options[i + 1] = new Option(arrInfoMs[i], i + 1);
        }
    }

    function SetProxy(sel) {

        var selUserID = sel.value;

        if (selUserID == "0") {
            selUserID = DOC.getElementById("accountuserid").innerText;
        }

        //총 연차수 가져오기
        //var holi_receive1 = GetEtcReceive(selUserID, NowYM, "1");
        var holi_receive1 = 0;
        //남은 연차수 가져오기
        //var janDays1 = GetEtcJanDays(selUserID, NowYM, "1");
        var janDays1 = 0;
        DOC.getElementById("janDays").innerText = janDays1 + "일";
        DOC.getElementById("holi_receive").innerText = holi_receive1 + "일";

        DOC.getElementById("userid").innerText = selUserID;

        DOC.getElementById("tdjandays1").innerText = janDays1;
        DOC.getElementById("tdholireceive1").innerText = holi_receive1;

        DOC.getElementById("WkItemCd").selectedIndex = 0;

        //초기화
        DOC.getElementById("SPAN_STARTDATE").innerHTML = "&nbsp;&nbsp;&nbsp;&nbsp;년 &nbsp;&nbsp;월 &nbsp;&nbsp;일 부터";
        DOC.getElementById("SPAN_ENDDATE").innerHTML = "&nbsp;&nbsp;&nbsp;&nbsp;년 &nbsp;&nbsp;월 &nbsp;&nbsp;일 까지";
        DOC.getElementById("SPAN_DURATION").innerHTML = "&nbsp;&nbsp;";
        DOC.getElementById("duration").innerText = "0";
        DOC.getElementById("start_date").innerText = "";
        DOC.getElementById("end_date").innerText = "";
        DOC.getElementById("start_half").innerText = "";
        DOC.getElementById("end_half").innerText = "";
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
                    alert("휴가 기간을 선택해 주세요.");
                    parent.ApprovalType = "";
                    parent.Common.SetLoading(false);
                    return;
                }

                if (DOC.getElementById("BODY_etc_Reason").innerText == "") {
                    alert("사유를 입력하세요.");
                    DOC.getElementById("BODY_etc_Reason").focus();
                    parent.ApprovalType = "";
                    parent.Common.SetLoading(false);
                    return;
                }

                // 시간체크
                if (DOC.getElementById("SelStartH").selectedIndex == 0 || DOC.getElementById("SelStartM").selectedIndex == 0) {
                    alert("시작 시간을 선택하세요.");
                    parent.ApprovalType = "";
                    parent.Common.SetLoading(false);
                    return;
                }
                else {
                    if (DOC.getElementById("SelEndH").selectedIndex == 0 || DOC.getElementById("SelEndM").selectedIndex == 0) {
                        alert("종료 시간을 선택하세요.");
                        parent.ApprovalType = "";
                        parent.Common.SetLoading(false);
                        return;
                    }
                    else {
                        if (DOC.getElementById("start_date").innerText == DOC.getElementById("end_date").innerText) {
                            if (DOC.getElementById("SelStartH").selectedIndex >= DOC.getElementById("SelEndH").selectedIndex) {
                                if (DOC.getElementById("SelStartM").selectedIndex >= DOC.getElementById("SelEndM").selectedIndex) {
                                    alert("시간 설정이 잘못되었습니다.");
                                    parent.ApprovalType = "";
                                    parent.Common.SetLoading(false);
                                    return;
                                }
                            }
                        }
                    }

                }

                DOC.getElementById("reason").innerText = DOC.getElementById("BODY_etc_Reason").innerText;

                // 잔여일자 체크  (잔여연차-신청연차)
                var chkDuration = parseFloat(DOC.getElementById("tdjandays1").innerText) - parseFloat(DOC.getElementById("duration").innerText);
                if (chkDuration < 0) {
                    alert("잔여연차를 초과하였습니다.");
                    parent.ApprovalType = "";
                    parent.Common.SetLoading(false);
                    return;
                }

                parent.document.getElementById("tbEtc1").value = DOC.getElementById("start_date").innerText + ";;" + DOC.getElementById("end_date").innerText + ";;" + DOC.getElementById("start_half").innerText + ";;" + DOC.getElementById("end_half").innerText + ";;" + DOC.getElementById("duration").innerText + ";;" + DOC.getElementById("userid").innerText + ";;" + DOC.getElementById("WkItemCd").value + ";;" + DOC.getElementById("reason").innerText;
            }
        }

        parent.AddSubmit(); 		//framework func
    }

    function SelectDateCode() {
        url = "/DeskPlusEIP/Common/Calendar/CalendarDualDialog.aspx?func=GetDateUseDay";
        window.open(url, "CalendarDialog", "left=350, top=100, toolbar=no,width=780,height=380,directories=no,status=no,scrollbars=no,resize=no,menubar=no");
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
        if (p_StartHalf == 1 || p_StartHalf == 2) p_duration = minusChk(p_duration);
        if (p_EndHalf == 1 || p_EndHalf == 2) p_duration = minusChk(p_duration);

        // 날짜체크
        var chkDate = CompareDT(p_startDate, p_endDate, "lessequal", "ko");
        if (!chkDate) {
            alert("시작일자가 종료일자보다 작아야 합니다.");
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

        //연차, 휴일근무, 리플레시 일때만 일수 반영
        if (DOC.getElementById("WkItemCd").value == "1") {
            DOC.getElementById("SPAN_DURATION").innerText = p_duration;
            DOC.getElementById("duration").innerText = p_duration;
        }
        else {
            DOC.getElementById("SPAN_DURATION").innerText = "0";
            DOC.getElementById("duration").innerText = "0";
        }

        DOC.getElementById("start_date").innerText = p_startDate;  //시작일
        DOC.getElementById("end_date").innerText = p_endDate;      //종료일
        DOC.getElementById("start_half").innerText = p_StartHalf;  //시작일 반차
        DOC.getElementById("end_half").innerText = p_EndHalf;      //종료일 반차

        if (DOC.getElementById("UPNM")) {
            DOC.getElementById("username").innerText = DOC.getElementById("UPNM").value; //유저명
        }
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
        tagId = _event.srcElement.id;
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
</script>
</head>
<body leftMargin="0" topMargin="0" scroll="no" onload="init()">
    <form id="Form1" method="post" runat="server">
    </form>
</body>
</html>
