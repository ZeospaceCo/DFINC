<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DraftAdd.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Approval.DraftAdd" EnableViewState="true" %>
<%@ Register Src="../Portal/Menu.ascx" TagName="Menu" TagPrefix="uc1" %>
<%@ Register Src="../Portal/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>
<!DOCTYPE html>
<html lang="ko">
<head>
    <title></title>
    <meta charset="uft8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=3.0, minimum-scale=1.0, user-scalable=yes, target-densitydpi=medium-dpi" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/DStyle/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/DStyle/all.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/Base/Css/<%=SkinType%>/navimobile.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Html/Js/JModal/DeskPlusModal.css"/>
</head>
<body class="mobile navihide scrolldown">
    <iframe name="hid_func" style="display: none"></iframe>
    <form id="form1" name="form1" runat="server" >
    <input type="text" name="companyID" value="<%=CompanyID%>" style="display: none" />
    <input type="text" name="userID" value="<%=UserID%>" style="display: none" />
    <input type="text" name="swfUploadGUID" value="<%=SwfUploadGUID%>" style="display: none" />
    <div id="divprg" align="center" class="loading">
    </div>
    <div data-role="page" id="ApprovalApprove">
        <uc1:Menu ID="Menu1" runat="server" />
        <div class="bodycontainer">
            <div class="introcontentblock bottomcontent">
                <ul class="mobilecontent">
                    <li class="title text-primary">
                        <span id="listTitle">
                            <i class="fas fa-file-signature  dropbtn "></i><%=GetResource("WriteDoc")%>
                        </span>
                        <div class="float-right date text-dark"><%=Today%>&nbsp;&nbsp;</div>
                    </li>
                    <li class="contentblock msgNotice" id="msgNotice" style="display:none;" onclick="NoticeHide();">
                    </li>
                    <li class="contentblock">
                        <div class="container-fluid buttongroup" id="dButton">
                            <div class="float-left">
                                <button type="button" id="btnDraft" class="btn btn-lg btn-success">
                                    <span class="btnb"><%=GetResource("PrivateDraft")%></span>
                                </button>
                                <button type="button" id="btnBaseDoc" class="btn btn-lg btn-primary">
                                    <span class="btnb"><%=GetResource("BaseDoc")%></span>
                                </button>
                                <button type="button" id="btAddFile" class="btn btn-lg btn-primary">
                                    <span class="btnb"><%=GetResource("SelectFile")%></span>
                                </button>
                                <input type="file" name="upload" id="upload" onchange="fileChange(this);" style="display:none;" />
                                <button type="button" id="btEDM" class="btn btn-lg btn-primary" runat="server">
                                    <span class="btnb">EDM</span>
                                </button>
                                <button type="button" id="btnAdd" class="btn btn-lg btn-danger">
                                    <span class="btnb"><%=GetResource("DocAdd")%></span>
                                </button>                               
                            </div>
                        </div>
                    </li>
                    <li class="contentblock">
                        <div class="title"><%=GetResource("SelApprover")%></div>    
                        <div class="container-fluid">   
                            <div class="float-left">
                                <div class="search-container float-right">
                                    <input type="text" id="tbSearch" class="keyword form-control" maxlength="100" runat="server" />
                                    <button type="button" class="btn btn-xs btn-primary" id="btnSearch" style="float:none;">
                                        <%=GetResource("Search")%>
                                    </button>
                                </div>
                            </div>                                
                        </div>
                        <div class="container-fluid">  
                                <div id="lbDeptUser" class="form-control" style="height:80px;width:100%;overflow-y:scroll"></div>    
                        </div> 
                    </li>
                    <li class="contentblock2 bg-white">
                        <div class="title" id="contentDoc">
                             <iframe id="ifrmForm" name="ifrmForm" src="about:blank" onload="autoResize(this)" ></iframe>
                        </div>                       
                    </li>                    
                    <li class="contentblock">
                        <div class="container-fluid">
                            <div class="wrTable" id="divAddtional" runat="server">
                                <div class="title" >
                                    <%=GetResource("DeptInfo")%>
                                </div>
                                <div class="area1">
                                    <span><asp:DropDownList ID="ddlAdditionDept" runat="server" CssClass="form-control"></asp:DropDownList></span>
                                </div>
                            </div>
                            <div class="wrTable">
                                <div class="title">
                                    <%=GetResource("Form3")%>
                                </div>
                                <div class="area1">
                                    <span><asp:DropDownList ID="ddlGuard" runat="server" CssClass="form-control"></asp:DropDownList></span>
                                </div>
                            </div>
                            <div class="wrTable" id="trCarbonView" runat="server">
                                <div class="title">
                                    <%=GetResource("CarbonView")%>
                                </div>
                                <div class="area1">
                                    <span><asp:DropDownList ID="ddlCarbonView" runat="server" CssClass="form-control"></asp:DropDownList></span>
                                </div>
                            </div>
                            <div class="wrTable">
                                <div class="title">
                                    <%=GetResource("ViewAuth")%>
                                </div>
                                <div class="area1">
                                    <span><asp:DropDownList ID="ddlAccessTitle" runat="server" CssClass="form-control"></asp:DropDownList></span>
                                </div>
                            </div>
                            <div style="height:10px;"></div>
                            <table style="font-size:11px;" class="table_typeE" id="gradeinfo">
                            <colgroup>
                                <col width="50px" />
                                <col />
                            </colgroup>
                            <tbody>
                            <%=AccessTitleGrade%>
                            </tbody>
                            </table>
                        </div>
                        <div class="title"><%=GetResource("Opinion")%>(<span class="charsRemaining"></span> <%=GetResource("InChar")%>)</div>
                        <div class="container-fluid">
                            <textarea id="memo" class="form-control ta4" maxlength="200" runat="server"></textarea>
                        </div>
                    </li> 
                </ul>
                <ul class="mobliecontent" id="beforeBase" style="display:none">
                 <li class="contentblock bg-title">
                        <div class="title"> * <%=GetResource("Existing")%>&nbsp;<%=GetResource("BaseDoc")%>&nbsp;(<%=GetResource("DelAlert2")%>)</div>
                 </li>
                </ul>                 
                <ul class="mobliecontent" id="beforebaseList"></ul>  

                <ul class="mobliecontent" id="uBase" style="display:none">
                 <li class="contentblock bg-title">
                        <div class="title"> * <%=GetResource("BaseDoc")%></div>
                 </li>
                </ul>                 
                <ul class="mobliecontent" id="baseList"></ul>  

                <ul class="mobliecontent" id="beforeFile" style="display:none">
                 <li class="contentblock bg-title">
                        <div class="title"> * <%=GetResource("Existing")%>&nbsp;<%=GetResource("Attach")%>&nbsp;(<%=GetResource("DelAlert")%>)</div>
                 </li>
                </ul>  
                <ul class="mobliecontent" id="beforefileList" ></ul>  
                               
                <ul class="mobliecontent" id="uFile" style="display:none">
                 <li class="contentblock bg-title">
                        <div class="title"> * <%=GetResource("Attach")%></div>
                 </li>
                </ul>  
                <ul class="mobliecontent" id="fileList" ></ul> 

                <ul class="mobilecontent">
                    <li class="contentblock">
                            <div class="container-fluid buttongroup">
                                <div class="float-left">
                                    <button type="button" id="btnDraftV" class="btn btn-lg btn-success">
                                        <span class="btnb"><%=GetResource("PrivateDraft")%></span>
                                    </button>
                                    <button type="button" id="btnBaseDocV" class="btn btn-lg btn-primary">
                                        <span class="btnb"><%=GetResource("BaseDoc")%></span>
                                    </button>
                                    <button type="button" id="btAddFileV" class="btn btn-lg btn-primary">
                                        <span class="btnb"><%=GetResource("SelectFile")%></span>
                                    </button>         
                                    <button type="button" id="btEDMV" class="btn btn-lg btn-primary" runat="server">
                                        <span class="btnb">EDM</span>
                                    </button>                                  
                                    <button type="button" id="btnAddV" class="btn btn-lg btn-danger">
                                        <span class="btnb"><%=GetResource("DocAdd")%></span>
                                    </button>
                                </div>
                            </div>
                        </li>
                  </ul>  
            </div>
        </div>
    </div>  
    <div style="display:none;">

        <iframe id="ifrmProcessing" name="ifrmProcessing" width="100" height="100" style="display: none" src="about:blank"></iframe>

        <asp:LinkButton ID="lbtnSubmit" runat="server" OnClick="lbtnSubmit_Click"></asp:LinkButton>

        <textarea id="htaFormHTML" style="DISPLAY: none" name="htaFormHTML" rows="2" cols="40" runat="server"></textarea>
        <textarea id="htaFormTEXT" style="DISPLAY: none" name="htaFormTEXT" rows="2" cols="40" runat="server"></textarea>
        <textarea id="htaEditorHTML" style="DISPLAY: none" name="htaEditorHTML" rows="2" cols="40" runat="server"></textarea>
        <asp:TextBox ID="htaEditorMime" Style="display: none"   Width="0" TextMode="MultiLine" runat="server"></asp:TextBox>

        <textarea id="htaXmlSeq" style="DISPLAY: none" name="htaXmlSeq" rows="2" cols="40" runat="server"></textarea>
        <textarea id="htaXmlSeqZQ" style="DISPLAY: none" name="htaXmlSeqZQ" rows="2" cols="40" runat="server"></textarea>
        <textarea id="htaXmlSeqZC" style="DISPLAY: none" name="htaXmlSeqZC" rows="2" cols="40" runat="server"></textarea>

        <asp:textbox id="tbDeptID" Runat="server" Width="0"></asp:textbox>
        <asp:textbox id="tbDeptName" Runat="server" Width="0"></asp:textbox>
        <asp:textbox id="tbLifeTime" Runat="server" Width="0"></asp:textbox>
        <asp:textbox id="tbSubject" Runat="server" Width="0"></asp:textbox>
        <asp:textbox id="tbIsLinkedEtc" Runat="server" Width="0"></asp:textbox>
        <asp:textbox id="tbIsUseFlow" Runat="server" Width="0"></asp:textbox>

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
        <input name="tbAddDocID" type="text" id="tbAddDocID" value=""/ size="160">   <!-- For. 개발용도: 근거문서 입력용도 //-->

    </div>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.form.min.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.hashtable.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/bootstrap.min.js"></script>       
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/navimobile.js"></script>
    <style>
        
    #contentDoc { width: 100%; padding: 0; position:relative; left:0px; top:0px; overflow: hidden;}
    #ifrmForm { width:700px; position:relative; left:0px; top:0px; border:0px }
    #ifrmForm {-moz-transform-origin: 0px 0; -o-transform-origin: 0 0; -webkit-transform-origin: 0 0; }

    </style>
    <script type="text/javascript">
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
        var Subject = "<%=Subject %>";
        var EventArgs = "<%=EventArgs %>";
        var LegacyKey = "";
        var ParamEtc = "";
        var CustomFilePath = "<%=CustomFilePath%>";
        var CustomSection = "ADD";
        var AccountTag = "ZD";
        var ApprovalType = "";
        // 이관 관련 속성
        var IsLinked = "<%=IsLinked%>";
        var IsLinkedType = "<%=IsLinkedType%>";

        var BaseDocIDs = ""; 
        var BaseDocCnt = 0;                   

        var DraftID = "<%=DraftID %>";
        var WriteTimeYMD = "<%=WriteTimeYMD %>";
        var SealPath = "<%=SealPath%>";        
        var FormID = "<%=FormID%>";
        var IsLegacy = "<%=IsLegacy%>";
        var ApprovalTag = "ZD0";
        var ZACount = 0;  // 이전 결재에 ZA 존재여부 체크

        var tagDic = new jQuery.Hashtable();
        var favDic = new jQuery.Hashtable();        // 자주지정하는 결재자정보
        var formSeqDic = new jQuery.Hashtable();   // 결재라인정보
        var appSeqDic = new jQuery.Hashtable();     // For, 결재자지정정보 (발신-ZD.ZH.ZA)
        var appSeqDicZR = new jQuery.Hashtable();     // For, 결재자지정정보 (ZR)
        var appSeqDicZQ = new jQuery.Hashtable();     // For, 결재자지정정보 (수신-ZQ.ZP)
        var appSeqDicZC = new jQuery.Hashtable();     // For, 결재자지정정보 (참조-ZC)

        var appSeqKeyPush = new Array();              // 선택한 순서로 결재진행 표시 ByFreeman 20131023

        var appSeqKey = new Array(); //appSeqArr
        var appSeqKeyHistory = new Array();         // 결재순서변경 시, 변경 전후 Key.   변경전Key(Value):변경후Key
        var appSeqKeyHistoryCnt = 0;

        var ifrmObj = null;                     // ifrmForm object
        var nowEl = null, preEl = null;         // ifrmForm 현재 객체 & 이전 객체
        var clientX = null, clientY = null;

        var dicZD = new jQuery.Hashtable();        // 발신결재정보
        var dicZH = new jQuery.Hashtable();        // 발신결재정보
        var dicZA = new jQuery.Hashtable();        // 발신결재정보
        var dicZP = new jQuery.Hashtable();        // 수신결재정보
        var dicZQ = new jQuery.Hashtable();        // 수신결재정보
        var dicZC = new jQuery.Hashtable();        // 참조결재정보
       
        var SwfUploadGUID = "<%=SwfUploadGUID %>"; 

        var ImagePath = "/DeskPlusMobile/Content/ko/Base/Img";
        var TmpSize = "";
        var BlockExt = "<%=blockExt %>"; 
        var UpFileSize = "<%=upFileSize %>"; 
        var FormActionPath = "";
        var PageNo = "<%=pageNo%>";
        var SearchValue = "<%=searchValue%>";
        var UPwd = "<%=uPwd%>";
        var DHeight = "45";

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

        var Msg_FlowErrorSet = "<%=GetResource("Msg_FlowErrorSet") %>";
        
        var R_PROXY = "<%=GetResource("ProxyS") %>";
        var R_Msg_ErrLife = "<%=GetResource("Msg_ErrLife") %>";
        var R_Msg_ErrOpinion = "<%=GetResource("Msg_ErrOpinion") %>";
        var R_TitSub  = "<%=GetResource("TitSub") %>";        
        var R_Msg_AlertEDMCheck= "<%=GetResource("Msg_AlertEDMCheck") %>";
        var R_ModfyAppLine = "<%=GetResource("ModfyAppLine") %>";
        var R_Msg_ConfirmAppLine = "<%=GetResource("Msg_ConfirmAppLine") %>";

        var R_Msg_ErrSelUnit = "<%=GetResource("Msg_ErrSelUnit") %>"; 
        var R_Msg_ErrReCancel = "<%=GetResource("Msg_ErrReCancel") %>"; 
        var R_MSG_SubDept = "<%=GetResource("MSG_SubDept") %>";   
        var R_DeptSet = "<%=GetResource("DeptSet") %>";       

        var R_ReLoadDoc = "<%=GetResource("ReLoadDoc") %>";
        var R_Menu = "<%=GetResource("Menu")%>";
        var R_SignNo = "<%=GetResource("SignNo")%>";
        var R_Dept = "<%=GetResource("Dept")%>";
        var R_ProcessingError = "<%= GetResource("ProcessingError") %>";
        var R_MaxFile = "<%=GetResource("MaxFile") %>";   
        var R_CheckFileType = "<%=GetResource("CheckFileType") %>";
        var R_ProcessingWait = "<%=GetResource("ProcessingWait") %>";
        var R_InputSubject = "<%=GetResource("InputSubject") %>";
        var R_Msg_SeqErrZA = "<%=GetResource("Msg_SeqErrZA") %>";
        var R_Msg_SeqErrZH = "<%=GetResource("Msg_SeqErrZH") %>";
        var R_DelList = "<%=GetResource("DelList") %>";

    </script>
    <script>

        history.pushState(null, null, location.href);

        window.onpopstate = function (event) {

        }

	</script>
    <script type="text/javascript" src="/DeskPlusMobile/Js/Approval/Common.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Js/Approval/CommonFlow.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Js/Approval/DraftAdd.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusMobile/Js/Approval/Mod.js");
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
</body>
</html>
