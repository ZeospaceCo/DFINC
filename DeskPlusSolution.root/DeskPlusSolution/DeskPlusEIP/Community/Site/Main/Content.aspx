<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Content.aspx.cs" Inherits="Zeospace.DeskPlus.Community.Web.Site.Main.Content" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Community.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Community.css"/>
    <link rel="stylesheet" type="text/css" href="<%=CommunityPath%>/Community.css" />    
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />    
</head>
<body>
    <form id="form1" runat="server">
<div id="layout_body" class="write">

    <div class="cm_siteMain">

        <!-- 타이틀 //-->
        <div class="header">
        <div class="gap">
            <span class="tit" id="userInfo" runat="server"></span>
            <ul class="info">
                <li><span class="txtbul" id="ComInfo1" runat="server"></span></li>
                <li><span class="txtbul" id="ComInfo2" runat="server"></span></li>
                <li><span class="txtbul" id="ComInfo3" runat="server"></span></li>
            </ul>
        </div>
        </div>

        <!-- 화면관리 //-->
        <div class="viewer"><asp:Label title="" ID="lbContent" runat="server"></asp:Label></div>


        <div style="height:10px;"></div>
        <!-- 기본게시판 //-->
        <div class="default">
            <div class="issue">
            
                <h2><%=GetResource("IssueTitle")%></h2>
                
                <div id="phIssueList" style="clear: both;"></div>  
                
            </div>
            <div style="height:20px;"></div>
            <div class="replay" >
            
                <h2><%=GetResource("ReplyBest")%></h2>
                
                <div id="phReplyList" style="clear: both;"></div>
                

            </div>
            <div class="cnt">
            
                <h2><%=GetResource("ClickBest")%></h2>
                
                <div id="phViewList" style="clear: both;"></div>
                

            </div>
        </div>

        <div style="height:20px;"></div>
        <!-- 시작화면 //-->
        <asp:Panel ID="plList" runat="server"> </asp:Panel>
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
    <form id="frmTotalDownLoad" name="frmTotalDownLoad" method="post" target="openWin">
    <input type="hidden" name="fileList" id="fileList" />
    <iframe id="ifrmAttachFile" frameborder="0" width="0" height="0" scrolling="no" src="about:blank">
    </iframe>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type='text/javascript' src='/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js'></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>    
    <script type="text/javascript">
        var CommunityID = "<%=CommunityID %>";
        var Grade = "<%=Grade %>"; 
        var ImagePath = "<%=BaseImgPath%>";
        var R_Number = "<%=GetResource("Number") %>";
        var R_MenuName = "<%=GetResource("MenuName") %>";
        var R_Title = "<%=GetResource("Title") %>";        
        var R_Registrant = "<%=GetResource("Registrant") %>";
        var R_RegDate = "<%=GetResource("RegDate") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_Del = "<%=GetResource("Del") %>";  
        var R_CompleteConfirm = "<%=GetResource("CompleteConfirm") %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
        var R_DelIssue = "<%=GetResource("DelIssue") %>";
        var R_ViewCount = "<%=GetResource("ViewCount") %>";
        var R_ReplyCount = "<%=GetResource("ReplyCount") %>";
        var R_Gubun ="<%=GetResource("Gubun") %>";
        var R_AllDownLoad = "<%=GetResource("AllDownLoad") %>";        
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Community/Js/Site/Main/Content.js"></script>    
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Community/Js/Site/Main/Content.js");
    </script>
</body>
</html>
