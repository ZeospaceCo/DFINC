Approve = {}

$(function () {

    $(document).ready(function () {

        // [인쇄]
        //$('#cbtnPrint').click(function (e) { e.preventDefault(); Approve.printElem({ leaveOpen: false, printMode: 'popup' }); });
        $('#cbtnPrint').click(function (e) {
            e.preventDefault();

            ParamArray = { 'childDocID': ChildDocID };
            CallEncrypt(CallGetParam());
            var url = "/DeskPlusEIP/Approval/Common/Popup/Print.aspx?p=" + GetEncryptValue;

            if ($.browser.msie) {
                WindowObjectReference = PopupModalWinOpen(url, "800", "640")
            } else {
                PopupWin("800", "640", url, "", "", true);
            }
        });

        // [목록]
        $('#cbtnCancel').click(function (e) { e.preventDefault(); Approve.List(); });
        // [이미지 보기] - 첨부파일
        $('#cbtnShowImage').click(function (e) { e.preventDefault(); Approve.ShowImage(); });
        // [수정내역]
        $('#cbtnHistory').click(function (e) { e.preventDefault(); Approve.History(); });
        // [결재정보]
        $('#cbtnDocInfo').click(function (e) { e.preventDefault(); Approve.DocInfo(); });
        // [후열처리]
        $('#cbtnApproveL').click(function (e) { e.preventDefault(); Approve.Submit('L'); });
        // [문서확인]
        $('#cbtnConfirm').click(function (e) { e.preventDefault(); Approve.Submit('C'); });

        // [ZIP선택다운로드]
        $('#cbtnZIP').click(function (e) { e.preventDefault(); Approve.ZIPDownLoad(); });

        //결재의견 보기
        $('#cbtnOpinionInfo').click(function (e) { e.preventDefault(); Approve.OpinionInfo(); });

        if (ListID == "" && FolderStatusID == "") {
            Approve.GetViewing();   // 일반적인 경우는 리스트에서 선체크 됨. 단, 메신저등을 통한 직접호출 시, viewing여부(병렬합의&수정) 체크. 
        }

        Approve.SetViewing("Y");    // 결재페이지 처리.

        Approve.Init();

        Approve.GetAttachFileList(); Approve.GetAttachFileCnt();    //첨부파일정보
        Approve.GetBaseDocList(); Approve.GetBaseDocCnt();          //근거문서정보
        Approve.Resource();

        Approve.OpinionCount(); //결재의견 존재여부

        //크롬에서 unload Viewing 처리
        if (navigator.sendBeacon) {
            window.addEventListener("beforeunload", function (e) {
                
                function sleep(delay) {  
                    const start = new Date().getTime();    
                    while (new Date().getTime() < start + delay);
                }

                ParamArray = { 'documentID': DocumentID, 'sequence': Sequence, 'approvalTag': ApprovalTag, 'viewing': 'N' };
                CallEncrypt(CallGetParam())

                navigator.sendBeacon('/DeskPlusEIP/Approval/Process/Basic/SetViewing.aspx?p=' + GetEncryptValue, "");

                sleep(100);
            });
        }

        $("#zeoModalContact").draggable({ disabled: false });

        __addEventListener(document, "click", Approve.click);  //for,ContextMenu (첨부파일,근거문서)
    });
});
/*
// unload체크
$(window).unload(function () {
Approve.SetViewing("N");
});
*/


// unload체크 
$(window).on('beforeunload', function () {
    ParamArray = { 'documentID': DocumentID, 'sequence': Sequence, 'approvalTag': ApprovalTag, 'viewing': 'N' };
    CallEncrypt(CallGetParam())

    var obj = eval("Approve.SuccessSetViewing");
    var url = "/DeskPlusEIP/Approval/Ws/Process/Basic/Approve.asmx/SetViewing";

    var xmlHttp = new XMLHttpRequest();
    xmlHttp.open("get", url + "?p=" + GetEncryptValue, true);
    xmlHttp.send(null);

});
/**********************************************************************************************
병렬합의 업데이트
**********************************************************************************************/
Approve.SetViewing = function (_viewing) { //alert(_viewing)
    ParamArray = { 'documentID': DocumentID, 'sequence': Sequence, 'approvalTag': ApprovalTag, 'viewing': _viewing };
    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Approve.SuccessSetViewing");
    var url = "/DeskPlusEIP/Approval/Ws/Process/Basic/Approve.asmx/SetViewing";
    AjaxService(url, data, obj);
}
Approve.SuccessSetViewing = function (data, state) {
}

/**********************************************************************************************
병렬합의 체크
**********************************************************************************************/
Approve.GetViewing = function () { //alert(_viewing)

    ParamArray = { 'documentID': DocumentID, 'sequence': Sequence, 'approvalTag': ApprovalTag, 'targetUserID': TargetUserID };
    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Approve.SuccessGetViewing");
    var url = "/DeskPlusEIP/Approval/Ws/Process/Basic/Approve.asmx/GetViewing";
    AjaxService(url, data, obj);
}
Approve.SuccessGetViewing = function (data, state) {
    if (data.d.toUpperCase() != "OK") {
        alert(data.d + R_MSG_Processing); //님이 수정 또는 결재 진행중 입니다.
        history.back();
    }
}

/**********************************************************************************************
인장 생성후 페이지 리로드
**********************************************************************************************/
var isRtnFunc = false;
function PopupWinReturnFunc() {
    if (isRtnFunc) {

        ParamArray = { 'p': 'p' };
        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Approve.SuccessGetSealPath");
        var url = "/DeskPlusEIP/Approval/Ws/Process/Basic/Approve.asmx/GetSealPath";
        AjaxService(url, data, obj);
    }
}
Approve.SuccessGetSealPath = function (data, status) {
    SealPath = data.d;
}

/**********************************************************************************************
초기화
**********************************************************************************************/
Approve.Init = function () {

    if (EventArgs != "") return;    //초기로딩 시만 처리.

    if (SealPath == "") {
        alert(R_MSG_ERRSEAL);

        isRtnFunc = true;
        var url = "/DeskPlusEIP/Approval/Common/Popup/GetSign.aspx";
        PopupWin(300, 300, url, "-500", "", true);
        return;
    }

    // 버튼처리
    Approve.SetDisplayBtn();

    // 양식로드
    if ($("#htaFormHTML").val() == "") {
        var msg = "<div style='font-size:12px;font-weight:bold;color:#FF7E00;padding:20px 0px 0px 0px;text-align:center;'>"
        msg += R_Msg_AlarmError;
        msg += "</div>";
        $("#htaFormHTML").val(msg);
    }

    ifrmForm.document.open();
    ifrmForm.document.write($("#htaFormHTML").val());
    ifrmForm.document.close();

    ifrmObj = ifrmForm.document;
    setTimeout("Approve.InitForm();", 100);
}
// 버튼처리
Approve.SetDisplayBtn = function () {

    if (ApprovalState != "C" && ApprovalState != "R") {
        $("#ulSubmitS").css("display", ""); // [후열처리]
    } else {
        $("#ulSubmitL").css("display", ""); // [문서확인]
    }
}

/**************************************************
커스텀연동 등 처리
***************************************************/
Approve.InitForm = function () {
    // 문서 수정불가 처리.
    Common.SetInActive($('#ifrmForm').get());

    // 커스텀연동 호출
    Common.GetLegacy(CustomFilePath);
}

/**************************************************
인쇄
***************************************************/
Approve.printElem = function (options) {

    var divToPrint = document.getElementById('ifrmForm');

    //스타일정보+내용
    var html = divToPrint.contentWindow.document.documentElement.outerHTML;
    newWin = window.open();
    newWin.document.write(html);

    //alert(navigator.userAgent)
    if (navigator.userAgent.indexOf("Chrome") > 0 || navigator.userAgent.indexOf("Safari") > 0) {
        setTimeout("newWin.focus(); newWin.print();", 600);     //for, Chrome & safari
    } else {
        newWin.location.reload();                               //for, exploror & firefox
        newWin.focus();
        newWin.print();
    }

    setTimeout("newWin.close();", 1000);
}

/**********************************************************************************************
다국어 처리
**********************************************************************************************/
Approve.Resource = function () {

    if ($("#j_selectall").length > 0) {
        $("#j_selectall").html(SelectAll);
    }
}

/**************************************************
목록으로 가기 
***************************************************/
Approve.List = function () {

    ParamArray = {
        'recordPerPage': RecordPerPage,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'folderStatusID': FolderStatusID
    };

    CallEncrypt(CallGetParam());
    location.href = TargetPage + "?p=" + GetEncryptValue;
}
/**********************************************************************************************
이미지 보기 (첨부파일)
**********************************************************************************************/
Approve.ShowImage = function () {

    var tmpMenu = "DocProcess";
    if (ApprovalState == "C") {
        tmpMenu = "Doc";
    }

    ParamArray = {
        'childDocID': ChildDocID,
        'menuName': tmpMenu
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Status/Common/GallerySubList.aspx";
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "ShowImage1", "800", "600")
}

/**********************************************************************************************
첨부파일 다운로드
**********************************************************************************************/
Approve.FileDownload = function (filePath, fileName, fileGuid) {
    var attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + filePath;
    window.open(attachFilePath);
}

/**********************************************************************************************
첨부 파일 정보 가져오기
**********************************************************************************************/
Approve.GetAttachFileList = function () {

    ParamArray = { 'documentID': DocumentID };
    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Approve.SuccessAttachFile");
    var url = "/DeskPlusEIP/Approval/Ws/Process/Basic/Approve.asmx/GetAttachFileList";

    AjaxService(url, data, obj);
}
Approve.SuccessAttachFile = function (data, status) {
    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Approval/Jt/Process/Basic/JTAttachFile.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate(data.d);

    if (data.d.length == 0) {
        $("#download").css("display", "none");
        $("#zipline1").css("display", "none");
        $("#zipline2").css("display", "none");
        $("#cbxAll").css("display", "none");
        $("#j_selectall").css("display", "none");
    }
}

/**********************************************************************************************
첨부 파일 카운트 호출
**********************************************************************************************/
Approve.GetAttachFileCnt = function () {

    ParamArray = { 'documentID': DocumentID };
    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Approve.SuccessAttachFileCnt");
    var url = "/DeskPlusEIP/Approval/Ws/Process/Basic/Approve.asmx/GetAttachFileCnt";

    AjaxService(url, data, obj);
}
Approve.SuccessAttachFileCnt = function (data, status) {
    $('#headerAttachCnt').text(data.d);
    if (data.d > 0) { $('#headerAttachCnt').attr("style", "cursor:pointer"); }

    if (data.d == "0") {
        $("#download").css("display", "none");
        $("#zipline1").css("display", "none");
        $("#zipline2").css("display", "none");
    }

    if ($('#cbtnShowImage').length == 0) {
        $("#zipline2").css("display", "none");
    }
}

/**********************************************************************************************
근거문서  리스트 호출
**********************************************************************************************/
Approve.GetBaseDocList = function () {

    ParamArray = { 'documentID': DocumentID };
    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Approve.SuccessBaseDoc");
    var url = "/DeskPlusEIP/Approval/Ws/Process/Basic/Approve.asmx/GetBaseDocList";

    AjaxService(url, data, obj);
}
Approve.SuccessBaseDoc = function (data, status) {
    $('#placeholder_baseDoc').setTemplateURL('/DeskPlusEIP/Approval/Jt/Process/Basic/JTBaseDoc.htm?seq=' + Guids());
    $('#placeholder_baseDoc').processTemplate(data.d);
}

/**********************************************************************************************
근거문서 보기
**********************************************************************************************/
Approve.ViewBaseDoc = function (_baseDocumnetID) {

    var url = "";

    if (_baseDocumnetID < 800000) {
        url = "/DeskPlusEIP/Approval/Folder33/Common/ViewBaseDoc.aspx";

        ParamArray = { 'documentID': _baseDocumnetID };
        CallEncrypt(CallGetParam());
        var param = "p=" + GetEncryptValue;
        WindowObjectReference = PopupWinOpen(url, param, "ViewBaseDoc", "1024", "700");
    }
    else {
        url = "/DeskPlusEIP/Approval/Folder/Common/ViewBaseDoc.aspx";

        ParamArray = { 'documentID': _baseDocumnetID };
        CallEncrypt(CallGetParam());

        var url = "/DeskPlusEIP/Approval/Folder/Common/ViewBaseDoc.aspx";
        var param = "p=" + GetEncryptValue;
        WindowObjectReference = PopupWinOpen(url, param, "ViewBaseDoc", "1200", "800");
    }
}

/**********************************************************************************************
근거문서 카운트 호출
**********************************************************************************************/
Approve.GetBaseDocCnt = function () {

    ParamArray = { 'documentID': DocumentID };
    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Approve.SuccessBaseDocCnt");
    var url = "/DeskPlusEIP/Approval/Ws/Process/Basic/Approve.asmx/GetBaseDocCnt";

    AjaxService(url, data, obj);
}
Approve.SuccessBaseDocCnt = function (data, status) {
    $('#headerBaseDocCnt').text(data.d);
    if (data.d > 0) { $('#headerBaseDocCnt').attr("style", "cursor:pointer"); }
}

/**********************************************************************************************
[수정내역]
**********************************************************************************************/
Approve.History = function () {

    ParamArray = {
        'docID': DocumentID,
        'childDocID': ChildDocID
        //,'menuName': "DocProcess"
    };
    CallEncrypt(CallGetParam());

    isRtnFunc = false;
    var url = "/DeskPlusEIP/Approval/Folder/Common/HistoryDoc.aspx?p=" + GetEncryptValue;
    PopupWin(600, 370, url, "-500", "", true);
}

/**********************************************************************************************
[결재정보]
**********************************************************************************************/
Approve.DocInfo = function () {
    ParamArray = {
        'childDocID': ChildDocID,
        'groupID': GroupID,
        'documentID': DocumentID
    };
    CallEncrypt(CallGetParam());

    isRtnFunc = false;

    if (ApprovalState == "C" || ApprovalState == "R") {
        var url = "/DeskPlusEIP/Approval/Folder/Common/DocUserSeq.aspx?p=" + GetEncryptValue;
        PopupWin(600, 530, url, "-500", "", true);
    }
    else {
        var url = "/DeskPlusEIP/Approval/Status/Common/DocUserSeq.aspx?p=" + GetEncryptValue;
        PopupWin(600, 530, url, "-500", "", true);
    }
}

/**********************************************************************************************
비밀번호 확인 및 첨언입력
**********************************************************************************************/
Approve.ConfirmPassword = function (_type) {

    if ($.browser.msie) {
        var url = "/DeskPlusEIP/Approval/Process/Basic/ConfirmPassword.aspx";
        WindowObjectReference = PopupModalWinOpen(url, "400", "350")
        return WindowObjectReference;
    }
    else { //모바일,태블리에서  window.showModalDialog 이 동작하지 않음

        ParamArray = { 'type': _type };
        CallEncrypt(CallGetParam());

        isRtnFunc = true;
        var url = "/DeskPlusEIP/Approval/Process/Basic/ConfirmPassword.aspx?p=" + GetEncryptValue;
        PopupWin(400, 350, url, "", "", true);
    }
}

/**********************************************************************************************
showModalDialog 를 지원하지 않는 브라우져에서 리턴 승인 처리
**********************************************************************************************/
function ConfirmPasswordFunc(_type) {
    switch (_type.toUpperCase()) {
        case "L":   // 승인
            Approve.ApproveL();
            break;
        case "C":   // 전결
            Approve.Confirm();
            break;
    }
}

/**********************************************************************************************
[문서내용처리/첨언처리]
**********************************************************************************************/
Approve.SetHtml = function (_isOpinion) {

    // 첨언처리
    if (_isOpinion) {
        Common.SetOpinion(UserName, TitleName, DutyName, DeptName, $("#tbOpinion").val());
    }

    var content = RemoveScriptTag($("HTML", ifrmObj).html());

    // 문서내용(Html)처리
    //$("#htaFormHTML").val("<HTML>" + $("HTML", ifrmObj).html() + "</HTML>");
    $("#htaFormHTML").val("<HTML>" + content + "</HTML>");

    $("#htaFormTEXT").val($("HTML", ifrmObj).text() + GetInputVal(ifrmObj));
}

/**********************************************************************************************
[후열처리(L) / 문서확인(C)]
수정 : showModalDialog 를 지원하지 않는 브라우져는 승인처리를 별도 구현함 ConfirmPasswordFunc()
**********************************************************************************************/
var msgReason = "";
Approve.Submit = function (_approvalType) {

    // 이중클릭 확인
    if (ApprovalType != "") { alert(R_MsgCheckClick); return; }

    switch (_approvalType.toUpperCase()) {
        case "L":   // 후열처리

            // 후열사유 체크
            if ($("#ddlReason").val() == "") {
                alert(R_Msg_LaterOnSelItem); return;
            } else {
                msgReason = $("#ddlReason").val() + "<br>[" + R_ApprovalTypeL + "]";
            }

            if ($.browser.msie) {
                if (!Approve.ConfirmPassword("L")) { return; }
                Approve.ApproveL();
            } else {
                Approve.ConfirmPassword("L");
            }
            break;
        case "C":   // 문서확인
            if ($.browser.msie) {
                if (!Approve.ConfirmPassword("C")) { return; }
                Approve.Confirm();
            } else {
                Approve.ConfirmPassword("C");
            }
            break;
    }
}
SubmitCustomCall = function (_approvalType) {

    CallComplete = true;

    switch (_approvalType.toUpperCase()) {
        case "L":   // 후열처리
            Approve.ApproveL(); break;
        case "C":   // 문서확인
            Approve.Confirm(); break;
    }
}
/**********************************************************************************************
[후열처리]
**********************************************************************************************/
Approve.ApproveL = function () {

    // 커스텀 처리
    ApprovalType = "L";
    if (CustomFilePath != "" && ifrmProcessing.onComplete) {        // 모달로 확인하여, 커스텀이고, onComplete함수가 있는경우, 해당 함수호출.
        if (!CallComplete) {
            ifrmProcessing.onComplete(); return;
        }
    }

    // 인장처리 (+결재정보) - 후열사유
    //Approve.SetStamp(ApprovalTag, msgReason, UserName, DeptName, TitleName, DutyName);
    Approve.SetStamp(ApprovalTag, msgReason, OrginUserName, OrginDeptName, OrginTitleName, OrginDutyName); // 원결재자 이름으로 표시

    // 문서내용처리 / 첨언처리
    Approve.SetHtml(true);

    __doPostBack('lbtnSubmit', "L");
}
/**********************************************************************************************
[문서확인]
**********************************************************************************************/
Approve.Confirm = function () {

    // 인장처리 (+결재정보) - 문서확인
    var msgTit = "[" + LaterOnTit + "]";
    Approve.SetStampConfirm(ApprovalTag, msgTit, UserName, DeptName, TitleName, DutyName);

    // 문서내용처리 / 첨언처리
    Approve.SetHtml(true);

    __doPostBack('lbtnSubmit', "C");
}

/**************************************************
후열사유처리
***************************************************/
Approve.SetStamp = function (_tagID, _msgReason, _userName, _deptName, _titleName, _dutyName) {

    // 승인자 인장처리
    if ($("#" + _tagID, ifrmObj).length > 0) {
        $("#" + _tagID, ifrmObj).html(_msgReason);
        $("#" + _tagID, ifrmObj).attr("isapp", "true");
    }

    // 서버시간 체크
    Common.GetServerTime();

    // 결재란처리 (0~15)
    var brTag = " ";    //  <br>
    if ($("#" + _tagID + "_INFO0", ifrmObj).length > 0) {    //결재일
        $("#" + _tagID + "_INFO0", ifrmObj).html(todayObj);
    }
    if ($("#" + _tagID + "_INFO1", ifrmObj).length > 0) {    //직급명 + 이름
        $("#" + _tagID + "_INFO1", ifrmObj).html(_titleName + brTag + _userName);
    }
    if ($("#" + _tagID + "_INFO2", ifrmObj).length > 0) {    //직급명 + 이름 + 결재일
        $("#" + _tagID + "_INFO2", ifrmObj).html(_titleName + brTag + _userName + brTag + todayObj);
    }
    if ($("#" + _tagID + "_INFO3", ifrmObj).length > 0) {    //이름
        $("#" + _tagID + "_INFO3", ifrmObj).html(_userName);
    }
    if ($("#" + _tagID + "_INFO4", ifrmObj).length > 0) {    //직급명
        $("#" + _tagID + "_INFO4", ifrmObj).html(_titleName);
    }
    if ($("#" + _tagID + "_INFO5", ifrmObj).length > 0) {    //부서명
        $("#" + _tagID + "_INFO5", ifrmObj).html(_deptName);
    }
    if ($("#" + _tagID + "_INFO6", ifrmObj).length > 0) {    //이름 + 결재일
        $("#" + _tagID + "_INFO6", ifrmObj).html(_userName + brTag + todayObj);
    }
    if ($("#" + _tagID + "_INFO7", ifrmObj).length > 0) {    //부서명 + 직급명 + 이름 + 결재일
        $("#" + _tagID + "_INFO7", ifrmObj).html(_deptName + brTag + _titleName + brTag + _userName + brTag + todayObj);
    }
    if ($("#" + _tagID + "_INFO8", ifrmObj).length > 0) {    //결재일,시간까지
        $("#" + _tagID + "_INFO8", ifrmObj).html(timeObj);
    }
    if ($("#" + _tagID + "_INFO9", ifrmObj).length > 0) {    //직책명
        $("#" + _tagID + "_INFO9", ifrmObj).html(_dutyName);
    }
    if ($("#" + _tagID + "_INFO10", ifrmObj).length > 0) {    //직책명 + 이름
        $("#" + _tagID + "_INFO10", ifrmObj).html(_dutyName + brTag + _userName);
    }
    if ($("#" + _tagID + "_INFO11", ifrmObj).length > 0) {    //직책명 + 이름+ 결재일
        $("#" + _tagID + "_INFO11", ifrmObj).html(_dutyName + brTag + _userName + brTag + todayObj);
    }
    if ($("#" + _tagID + "_INFO12", ifrmObj).length > 0) {    //부서명 + 직책명 + 이름 + 결재일
        $("#" + _tagID + "_INFO12", ifrmObj).html(_deptName + brTag + _dutyName + brTag + _userName + brTag + todayObj);
    }
    if ($("#" + _tagID + "_INFO13", ifrmObj).length > 0) {    //이름 + 결재일 + 시간까지.
        $("#" + _tagID + "_INFO13", ifrmObj).html(_userName + brTag + timeObj);
    }
    if ($("#" + _tagID + "_INFO14", ifrmObj).length > 0) {    //결재일(월일)
        $("#" + _tagID + "_INFO14", ifrmObj).html(MonthDayObj);
    }
    if ($("#" + _tagID + "_INFO15", ifrmObj).length > 0) {    //시간
        $("#" + _tagID + "_INFO15", ifrmObj).html(timeExObj);
    }
}

/**************************************************
문서확인 처리
***************************************************/
Approve.SetStampConfirm = function (_tagID, _msgReason, _userName, _deptName, _titleName, _dutyName) {

    var strOld = "";

    if ((ChildDocID != DocumentID) && (AccountTag == "ZD")) {
        // 이전문서
        ifrmParent.document.open();
        ifrmParent.document.write($("#PARENTDOCUMENT_ZQ01", ifrmObj).html());
        ifrmParent.document.close();

        ifrmParentObj = ifrmParent.document;

        var zTags = $("span", ifrmParentObj);
        $.each(zTags, function (i, o) {
            if (o.id.indexOf("_ZD0") > 0 && $("#" + o.id, ifrmObj).attr("IsAsign")) {
                if (strOld == "") {
                    strOld = o.id.split("_")[0] + "_";
                }
            }
        });
    }
    else {
        if (AccountTag.indexOf("ZQ") > -1) {
            var tmpIndex = parseInt(AccountTag.replace("ZQ", "")) + 1;

            if (tmpIndex < 10) {
                tmpIndex = "0" + tmpIndex;
            }

            if ($("#PARENTDOCUMENT_ZQ" + tmpIndex, ifrmObj).html() != undefined) {
                // 이전문서
                ifrmParent.document.open();
                ifrmParent.document.write($("#PARENTDOCUMENT_ZQ" + tmpIndex, ifrmObj).html());
                ifrmParent.document.close();

                ifrmParentObj = ifrmParent.document;

                var zTags = $("span", ifrmParentObj);
                $.each(zTags, function (i, o) {
                    if (o.id.indexOf("_ZD0") > 0 && $("#" + o.id, ifrmObj).attr("IsAsign")) {
                        if (strOld == "") {
                            strOld = o.id.split("_")[0] + "_";
                        }
                    }
                });
            }
        }
    }

    // 승인자 인장처리
    if ($("#" + strOld + _tagID, ifrmObj).length > 0) {
        $("#" + strOld + _tagID, ifrmObj).html(_msgReason);
        $("#" + strOld + _tagID, ifrmObj).attr("isapp", "true");
    }

    // 서버시간 체크
    Common.GetServerTime();

    // 결재란처리 (0~15)
    var brTag = " ";    //  <br>
    if ($("#" + strOld + _tagID + "_INFO0", ifrmObj).length > 0) {    //결재일
        $("#" + strOld + _tagID + "_INFO0", ifrmObj).html(todayObj);
    }
    if ($("#" + strOld + _tagID + "_INFO1", ifrmObj).length > 0) {    //직급명 + 이름
        $("#" + strOld + _tagID + "_INFO1", ifrmObj).html(_titleName + brTag + _userName);
    }
    if ($("#" + strOld + _tagID + "_INFO2", ifrmObj).length > 0) {    //직급명 + 이름 + 결재일
        $("#" + strOld + _tagID + "_INFO2", ifrmObj).html(_titleName + brTag + _userName + brTag + todayObj);
    }
    if ($("#" + strOld + _tagID + "_INFO3", ifrmObj).length > 0) {    //이름
        $("#" + strOld + _tagID + "_INFO3", ifrmObj).html(_userName);
    }
    if ($("#" + strOld + _tagID + "_INFO4", ifrmObj).length > 0) {    //직급명
        $("#" + strOld + _tagID + "_INFO4", ifrmObj).html(_titleName);
    }
    if ($("#" + strOld + _tagID + "_INFO5", ifrmObj).length > 0) {    //부서명
        $("#" + strOld + _tagID + "_INFO5", ifrmObj).html(_deptName);
    }
    if ($("#" + strOld + _tagID + "_INFO6", ifrmObj).length > 0) {    //이름 + 결재일
        $("#" + strOld + _tagID + "_INFO6", ifrmObj).html(_userName + brTag + todayObj);
    }
    if ($("#" + strOld + _tagID + "_INFO7", ifrmObj).length > 0) {    //부서명 + 직급명 + 이름 + 결재일
        $("#" + strOld + _tagID + "_INFO7", ifrmObj).html(_deptName + brTag + _titleName + brTag + _userName + brTag + todayObj);
    }
    if ($("#" + strOld + _tagID + "_INFO8", ifrmObj).length > 0) {    //결재일,시간까지
        $("#" + strOld + _tagID + "_INFO8", ifrmObj).html(timeObj);
    }
    if ($("#" + strOld + _tagID + "_INFO9", ifrmObj).length > 0) {    //직책명
        $("#" + strOld + _tagID + "_INFO9", ifrmObj).html(_dutyName);
    }
    if ($("#" + strOld + _tagID + "_INFO10", ifrmObj).length > 0) {    //직책명 + 이름
        $("#" + strOld + _tagID + "_INFO10", ifrmObj).html(_dutyName + brTag + _userName);
    }
    if ($("#" + strOld + _tagID + "_INFO11", ifrmObj).length > 0) {    //직책명 + 이름+ 결재일
        $("#" + strOld + _tagID + "_INFO11", ifrmObj).html(_dutyName + brTag + _userName + brTag + todayObj);
    }
    if ($("#" + strOld + _tagID + "_INFO12", ifrmObj).length > 0) {    //부서명 + 직책명 + 이름 + 결재일
        $("#" + strOld + _tagID + "_INFO12", ifrmObj).html(_deptName + brTag + _dutyName + brTag + _userName + brTag + todayObj);
    }
    if ($("#" + strOld + _tagID + "_INFO13", ifrmObj).length > 0) {    //이름 + 결재일 + 시간까지.
        $("#" + strOld + _tagID + "_INFO13", ifrmObj).html(_userName + brTag + timeObj);
    }
    if ($("#" + strOld + _tagID + "_INFO14", ifrmObj).length > 0) {    //결재일(월일)
        $("#" + strOld + _tagID + "_INFO14", ifrmObj).html(MonthDayObj);
    }
    if ($("#" + strOld + _tagID + "_INFO15", ifrmObj).length > 0) {    //시간
        $("#" + strOld + _tagID + "_INFO15", ifrmObj).html(timeExObj);
    }
}



var oContextMenuCtrl = null;
var AttachFileInfoList = null;  //첨부파일 리스트
var BaseDocInfoList = null;     //근거문서 리스트
/**************************************************
클릭 이벤트 분기
***************************************************/
Approve.click = function (e) {

    var evt = new __event(e);
    var el = evt.target;
    var action = el.getAttribute("action");

    switch (action) {
        case "attach": Approve.ShowCtxMenuAttach(e); break;    //첨부파일 건수클릭
        case "basedoc": Approve.ShowCtxMenuBase(e); break;     //근거문서 건수클릭
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}

/**********************************************************************************************
첨부파일 건수 클릭 시 : headerAttachCnt
**********************************************************************************************/
Approve.ShowCtxMenuAttach = function (e) {

    if ($("#headerAttachCnt").text() == "0") return;

    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;

    // 첨부파일 리스트 가져오기
    Approve.GetAttachFileListMenu(DocumentID);

    if (AttachFileInfoList == "1") {
        alert(R_ErrorAttachFile); return;
    } else {

        oContextMenuCtrl = new DeskPlusEIP.Component.ContextMenuCtrl();
        oContextMenuCtrl.init();

        var attachArr = AttachFileInfoList.split("|");
        var attachFile = "", attachFileName = "", attachFilePath = "";

        //전체 다운로드
        /*
        if ($.browser.msie) {
        if (attachArr.length > 1) {
        oItem = oContextMenuCtrl.addItem(R_AllDownLoad, Approve.TotalDownLoad, ImagePath + "/Icon/folders.gif");
        oItem.txt.setAttribute("arg", AttachFileInfoList, 0);
        oContextMenuCtrl.addBlank();
        }
        }
        */

        //파일별 다운로드
        for (i = 0; i < attachArr.length; i++) {
            attachFile = attachArr[i].split(":");
            attachFileName = attachFile[0];
            attachFilePath = attachFile[1];
            attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + attachFilePath;

            var oItem = oContextMenuCtrl.addItem(attachFileName, Approve.DownLoad, "EXT");
            oItem.txt.setAttribute("arg", attachFilePath, 0);
        }
        oContextMenuCtrl.show(e, null, e.clientX, e.clientY);
    }

}
/**********************************************************************************************
첨부파일 리스트 조회(웹서비스)
**********************************************************************************************/
Approve.GetAttachFileListMenu = function (_docID) {

    var url = "/DeskPlusEIP/Approval/Ws/Process/Basic/Approve.asmx/GetAttachFileListMenu";
    var data = "documentID=" + _docID;

    var obj = eval("Approve.SuccessAttFileListMenu");
    CallEncrypt(data);
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}
Approve.SuccessAttFileListMenu = function (data, status) {
    AttachFileInfoList = data.d;
}
/**********************************************************************************************
첨부파일 열기
**********************************************************************************************/
Approve.DownLoad = function (_e) {
    var evt = new __event(_e);
    if (evt.target) {
        var arg = evt.target.getAttribute("arg");
        window.open(arg);
    }
}
/**********************************************************************************************
첨부파일 전체 다운로드
**********************************************************************************************/
Approve.TotalDownLoad = function (_e) {

    var evt = new __event(_e);
    if (evt.target) {
        var arg = evt.target.getAttribute("arg");
        $("#fileList").val(arg);

        document.getElementById("frmTotalDownLoad").action = "/DeskPlusEIP/Common/DownLoad/TotalDownLoad.aspx";
        PopupWinOpen("", "", "openWin", "600", "400")
        document.getElementById("frmTotalDownLoad").submit();
    }
}


/**********************************************************************************************
근거문서 건수 클릭 시 : headerBaseDocCnt
**********************************************************************************************/
Approve.ShowCtxMenuBase = function (e) {

    if ($("#headerBaseDocCnt").text() == "0") return;

    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;

    // 근거문서 리스트 가져오기
    Approve.GetBaseDocListMenu(DocumentID);

    if (BaseDocInfoList == "1") {
        alert(R_ErrorBaseDoc); return;
    } else {
        //alert(e.clientX + "," + e.clientY)
        oContextMenuCtrl = new DeskPlusEIP.Component.ContextMenuCtrl();
        oContextMenuCtrl.init();

        var arrobj = BaseDocInfoList.split(";");        //_info.BaseDocumentID + ":" + _info.Name + ";";
        var objid = "", objnm = "";

        //근거문서보기
        for (i = 0; i < arrobj.length; i++) {

            objid = arrobj[i].split(":")[0];
            objnm = arrobj[i].split(":")[1];

            var oItem = oContextMenuCtrl.addItem(objnm, Approve.BaseDocLoad, "EXT");
            oItem.txt.setAttribute("arg", objid, 0);
        }

        oContextMenuCtrl.show(e, null, e.clientX, e.clientY);
    }
}
/**********************************************************************************************
근거문서 리스트 조회(웹서비스)
**********************************************************************************************/
Approve.GetBaseDocListMenu = function (_docID) {

    var url = "/DeskPlusEIP/Approval/Ws/Process/Basic/Approve.asmx/GetBaseDocListMenu";
    var data = "documentID=" + _docID;
    var obj = eval("Approve.SuccessGetBaseDocListMenu");
    CallEncrypt(data);
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}
Approve.SuccessGetBaseDocListMenu = function (data, status) {
    BaseDocInfoList = data.d;
}
/**********************************************************************************************
근거문서보기
**********************************************************************************************/
Approve.BaseDocLoad = function (_e) {
    var evt = new __event(_e);
    if (evt.target) {
        var arg = evt.target.getAttribute("arg");
        Approve.ViewBaseDoc(arg);  //근거문서보기
    }
}

/**********************************************************************************************
ZIP선택다운로드
**********************************************************************************************/
Approve.ZIPDownLoad = function () {

    if (DocumentID != "") {

        var fileIDList = "";
        fileIDList = Approve.CheckedValue();

        if (fileIDList != "") {
            ParamArray = {
                'module': 'APPROVAL'
                , 'listID': DocumentID
                , 'fileID': fileIDList
            };
            CallEncrypt(CallGetParam());

            var attachFilePath = "/DeskPlusEIP/Common/Zip/CreateZipFileSelect.aspx?p=" + GetEncryptValue;
            window.open(attachFilePath);
        }
        else {
            alert(R_MSG_SelectDownLoad);
        }
    }
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 선택된 값 가져오기
***************************************************/
Approve.CheckedValue = function () {
    var allVals = $('.cbxFile:checked').map(function () {
        return $(this).val();
    }).get();

    return allVals;
}

/**************************************************
작성자   :
작성일자 : 
작성목적 : 체크박스(전체)
***************************************************/
Approve.checkedAllValue = function () {
    if ($("#cbxAll").is(":checked")) {
        $('.cbxFile').attr("checked", true);
    }
    else {
        $('.cbxFile').attr("checked", false);
    }
}

Approve.GetContentHtmlList = function () {
    if ($("#htaFormHTML").val() == "") {
        var msg = "<div style='font-size:12px;font-weight:bold;color:#FF7E00;padding:20px 0px 0px 0px;text-align:center;'>"
        msg += R_Msg_AlarmError;
        msg += "</div>";
        $("#htaFormHTML").val(msg);
    }

    ifrmForm.document.open();
    ifrmForm.document.write($("#htaFormHTML").val());
    ifrmForm.document.close();

    // 문서 수정불가 처리.
    Common.SetInActive($('#ifrmForm').get());
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2022-01-05
작성목적 : 결재의견 존재 여부
**********************************************************************************************/
Approve.OpinionCount = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Common/Popup/OpinionView.asmx/GetOpinionCount";

    ParamArray = {
        'companyID': CompanyID,
        'childDocID': ChildDocID
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Approve.SuccessOpinionCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2022-01-05
작성목적 : 결재의견 존재 여부 이벤트
**********************************************************************************************/
Approve.SuccessOpinionCount = function (data, status) {
    
    var count = parseInt(data.d || 0, 10);

    if (count > 0) {
        Approve.OpinionInfo();
    }

    var baseText = $("#cbtnOpinionInfo").text();

    $("#cbtnOpinionInfo").text(baseText + " (" + count + "건)");
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2022-01-05
작성목적 : 결재의견 조회
**********************************************************************************************/
Approve.OpinionInfo = function () {
    ParamArray = {
        'companyID': CompanyID,
        'childDocID': ChildDocID
    };
    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Common/Popup/OpinionView.aspx?p=" + GetEncryptValue;
    PopupWin(600, 540, url, "-500", "", true);
}