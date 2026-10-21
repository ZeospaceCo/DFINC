<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.FormFlow.Receive.Add" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <meta name="SKYPE_TOOLBAR" content ="SKYPE_TOOLBAR_PARSER_COMPATIBLE"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
</head>
<body>
    <form id="form1" runat="server">
<div id="layout_body" class="view">
    <div class="layout_full">
        <!-- Loading 처리 //-->
        <div id="divLoading" class="divLoading"></div>
        <div id="layout_header">
            <h1><%=GetResource("ReceiveDoc")%></h1>
            <!-- 페이지타이틀 //-->
            <ul class="btn_right" id="ulSubmit">
                <li><cc3:ClientSmartButton ID="cbtnFlowAdd" runat="server"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="cbtnFlowDel" runat="server"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="cbtnSubmit" runat="server" class="btn btn-secondary"></cc3:ClientSmartButton></li>
                <li><a onclick="Add.List()" class="btn"><%=GetResource("List")%></a></li>  
            </ul>
            <ul class="btn_right" id="ulZQ">
                <cc3:ClientSmartButton ID="cbtnEdit" runat="server" BeginEndTag="li" />
                <li><cc3:ClientSmartButton ID="cbtnReject" runat="server" class="btn btn-secondary"></cc3:ClientSmartButton></li>
            </ul>
            <ul class="btn_right" id="ulZP">
                <li><cc3:ClientSmartButton ID="cbtnComplete" runat="server" class="btn btn-secondary"></cc3:ClientSmartButton></li>
            </ul>
            <ul class="btn_right" id="ulCmn">
                <li><cc3:ClientSmartButton ID="cbtnModify" runat="server"></cc3:ClientSmartButton></li>            
            </ul>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnPreview" runat="server"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="cbtnPrivateNet" runat="server"></cc3:ClientSmartButton></li>
                <li id="liEDMS" runat="server" visible="false"><cc3:ClientSmartButton ID="cbtnEDM" runat="server"></cc3:ClientSmartButton></li>
                <li style="display:none"><cc3:ClientSmartButton ID="cbtnModSeq" runat="server" ></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="cbtnOpinionInfo" runat="server" /></li>
            </ul>
            <ul class="btn_right" id="ulSet" style="display:none">
                <li><cc3:ClientSmartButton ID="cbtnSetLine" runat="server" ></cc3:ClientSmartButton></li>
            </ul>
        </div>

        <div id="layout_content">
             <div class="row">
                <div class="col-12 col-lg-9 col-xl-8 content-area-body-left" ><!-- width 100%(col-9) -->
                    <!-- 양식로드 //-->
                    <iframe id="ifrmForm" name="ifrmForm" src="about:blank" frameborder="0" runat="server" class="app_iframe"></iframe>
                </div><!-- end col-9 content-area-body -->

                <div class="col-12 col-lg-3 col-xl-4 content-area-body-right"  ><!-- width 100%(col-3) -->
                    <ul>
                        <li class="title">
                            <!-- 파일리스트 시작 //-->
                            <%=GetResource("AttachName") %>
                        </li>
                        <li>
                            <div id="phFileList" style="clear:both;"></div>
                        </li>
                        <!-- 파일리스트 끝 //-->

                        <li class="title">
                            <!-- 첨부파일 //-->
                            <%=GetResource("AddAttachFile")%>
                            <div class="float-right">
							    <a id="cbtnFileAdd" title="<%=GetResource("FileAdd")%>" ><i class="fas fa-plus-circle text-secondary "></i></a>
                            </div>
                        </li>
                        <li>
                            <div id="phFile" style="clear:both;"></div>
                        </li>

                        <!-- 근거문서 리스트 시작 //-->
                        <li class="title">
                            <%=GetResource("BaseDoc")%>
                        </li>
                        <li>
                            <div id="phBaseList" style="clear:both;"></div>
                        </li>

                        <!-- 근거문서 //-->
                        <li class="title">
                            <%=GetResource("AddBaseDoc2")%>
                            <div class="float-right">
                                <a href="#" id="cbtnDocAdd" title="<%=GetResource("BaseDocAdd")%>" ><i class="fas fa-plus-circle text-secondary "></i></a>
                            </div>
                        </li>
                        <li >
                        	<ul class="itemlist" id="contentTable">
                                <li id="noContent"></li>
                            </ul>
                        </li>
                    </ul>
                </div><!-- end col-3 content-area-body -->

            </div><!-- end row  -->
        </div><!-- end layout_content  -->

    </div><!-- end layout_full  -->
</div><!-- end layout_body  -->

<div style="display:none;">
    <textarea id="txtDebug" style="DISPLAY:" name="txtDebug" rows="2" cols="40"></textarea>    
    <iframe id="ifrmParent" name="ifrmParent" src="about:blank" frameborder="0"></iframe>    
    <iframe id="ifrmProcessing" name="ifrmProcessing" width="100" height="100" src="about:blank"></iframe>
    <asp:LinkButton ID="lbtnSubmit" runat="server" OnClick="lbtnSubmit_Click"></asp:LinkButton>
    
    <textarea id="htaFormHTML" style="DISPLAY: none" name="htaFormHTML" rows="2" cols="40" runat="server"></textarea>
    <textarea id="htaFormTEXT" style="DISPLAY: none" name="htaFormTEXT" rows="2" cols="40" runat="server"></textarea>
    <textarea id="htaFormNet" style="DISPLAY: none" name="htaFormNet" rows="2" cols="40" runat="server"></textarea>
    <textarea id="htaEditorHTML" style="DISPLAY: none" name="htaEditorHTML" rows="2" cols="40" runat="server"></textarea>
    <asp:TextBox ID="htaEditorMime" Style="display: none"   Width="0" TextMode="MultiLine" runat="server"></asp:TextBox>

    <textarea id="htaXmlSeq" style="DISPLAY: none" name="htaXmlSeq" rows="2" cols="40" runat="server"></textarea>
    <textarea id="htaXmlSeqZQ" style="DISPLAY: none" name="htaXmlSeqZQ" rows="2" cols="40" runat="server"></textarea>
    <textarea id="htaXmlSeqZC" style="DISPLAY: none" name="htaXmlSeqZC" rows="2" cols="40" runat="server"></textarea>

    <asp:textbox id="tbIsGuard" Runat="server" Width="0"></asp:textbox>
    <asp:textbox id="tbAccessTitleID" Runat="server" Width="0"></asp:textbox>
    <asp:textbox id="tbOpinion" Runat="server" Width="0"></asp:textbox>
    <asp:textbox id="tbLifeTime" Runat="server" Width="0"></asp:textbox>
    <asp:textbox id="tbSubject" Runat="server" Width="0"></asp:textbox>
    <asp:textbox id="tbIsLinkedEtc" Runat="server" Width="0"></asp:textbox>
    <asp:textbox id="tbIsUseFlow" Runat="server" Width="0"></asp:textbox>
    <asp:textbox id="tbCarbonView" Runat="server" Width="0"></asp:textbox>

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

    <asp:TextBox ID="tbBaseDocIDs" runat="server"></asp:TextBox>

    <asp:textbox id="tbTargetInfo" Runat="server" Width="0"></asp:textbox>    <!-- 반려 시, 반려대상정보 //-->
    <asp:textbox id="tbAllAlarmInfo" Runat="server" Width="0"></asp:textbox>    <!-- 반려 시, 반려알람대상자정보 //-->
    <asp:textbox id="tbAddAlarmInfo" Runat="server" Width="0"></asp:textbox>    <!-- 반려 시, 추가 반려알람대상자정보 //-->
    <input name="tbAddDocID" type="text" id="tbAddDocID" value=""/>   <!-- For. 개발용도: 근거문서 입력용도 //-->

    <!--  파일 첨부시 필요한 필드 //-->
    <asp:TextBox ID="tbHiddenSpoolPath" runat="server" Width="0px"></asp:TextBox>
    <asp:TextBox ID="tbHiddenFileList" runat="server" Width="0px"></asp:TextBox>
    <asp:TextBox ID="tbHiddenSaveFileList" runat="server" Width="0px"></asp:TextBox>
    <iframe id="ifrmDelFile" style="display: none" name="ifrmDelFile" src="about:blank" frameborder="0"></iframe>
</div>

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type='text/javascript' src='/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js'></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.hashtable.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal2.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>

    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var FullUrl = "<%=FullDomainPath%>";
        var CompanyID = "<%=CompanyID %>";
        var CompanyName = "<%=CompanyName %>";
        var RootDeptID = "<%=RootDeptID %>";
        var RootDeptName = "<%=RootDeptName %>";
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
        var CustomFilePath = "<%=CustomFilePath %>";
        var ApprovalStatusMenuType = "<%=ApprovalStatusMenuType %>";
        var CustomSection = "ADDRECEIVE";
        var ApprovalType = "";
        var BaseDocIDs = "<%=BaseDocIDs %>";    //근거문서들 추가순서대로의 documentID(|docID^docNumber;|docID^docNumber;)
        var BaseDocCnt = 0;                     //근거문서 추가 갯수
        var PWD = "<%=CurrentUserInfo.Password.ToString() %>";
        var ProxyUserID = "<%=ProxyUserID %>";

        var SealPath = "<%=SealPath%>";
        var FormID = "<%=FormID%>";
        //var IsLegacy = "<%=IsLegacy%>";       // 양식로딩 시, 연동모듈 event호출하지 않기위해 강제 처리.
        var IsLegacy = "0";
        var NonActiveX = "<%=NonActiveX %>";    // activeX(F) , nonActiveX(T)

        var ParentDocumentID = "<%=ParentDocumentID %>";
        var AccountTag = "<%=AccountTag %>";
        var Sequence = "<%=Sequence %>";
        var GroupID = "<%=GroupID %>";
        var ApprovalTag = "ZD0";
        var ZACount = 0;  // 이전 결재에 ZA 존재여부 체크

        var RecordPerPage = "<%=RecordPerPage %>";
        var CurrentPage = "<%=CurrentPage %>";
        var SearchTerm = "<%=SearchTerm %>";
        var SearchTermValue = "<%=SearchTermValue %>";
        var SearchField = "<%=SearchField %>";
        var SearchValue = "<%=SearchValue %>";
        var Order = "<%=Order %>";
        var FolderStatusID = "<%=FolderStatusID %>";
        var TargetPage = "<%=TargetPage %>";
        var TargetUserID = "<%=TargetUserID %>";    //원결재자 ID
        var TargetDeptID = "<%=TargetDeptID %>";    //원결재자의 부서ID
        var TargetDeptName = "<%=TargetDeptName %>";    //원결재자의 부서명
        var ChangeDeptID = "<%=ChangeDeptID %>";    //원결재자의 변경부서ID

        var tagDic = new jQuery.Hashtable();
        var favDic = new jQuery.Hashtable();        // 자주지정하는 결재자정보
        var formSeqDic = new jQuery.Hashtable();   // 결재라인정보
        var appSeqDic = new jQuery.Hashtable();     // For, 결재자지정정보 (발신-ZD.ZH.ZA)
        var appSeqDicZR = new jQuery.Hashtable();     // For, 결재자지정정보 (ZR)
        var appSeqDicZQ = new jQuery.Hashtable();     // For, 결재자지정정보 (수신-ZQ.ZP)
        var appSeqDicZC = new jQuery.Hashtable();     // For, 결재자지정정보 (참조-ZC)

        var appSeqKeyPush = new Array();           // 선택한 순서로 결재진행 표시 ByFreeman 20131023

        var appSeqKey = new Array(); //appSeqArr
        var appSeqKeyHistory = new Array();         // 결재순서변경 시, 변경 전후 Key.   변경전Key(Value):변경후Key
        var appSeqKeyHistoryCnt = 0;

        var ifrmObj = null;                     // ifrmForm object
        var ifrmParentObj = null;               // ifrmParentForm object (이전문서)
        var nowEl = null, preEl = null;         // ifrmForm 현재 객체 & 이전 객체
        var clientX = null, clientY = null;

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang %>";
        var PreLangReal = "<%=PreLangReal %>"; //ja     

        var SwfUploadGUID = "<%=SwfUploadGUID %>";  //flash upload GUID
        var FileType = "*.*";
        var FileTypeDesc = "All Files (*.*)";
        var LegacyKey = ""; //더미
        var ParamEtc = "";  //더미
        var chkOneClick = false;    // 이중클릭 체크( btn : 결재라인지정)
        var IsMyTurnState = false; //현재 결재순서 여부 확인
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
        var TagDic_Print = "<%=GetResource("TagDic_Print") %>";

        var Msg_ErrorUserInfo = "<%=GetResource("Msg_ErrorUserInfo") %>";
        var Msg_ErrorUserNon = "<%=GetResource("Msg_ErrorUserNon") %>";
        var Msg_ErrorUserBefore = "<%=GetResource("Msg_ErrorUserBefore") %>";
        var Tit_Cancel = "<%=GetResource("Tit_Cancel") %>";
        var Tit_Life1 = "<%=GetResource("Tit_Life1") %>";
        var Tit_Life2 = "<%=GetResource("Tit_Life2") %>";
        var Tit_Life3 = "<%=GetResource("Tit_Life3") %>";
        var Tit_Life4 = "<%=GetResource("Tit_Life4") %>";
        var Tit_Life5 = "<%=GetResource("Tit_Life5") %>";
        var Tit_LifeYear = "<%=GetResource("Tit_Life") %>";

        var Tit_FlowAdd = "<%=GetResource("Tit_FlowAdd") %>";
        var Tit_FlowDel = "<%=GetResource("Tit_FlowDel") %>";
        var Msg_SetLine = "<%=GetResource("Msg_SetLine") %>";
        var Tit_File = "<%=GetResource("Tit_File") %>";

        var Msg_ErrSelForm= "<%=GetResource("Msg_ErrSelForm") %>";
        var Msg_Del= "<%=GetResource("Msg_Del") %>";
        var DocInfo= "<%=GetResource("DocInfo") %>";
        var DocNumber= "<%=GetResource("DocNumber") %>";
        var Subject= "<%=GetResource("Subject") %>";
        var DocView= "<%=GetResource("DocView") %>";
        var DocSubject= "<%=GetResource("Subject") %>";
        var NoContent= "<%=GetResource("NoContent") %>";
        var Msg_FlowErrorSet = "<%=GetResource("Msg_FlowErrorSet") %>";
        var R_MSG_SelectDownLoad = "<%=GetResource("MSG_SelectDownLoad")%>";

        //
        var R_LIFE = "<%=GetResource("R_LIFE") %>";
        var R_MSG_ERRSEAL = "<%=GetResource("AddSign") %>";
        var R_MSG_FAIL = "<%=GetResource("MSG_FAIL") %>";
        var R_FileName  = "<%=GetResource("FileName") %>";
        var R_FileType  = "<%=GetResource("FileType") %>";
        var R_FileLen   = "<%=GetResource("FileLen") %>";
        var R_FileDown = "<%=GetResource("FileDownload") %>";
        var R_FileDel   = "<%=GetResource("Del") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_Msg_ErrSelNet = "<%=GetResource("Msg_ErrSelNet") %>";
        var R_Msg_ErrSubj = "<%=GetResource("Msg_ErrSubj") %>";
        var R_Msg_ErrLife = "<%=GetResource("Msg_ErrLife") %>";
        var R_Msg_ErrOpinion = "<%=GetResource("Msg_ErrOpinion") %>";
        var R_Msg_ErrOpinionNon = "<%=GetResource("Msg_ErrOpinionNon") %>";
        var R_Msg_ErrSetZA = "<%=GetResource("Msg_ErrSetZA") %>";
        var R_Msg_ErrSetZH = "<%=GetResource("Msg_ErrSetZH") %>";
        var R_Msg_ErrNetNon = "<%=GetResource("Msg_ErrNetNon") %>";
        var R_Msg_ErrRejectTarget= "<%=GetResource("Msg_ErrRejectTarget") %>";
        var R_Msg_ErrNetNonReplace= "<%=GetResource("Msg_ErrNetNonReplace") %>";
        var R_Msg_ErrSelUser= "<%=GetResource("Msg_ErrSelUser") %>";
        var R_Msg_Change= "<%=GetResource("Msg_Change") %>";
        var R_Msg_ErrNetNonCom= "<%=GetResource("Msg_ErrNetNonCom") %>";
        var R_Msg_ErrComplete = "<%=GetResource("Msg_ErrComplete") %>";
        var R_TitSub  = "<%=GetResource("TitSub") %>";
        var R_Msg_AlertUserInfo= "<%=GetResource("Msg_AlertUserInfo") %>";
        var R_MsgCheckClick = "<%=GetResource("MsgCheckClick") %>";
        var R_MsgAlertScript = "<%=GetResource("MsgAlertScript") %>";
        var R_ConfirmAdd = "<%=GetResource("ConfirmAdd") %>";
        var R_ModifyList  = "<%=GetResource("ModifyList") %>";
        var R_PROXY = "<%=GetResource("ProxyS") %>";
        var R_MyTurnSusin = "<%=GetResource("MyTurnSusin") %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/FormFlow/Common/Common.js?v1=2"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/FormFlow/Common/CommonFlow.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/FormFlow/Receive/Add.js?v1=4"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/FormFlow/Common/Common.js");
        //JsCall("/DeskPlusEIP/Approval/Js/FormFlow/Common/CommonFlow.js");
        //JsCall("/DeskPlusEIP/Approval/Js/FormFlow/Receive/Add.js");
    </script>
    <div id="zeoEditorDIV" style="display:none; width:300px; height:150px; top:220px; left:120px; position:absolute; z-index:2;">
        <iframe src="about:blank" id="zeoEditorIfrm" frameborder="0" scrolling="no" width="300px" height="150px"></iframe>
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
    <form name="frmTotalDownLoad" id="frmTotalDownLoad" method="post" target="openWin">
    <input type="hidden" name="fileList" id="fileList" />
    <iframe id="ifrmAttachFile" frameborder="0" scrolling="no" src="about:blank" ></iframe>
    </form>
</body>
</html>
