<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetFavList.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.FormFlow.Common.GetFavList" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
</head>
<body>
<form id="form1" runat="server">

<div class="layer_popup">
    <div class="inside">
        <div class="popup_header">
            <h1><%=GetResource("Msg_Resource03")%></h1>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnClose" runat="server"></cc3:ClientSmartButton></li>
            </ul>
        </div>
        <div class="dotLine"></div>
        
        <div class="popup_layout_style3" style="min-height:170px;">

                <div class="layout_scroll"  style="min-height:200px;height:200px;"> 
                    <!-- 테이블 시작 //-->
                    
                    <div id="placeholder" style="clear: both;"></div>
                    
                    <!-- 테이블 끝 //-->
                </div>
                <!-- 페이징 끝 //-->
                <span class="app_subtxt"><%=GetResource("MsgFavSeqInfo3")%></span>

            </div>      
        
    </div>
</div>

</form>

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>           
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.hashtable.js"></script>

    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var TotalItemCount = "<%=TotalItemCount %>"; //게시물수
        var RecordPerPage = 5; //보여줄게시물수
        var Order = "<%=Order %>"; //정렬      

        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>"; 
        var R_Number = "<%=GetResource("Number") %>";
        var R_Subject = "<%=GetResource("Subject") %>";    //제목
        var R_Memo = "<%=GetResource("ETC") %>";       //기타,메모
        var R_AddDate = "<%=GetResource("AddDate") %>"; 
        var R_NoData = "<%=GetResource("NoContent") %>"; 

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang %>";
        var PreLangReal = "<%=PreLangReal %>"; //ja     

        var dicZD = new jQuery.Hashtable();        // 발신결재정보
        var dicZH = new jQuery.Hashtable();        // 발신결재정보
        var dicZA = new jQuery.Hashtable();        // 발신결재정보
        var dicZP = new jQuery.Hashtable();        // 수신결재정보
        var dicZQ = new jQuery.Hashtable();        // 수신결재정보
        var dicZC = new jQuery.Hashtable();        // 참조결재정보
        
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/FormFlow/Common/GetFavList.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/FormFlow/Common/GetFavList.js");
    </script>
</body>
</html>
