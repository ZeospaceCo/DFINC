<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetDocFlow.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Status.Common.GetDocFlow" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_AppStatus.css"/>

</head>
<body style="background:none transparent;">
    <form id="form1" runat="server">

    <div class="layer_popup">
        <div class="inside">
            <div class="popup_header">
                <h1><%=GetResource("ConfirmAppInfo")%></h1>
                <ul class="btn_right">
                    <li><cc3:ClientSmartButton ID="cbtnClose" runat="server" /></li>
                </ul>
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3">
            
                <ul class="apH_statTit">
                    <li><%=GetResource("AppProcess")%></li>
                </ul>
                <div class="apH_flow" style="min-height:280px;">
                    <div id="phFlow"></div>
	                <div id="phFlowSubTit" class="subTit"></div>
                    <div id="phFlowSub" class="sub" style="display:none;"></div>
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
        var DocumentID = "<%=DocumentID %>";
        var AccountTag = "<%=AccountTag %>";
        var IsComplete = "<%=IsComplete %>";
        var BaseImgPath = "<%=BaseImgPath %>";
        var IsZP = "<%=IsZP %>";

        var R_FormName = "<%=GetResource("FormName3")%>";
        var R_Subject = "<%=GetResource("Subject")%>";
        var R_WriteDate = "<%=GetResource("WriteDate")%>";
        var R_Report = "<%=GetResource("Report")%>";
        var R_Receive = "<%=GetResource("Receive")%>";
        var R_Carboncopy = "<%=GetResource("Carboncopy")%>";
        var R_ViewZPZQ = "<%=GetResource("ViewZPZQ")%>";
        var R_ZD = "<%=GetResource("R_ZD")%>";
        var R_ZA = "<%=GetResource("R_ZA")%>";
        var R_ZH = "<%=GetResource("R_ZH")%>";
        var R_ZR = "<%=GetResource("R_ZR")%>";
        var R_LaterOnTit = "<%=GetResource("LaterOnTit")%>";
        var R_ProxyTit = "<%=GetResource("ProxyTit")%>";
        var R_TitSub = "<%=GetResource("TitSub")%>";

        var R_InfoH = "<%=GetResource("R_InfoH")%>";        //상신보류중
        var R_InfoW = "<%=GetResource("R_InfoW")%>";        //대기중
        var R_InfoI = "<%=GetResource("R_InfoI")%>";        //진행중
        var R_InfoC = "<%=GetResource("R_InfoC")%>";        //완료
        var R_InfoR = "<%=GetResource("R_InfoR")%>";        //반려
        var R_InfoD = "<%=GetResource("R_InfoD")%>";        //보류중
        var R_InfoY = "<%=GetResource("R_InfoY")%>";        //검토중
        var R_InfoT = "<%=GetResource("R_InfoT")%>";        //반려대상
        var R_InfoTR = "<%=GetResource("R_InfoTR")%>";      //반려
        var R_InfoZR = "<%=GetResource("R_InfoZR")%>";      //수신반려
        var R_InfoZ = "<%=GetResource("R_InfoZ")%>";        //수신
        var R_InfoCR = "<%=GetResource("R_InfoCR")%>";      //참조

    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Status/Common/GetDocFlow.js"></script>       
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Status/Common/GetDocFlow.js");
    </script>

</body>
</html>
