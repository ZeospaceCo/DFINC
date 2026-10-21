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

        // [이미지 보기] - 첨부파일
        $('#cbtnShowImage').click(function (e) { e.preventDefault(); Approve.ShowImage(); });
        // [수정내역]
        $('#cbtnHistory').click(function (e) { e.preventDefault(); Approve.History(); });
        // [결재정보]
        $('#cbtnDocInfo').click(function (e) { e.preventDefault(); Approve.DocInfo(); });
        // [문서확인]
        $('#cbtnConfirm').click(function (e) { e.preventDefault(); Approve.Submit('C'); });

        // [ZIP선택다운로드]
        $('#cbtnZIP').click(function (e) { e.preventDefault(); Approve.ZIPDownLoad(); });

        //결재의견 보기
        $('#cbtnOpinionInfo').click(function (e) { e.preventDefault(); Approve.OpinionInfo(); });

        if (ListID == "" && FolderStatusID == "" && ApprovalState != "C") {
            Approve.GetViewing();   // 일반적인 경우는 리스트에서 선체크 됨. 단, 메신저등을 통한 직접호출 시, viewing여부(병렬합의&수정) 체크. 
            if (IsApproving) { Approve.List(); return; }
        }

        Approve.SetViewing("Y");    // 결재페이지 처리.

        Approve.Init();

        Approve.GetAttachFileList(); Approve.GetAttachFileCnt();    //첨부파일정보
        Approve.GetBaseDocList(); Approve.GetBaseDocCnt();          //근거문서정보
        Approve.Resource();

        Approve.OpinionCount(); //결재의견 존재여부

        //크롬에서 unload Viewing 처리
        if (navigator.sendBeacon) {
            window.addEventListener("beforeunload", function logData() {
                if (!IsApproving) {
                    ParamArray = { 'documentID': DocumentID, 'sequence': Sequence, 'approvalTag': ApprovalTag, 'viewing': 'N' };
                    CallEncrypt(CallGetParam())

                    navigator.sendBeacon('/DeskPlusEIP/Approval/Process/Basic/SetViewing.aspx?p=' + GetEncryptValue, "");
                }
            });
        }

        $("#zeoModalContact").draggable({ disabled: false });

        __addEventListener(document, "click", Approve.click);  //for,ContextMenu (첨부파일,근거문서)
    });
});

/*
// unload체크
$(window).unload(function () {
if (!IsApproving) {
Approve.SetViewing("N");        // 2015.01.12 : SetViewing호출 시, DocProcess's Viewing 필드도 업데이트 됨. 따라서, 해당 분기부분 처리
}
});
*/

// unload체크
$(window).on('beforeunload', function () {
    if (!IsApproving) {
        ParamArray = { 'documentID': DocumentID, 'sequence': Sequence, 'approvalTag': ApprovalTag, 'viewing': 'N' };
        CallEncrypt(CallGetParam())

        var obj = eval("Approve.SuccessSetViewing");
        var url = "/DeskPlusEIP/Approval/Ws/Process/Basic/Approve.asmx/SetViewing";

        var xmlHttp = new XMLHttpRequest();
        xmlHttp.open("get", url + "?p=" + GetEncryptValue, true);
        xmlHttp.send(null);
    }
});

/**********************************************************************************************
병렬합의 업데이트
**********************************************************************************************/
Approve.SetViewing = function (_viewing) { //alert(_viewing)

    if (CompanyID == 0) return; //submit 처리 후, 미호출

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

    if (CompanyID == 0) return; //submit 처리 후, 미호출

    ParamArray = { 'documentID': DocumentID, 'sequence': Sequence, 'approvalTag': ApprovalTag, 'targetUserID': UserID };
    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Approve.SuccessGetViewing");
    var url = "/DeskPlusEIP/Approval/Ws/Process/Basic/Approve.asmx/GetViewing";
    AjaxService(url, data, obj);
}
Approve.SuccessGetViewing = function (data, state) {
    if (data.d.toUpperCase() != "OK" && data.d != "") {
        IsApproving = true;
        alert(data.d + R_MSG_Processing); //님이 수정 또는 결재 진행중 입니다.
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

/**********************************************************************************************
커스텀연동 등 처리
**********************************************************************************************/
Approve.InitForm = function () {
    // 문서 수정불가 처리.
    Common.SetInActive($('#ifrmForm').get());
}

/**********************************************************************************************
인쇄
**********************************************************************************************/
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
    //WindowObjectReference = window.open(attachFilePath, "FileDownload", "menubar=no,location=no,resizable=no,scrollbars=no,status=no");
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
        $("#cbxAll").css("display", "none");
        $("#j_selectall").css("display", "none");
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

    if (ApprovalState == "C") {
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

        ParamArray = { 'nosign': 'Y' };
        CallEncrypt(CallGetParam());

        var url = "/DeskPlusEIP/Approval/Process/Basic/ConfirmPassword.aspx?p=" + GetEncryptValue;
        WindowObjectReference = PopupModalWinOpen(url, "400", "350")
        return WindowObjectReference;
    }
    else { //모바일,태블리에서  window.showModalDialog 이 동작하지 않음

        ParamArray = { 'type': _type, 'nosign': 'Y' };
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
    Approve.Confirm();
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
    $("#htaFormHTML").val("<HTML>" + content + "</HTML>");
    $("#htaFormTEXT").val($("HTML", ifrmObj).text() + GetInputVal(ifrmObj));
}

/**********************************************************************************************
문서확인(C)
수정 : showModalDialog 를 지원하지 않는 브라우져는 승인처리를 별도 구현함 ConfirmPasswordFunc()
**********************************************************************************************/
var msgReason = "";
Approve.Submit = function (_type) {

    // 이중클릭 확인
    if (ApprovalType != "") { alert(R_MsgCheckClick); return; }

    if ($.browser.msie) {
        if (!Approve.ConfirmPassword("C")) { return; }
        Approve.Confirm();
    } else {
        Approve.ConfirmPassword("C");
    }
}

/**********************************************************************************************
[문서확인]
**********************************************************************************************/
Approve.Confirm = function () {

    if (ApprovalState != "C") {
        Approve.SetConfirm();           // 진행중 문서인 경우, 바로 Submit처리. (알람URL의 경우도, state값은 '', 의미없음)ㅇ????????????????????????
    } else {
        Approve.GetHTML(DocumentID);    // 완료된 문서인 경우, 문서내용 다시 가져온후, Submit처리.
    }
}

Approve.SetConfirm = function () {
    // 인장처리 (+결재정보) - 문서확인
    var msgTit = "[" + ConfirmTit + "]";        //확인
    Approve.SetStamp(ApprovalTag, msgTit, UserName, DeptName, TitleName, DutyName);

    // 문서내용처리 / 첨언처리
    Approve.SetHtml(true);

    __doPostBack('lbtnSubmit', '');
}

/**************************************************
후열사유/문서확인 처리
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

/**********************************************************************************************
병렬합의 업데이트
**********************************************************************************************/
Approve.GetHTML = function (_docid) { //alert(_docid)
    ParamArray = { 'docid': _docid };
    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Approve.SuccessGetHTML");
    var url = "/DeskPlusEIP/Approval/Ws/Status/ListConfirm.asmx/GetHTML";
    AjaxService(url, data, obj);
}
Approve.SuccessGetHTML = function (data, state) {

    if (data.d != "") {

        ifrmForm.document.open();
        ifrmForm.document.write(data.d);
        ifrmForm.document.close();

        ifrmObj = ifrmForm.document;
        setTimeout("Approve.SetConfirm();", 100);
    }
}

/**********************************************************************************************
목록으로 가기 
**********************************************************************************************/
Approve.List = function () {
    window.open("about:blank", "_self").close();
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