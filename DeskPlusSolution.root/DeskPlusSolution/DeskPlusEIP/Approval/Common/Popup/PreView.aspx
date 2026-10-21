<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PreView.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Common.Popup.PreView" %>
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
            <h1><%=this.GetResource("Preview")%> <span style="display:none"> /<%=this.GetResource("Print") %></span></h1>
            <ul class="btn_right">
                <li style="display:none"><a href="#" title="" class="btn">시행문보기</a></li>
                <cc3:ClientSmartButton ID="cbtnPrint" runat="server" BeginEndTag="li" Text="인쇄" Visible="false" />
                <li><a href="#" id="Close" class="btn"><%=GetResource("Close")%></a></li>                    
            </ul>
        </div>
        <div class="dotLine"></div>
        <div>

            <iframe id="ifrmForm" class="app_iframe" name="ifrmForm" src="about:blank" frameBorder="0" width="100%" height="550px"></iframe>

        </div>
    </div>
</div>
<div style="display:none">
    <textarea id="htaArea" style="DISPLAY: none" name="htaPrintArea" rows="2" cols="40"></textarea>
    <textarea id="htaTest" style="DISPLAY: " name="htaTest" rows="2" cols="40"></textarea>
</div>
</form>

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type='text/javascript' src='/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js'></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.print.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var targetObj = null;
        var ifrmObj = null;                     // ifrmForm object
        var NonActiveX = "<%=NonActiveX %>";    // activeX(F) , nonActiveX(T)
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Common/Popup/PreView.js"></script>       
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Common/Popup/PreView.js");
    </script>

</body>
</html>
