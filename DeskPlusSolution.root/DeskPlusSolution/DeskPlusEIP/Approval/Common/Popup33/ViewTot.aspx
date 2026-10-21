<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewTot.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Common.Popup33.ViewTot" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase_Old.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase_Old.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div id="layout_body" class="write" style="display: none"></div>
        <div id="layer_popup">
            <div class="top-left"></div>
            <div class="top-right"></div>
            <div class="inside">
                <div id="layout_header">
                    <!-- 페이지타이틀 //-->
                    <h1>
                        <span><%=GetResource("DocView")%></span>
                        <span class="app_caption" style="font-weight:bold;">
                            * <%=GetResource("Form3")%>(<b><span id="headerIsGuard"></span></b>)
                            / <%=GetResource("ViewAuth")%>(<b><span id="headerAccessTitleID"></span></b>)
                            <span style="width:5px;display:inline-block;"></span>
                            * <%=GetResource("AttachName")%>(<strong><span id="headerAttachCnt" action="attach" runat="server">0</span><%=GetResource("EA")%></strong>) 
                            / <%=GetResource("BaseDoc")%>(<strong><span id="headerBaseDocCnt" action="basedoc" runat="server">0</span><%=GetResource("EA")%></strong>)
                        </span>
                    </h1>
                    <!-- 버튼 //-->
                    <ul class="btn_left"> 
                        <li><cc3:ClientSmartButtonOld ID="cbtnPrint" runat="server" /></li>
                        <li class="cot"><cc3:ClientSmartButtonOld ID="cbtnZipFile" runat="server" /></li>
                        <li class="cot"><cc3:ClientSmartButtonOld ID="cbtnDocInfo" runat="server" /></li>
                        <li class="cot"><cc3:ClientSmartButtonOld ID="cbtnMail" runat="server"/></li>
                        <li class="cot"><cc3:ClientSmartButtonOld ID="cbtnForwardList" runat="server"/></li>
                    </ul>
                    <ul class="btn_right">
                        <li class="cot" style="display: none"><cc3:ClientSmartButtonOld ID="cbtnBoard" runat="server" BeginEndTagClass="btn_blue" /></li>
                        <li class="cot"><cc3:ClientSmartButtonOld ID="cbtnOpinion" runat="server" BeginEndTagClass="btn_blue" /></li>
                        <li class="cot" style="display: none"><cc3:ClientSmartButtonOld ID="cbtnOpinionAdd" runat="server" BeginEndTagClass="btn_blue" /></li>
                        <li class="cot"><cc3:ClientSmartButtonOld ID="cbtnForward" runat="server" BeginEndTagClass="btn_blue"  /></li>
                        <li class="cot" style="display: none"><cc3:ClientSmartButtonOld ID="cbtnCir" runat="server" BeginEndTagClass="btn_blue" /></li>
                        <li class="sec"></li>
                        <li><a onclick="javascript:window.open('about:blank', '_self').close();"><span><%=GetResource("Close")%></span></a></li>                      
                    </ul>
                </div>
                <div class="dotLine"></div>
                <div class="popup_layout_style3" style="height: 620px;">

                    <!-- 양식로드 //-->
                    <iframe id="ifrmForm" class="app_iframe" name="ifrmForm" src="about:blank" frameBorder="0" style="width: 99%" height="550"></iframe>

                    <!-- 첨부파일 //-->
                    <div class="layout_subbox">
                        <h2><%=GetResource("AttachName")%></h2>
                        <ul class="btn_right">
                            <li id="zipline1"><cc3:ClientSmartButtonOld ID="cbtnZIP" runat="server" /></li>
                            <li id="zipline2" class="sec"></li>
                            <li id="pdfbatch"><cc3:ClientSmartButtonOld ID="cbtnPDF" runat="server" /></li>
                            <cc3:ClientSmartButtonOld ID="cbtnShowImage" runat="server" BeginEndTag="li" />
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
        <asp:LinkButton ID="lbtnSubmit" runat="server" OnClick="lbtnSubmit_Click"></asp:LinkButton>
        <textarea id="htaFormHTML" style="display: none" name="htaFormHTML" rows="2" cols="40" runat="server"></textarea>
        <textarea id="htaFormTEXT" style="display: none" name="htaFormTEXT" rows="2" cols="40" runat="server"></textarea>
        <asp:TextBox ID="tbOpinion" runat="server"></asp:TextBox>
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js?v1=1"></script>    
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
        var Number2    = "<%=Number2 %>"; 
        var GroupID = "<%=GroupID %>";
        var IsAuth = "<%=IsAuth %>";
        IsAuth = IsAuth.toUpperCase();
        var ApprovalState = "<%= ApprovalState %>";
        var AccessTitleName = "<%=AccessTitleName %>"; 
        var IsGuardName = "<%=IsGuardName %>"; 
        var ForwardDocumentID   = "<%=ForwardDocumentID %>";

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
        var R_Msg_AlarmErrorAuth= "<%=GetResource("Msg_AlarmErrorAuth")%>";
        var R_MSG_SelectDownLoad = "<%=GetResource("MSG_SelectDownLoad")%>";

        var R_NoFileData = "<%=GetResource("NoFileData")%>";
        var R_ErrorAttachFile = "<%=GetResource("ErrorAttachFile")%>";
        var R_ErrorBaseDoc = "<%=GetResource("ErrorBaseDoc")%>";
        var R_AllDownLoad = "<%=GetResource("AllDownLoad")%>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Common/Popup33/ViewTot.js?v1=1"></script> 
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Common/Popup/ViewTot.js");
    </script>
    <!-- ############# 결재 문서 백업 ############## -->
    <div style="display: none">
    <form id="frmBackup" name="frmBackup" target="ifrm" method="post">
    <input type="text" style="DISPLAY: none" id="module" name="module" value="" />
    <input type="text" style="DISPLAY: none" id="listID" name="listID" value="" />
    <input type="text" style="DISPLAY: none" id="cirDocID" name="cirDocID" value="" />
    <textarea id="contents" style="DISPLAY: none" name="Contents" rows="2" cols="40" runat="server"></textarea>
    </form>
    </div>
    <!-- ############# 결재 문서 백업 ############## -->
    <form id="frmTotalDownLoad" name="frmTotalDownLoad" method="post" target="openWin">
    <input type="hidden" name="fileList" id="fileList" />
    <iframe id="ifrmAttachFile" frameborder="0" width="0" height="0" scrolling="no" src="about:blank"></iframe>
</body>
</html>