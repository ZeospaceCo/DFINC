<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewHistory.aspx.cs" Inherits="Zeospace.DeskPlus.ETC.Web.Holiday.Popup.ViewHistory" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_ETC.css"/>
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_ETC.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />    
</head>
<body>
    <form id="form1" runat="server">

    <div class="layer_popup">
        <div class="inside">
            <div class="popup_header">
                <h1><asp:Literal ID="ltlHeader" runat="server"></asp:Literal></h1>
                <ul class="btn_right">                    
                    <li><cc3:ClientSmartButton ID="cbtnClose" runat="server" /></li>
                </ul>
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3">
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">        
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var UserID = "<%=UserID%>";        
        var StartDate = "<%=StartDate%>";
        var Type = "<%=Type%>";
        var PageUrl = "<%= Request.Url.ToString() %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_HoliTitle = "<%=GetResource("HoliTitle") %>";     
        var R_YearText = "<%=GetResource("YearText") %>"; 
        var R_ModName = "<%=GetResource("ModUser")%>";    //"수정자";
        var R_ModDate = "<%=GetResource("ModDate")%>";   //"수정일";  
        var R_HoliType1 = "<%=GetResource("HoliType1") %>";
        var R_HoliType2 = "<%=GetResource("HoliType2") %>";
        var R_HoliType3 = "<%=GetResource("HoliType3") %>";   
        var R_NoContent = "<%=GetResource("NoContent") %>";              
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/ETC/Js/Holiday/Popup/ViewHistory.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/ETC/Js/Holiday/Popup/ViewHistory.js");
    </script>
</body>
</html>