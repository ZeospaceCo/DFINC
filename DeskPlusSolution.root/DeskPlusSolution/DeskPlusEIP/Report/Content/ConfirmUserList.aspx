<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfirmUserList.aspx.cs" Inherits="Zeospace.DeskPlus.Report.Web.Content.ConfirmUserList" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title>
        <%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
</head>
<body>
    <form id="form1" runat="server">
    <div class="layer_popup">
        <div class="inside">
            <div class="popup_header">
                <h1><%=GetResource("ReportUser")%></h1>
                <ul class="btn_right">
                    <li><cc3:ClientSmartButton ID="CbtnConfirm" runat="server"  class="btn btn-secondary"/></li>
                    <li><cc3:ClientSmartButton ID="CbtnClose" runat="server"/></li>
                </ul>
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3"  style="min-height:400px;height:410px;">
                <div id="scrollDiv">
                    
                    <div id="placeholder" style="clear: both;"></div>
                                                    
                </div>
            </div>
        </div>
    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js" ></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var StartDate  = "<%=StartDate%>";
        var Gubun  = "<%=Gubun%>";
        var R_ReportUser = "<%=GetResource("ReportUser") %>";
        var R_WorkView = "<%=GetResource("Preview") %>";   
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_Gubun = "<%=GetResource("Gubun") %>";
        var R_Save = "<%=GetResource("Save") %>";
        var R_ConfirmRequest = "<%=GetResource("ConfirmRequest") %>";
        var R_WorkConfirm = "<%=GetResource("WorkConfirm") %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Report/Js/Content/ConfirmUserList.js" ></script>
    <script type="text/javascript">
//        JsCall("/DeskPlusEIP/Report/Js/Content/ConfirmUserList.js");
    </script>
</body>
</html>