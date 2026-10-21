<%@ Page language="c#" AutoEventWireup="true" Inherits="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BasePage" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Collections" %>
<%@ Import Namespace="ComponentArt.Web.UI" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper" %>


<script language="C#" runat="server">
    private string conn = ConfigurationManager.ConnectionStrings["SQLConnSolution"].ConnectionString;
	private void Page_Load(object sender, System.EventArgs e)
	{
	}
	
	[ComponentArtCallbackMethod]
    public string SetExpenseApprovalComplete(string _documentid)
	{
		try
		{

            string Sql = "update [GW_Interface TableUser] set Status = 'P' where documentid = '" + _documentid + "'";

            SQLHelper.ExecuteNonQuery(conn, CommandType.Text, Sql, null);

			return "true";
		}
		catch (Exception ex)
		{
			Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("SetSpeakerEvaluation == ",ex);
			return "-1";
		}
	}


    [ComponentArtCallbackMethod]
    public string SetExpenseApprovalReject(string _documentid)
    {
        try
        {

            string Sql = "update [GW_Interface TableUser] set Status = 'T', DocumentID = 0,  LegacyKey = '' where documentid = '" + _documentid + "'";

            SQLHelper.ExecuteNonQuery(conn, CommandType.Text, Sql, null);

            return "true";
        }
        catch (Exception ex)
        {
            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("SetSpeakerEvaluation == ", ex);
            return "-1";
        }
    }    
</script>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
<title></title>
<script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
<script language="javascript">
    var DOC = parent.ifrmForm.document;
    var docID = parent.DocumentID;
    var isLast = parent.IsLast; 				// 최종결재여부 (Y/N)
    var isZQ = parent.IsZQ; 					// 반려가능수신(ZQ) 존재여부 (Y/N)


    /**********************************************************************************************
    작성자   : 
    작성일자 : 
    작성목적 : 문서로딩시, 초기화작업
    **********************************************************************************************/
    function init() {
        // 두번째 결재자 본문 수정할때
    }

    function onComplete() {
        //최종 결재 시	
        if (isLast == "Y" && parent.ApprovalType != "R") {
            var rtnValue = SetExpenseApprovalComplete(docID);
            if (rtnValue == "-1") {
                alert("문제가 발생 했습니다.\n관리자에게 문의 해 주십시오.");
                return;
            }

        }

        if (parent.ApprovalType == "R") {
            var rtnValue = SetExpenseApprovalReject(docID);
            if (rtnValue == "-1") {
                alert("문제가 발생 했습니다.\n관리자에게 문의 해 주십시오.");
                return;
            }
        }

        parent.SubmitCustomCall(parent.ApprovalType);	// S(승인), A(전결), R(반송), RC(반송취소)
    }




</script>
</head>
<body onload="init()">
    <form id="Form1" method="post" runat="server">
    </form>
</body>
</html>
