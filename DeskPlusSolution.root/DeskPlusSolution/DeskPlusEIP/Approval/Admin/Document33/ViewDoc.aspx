<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewDoc.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Admin.Document33.ViewDoc" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase_old.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase_Old.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />

</head>
<body>
    <form id="form1" runat="server">
        <div id="layer_popup">
            <div class="top-left"></div>
            <div class="top-right"></div>
            <div class="inside">
                <div id="popup_header">
                    <!-- 페이지타이틀 //-->
                    <h1>
                        <span><%=GetResource("DocView")%></span>
                        <span class="app_caption">
                            * <a id="AccessTitleID" style="color: Blue"><%=GetResource("ViewAuth")%>(<b><span id="headerAccessTitleID"></span></b>)</a>
                            / <%=GetResource("AttachName")%>(<strong><span id="headerAttachCnt" action="attach">0</span><%=GetResource("EA")%></strong>) 
                            / <%=GetResource("BaseDoc")%>(<strong><span id="headerBaseDocCnt" action="basedoc">0</span><%=GetResource("EA")%></strong>)</span>
                    </h1>
                    <!-- 버튼 //-->
                    <ul class="btn_left"> 
                        <cc3:ClientSmartButtonOld ID="cbtnPrint" runat="server" BeginEndTag="li" />
                        <cc3:ClientSmartButtonOld ID="cbtnZipFile" runat="server" BeginEndTag="li" />
                        <cc3:ClientSmartButtonOld ID="cbtnHistory" runat="server" BeginEndTag="li" />
                        <cc3:ClientSmartButtonOld ID="cbtnDocInfo" runat="server" BeginEndTag="li" />
                        <cc3:ClientSmartButtonOld ID="cbtnEDM" runat="server" BeginEndTag="li" />
                        <cc3:ClientSmartButtonOld ID="cbtnMail" runat="server" BeginEndTag="li" />
                    </ul>
            
                    <ul class="btn_right">              
                        <cc3:ClientSmartButtonOld ID="cbtnDisuse" runat="server" BeginEndTag="li"  BeginEndTagClass="btn_red" />
                        <cc3:ClientSmartButtonOld ID="cbtnDel" runat="server" BeginEndTag="li" BeginEndTagClass="btn_red" />          
                        <cc3:ClientSmartButtonOld ID="cbtnClose" runat="server" BeginEndTag="li" />                        
                    </ul>
                </div>

                <div id="layout_content">

                    <!-- 양식로드 //-->
                    <iframe id="ifrmForm" class="app_iframe" name="ifrmForm" src="about:blank" frameBorder="0" width="100%" height=550></iframe>

                    <!-- 첨부파일 //-->
                    <div class="layout_subbox" id="solbatch" title="<%=GetResource("BatchPDFMsg")%>">
                        <h2><%=GetResource("AttachName")%></h2>
                        <ul class="btn_right">
                            <li id="zipline1"><cc3:ClientSmartButtonOld ID="cbtnZIP" runat="server" /></li>
                            <li id="zipline2" class="sec"></li>
                            <li id="pdfbatch"><cc3:ClientSmartButtonOld ID="cbtnPDF" runat="server" /></li>
                            <li><cc3:ClientSmartButtonOld ID="cbtnShowImage" runat="server" /></li>
                        </ul>
                    </div>
                    <div id="placeholder_file" style="clear: both;"></div>
                    
                    <!-- 근거문서 //-->
                    <div class="layout_subbox">
                        <h2><%=GetResource("BaseDoc")%></h2>
                    </div>
                    <div id="placeholder_baseDoc" style="clear: both;"></div>
                </div>

            </div>
        <div class="bottom-left"></div>
        <div class="bottom-right"></div>
    </div>
    <div style="display:none">
        <iframe id="ifrm" name="ifrm" src="about:blank"></iframe>
        <textarea id="htaFormHTML" style="DISPLAY: none" name="htaFormHTML" rows="2" cols="40" runat="server"></textarea>
        <textarea id="htaFormTEXT" style="DISPLAY: none" name="htaFormTEXT" rows="2" cols="40" runat="server"></textarea>
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
        var DocumentID = "<%=DocumentID %>";
        var ChildDocID = "<%=ChildDocID %>";
        var GroupID = "<%=GroupID %>";
        var AccountTag = "<%=AccountTag %>";
        var Number2    = "<%=Number2 %>"; 
        var AccessTitleName = "<%=AccessTitleName %>"; 
        
        var R_FileName = "<%=GetResource("FileName") %>";
        var R_FileType = "<%=GetResource("FileType") %>";
        var R_FileLen = "<%=GetResource("FileLen") %>";
        var R_FileDown = "<%=GetResource("FileDownload") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";

        var R_DocInfo = "<%=GetResource("DocInfo") %>";
        var R_DocNumber = "<%=GetResource("DocNumber") %>";
        var R_Subject = "<%=GetResource("Subject") %>";
        var R_ViewDoc = "<%=GetResource("DocView") %>";

        var R_Msg_Backup = "<%=GetResource("Msg_Backup") %>";
        var R_Msg_Mail = "<%=GetResource("Msg_Mail") %>";
        var R_Msg_AlarmError = "<%=GetResource("Msg_AlarmError")%>";

        var R_MSG_Del2 = "<%=GetResource("MSG_Del2")%>";          //"영구삭제하면 복구가 안됩니다. 삭제하시겠습니까?";
        var R_MSG_Del4 = "<%=GetResource("MSG_Del4")%>";          //"선택한 문서가 완전 영구삭제되었습니다.";

        var R_DisuseDoc = "<%=GetResource("DisuseDoc") %>";
        var R_MSG_SUCCESS = "<%=GetResource("MSG_SUCCESS")%>"; 	
        var R_MSG_PDFError = "<%=GetResource("MSG_PDFError")%>";

        var CompanyID = "<%=CurrentUserInfo.CompanyID.ToString() %>";
        var LoginID = "<%=CurrentUserInfo.LogInID.ToString() %>";
        var PWD = "<%=CurrentUserInfo.Password.ToString() %>";
        var UserID = "<%=CurrentUserInfo.UserID.ToString() %>";

        var R_NoFileData = "<%=GetResource("NoFileData")%>";
        var R_ErrorAttachFile = "<%=GetResource("ErrorAttachFile")%>";
        var R_ErrorBaseDoc = "<%=GetResource("ErrorBaseDoc")%>";
        var R_AllDownLoad = "<%=GetResource("AllDownLoad")%>";
        var R_MSG_SelectDownLoad = "<%=GetResource("MSG_SelectDownLoad")%>";

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Admin/Document33/ViewDoc.js?v1=1"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Admin/Document33/ViewDoc.js");
    </script>
    <form id="frmTotalDownLoad" name="frmTotalDownLoad" method="post" target="openWin">
    <input type="hidden" name="fileList" id="fileList" />
    <iframe id="ifrmAttachFile" frameborder="0" width="0" height="0" scrolling="no" src="about:blank"></iframe>
</body>
</html>
