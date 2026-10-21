<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Folder.Common.View" %>
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
        <div id="layout_body" class="view">
            <div class="layout_full">
                <div id="layout_header">
                    <!-- 페이지타이틀 //-->
                    <h1><%=GetResource("DocView")%></h1>
                    <!-- 버튼 //-->
                    <ul class="btn_right">
                        <li><cc3:ClientSmartButton ID="cbtnPrint" runat="server" /></li>
                        <li class="cot"><cc3:ClientSmartButton ID="cbtnZipFile" runat="server" /></li>
                        <li class="cot"><cc3:ClientSmartButton ID="cbtnHistory" runat="server" /></li>
                        <li class="cot"><cc3:ClientSmartButton ID="cbtnDocInfo" runat="server" /></li>
                        <li class="cot"><cc3:ClientSmartButton ID="cbtnEDM" runat="server" /></li>
                        <li class="cot"><cc3:ClientSmartButton ID="cbtnMail" runat="server"/></li>
                        <li class="cot"><cc3:ClientSmartButton ID="cbtnForwardList" runat="server"/></li>
                        <li class="cot"><cc3:ClientSmartButton ID="cbtnOpinionInfo" runat="server" /></li>
                        <li class="cot"><cc3:ClientSmartButton ID="cbtnBoard" runat="server" class="btn btn-secondary" /></li>
                        <li class="cot"><cc3:ClientSmartButton ID="cbtnOpinion" runat="server" class="btn btn-secondary" /></li>
                        <li class="cot"><cc3:ClientSmartButton ID="cbtnOpinionAdd" runat="server" class="btn btn-secondary" /></li>
                        <li class="cot"><cc3:ClientSmartButton ID="cbtnForward" runat="server" class="btn btn-secondary" /></li>
                        <li class="cot"><cc3:ClientSmartButton ID="cbtnPassed" runat="server" class="btn btn-secondary" /></li>
                        <li class="cot"><cc3:ClientSmartButton ID="cbtnCir" runat="server" class="btn btn-secondary" /></li>
                        <li><cc3:ClientSmartButton ID="cbtnList" runat="server" /></li>
                    </ul>
                </div>
                <div id="layout_caption">
                    <span class="app_caption" style="font-weight:bold;">
                        * <%=GetResource("Form3")%>(<b><span id="headerIsGuard"></span></b>)
                        / <a id="AccessTitleID" style="color: Blue"><%=GetResource("ViewAuth")%>(<b><span id="headerAccessTitleID"></span></b>)</a>
                        / <%=GetResource("CarbonView")%>(<b><span id="headerCarbonView"></span></b>)
                        <span style="width:5px;display:inline-block;"></span>
                        * <%=GetResource("AttachName")%>(<strong><span id="headerAttachCnt" action="attach">0</span><%=GetResource("EA")%></strong>) 
                        / <%=GetResource("BaseDoc")%>(<strong><span id="headerBaseDocCnt" action="basedoc">0</span><%=GetResource("EA")%></strong>)
                    </span>
                </div>

                <div id="layout_content">
                    <div class="row">
                        <div class="col-12 col-lg-9 col-xl-8 content-area-body-left"><!-- width 100%(col-12) -->

                            <!-- 양식로드 //-->
                            <iframe id="ifrmForm" class="app_iframe" name="ifrmForm" src="about:blank" frameBorder="0" width="100%" height=550></iframe>

                        </div><!-- end col-3 content-area-body-right -->
                        <div class="col-12 col-lg-3 col-xl-4 content-area-body-right"><!-- width 100%(col-12) -->
                    	    <ul>
							    <li id="download" class="download" style="min-height: 47px">
                                    <ul id="solbatch" >
                                        <li id="zipline1"><cc3:ClientSmartButton ID="cbtnZIP" runat="server" class="btn btn-sm bg-white" /></li>
                                        <li id="zipline2" class="sec btn btn-sm bg-white"></li>
                                        <li id="pdfbatch"><cc3:ClientSmartButton ID="cbtnPDF" runat="server" class="btn btn-sm bg-white" /></li>
                                        <li><cc3:ClientSmartButton ID="cbtnShowImage" runat="server" class="btn btn-secondary" /></li>
                                    </ul>
                                </li>
                        	    <li class="title">
							    <%=GetResource("AttachName")%>
							    </li>
                                <li>
                                    <div id="placeholder_file" style="clear: both;"></div>
                                </li>
							    <!-- 근거문서 //-->
                           	    <li class="title">
							    <%=GetResource("BaseDoc")%>
							    </li>            
                                <li>                                
                                    <div id="placeholder_baseDoc" style="clear: both;"></div>                    
                                </li>
                            </ul>
                	    </div><!-- end col-3 content-area-body-right -->
				    </div><!-- end row  -->
                </div><!-- end layout_content  -->


            </div><!-- end layout_full  -->
        </div><!-- end layout_body  -->
        <div style="display: none">
            <iframe id="ifrm" name="ifrm" src="about:blank"></iframe>
            <asp:LinkButton ID="lbtnSubmit" runat="server" OnClick="lbtnSubmit_Click"></asp:LinkButton>
            <textarea id="htaFormHTML" style="display: none" name="htaFormHTML" rows="2" cols="40" runat="server"></textarea>
            <textarea id="htaFormTEXT" style="display: none" name="htaFormTEXT" rows="2" cols="40" runat="server"></textarea>
            <asp:TextBox ID="tbOpinion" runat="server"></asp:TextBox>
        </div>

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
        var ForwardDocumentID   = "<%=ForwardDocumentID %>";     
        var RecordPerPage = "<%=RecordPerPage %>";
        var CurrentPage = "<%=CurrentPage %>"; 
        var SearchTerm = "<%=SearchTerm %>"; 
        var SearchTermValue = "<%=SearchTermValue %>"; 
        var SearchField = "<%=SearchField %>"; 
        var SearchValue = "<%=SearchValue %>"; 
        var Order = "<%=Order %>"; 
        var FolderStatusID = "<%=FolderStatusID %>"; 
        var TargetPage = "<%=TargetPage %>"; 
        var ChildDocID = "<%=ChildDocID %>"; 
        var GroupID = "<%=GroupID %>"; 
        var Number2    = "<%=Number2 %>"; 

        var CompanyID = "<%=CurrentUserInfo.CompanyID.ToString() %>";
        var LoginID = "<%=CurrentUserInfo.LogInID.ToString() %>";
        var PWD = "<%=CurrentUserInfo.Password.ToString() %>";
        var UserID = "<%=CurrentUserInfo.UserID.ToString() %>";

        var AccessTitleName = "<%=AccessTitleName %>"; 
        var IsGuardName = "<%=IsGuardName %>"; 
        var IsCarbonView = "<%=IsCarbonView %>";

        /**********************************************************************************************
            문서함에서 사용하는 파라미터
        **********************************************************************************************/        
        var CabinetID = "<%=CabinetID %>";
        var Types = "<%=Types %>";
        var FormID = "<%=FormID %>"; //양식id
        var YYYY = "<%=YYYY %>"; //년도
        var YYYYMM = "<%=YYYYMM %>"; //년월
        var HeaderName = "<%=HeaderName %>"; //말머리 
        var IsReject = "<%=IsReject %>";

        var R_FileName = "<%=GetResource("FileName") %>";
        var R_FileType = "<%=GetResource("FileType") %>";
        var R_FileLen = "<%=GetResource("FileLen") %>";
        var R_FileDown = "<%=GetResource("FileDownload") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_Msg_Backup = "<%=GetResource("Msg_Backup") %>";
        var R_Msg_Mail = "<%=GetResource("Msg_Mail") %>";
        var R_MSG_Forward = "<%=GetResource("MSG_Forward")%>"; 	//전달할 문서를 선택해 주세요	
        var R_MSG_SelTransferDoc = "<%=GetResource("MSG_SelTransferDoc")%>"; 
        var R_MSG_SUCCESS = "<%=GetResource("MSG_SUCCESS")%>"; 		//처리되었습니다.
        var R_DocInfo = "<%=GetResource("DocInfo") %>";
        var R_DocNumber= "<%=GetResource("DocNumber") %>";
        var R_Subject= "<%=GetResource("Subject") %>";
        var R_ViewDoc = "<%=GetResource("DocView") %>";
        var R_MSG_Cir = "<%=GetResource("MSG_Cir")%>"; 	
        var R_Msg_AlarmError = "<%=GetResource("Msg_AlarmError")%>";
        var R_MSG_PDFError = "<%=GetResource("MSG_PDFError")%>";
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
    <!--script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Folder/Common/View.js??v=3"></script-->
    <script type="text/javascript">
        JsCall("/DeskPlusEIP/Approval/Js/Folder/Common/View.js");
    </script>

    </form>
    <!-- ############# start of modal ############## -->
    <div id="zeoModalContact">
        <!-- 드래그공간(br Tag 처리) //-->
        <div id="zeoModalDrag" class="modal-drag">
            <p>
            </p>
        </div>
        <form id="zeoModalForm" name="zeoModalForm" target="zeoModalIfrm" method="post">
        </form>
        <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="no" src="about:blank">
        </iframe>
    </div>
    <div id="zeoModalBG">
    </div>
    <!-- ############# end of modal ############## -->
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
