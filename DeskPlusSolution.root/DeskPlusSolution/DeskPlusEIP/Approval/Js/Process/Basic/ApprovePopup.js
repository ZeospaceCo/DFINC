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
        // [승인]
        $('#cbtnApproveS').click(function (e) {
            e.preventDefault();

            // 결재순서인지 체크
            Approve.GetIsMyTurn();
            if (!IsMyTurnState) {
                Approve.SetViewing("N");
                alert(R_MyTurnAppState); //현재 결재대상문서가 아닙니다. 문서보기 페이지로 이동합니다.

                ParamArray = {
                    'companyID': CompanyID,
                    'documentID': DocumentID
                };

                CallEncrypt(CallGetParam());
                location.href = "/DeskPlusEIP/Approval/Common/Popup/ViewTot.aspx?p=" + GetEncryptValue;
                return;
            }

            // 상신보류 상태인지 체크
            Approve.GetApprovalState();
            if (!IsDelayState) {
                Approve.Submit('S');
            }
            else {
                Approve.SetViewing("N");
                alert(R_DelayAppState); //상신자가 상신보류 처리하였습니다. 잠시후에 다시 시도해주세요.
            }
        });
        // [전결]
        $('#cbtnApproveA').click(function (e) {
            e.preventDefault();

            // 결재순서인지 체크
            Approve.GetIsMyTurn();
            if (!IsMyTurnState) {
                Approve.SetViewing("N");
                alert(R_MyTurnAppState); //현재 결재대상문서가 아닙니다. 문서보기 페이지로 이동합니다.

                ParamArray = {
                    'companyID': CompanyID,
                    'documentID': DocumentID
                };

                CallEncrypt(CallGetParam());
                location.href = "/DeskPlusEIP/Approval/Common/Popup/ViewTot.aspx?p=" + GetEncryptValue;
                return;
            }

            // 상신보류 상태인지 체크
            Approve.GetApprovalState();

            if (!IsDelayState) {
                Approve.Submit('A');
            }
            else {
                Approve.SetViewing("N");
                alert(R_DelayAppState); //상신자가 상신보류 처리하였습니다. 잠시후에 다시 시도해주세요.
            }
        });
        // [반려]
        $('#cbtnApproveR').click(function (e) {
            e.preventDefault();

            // 결재순서인지 체크
            Approve.GetIsMyTurn();
            if (!IsMyTurnState) {
                Approve.SetViewing("N");
                alert(R_MyTurnAppState); //현재 결재대상문서가 아닙니다. 문서보기 페이지로 이동합니다.

                ParamArray = {
                    'companyID': CompanyID,
                    'documentID': DocumentID
                };

                CallEncrypt(CallGetParam());
                location.href = "/DeskPlusEIP/Approval/Common/Popup/ViewTot.aspx?p=" + GetEncryptValue;
                return;
            }

            // 상신보류 상태인지 체크
            Approve.GetApprovalState();

            if (!IsDelayState) {
                Approve.Submit('R');
            }
            else {
                Approve.SetViewing("N");
                alert(R_DelayAppState); //상신자가 상신보류 처리하였습니다. 잠시후에 다시 시도해주세요.
            }
        });
        // [보류]
        $('#cbtnApproveD').click(function (e) {
            e.preventDefault();

            // 결재순서인지 체크
            Approve.GetIsMyTurn();
            if (!IsMyTurnState) {
                Approve.SetViewing("N");
                alert(R_MyTurnAppState); //현재 결재대상문서가 아닙니다. 문서보기 페이지로 이동합니다.

                ParamArray = {
                    'companyID': CompanyID,
                    'documentID': DocumentID
                };

                CallEncrypt(CallGetParam());
                location.href = "/DeskPlusEIP/Approval/Common/Popup/ViewTot.aspx?p=" + GetEncryptValue;
                return;
            }

            // 상신보류 상태인지 체크
            Approve.GetApprovalState();

            if (!IsDelayState) {
                Approve.Submit('D');
            }
            else {
                Approve.SetViewing("N");
                alert(R_DelayAppState); //상신자가 상신보류 처리하였습니다. 잠시후에 다시 시도해주세요.
            }
        });
        // [보류취소]
        $('#cbtnApproveDC').click(function (e) { e.preventDefault(); Approve.Submit('DC'); });
        // [합의]
        $('#cbtnApproveH').click(function (e) {
            e.preventDefault();

            // 결재순서인지 체크
            Approve.GetIsMyTurn();
            if (!IsMyTurnState) {
                Approve.SetViewing("N");
                alert(R_MyTurnAppState); //현재 결재대상문서가 아닙니다. 문서보기 페이지로 이동합니다.

                ParamArray = {
                    'companyID': CompanyID,
                    'documentID': DocumentID
                };

                CallEncrypt(CallGetParam());
                location.href = "/DeskPlusEIP/Approval/Common/Popup/ViewTot.aspx?p=" + GetEncryptValue;
                return;
            }

            // 상신보류 상태인지 체크
            Approve.GetApprovalState();

            if (!IsDelayState) {
                Approve.Submit('H');
            }
            else {
                Approve.SetViewing("N");
                alert(R_DelayAppState); //상신자가 상신보류 처리하였습니다. 잠시후에 다시 시도해주세요.
            }
        });
        // [합의불가]
        $('#cbtnApproveHR').click(function (e) {
            e.preventDefault();

            // 결재순서인지 체크
            Approve.GetIsMyTurn();
            if (!IsMyTurnState) {
                Approve.SetViewing("N");
                alert(R_MyTurnAppState); //현재 결재대상문서가 아닙니다. 문서보기 페이지로 이동합니다.

                ParamArray = {
                    'companyID': CompanyID,
                    'documentID': DocumentID
                };

                CallEncrypt(CallGetParam());
                location.href = "/DeskPlusEIP/Approval/Common/Popup/ViewTot.aspx?p=" + GetEncryptValue;
                return;
            }

            // 상신보류 상태인지 체크
            Approve.GetApprovalState();

            if (!IsDelayState) {
                Approve.Submit('HR');
            }
            else {
                Approve.SetViewing("N");
                alert(R_DelayAppState); //상신자가 상신보류 처리하였습니다. 잠시후에 다시 시도해주세요.
            }
        });
        // [결재취소]
        $('#cbtnApproveSC').click(function (e) { e.preventDefault(); Approve.Submit('SC'); });
        // [회람]
        $('#cbtnCir').click(function (e) { e.preventDefault(); Approve.Cir(); });

        // [ZIP선택다운로드]
        $('#cbtnZIP').click(function (e) { e.preventDefault(); Approve.ZIPDownLoad(); });

        //결재의견 보기
        $('#cbtnOpinionInfo').click(function (e) { e.preventDefault(); Approve.OpinionInfo(); });

        if (ListID == "" && FolderStatusID == "") {
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

    ParamArray = { 'documentID': DocumentID, 'sequence': Sequence, 'approvalTag': ApprovalTag, 'targetUserID': TargetUserID };
    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Approve.SuccessGetViewing");
    var url = "/DeskPlusEIP/Approval/Ws/Process/Basic/Approve.asmx/GetViewing";
    AjaxService(url, data, obj);
}
Approve.SuccessGetViewing = function (data, state) {//alert(data.d)
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

    // 버튼처리
    Approve.SetDisplayBtn();

    if (ApprovalTag.indexOf("ZH") > -1) {
        $('#cbtnApproveA').css("display", "none");
    }

    // 양식로드
    if ($("#htaFormHTML").val() == "") {
        var msg = "<div style='font-size:12px;font-weight:bold;color:#FF7E00;padding:20px 0px 0px 0px;text-align:center;'>"
        msg += R_Msg_AlarmErrorView;
        msg += "</div>";
        $("#htaFormHTML").val(msg);
    }

    ifrmForm.document.open();
    ifrmForm.document.write($("#htaFormHTML").val());
    ifrmForm.document.close();

    // ZA인장 이미지 새로고침
    Approve.GetSeq(ChildDocID);

    ifrmObj = ifrmForm.document;
    setTimeout("Approve.InitForm();", 100);
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 이전결재라인 중 ZA인장 이미지 새로고침 (합의불가->결재취소->합의 할 경우 인장이 바뀌지 않는 현상 때문)
***************************************************/
Approve.GetSeq = function (_docID) {
    if (_docID == 0) return;

    var url = "/DeskPlusEIP/Approval/Ws/FormFlow/Common/Common.asmx/GetSeq";
    ParamArray = { 'docID': _docID };
    var obj = eval("Approve.SuccessGetSeq");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
Approve.SuccessGetSeq = function (data, status) {
    if (data != null) {
        $.each(data.d, function (i, o) {
            if (data.d[i].ApprovalTag.indexOf("ZA") > -1) {
                if (data.d[i].ApprovalType.trimText() != "") {
                    var src = $('#' + data.d[i].ApprovalTag + '>img', ifrmForm.document).attr("src");
                    if (src != undefined) {
                        if (src.indexOf("?number=") < 0) {
                            src = src + "?number=" + Math.random() * 1000;
                            $('#' + data.d[i].ApprovalTag + '>img', ifrmForm.document).attr("src", src);
                        }
                    }
                }
            }
        });
    }
}

// 버튼처리
Approve.SetDisplayBtn = function () {

    if (ApprovalState == "R") {                 // [결재취소]
        $("#ulSubmitSC").css("display", ""); return;
    }

    if (ApprovalState == "D") {                 // [보류취소]
        $("#ulSubmitDC").css("display", ""); return;
    }

    if (ApprovalTag.indexOf("ZA") == 0) {       // [합의/합의불가]
        $("#ulSubmitH").css("display", ""); return;
    }

    $("#ulSubmit").css("display", "");          //[승인/전결/반려/보류/문서수정]
}


/**********************************************************************************************
커스텀연동 등 처리
**********************************************************************************************/
Approve.InitForm = function () {
    // 문서 수정불가 처리.
    Common.SetInActive($('#ifrmForm').get());

    // 커스텀연동 호출
    Common.GetLegacy(CustomFilePath);
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

/**********************************************************************************************
이미지 보기 (첨부파일)
**********************************************************************************************/
Approve.ShowImage = function () {
    ParamArray = {
        'childDocID': ChildDocID,
        'menuName': "DocProcess"
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
    //window.open(attachFilePath);
    location.href = attachFilePath;
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
        'childDocID': ChildDocID,
        'menuName': "DocProcess"
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
    var url = "/DeskPlusEIP/Approval/Status/Common/DocUserSeq.aspx?p=" + GetEncryptValue;
    PopupWin(600, 530, url, "-500", "", true);
}

/**********************************************************************************************
비밀번호 확인 및 첨언입력
**********************************************************************************************/
Approve.ConfirmPassword = function (_type) {

    if ($.browser.msie) {
        var url = "/DeskPlusEIP/Approval/Process/Basic/ConfirmPassword.aspx";
        WindowObjectReference = PopupModalWinOpen(url, "750", "590")
        return WindowObjectReference;
    }
    else { //모바일,태블리에서  window.showModalDialog 이 동작하지 않음

        ParamArray = { 'type': _type };
        CallEncrypt(CallGetParam());

        isRtnFunc = true;
        var url = "/DeskPlusEIP/Approval/Process/Basic/ConfirmPassword.aspx?p=" + GetEncryptValue;
        PopupWin(750, 590, url, "", "", true);
    }
}

/**********************************************************************************************
showModalDialog 를 지원하지 않는 브라우져에서 리턴 승인 처리
**********************************************************************************************/
function ConfirmPasswordFunc(_type) {
    switch (_type.toUpperCase()) {
        case "S":   // 승인
            Approve.ApproveS();
            break;
        case "A":   // 전결
            Approve.ApproveA();
            break;
        case "R":   // 반려
            Approve.ApproveR();
            break;
        case "D":   // 보류
            Approve.ApproveD();
            break;
        case "DC":  // 보류취소
            Approve.ApproveDC();
            break;
        case "H":   // 합의
            Approve.ApproveH();
            break;
        case "HR":  //합의불가
            Approve.ApproveHR();
            break;
        case "SC":  // 결재취소
            Approve.ApproveSC();
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

    // ZA인장 이미지 새로고침
    Approve.GetSeq(ChildDocID);

    var content = RemoveScriptTag($("HTML", ifrmObj).html());

    // 문서내용(Html)처리
    //$("#htaFormHTML").val("<HTML>" + $("HTML", ifrmObj).html() + "</HTML>");
    $("#htaFormHTML").val("<HTML>" + content + "</HTML><ZEOEND>");
    $("#htaFormTEXT").val($("HTML", ifrmObj).text());
}

/**********************************************************************************************
[승인(S) / 전결(A) / 반려(R) / 보류(D) / 보류취소(DC) / 합의(H) / 합의불가(HR)]
수정 : showModalDialog 를 지원하지 않는 브라우져는 승인처리를 별도 구현함 ConfirmPasswordFunc()
**********************************************************************************************/
Approve.Submit = function (_approvalType) {

    // 이중클릭 확인
    if (ApprovalType != "") { alert(R_MsgCheckClick); return; }

    switch (_approvalType.toUpperCase()) {
        case "S":   // 승인
            if ($.browser.msie) {
                if (!Approve.ConfirmPassword("S")) { return; }
                Approve.ApproveS();
            } else {
                Approve.ConfirmPassword("S");
            }
            break;
        case "A":   // 전결
            if ($.browser.msie) {
                if (!Approve.ConfirmPassword("A")) { return; }
                Approve.ApproveA();
            } else {
                Approve.ConfirmPassword("A");
            }
            break;
        case "R":   // 반려
            Approve.ApproveR();
            break;
        case "D":   // 보류
            Approve.ApproveD();
            break;
        case "DC":  // 보류취소
            Approve.ApproveDC();
            break;
        case "H":   // 합의
            if ($.browser.msie) {
                if (!Approve.ConfirmPassword("H")) { return; }
                Approve.ApproveH();
            } else {
                Approve.ConfirmPassword("H");
            }
            break;
        case "HR":  //합의불가
            if ($.browser.msie) {
                if (!Approve.ConfirmPassword("HR")) { return; }
                Approve.ApproveHR();
            } else {
                Approve.ConfirmPassword("HR");
            }
            break;
        case "SC":  // 결재취소
            Approve.ApproveSC();
            break;
    }
}
SubmitCustomCall = function (_approvalType) {

    CallComplete = true;

    switch (_approvalType.toUpperCase()) {
        case "S":   // 승인
            Approve.ApproveS(); break;
        case "A":   // 전결
            Approve.ApproveA(); break;
        case "R":   // 반려
            Approve.ApproveR(); break;
        case "D":   // 보류
            Approve.ApproveD(); break;
        case "DC":  // 보류취소
            Approve.ApproveDC(); break;
        case "H":   // 합의
            Approve.ApproveH(); break;
        case "HR":  //합의불가
            Approve.ApproveHR(); break;
        case "SC":  //결재취소
            Approve.ApproveSC(); break;
    }
}

/**********************************************************************************************
[승인]
**********************************************************************************************/
Approve.ApproveS = function () {

    // 커스텀 처리
    ApprovalType = "S";
    if (CustomFilePath != "" && ifrmProcessing.onComplete) {        // 모달로 확인하여, 커스텀이고, onComplete함수가 있는경우, 해당 함수호출.
        if (!CallComplete) {
            ifrmProcessing.onComplete(); return;
        }
    }

    // 인장처리 (+결재정보)
    Common.SetStamp(ApprovalTag, OrginSealPath, OrginUserName, OrginDeptName, OrginTitleName, OrginDutyName, ProxyUserID);

    $("#tbSealPath").val(SealPath);

    // 문서내용처리 / 첨언처리
    Approve.SetHtml(true);

    __doPostBack('lbtnSubmit', "S");
}

/**********************************************************************************************
[전결]
**********************************************************************************************/
Approve.ApproveA = function () {

    // 커스텀 처리
    ApprovalType = "A";
    if (CustomFilePath != "" && ifrmProcessing.onComplete) {        // 모달로 확인하여, 커스텀이고, onComplete함수가 있는경우, 해당 함수호출.
        if (!CallComplete) {
            ifrmProcessing.onComplete(); return;
        }
    }

    // 인장처리 (+결재정보)
    SealPath = FullDomainPath + ImagePath + "/Lang/" + PreLang + "/app_stamp_A.gif";

    Common.SetStamp(ApprovalTag, SealPath, OrginUserName, OrginDeptName, OrginTitleName, OrginDutyName, ProxyUserID);
    $("#tbSealPath").val(SealPath);

    // 문서내용처리 / 첨언처리
    Approve.SetHtml(true);

    __doPostBack('lbtnSubmit', "A");
}


/**********************************************************************************************
[반려]
**********************************************************************************************/
var targetInfo = "";
Approve.ApproveR = function () {

    // 반려대상
    if (!CallComplete) {
        if ($.browser.msie) {
            targetInfo = Approve.ConfirmReject();
            if (targetInfo == "!NULL" || targetInfo == "") { return; }

            ApproveRProcess();
        } else {
            ParamArray = { 'documentID': DocumentID, 'groupID': GroupID, 'isType': '1' };
            CallEncrypt(CallGetParam());

            var url = "/DeskPlusEIP/Approval/Process/Basic/ConfirmReject.aspx?p=" + GetEncryptValue;
            PopupWin(600, 588, url, "", "", true);
        }

    } else {
        ApproveRProcess();
    }
}

/**********************************************************************************************
반려대상
**********************************************************************************************/
Approve.ConfirmReject = function () {
    ParamArray = { 'documentID': DocumentID, 'groupID': GroupID, 'isType': '1' };
    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Process/Basic/ConfirmReject.aspx?p=" + GetEncryptValue;
    WindowObjectReference = PopupModalWinOpen(url, "600", "588");

    var rtnval = (WindowObjectReference == undefined || WindowObjectReference == "") ? "!NULL" : WindowObjectReference;
    //_targetInfo : [0]DocumentID-[1]Sequence-[2]ApprovalTag-[3]UserID

    return rtnval;
}

function ConfirmRejectFunc(_targetInfo) {
    if (_targetInfo == "!NULL" || _targetInfo == "") { return; }

    targetInfo = _targetInfo;

    ApproveRProcess();
}

var isTargetZD = false;
function ApproveRProcess() {

    if (targetInfo == "") return;

    //targetInfo : [0]DocumentID-[1]Sequence-[2]ApprovalTag-[3]UserID-[4]ApprovalType
    //반려대상부서가 발신(최초)부서인지 확인.DocumentID / GroupID (For,CustomScript)
    if (AccountTag == "ZD") {
        isTargetZD = true;
    } else {
        isTargetZD = (targetInfo.indexOf(GroupID + "-") == 0) ? true : false;
    }
    //alert("isTargetZD:" + isTargetZD);return;

    // 커스텀 처리
    ApprovalType = "R";
    if (CustomFilePath != "" && ifrmProcessing.onComplete) {        // 모달로 확인하여, 커스텀이고, onComplete함수가 있는경우, 해당 함수호출.
        if (!CallComplete) {
            ifrmProcessing.onComplete(); return;
        }
    }

    // 인장처리 (+결재정보)
    SealPath = FullDomainPath + ImagePath + "/Lang/" + PreLang + "/app_stamp_R.gif";

    Common.SetStamp(ApprovalTag, SealPath, OrginUserName, OrginDeptName, OrginTitleName, OrginDutyName, ProxyUserID);
    $("#tbSealPath").val(SealPath);

    // 문서내용처리 / 첨언처리
    Approve.SetHtml(true);

    //반려대상지정
    $("#tbTargetInfo").val(targetInfo); //_targetInfo : [0]DocumentID-[1]Sequence-[2]ApprovalTag-[3]UserID
    __doPostBack('lbtnSubmit', "R");
}

/**********************************************************************************************
[보류]
**********************************************************************************************/
Approve.ApproveD = function () {

    if (confirm(R_MSG_Delay)) { //보류 하시겠습니까?

        // 문서내용처리 / 첨언처리
        Approve.SetHtml(false);

        __doPostBack('lbtnSubmit', "D");
    }
}

/**********************************************************************************************
[보류취소]
**********************************************************************************************/
Approve.ApproveDC = function () {

    if (confirm(R_MSG_CancelDelay)) { //보류취소 하시겠습니까?

        // 문서내용처리 / 첨언처리
        Approve.SetHtml(false);

        __doPostBack('lbtnSubmit', "DC");
    }
}

/**********************************************************************************************
[합의]
**********************************************************************************************/
Approve.ApproveH = function () {

    // 커스텀 처리
    ApprovalType = "S";
    if (CustomFilePath != "" && ifrmProcessing.onComplete) {        // 모달로 확인하여, 커스텀이고, onComplete함수가 있는경우, 해당 함수호출.
        if (!CallComplete) {
            ifrmProcessing.onComplete(); return;
        }
    }

    // 인장처리 (+결재정보)
    SealPath = FullDomainPath + ImagePath + "/Lang/" + PreLang + "/app_stamp_H.gif";

    Common.SetStamp(ApprovalTag, SealPath, OrginUserName, OrginDeptName, OrginTitleName, OrginDutyName, ProxyUserID);
    $("#tbSealPath").val(SealPath);

    // 문서내용처리 / 첨언처리
    Approve.SetHtml(true);

    __doPostBack('lbtnSubmit', "S");
}

/**********************************************************************************************
[합의불가]
**********************************************************************************************/
Approve.ApproveHR = function () {

    // 커스텀 처리
    ApprovalType = "S";
    if (CustomFilePath != "" && ifrmProcessing.onComplete) {        // 모달로 확인하여, 커스텀이고, onComplete함수가 있는경우, 해당 함수호출.
        if (!CallComplete) {
            ifrmProcessing.onComplete(); return;
        }
    }

    // 인장처리 (+결재정보)
    SealPath = FullDomainPath + ImagePath + "/Lang/" + PreLang + "/app_stamp_HR.gif";

    Common.SetStamp(ApprovalTag, SealPath, OrginUserName, OrginDeptName, OrginTitleName, OrginDutyName, ProxyUserID);
    $("#tbSealPath").val(SealPath);

    // 문서내용처리 / 첨언처리
    Approve.SetHtml(true);

    __doPostBack('lbtnSubmit', "S");
}

/**********************************************************************************************
[결재취소]
**********************************************************************************************/
Approve.ApproveSC = function () {

    if (confirm(R_MSG_CancelApprove)) { //결재취소 하시겠습니까?

        // 이전결재라인 가져오기
        var url = "/DeskPlusEIP/Approval/Ws/Process/Basic/Approve.asmx/GetSeqLine";
        ParamArray = { 'docID': DocumentID, 'sequence': Sequence, 'approvalTag': ApprovalTag };
        var obj = eval("Approve.SuccessApproveSC");
        CallEncrypt(CallGetParam());
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }
}
Approve.SuccessApproveSC = function (data, status) {
    if (data != null) {
        $.each(data.d, function (i, o) {
            Approve.DefaultApprovals(data.d[i].ApprovalTag, data.d[i].UserID, data.d[i].NameBase, data.d[i].DeptID, data.d[i].DeptBase, data.d[i].TitleID, data.d[i].TitleBase, data.d[i].DutyID, data.d[i].DutyBase, "M", "0", "");
        });

        // 문서내용처리 / 첨언처리
        Approve.SetHtml(true);

        __doPostBack('lbtnSubmit', "SC");
    }
    else {
        alert(R_MSG_FAIL);
    }
}
Approve.DefaultApprovals = function (_tagID, _userID, _userName, _deptID, _deptName, _titleID, _titleName, _dutyID, _dutyName, _isGroup, _subTree, _userList) {

    if ($("#" + _tagID, ifrmObj).length > 0) {              //인장
        $("#" + _tagID, ifrmObj).html(_userName);
    }

    if ($("#" + _tagID + "_INFO0", ifrmObj).length > 0) {    //결재일
        $("#" + _tagID + "_INFO0", ifrmObj).html("&nbsp;");
    }
    if ($("#" + _tagID + "_INFO1", ifrmObj).length > 0) {    //직급명 + 이름
        $("#" + _tagID + "_INFO1", ifrmObj).html("&nbsp;");
    }
    if ($("#" + _tagID + "_INFO2", ifrmObj).length > 0) {    //직급명 + 이름 + 결재일
        $("#" + _tagID + "_INFO2", ifrmObj).html("&nbsp;");
    }
    if ($("#" + _tagID + "_INFO3", ifrmObj).length > 0) {    //이름
        $("#" + _tagID + "_INFO3", ifrmObj).html("&nbsp;");
    }
    if ($("#" + _tagID + "_INFO4", ifrmObj).length > 0) {    //직급명
        $("#" + _tagID + "_INFO4", ifrmObj).html("&nbsp;");
    }
    if ($("#" + _tagID + "_INFO5", ifrmObj).length > 0) {    //부서명
        $("#" + _tagID + "_INFO5", ifrmObj).html("&nbsp;");
    }
    if ($("#" + _tagID + "_INFO6", ifrmObj).length > 0) {    //이름 + 결재일
        $("#" + _tagID + "_INFO6", ifrmObj).html("&nbsp;");
    }
    if ($("#" + _tagID + "_INFO7", ifrmObj).length > 0) {    //부서명 + 직급명 + 이름 + 결재일
        $("#" + _tagID + "_INFO7", ifrmObj).html("&nbsp;");
    }
    if ($("#" + _tagID + "_INFO8", ifrmObj).length > 0) {    //결재일,시간까지
        $("#" + _tagID + "_INFO8", ifrmObj).html("&nbsp;");
    }
    if ($("#" + _tagID + "_INFO9", ifrmObj).length > 0) {    //직책명
        $("#" + _tagID + "_INFO9", ifrmObj).html("&nbsp;");
    }
    if ($("#" + _tagID + "_INFO10", ifrmObj).length > 0) {    //직책명 + 이름
        $("#" + _tagID + "_INFO10", ifrmObj).html("&nbsp;");
    }
    if ($("#" + _tagID + "_INFO11", ifrmObj).length > 0) {    //직책명 + 이름+ 결재일
        $("#" + _tagID + "_INFO11", ifrmObj).html("&nbsp;");
    }
    if ($("#" + _tagID + "_INFO12", ifrmObj).length > 0) {    //부서명 + 직책명 + 이름 + 결재일
        $("#" + _tagID + "_INFO12", ifrmObj).html("&nbsp;");
    }
    if ($("#" + _tagID + "_INFO13", ifrmObj).length > 0) {    //이름 + 결재일 + 시간까지.
        $("#" + _tagID + "_INFO13", ifrmObj).html("&nbsp;");
    }
    if ($("#" + _tagID + "_INFO14", ifrmObj).length > 0) {    //결재일(월일)
        $("#" + _tagID + "_INFO14", ifrmObj).html("&nbsp;");
    }
    if ($("#" + _tagID + "_INFO15", ifrmObj).length > 0) {    //시간
        $("#" + _tagID + "_INFO15", ifrmObj).html("&nbsp;");
    }


}

/**************************************************
목록으로 가기 
***************************************************/
Approve.List = function () {

    if (FuncCall != "") {
        eval("top.opener." + FuncCall);
        setTimeout("window.open('about:blank', '_self').close();", 100);
    } else {
        //window.open('about:blank', '_self').close();

        ParamArray = {};    //상신한문서로 카테고리아이디 맞추기.'p':'p'
        CallEncrypt(CallGetParam());
        var defaultPage = "/DeskPlusEIP/Approval/Status/Default.aspx";
        if (typeof ApprovalStatusMenuType != "undefined" && ApprovalStatusMenuType.toUpperCase() == "NEW") {
            defaultPage = "/DeskPlusEIP/Approval/Status/DefaultAll.aspx";
        }
        var url = defaultPage + "?p=" + GetEncryptValue;

        document.location.href = url;
    }
}


/**********************************************************************************************
회람하기
**********************************************************************************************/
Approve.Cir = function () {

    ParamArray = {
        'childDocID': ChildDocID,
        'menuName': "DocProcess"
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Folder/Dept/CirMemo.aspx?p=" + GetEncryptValue;
    PopupWin(800, 500, url, "-500", "", true);

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

    // 양식로드
    if ($("#htaFormHTML").val() == "") {
        var msg = "<div style='font-size:12px;font-weight:bold;color:#FF7E00;padding:20px 0px 0px 0px;text-align:center;'>"
        msg += R_Msg_AlarmErrorView;
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

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 상신보류 상태 체크
**********************************************************************************************/
Approve.GetApprovalState = function () { //alert(_viewing)

    if (CompanyID == 0) return; //submit 처리 후, 미호출

    ParamArray = { 'documentID': DocumentID };
    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Approve.SuccessGetApprovalState");
    var url = "/DeskPlusEIP/Approval/Ws/Process/Basic/Approve.asmx/GetApprovalState";
    AjaxService(url, data, obj);
}
Approve.SuccessGetApprovalState = function (data, state) {
    if (data.d.toUpperCase() == "1") {
        IsDelayState = true; // 상신보류 상태

    }
    else {
        IsDelayState = false;
    }
}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 현재 결재순서 여부 확인
**********************************************************************************************/
Approve.GetIsMyTurn = function () {

    if (CompanyID == 0) return;

    ParamArray = {
        'documentID': DocumentID,
        'sequence': Sequence,
        'approvalTag': ApprovalTag,
        'targetUserID': TargetUserID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Approve.SuccessGetIsMyTurn");
    var url = "/DeskPlusEIP/Approval/Ws/Process/Basic/Approve.asmx/GetIsMyTurn";
    AjaxService(url, data, obj);
}
Approve.SuccessGetIsMyTurn = function (data, state) {
    if (data.d.toUpperCase() == "Y") {
        IsMyTurnState = true;

    }
    else {
        IsMyTurnState = false;
    }
}
