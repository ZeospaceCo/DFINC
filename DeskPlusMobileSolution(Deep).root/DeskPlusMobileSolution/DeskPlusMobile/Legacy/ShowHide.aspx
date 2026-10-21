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


    private void Page_Load(object sender, System.EventArgs e)
    {
        if (!IsPostBack)
        {
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
    var isZD = true;  // 발신여부 (발신true,수신false)	

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
                jQuery.each(parent.$("#btnShowHideGroup"), function () { this.style.display = ""; });
                isZD = true;
                break;
            case "ADDRECEIVE":
                // 문서상신(수신)
                isZD = false;

                break;
            case "MOD":
                // 수정후재상신(발신)
                jQuery.each(parent.$("#btnShowHideGroup"), function () { this.style.display = ""; });
                isZD = true;

                break;
            case "MODRECEIVE":
                // 수정후재상신(수신)
                jQuery.each(parent.$("#btnShowHideGroup"), function () { this.style.display = ""; });
                isZD = false;
                break;
            case "MODCONTENT":
                //결재선 및 본문수정 (수발신)
                jQuery.each(parent.$("#btnShowHideGroup"), function () { this.style.display = ""; });
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

        DOC.getElementById("approvalshowhide").style.display = "";

        if (isZD = true && (section.toUpperCase() == "ADD" || section.toUpperCase() == "MOD" || section.toUpperCase() == "MODCONTENT")) {
            var rlt = true;
            if (section.toUpperCase() == "MODCONTENT") //결재선 및 본문수정(수신)인 경우 제외
            {
                if (parent.AccountTag.indexOf("ZQ") == 0 || parent.AccountTag.indexOf("ZP") == 0) {
                    rlt = false;
                }
            }

            if (rlt) {

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
        tagId = _event.srcElement.id;
        if (tagId == "") { return; }
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
        tagId = _event.srcElement.id;
        if (tagId == "") { return; }
    }


    /**********************************************************************************************
    작성자   : 
    작성일자 : 
    작성목적 : 상신이후 호출함수
    **********************************************************************************************/
    function showHide() {
        var chkText = parent.$("#btnShowHide").text();

        if (chkText == "결재라인▽") {           
            parent.$("#btnShowHide").text("결재라인△");
            DOC.getElementById("approvalshowhide").style.display = "";
        }
        else {
            parent.$("#btnShowHide").text("결재라인▽");
            DOC.getElementById("approvalshowhide").style.display = "none";
        }
    }


</script>

</head>
<body leftMargin="0" topMargin="0" scroll="no" onload="init()">
    <form id="Form1" method="post" runat="server">
    </form>
</body>
</html>
