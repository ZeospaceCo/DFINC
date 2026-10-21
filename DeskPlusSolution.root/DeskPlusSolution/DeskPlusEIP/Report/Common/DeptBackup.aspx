<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeptBackup.aspx.cs" Inherits="Zeospace.DeskPlus.Report.Web.Common.DeptBackup" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"
    TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title>
        <%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Board.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Board.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="layer_popup">
        <div class="inside" style="min-height: 325px">
            <div class="popup_header">
                <h1><%=GetResource("DeletedDept")%></h1>
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnClose" runat="server" BeginEndTag="li" />
                </ul>
            </div>
            <div class="dotLine">
            </div>
            <div class="popup_layout_style3">
                <!-- 테이블 시작 //-->
                <div id="placeholder" style="clear: both;"></div>
                <!-- 테이블 끝 //-->
            </div>
        </div>
    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var R_DeptID = "<%=GetResource("DeptCode")%>";
        var R_DeptName = "<%=GetResource("DeptName")%>";
        var R_NoContent = "<%=GetResource("NoContent")%>";
        var Callback = "<%=Callback %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Report/Js/Common/DeptBackup.js"></script>
    <script type="text/javascript">
//        JsCall("/DeskPlusEIP/Report/Js/Common/DeptBackup.js");
    </script>
</body>
</html>