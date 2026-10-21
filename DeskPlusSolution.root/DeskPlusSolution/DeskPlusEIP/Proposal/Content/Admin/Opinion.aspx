<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Opinion.aspx.cs" Inherits="Zeospace.DeskPlus.Proposal.Web.Content.Admin.Opinion" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />    
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Proposal.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Proposal.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />    
</head>
<body>
    <form id="form1" runat="server">
    <div id="layer_popup">

        <div class="inside">
            <div id="popup_header">
                <!-- 페이지타이틀 //-->
                <h1>심사관 의견</h1>
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="CbtnClose" runat="server" Text="닫기"  BeginEndTag="li" />
                </ul>
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3"  style="padding-top:2px;height:120px;min-height:120px; border:0px solid #f00;">
                <div id="scrollDiv" >
                    <textarea name="textarea" id="textarea" class="textarea" runat="server" style="width: 97%; border: 1px solid rgb(210, 210, 210);"></textarea>
                </div>
            </div>
        </div>
    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var ProposalNo = "<%=ProposalNo %>";
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var WindowObjectReference = null; //새창용
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja     
    </script>    
    <script type="text/javascript" src="/DeskPlusEIP/Proposal/Js/Content/Admin/Opinion.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Proposal/Js/Content/Admin/Opinion.js");
    </script>
</body>
</html>
