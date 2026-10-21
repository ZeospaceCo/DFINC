<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetJudgeUserList.aspx.cs" Inherits="Zeospace.DeskPlus.Proposal.Web.Common.GetJudgeUserList" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Proposal.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Proposal.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />    
</head>
<body>
    <form id="form1" runat="server">
    <div id="layer_popup">
        <div class="inside">
            <div id="popup_header">
                <!-- 페이지타이틀 //-->
                <h1>1차심사자리스트</h1>
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnAdd" runat="server" Text="확인"  BeginEndTag="li" Class="btn btn-secondary" />
                    <cc3:ClientSmartButton ID="CbtnClose" runat="server" Text="닫기"  BeginEndTag="li" />
                </ul>
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3" style="padding-top:2px;height:404px;min-height:404px; border:0px solid #f00;">
                <div id="scrollDiv" >
                
                <div id="phJudgeList" style="clear: both;"></div>
                
                <!-- 테이블 끝 //-->

                </div>            
            </div>
        </div>

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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var ShareArrayList = "";
        var JudgeList = "<%=JudgeUserList %>";
        var MenuID = "<%=MenuID %>";
        var WindowObjectReference = null; //새창용
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja     
    </script>  
    <script type="text/javascript" src="/DeskPlusEIP/Proposal/Js/Common/GetJudgeUserList.js"></script>  
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Proposal/Js/Common/GetJudgeUserList.js");
    </script>
</body>
</html>
