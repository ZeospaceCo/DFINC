<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Common.Popup.View" %>
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
<body>
    <form id="form1" runat="server">
        <div id="layer_popup" class="view">
            <div class="inside">
                <div id="popup_header">
                    <!-- 페이지타이틀 //-->
                    <h1><%=GetResource("DocView")%></h1>
                    <!-- 버튼 //-->
                    <div class="btn_right">
                        <ul class="float-left">
                            <cc3:ClientSmartButton ID="cbtnPrint" runat="server" BeginEndTag="li" Text="인쇄" />
                            <li><cc3:ClientSmartButton ID="cbtnOpinionInfo" runat="server" /></li>
                            <li><a onclick="javascript:window.open('about:blank', '_self').close();" class="btn"><%=GetResource("Close")%></a></li>                      
                        </ul>
                    </div>
                </div>
                <div id="layout_caption">
                    <span class="app_caption" style="font-weight:bold;">
                        * <%=GetResource("AttachName")%>(<strong><span id="headerAttachCnt" runat="server">0</span><%=GetResource("EA")%></strong>) / <%=GetResource("BaseDoc")%>(<strong><span id="headerBaseDocCnt" runat="server">0</span><%=GetResource("EA")%></strong>)
                        <span style="width:5px;display:inline-block;"></span>
                        <%=GetResource("Msg_AlarmAlert")%>
                    </span>
                </div>
                <div id="layout_content"><!-- 컨텐츠 하단영역 -->
                    <div class="row">
                        <div class="col col-lg-12 content-area-body-left" style="min-height:680px"><!-- width 100%(col-12) -->
                            <!-- 양식로드 //-->
                            <iframe id="ifrmForm" class="app_iframe" name="ifrmForm" src="about:blank" frameBorder="0" width="100%" height="550"></iframe>
                         </div><!-- end col content-area-body-left -->
                    </div><!-- end row  --> 
                </div><!-- end layout_content  -->

            </div><!-- end inside  -->
        </div><!-- end layer_popup  -->
        <div style="display:none">
            <iframe id="ifrm" name="ifrm" src="about:blank"></iframe>
            <textarea id="htaFormHTML" style="DISPLAY: none" name="htaFormHTML" rows="2" cols="40" runat="server"></textarea>
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
        var CompanyID = "<%=CompanyID %>";
        var DocumentID = "<%=DocumentID %>";
        var ChildDocID = "<%=ChildDocID %>";
        var R_Msg_AlarmError = "<%=GetResource("Msg_AlarmError")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Common/Popup/View.js"></script> 
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Common/Popup/View.js");
    </script>
   
</body>
</html>
