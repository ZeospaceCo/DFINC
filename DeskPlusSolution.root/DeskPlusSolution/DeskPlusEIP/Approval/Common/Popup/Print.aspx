<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Print.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Common.Popup.Print" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
</head>
<OBJECT id="factory" style="DISPLAY: none" codeBase="/DeskPlusEIP/Util/Component/Print/smsx2.cab#Version=7,5,0,20" classid="clsid:1663ed61-23eb-11d2-b92f-008048fdd814" viewastext></OBJECT>
<body>
 <form id="form1" runat="server">
<div class="layer_popup">
    <div class="inside">
        <div class="popup_header">
            <h1><%=this.GetResource("Print")%></h1>
            <ul class="btn_right"">
                <cc3:ClientSmartButton ID="cbtnPrint" runat="server" BeginEndTag="li" Text="인쇄" />
                <li><a href="#" id="Close" class="btn"><%=GetResource("Close")%></a></li>                    
            </ul>
            <div style="display:inline-block;float:right;padding-top:5px;padding-right:10px;">
                <input type="checkbox" id="chkOpinion" class="isopinion" /><span class="isopinion"><%=GetResource("PrintOpt01")%></span>
                &nbsp;
                <input type="checkbox" id="chkEtcs" class="isprint" /><span class="isprint"><%=GetResource("PrintOpt02")%></span>
            </div>
        </div>
        <div class="dotLine"></div>
        <div class="">

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
        var targetObj   = null;
        var ifrmObj     = null;
        var printarea   = null;                    // ifrmForm object
        var NonActiveX = "<%=NonActiveX %>";    // activeX(F) , nonActiveX(T)
        var ChildDocID = "<%=ChildDocID %>";
        var DocumentID = "<%=DocumentID %>";
        var FullDomainPath = "<%=FullDomainPath %>";

        var AttachName = "<%=GetResource("AttachName")%>";
        var BaseDoc = "<%=GetResource("BaseDoc")%>";

    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Common/Popup/Print.js?v1"></script>       

</body>
</html>
