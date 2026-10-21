<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApprovePopup.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Process.Confirm.ApprovePopup" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <meta name="SKYPE_TOOLBAR" content ="SKYPE_TOOLBAR_PARSER_COMPATIBLE"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body>
    <form id="form1" runat="server">
<div id="layout_body">
    <div class="layout_full">

        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%=GetResource("AppDoc")%></h1>
            <span class="app_caption">
                * <%=GetResource("AttachName")%>(<strong><span id="headerAttachCnt" action="attach">0</span><%=GetResource("EA")%></strong>) 
                / <%=GetResource("BaseDoc")%>(<strong><span id="headerBaseDocCnt" action="basedoc">0</span><%=GetResource("EA")%></strong>)
            </span>
            
            <!-- 버튼 //-->
            <ul class="btn_right">
                <cc3:ClientSmartButton ID="cbtnPrint" runat="server" BeginEndTag="li" />
                <cc3:ClientSmartButton ID="cbtnHistory" runat="server" BeginEndTag="li" />
                <cc3:ClientSmartButton ID="cbtnDocInfo" runat="server" BeginEndTag="li" />
                <cc3:ClientSmartButton ID="cbtnOpinionInfo" runat="server" BeginEndTag="li" />
                <cc3:ClientSmartButton ID="cbtnConfirm" runat="server" BeginEndTag="li" class="btn btn-secondary" />
                <li><a onclick="javascript:Approve.List()" class="btn"><span><%=GetResource("Close")%></span></a></li>                      
            </ul>
        </div>

        <div id="layout_content">
            <div class="row">
                <div class="col-12 col-lg-9 col-xl-8 content-area-body-left"><!-- width 100%(col-12) -->
                    <!-- 양식로드 //-->
                    <iframe id="ifrmForm" name="ifrmForm" src="about:blank" frameborder="0" runat="server" class="app_iframe"></iframe>
                </div><!-- end col-3 content-area-body-right -->
                <div class="col-12 col-lg-3 col-xl-4 content-area-body-right"><!-- width 100%(col-12) -->                        
                    <ul>
                       	<!-- 첨부파일 //-->
						<li id="download" class="download" style="min-height: 47px"> 
                            <ul id="solbatch" >
                                <li id="zipline1"><cc3:ClientSmartButton ID="cbtnZIP" runat="server"  class="btn btn-sm bg-white" /></li>
                                <li id="zipline2"  class="sec btn btn-sm bg-white" ></li>
                                <cc3:ClientSmartButton ID="cbtnShowImage" runat="server" BeginEndTag="li"  class="btn btn-sm bg-white" />
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
                        <li><div id="placeholder_baseDoc" style="clear: both;"></div></li>
                    </ul>
                </div><!-- end col-3 content-area-body-right -->
            </div><!-- end row  -->
        </div><!-- end layout_content  -->


    </div>
</div>

<div style="display:none;">

    <iframe id="ifrmProcessing" name="ifrmProcessing" width="100" height="100" runat="server"  src="about:blank"></iframe>
    <asp:LinkButton ID="lbtnSubmit" runat="server" OnClick="lbtnSubmit_Click"></asp:LinkButton>
    
    <textarea id="htaFormHTML" style="DISPLAY: none" name="htaFormHTML" rows="2" cols="40" runat="server"></textarea>
    <textarea id="htaFormTEXT" style="DISPLAY: none" name="htaFormTEXT" rows="2" cols="40" runat="server"></textarea>
    <asp:textbox id="tbOpinion" Runat="server" Width="0"></asp:textbox>
    <asp:textbox id="tbSealPath" Runat="server" Width="0"></asp:textbox>    <!-- 결재타입에 따른 인장경로값 //-->
    <asp:textbox id="tbTargetInfo" Runat="server" Width="0"></asp:textbox>    <!-- 반려 시, 반려대상정보 //-->

    <asp:textbox id="tbEtc1" Runat="server" Width="0"></asp:textbox>
    <asp:textbox id="tbEtc2" Runat="server" Width="0"></asp:textbox>
    <asp:textbox id="tbEtc3" Runat="server" Width="0"></asp:textbox>
    <asp:textbox id="tbEtc4" Runat="server" Width="0"></asp:textbox>
    <asp:textbox id="tbEtc5" Runat="server" Width="0"></asp:textbox>
    <asp:textbox id="tbEtc6" Runat="server" Width="0"></asp:textbox>
    <asp:textbox id="tbEtc7" Runat="server" Width="0"></asp:textbox>
    <asp:textbox id="tbEtc8" Runat="server" Width="0"></asp:textbox>
    <asp:textbox id="tbEtc9" Runat="server" Width="0"></asp:textbox>
    <asp:textbox id="tbEtc10" Runat="server" Width="0"></asp:textbox>

</div>

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.modaldialog.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var ChildDocID = "<%=ChildDocID %>";
        var CompanyID = "<%=CompanyID %>";
        var CompanyName = "<%=CompanyName %>";
        var UserID = "<%=UserID %>";
        var LoginID = "<%=LoginID %>";
        var UserName = "<%=UserName %>";
        var DeptID = "<%=DeptID %>";
        var DeptName = "<%=DeptName %>";
        var TitleID = "<%=TitleID %>";
        var TitleName = "<%=TitleName %>";
        var DutyID = "<%=DutyID %>";
        var DutyName = "<%=DutyName %>";
        var EmployeeNumber = "<%=EmployeeNumber %>";
        var UserMail = "<%=UserMail %>";
        var MobilePhone = "<%=MobilePhone %>";

        var EventArgs = "<%=EventArgs %>";
        var ApprovalType = "";
        var SealPath = "<%=SealPath%>";
        var FormID = "<%=FormID%>";

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang %>";
        var PreLangReal = "<%=PreLangReal %>"; //ja

        var FullDomainPath = "<%=FullDomainPath %>";
        var DocumentID = "<%=DocumentID %>";        //문서ID
        var GroupID = "<%=GroupID %>";              //원본(발신)문서ID
        var Sequence = "<%=Sequence %>";            //결재순서
        var ApprovalTag = "<%=ApprovalTag %>";      //결재태그
        var AccountTag = "<%=AccountTag %>";        //결재그룹
        var ApprovalState = "<%=ApprovalState %>";

        var RecordPerPage = "<%=RecordPerPage %>";
        var CurrentPage = "<%=CurrentPage %>";
        var SearchTerm = "<%=SearchTerm %>";
        var SearchTermValue = "<%=SearchTermValue %>";
        var SearchField = "<%=SearchField %>";
        var SearchValue = "<%=SearchValue %>";
        var Order = "<%=Order %>";
        var FolderStatusID = "<%=FolderStatusID %>";
        var ListID = "<%=ListID %>";
        var TargetPage = "<%=TargetPage %>";

        var tagDic = null, nowEl = null;
        var ifrmObj = null;                     // ifrmForm object
        var LegacyKey = ""; //더미
        var ParamEtc = "";  //더미
        var IsApproving = false;        //결재진행중여부
    </script>

    <script type="text/javascript">
        //리소스
        var TagDic_INFO0 = "<%=GetResource("TagDic_INFO0") %>";
        var TagDic_INFO1 = "<%=GetResource("TagDic_INFO1") %>";
        var TagDic_INFO2 = "<%=GetResource("TagDic_INFO2") %>";
        var TagDic_INFO3 = "<%=GetResource("TagDic_INFO3") %>";
        var TagDic_INFO4 = "<%=GetResource("TagDic_INFO4") %>";
        var TagDic_INFO5 = "<%=GetResource("TagDic_INFO5") %>";
        var TagDic_INFO6 = "<%=GetResource("TagDic_INFO6") %>";
        var TagDic_INFO7 = "<%=GetResource("TagDic_INFO7") %>";
        var TagDic_INFO8 = "<%=GetResource("TagDic_INFO8") %>";
        var TagDic_INFO9 = "<%=GetResource("TagDic_INFO9") %>";
        var TagDic_INFO10 = "<%=GetResource("TagDic_INFO10") %>";
        var TagDic_INFO11 = "<%=GetResource("TagDic_INFO11") %>";
        var TagDic_INFO12 = "<%=GetResource("TagDic_INFO12") %>";
        var TagDic_INFO13 = "<%=GetResource("TagDic_INFO13") %>";
        var TagDic_INFO14 = "<%=GetResource("TagDic_INFO14") %>";
        var TagDic_INFO15 = "<%=GetResource("TagDic_INFO15") %>";
        var TagDic_ZD0 = "<%=GetResource("TagDic_ZD0") %>";
        var TagDic_ZD = "<%=GetResource("TagDic_ZD") %>";
        var TagDic_ZP0 = "<%=GetResource("TagDic_ZP0") %>";
        var TagDic_ZP = "<%=GetResource("TagDic_ZP") %>";
        var TagDic_ZH = "<%=GetResource("TagDic_ZH") %>";
        var TagDic_ZC = "<%=GetResource("TagDic_ZC") %>";
        var TagDic_ZR = "<%=GetResource("TagDic_ZR") %>";
        var TagDic_ZA = "<%=GetResource("TagDic_ZA") %>";
        var TagDic_NO = "<%=GetResource("TagDic_NO") %>";
        var TagDic_DATE = "<%=GetResource("TagDic_DATE") %>";
        var TagDic_LIFE = "<%=GetResource("TagDic_LIFE") %>";
        var TagDic_SUBJ = "<%=GetResource("TagDic_SUBJ") %>";
        var TagDic_BODY = "<%=GetResource("TagDic_BODY") %>";
        var TagDic_BODY_etc = "<%=GetResource("TagDic_BODY_etc") %>";
        var TagDic_EDITOR = "<%=GetResource("TagDic_EDITOR") %>";
        var TagDic_COMPANY = "<%=GetResource("TagDic_COMPANY") %>";
        var TagDic_UPNM = "<%=GetResource("TagDic_UPNM") %>";
        var TagDic_EMPNUM = "<%=GetResource("TagDic_EMPNUM") %>";
        var TagDic_UPDPT = "<%=GetResource("TagDic_UPDPT") %>";
        var TagDic_UPJC = "<%=GetResource("TagDic_UPJC") %>";
        var TagDic_OPINION = "<%=GetResource("TagDic_OPINION") %>";
        var TagDic_OPINIONVIEW = "<%=GetResource("TagDic_OPINIONVIEW") %>";
        var TagDic_FLOWZD = "<%=GetResource("TagDic_FLOWZD") %>";
        var TagDic_FLOWZH = "<%=GetResource("TagDic_FLOWZH") %>";
        var TagDic_FLOWZA = "<%=GetResource("TagDic_FLOWZA") %>";
        var TagDic_FLOWZR = "<%=GetResource("TagDic_FLOWZR") %>";
        var TagDic_ENFORCEDEPTNAME = "<%=GetResource("TagDic_ENFORCEDEPTNAME") %>";
        var TagDic_ENFORCENAME = "<%=GetResource("TagDic_ENFORCENAME") %>";
        var TagDic_ENFORCERECEIVECARBON = "<%=GetResource("TagDic_ENFORCERECEIVECARBON") %>";
        var Msg_ErrorUserInfo = "<%=GetResource("Msg_ErrorUserInfo") %>";
        var Msg_ErrorUserNon = "<%=GetResource("Msg_ErrorUserNon") %>";
        var Msg_ErrorUserBefore = "<%=GetResource("Msg_ErrorUserBefore") %>";
        var Tit_Cancel = "<%=GetResource("Tit_Cancel") %>";
        var Tit_Life1 = "<%=GetResource("Tit_Life1") %>";
        var Tit_Life2 = "<%=GetResource("Tit_Life2") %>";
        var Tit_Life3 = "<%=GetResource("Tit_Life3") %>";
        var Tit_FlowAdd = "<%=GetResource("Tit_FlowAdd") %>";
        var Tit_FlowDel = "<%=GetResource("Tit_FlowDel") %>";
        var Msg_SetLine = "<%=GetResource("Msg_SetLine") %>";
        var Tit_File = "<%=GetResource("Tit_File") %>";
        var ConfirmTit = "<%=GetResource("ConfirmTit") %>";

        var Msg_ErrSelForm= "<%=GetResource("Msg_ErrSelForm") %>";
        var Msg_Del= "<%=GetResource("Msg_Del") %>";
        var DocInfo= "<%=GetResource("DocInfo") %>";
        var DocView= "<%=GetResource("DocView") %>";
        var NoContent= "<%=GetResource("NoContent") %>";
        var SelectAll= "<%=GetResource("SelectAll") %>";

        //
        var R_PROXY = "<%=GetResource("ProxyS") %>";
        var R_FileName = "<%=GetResource("FileName") %>";
        var R_FileType = "<%=GetResource("FileType") %>";
        var R_FileLen = "<%=GetResource("FileLen") %>";
        var R_FileDown = "<%=GetResource("FileDownload") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_MSG_ERRSEAL = "<%=GetResource("AddSign") %>";
        var R_MSG_CancelDelay = "<%=GetResource("MSG_CancelDelay") %>";
        var R_MSG_Delay = "<%=GetResource("MSG_Delay") %>";
        var R_MSG_Processing = "<%=GetResource("MSG_Processing") %>";
        var R_TitSub  = "<%=GetResource("TitSub") %>";
        var R_MSG_CancelApprove = "<%=GetResource("MSG_CancelApprove") %>";
        var R_Msg_LaterOnSelItem = "<%=GetResource("Msg_LaterOnSelItem") %>";
        var R_ApprovalTypeL = "<%=GetResource("ApprovalTypeL") %>";
        var R_Msg_AlertUserInfo= "<%=GetResource("Msg_AlertUserInfo") %>";
        var R_Msg_AlarmError = "<%=GetResource("Msg_AlarmError")%>";
        var R_Msg_AlarmErrorView = "<%=GetResource("Msg_AlarmErrorView")%>";
        var R_MsgCheckClick = "<%=GetResource("MsgCheckClick") %>";

        var R_NoFileData = "<%=GetResource("NoFileData")%>";
        var R_ErrorAttachFile = "<%=GetResource("ErrorAttachFile")%>";
        var R_ErrorBaseDoc = "<%=GetResource("ErrorBaseDoc")%>";
        var R_AllDownLoad = "<%=GetResource("AllDownLoad")%>";
        var R_MSG_SelectDownLoad = "<%=GetResource("MSG_SelectDownLoad")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Process/Confirm/ApprovePopup.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/FormFlow/Common/Common.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Process/Confirm/ApprovePopup.js");
        //JsCall("/DeskPlusEIP/Approval/Js/FormFlow/Common/Common.js");
    </script>

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
    <form name="frmTotalDownLoad" id="frmTotalDownLoad" method="post" target="openWin">
    <input type="hidden" name="fileList" id="fileList" />
    <iframe id="ifrmAttachFile" frameborder="0" scrolling="no" src="about:blank"></iframe>
    </form>
</body>
</html>
