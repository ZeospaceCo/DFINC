<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewBaseDoc.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Folder.Common.ViewBaseDoc" %>
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
<body scroll="auto">
    <form id="form1" runat="server">
    <div id="layer_popup" class="view">
        <div class="inside">
            <div id="popup_header">
                <!-- 페이지타이틀 //-->
                <h1><%=GetResource("ViewBaseDoc")%></h1>
                <!--span class="app_caption">
                    * <%=GetResource("AttachName")%>(<strong><span id="headerAttachCnt" action="attach">0</span><%=GetResource("EA")%></strong>)
                        / <%=GetResource("BaseDoc")%>(<strong><span id="headerBaseDocCnt" action="basedoc">0</span><%=GetResource("EA")%></strong>)</span-->
                <!-- 버튼 //-->
                <div class="btn_right">
                    <ul class="float-left">
                        <cc3:ClientSmartButton ID="cbtnPrint" runat="server" BeginEndTag="li"  />
                        <cc3:ClientSmartButton ID="cbtnZipFile" runat="server" BeginEndTag="li" />
                        <cc3:ClientSmartButton ID="cbtnDocInfo" runat="server" BeginEndTag="li" />
                        <cc3:ClientSmartButton ID="cbtnMail" runat="server" BeginEndTag="li" />
                        <cc3:ClientSmartButton ID="cbtnOpinionInfo" runat="server" BeginEndTag="li" />
                        <cc3:ClientSmartButton ID="cbtnCir" runat="server" BeginEndTag="li" class="btn btn-secondary" Visible="false" />                        
                        <cc3:ClientSmartButton ID="cbtnPost" runat="server" BeginEndTag="li" class="btn btn-secondary" Visible="false" /> 
                        <li><a onclick="self.close();" class="btn"><%=GetResource("Close")%></a></li>
                    </ul>
                </div>
            </div>
            <div class="dotLine"></div>
            <div id="layout_content"><!-- 컨텐츠 하단영역 -->
                <div class="row">
                    <div class="col-12 col-lg-9 content-area-body-left" style="min-height:680px"><!-- width 100%(col-12) -->
                        <!-- 양식로드 //-->
                        <iframe id="ifrmForm" class="app_iframe" name="ifrmForm" src="about:blank" frameBorder="0" width="100%" height=550></iframe>
                    </div><!-- end col content-area-body-left -->
                    
                    <div class="col-12 col-lg-3 content-area-body-right"  ><!-- width 12,3,4/12 -->
						<ul>
                            <!-- 첨부파일 //-->
                            <li id="download" class="download" style="min-height: 47px">
                                <ul>
                                    <li id="zipline1"><cc3:ClientSmartButton ID="cbtnZIP" runat="server" class="btn btn-sm bg-white" /></li>
                                    <li id="zipline2" class="sec btn btn-sm bg-white" ></li>
                                    <li><cc3:ClientSmartButton ID="cbtnShowImage" runat="server" class="btn btn-sm bg-white" /></li>
                                </ul>
                            </li>      
                            <li class="title"><%=GetResource("AttachName")%></li>                          
                            <li>
                                <div id="placeholder_file" style="clear: both;"></div>
                            </li>
            
                            <!-- 근거문서 //-->
                            <li class="title"><%=GetResource("BaseDoc")%></li>                       
                            <li>
                                <div id="placeholder_baseDoc" style="clear: both;"></div>
                            </li>
 						</ul>
                    </div><!-- end col content-area-body-right -->
                
                </div><!-- end row  --> 
            </div><!-- end layout_content  -->

        </div><!-- end inside  -->
    </div><!-- end layer_popup  -->
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var CompanyID = "<%=CurrentUserInfo.CompanyID.ToString() %>";
        var DocumentID = "<%=DocumentID %>"; 
        var ChildDocID = "<%=ChildDocID %>";   
        var DocState = "<%=DocState %>";

        var R_FileName = "<%=GetResource("FileName") %>";
        var R_FileType = "<%=GetResource("FileType") %>";
        var R_FileLen = "<%=GetResource("FileLen") %>";
        var R_FileDown = "<%=GetResource("FileDownload") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";

        var R_Msg_Mail = "<%=GetResource("Msg_Mail") %>";		//메일 발송할 결재 문서를 선택해 주세요
        var R_MSG_Cir = "<%=GetResource("MSG_Cir")%>"; 		//회람할 결재 문서를 선택해 주세요
        var R_DocInfo = "<%=GetResource("DocInfo") %>";
        var R_DocNumber = "<%=GetResource("DocNumber") %>";
        var R_Subject = "<%=GetResource("Subject") %>";
        var R_ViewDoc = "<%=GetResource("DocView") %>";
        var R_Msg_AlarmError = "<%=GetResource("Msg_AlarmError")%>";
        var R_MSG_SelectDownLoad = "<%=GetResource("MSG_SelectDownLoad")%>";

        var R_NoFileData = "<%=GetResource("NoFileData")%>";
        var R_ErrorAttachFile = "<%=GetResource("ErrorAttachFile")%>";
        var R_ErrorBaseDoc = "<%=GetResource("ErrorBaseDoc")%>";
        var R_AllDownLoad = "<%=GetResource("AllDownLoad")%>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>";
        var SelectAll= "<%=GetResource("SelectAll") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Folder/Common/ViewBaseDoc.js?v=1"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Folder/Common/ViewBaseDoc.js");
    </script>
    <form id="frmTotalDownLoad" name="frmTotalDownLoad" method="post" target="openWin">
    <input type="hidden" name="fileList" id="fileList" />
    <iframe id="ifrmAttachFile" frameborder="0" width="0" height="0" scrolling="no" src="about:blank"></iframe>
</body>
</html>
