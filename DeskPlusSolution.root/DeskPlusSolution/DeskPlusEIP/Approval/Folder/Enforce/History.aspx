<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="History.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Folder.Enforce.History" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body  scroll="auto">
    <form id="form1" runat="server">

    <div id="layer_popup">
        <div class="inside">
            <div id="popup_header">
                <!-- 페이지타이틀 //-->
                <h1><%=GetResource("EnforceHistory")%></h1>
                <span class="app_caption">* <%=GetResource("EnforceHistoryComment")%></span>
                <!-- 버튼 //-->
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnClose" runat="server" BeginEndTag="li" />
                </ul>
            </div>

            <div class="dotLine"></div>
            <div class="popup_layout_style3" style="min-height:200px">

                <!-- 테이블 시작 //-->
                
                <div id="placeholder" style="clear: both;"></div>
                
                <!-- 테이블 끝 //-->

            </div>

        </div>
    </div>	
    <div style="display:none">
    </div>		
    </form>
      <!-- ############# start of modal ############## -->
    <div id="zeoModalContact">
        <!-- 드래그공간(br Tag 처리) //-->        
        <div id="zeoModalDrag" class="modal-drag"><p></p></div>
        <form id="zeoModalForm" name="zeoModalForm" target="zeoModalIfrm" method="post"></form>
        <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="no" src="about:blank"></iframe>
    </div>
    <div id="zeoModalBG"></div>
    <!-- ############# end of modal ############## -->

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript">

        var EnforceID = "<%=EnforceID%>";   // 공문신청ID

        var R_NoContent = "<%=GetResource("NoContent")%>";
        var R_EnforceSendDate = "<%=GetResource("EnforceSendDate")%>";
        var R_EnforceSendType = "<%=GetResource("EnforceSendType")%>";
        var R_EnforceSendUser = "<%=GetResource("EnforceSendUser")%>";

    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Folder/Enforce/History.js"></script>    
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Folder/Enforce/History.js");
    </script>
</body>
</html>