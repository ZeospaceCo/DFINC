<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Folder.Carboncopy.View" %>
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
                        <cc3:ClientSmartButton ID="cbtnPrint" runat="server" BeginEndTag="li" />
                        <cc3:ClientSmartButton ID="cbtnZipFile" runat="server" BeginEndTag="li" />                        
                        <cc3:ClientSmartButton ID="cbtnDocInfo" runat="server" BeginEndTag="li" Visible="false"/>
                        <cc3:ClientSmartButton ID="cbtnMail" runat="server" BeginEndTag="li" />   
                        <cc3:ClientSmartButton ID="cbtnOpinionInfo" runat="server" BeginEndTag="li" />              
                        <cc3:ClientSmartButton ID="cbtnOpinion" runat="server" BeginEndTag="li" class="btn btn-secondary" />
                        <cc3:ClientSmartButton ID="cbtnList" runat="server" BeginEndTag="li" />                        
                    </ul>
                </div>
                <div id="layout_caption">
                    <span class="app_caption">
                        * <%=GetResource("AttachName")%>(<strong><span id="headerAttachCnt" action="attach"><%= AttachFileCount %></span><%=GetResource("EA")%></strong>) 
                        / <%=GetResource("BaseDoc")%>(<strong><span id="headerBaseDocCnt" action="basedoc"><%= BaseDocCount %></span><%=GetResource("EA")%></strong>)</span>
                </div>
                <div id="toPrint">
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
                    </div>
                </div>
            </div>
        </div>
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.print.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var CompanyID = "<%=CurrentUserInfo.CompanyID.ToString() %>";
        var CirDocumentID = "<%=CirDocumentID %>";   
        var ChildDocID = "<%=ChildDocID %>";    
        var LastDocID = "<%=LastDocID %>";
        var DocumentID = "<%=DocumentID %>";
        var RecordPerPage = "<%=RecordPerPage %>";
        var CurrentPage = "<%=CurrentPage %>"; 
        var SearchTerm = "<%=SearchTerm %>"; 
        var SearchTermValue = "<%=SearchTermValue %>"; 
        var SearchField = "<%=SearchField %>"; 
        var SearchValue = "<%=SearchValue %>"; 
        var Order = "<%=Order %>"; 
        var FolderStatusID = "<%=FolderStatusID %>"; 
        var TargetPage = "<%=TargetPage %>"; 

        /**********************************************************************************************
            문서함에서 사용하는 파라미터
        **********************************************************************************************/        
        var CabinetID = "<%=CabinetID %>";
        var Types = "<%=Types %>";
        var FormID = "<%=FormID %>"; //양식id
        var YYYY = "<%=YYYY %>"; //년도
        var YYYYMM = "<%=YYYYMM %>"; //년월
        var HeaderName = "<%=HeaderName %>"; //말머리 


        var R_FileName = "<%=GetResource("FileName") %>";
        var R_FileType = "<%=GetResource("FileType") %>";
        var R_FileLen = "<%=GetResource("FileLen") %>";
        var R_FileDown = "<%=GetResource("FileDownload") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_MSG_Backup2 = "<%=GetResource("MSG_Backup2") %>";
        var R_MSG_Mail3 = "<%=GetResource("MSG_Mail3")%>"; 	//메일 발송할 회람 문서를 선택해 주세요
        var R_DocInfo = "<%=GetResource("DocInfo") %>";
        var R_DocNumber= "<%=GetResource("DocNumber") %>";
        var R_Subject= "<%=GetResource("Subject") %>";
        var R_DocView = "<%=GetResource("DocView") %>";
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
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Folder/Carboncopy/View.js?v=1"></script>    
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Folder/Carboncopy/View.js");
    </script>
    <form id="frmTotalDownLoad" name="frmTotalDownLoad" method="post" target="openWin">
    <input type="hidden" name="fileList" id="fileList" />
    <iframe id="ifrmAttachFile" frameborder="0" width="0" height="0" scrolling="no" src="about:blank"></iframe>
</body>
</html>
