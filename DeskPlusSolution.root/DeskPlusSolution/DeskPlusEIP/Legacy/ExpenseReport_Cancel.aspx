<%@ Page language="c#" AutoEventWireup="true" %>
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
    public string SetExpenseApprovalReject(string _documentid)
    {
        try
        {

            string Sql = "update [GW_Interface TableUser] set Status = 'T', DocumentID = 0, LegacyKey = '' where documentid = '" + _documentid + "'";

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
<script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
<script language="javascript">
    var companyID = parent.CompanyID;
    var documentID = parent.cDocID;

    var DOC = parent.document;
    var section = parent.CustomSection;
    var isZD = true; 				// 발신여부 (발신true,수신false)

    /**********************************************************************************************
    작성자   : 
    작성일자 : 
    작성목적 : 문서로딩시, 초기화작업
    **********************************************************************************************/
    function init() {
        var rtnValue = SetExpenseApprovalReject(documentID);

        parent.ApproveDel(); 	//framework func

    }

    //상신전에 반드시 수행되어야 할 부분이 있다면 이부분에서 수행.
    function onComplete() {



    }


</script>
</head>
<body leftMargin="0" topMargin="0" scroll="no" onload="init()">
    <form id="Form1" method="post" runat="server">				
    </form>
</body>
</html>
