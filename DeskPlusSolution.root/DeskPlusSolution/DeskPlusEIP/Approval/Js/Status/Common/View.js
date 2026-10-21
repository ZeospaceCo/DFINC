View = {}

$(function () {

    $(document).ready(function () {

        //인쇄
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

        //목록
        $('#cbtnCancel').click(function (e) { e.preventDefault(); View.List(); });

        //이미지 보기(첨부파일)
        $('#cbtnShowImage').click(function (e) { e.preventDefault(); View.ShowImage(); });

        //백업
        $('#cbtnZipFile').click(function (e) {
            e.preventDefault();
            if (DocumentID != "") {
                ParamArray = {
                    'module': 'APPROVALPROCESS',
                    'listID': ChildDocID
                };
                CallEncrypt(CallGetParam());

                $("#ifrm")[0].src = "/DeskPlusEIP/Common/Zip/CreateZipFile.aspx?p=" + GetEncryptValue;
            }
            else {
                alert(R_Msg_Backup);
            }
        });

        //수정내역 보기
        $('#cbtnHistory').click(function (e) {
            e.preventDefault();

            ParamArray = {
                'docID': DocumentID,
                'childDocID': ChildDocID
            };
            CallEncrypt(CallGetParam());

            var url = "/DeskPlusEIP/Approval/Folder/Common/HistoryDoc.aspx?p=" + GetEncryptValue;
            PopupWin(600, 370, url, "-500", "", true);
        });

        //결재정보 보기
        $('#cbtnDocInfo').click(function (e) {
            e.preventDefault();

            ParamArray = {
                'childDocID': ChildDocID,
                'groupID': GroupID,
                'documentID': DocumentID
            };
            CallEncrypt(CallGetParam());

            var url = "/DeskPlusEIP/Approval/Status/Common/DocUserSeq.aspx?p=" + GetEncryptValue;
            PopupWin(600, 530, url, "-500", "", true);
        });

        //열람등급
        $('#AccessTitleID').click(function (e) {
            e.preventDefault();

            ParamArray = {
            };

            CallEncrypt(CallGetParam());
            var url = "/DeskPlusEIP/Approval/Folder/Common/AccessGrade.aspx?p=" + GetEncryptValue;
            PopupWin(600, 370, url, "-500", "", true);

        });

        //결재의견추가
        $('#cbtnOpinionAdd').click(function (e) { e.preventDefault(); View.OpinionAdd(); });

        //회람
        $('#cbtnCir').click(function (e) { e.preventDefault(); View.Cir(); });

        //메일발송
        $('#cbtnMail').click(function (e) {
            e.preventDefault();

            if (DocumentID != "") {

                ParamArray = {
                    'documentID': ChildDocID
                    , 'isComplete': '0'
                };
                CallEncrypt(CallGetParam());

                var url = "/DeskPlusEIP/Approval/Folder/Dept/SendMail.aspx?p=" + GetEncryptValue;
                PopupWin(800, 500, url, "-500", "", true);
            }
            else {
                alert(R_Msg_Mail);
            }
        });

        // [ZIP선택다운로드]
        $('#cbtnZIP').click(function (e) { e.preventDefault(); View.ZIPDownLoad(); });

        // [PDF일괄다운로드]
        $('#cbtnPDF').click(function (e) { e.preventDefault(); View.PDFDownLoad(); });

        //결재의견 보기
        $('#cbtnOpinionInfo').click(function (e) { e.preventDefault(); View.OpinionInfo(); });

        if ($("#htaFormHTML").val() == "") { ChildDocID = -1; }
        View.GetContentHtmlList();
        View.GetAttachFileList(); View.GetAttachFileCnt();  //첨부파일정보
        View.GetBaseDocList(); View.GetBaseDocCnt();        //근거문서정보
        View.Resource();

        $('#headerIsGuard').text(IsGuardName);              // 문서종류
        $('#headerAccessTitleID').text(AccessTitleName);    // 열람권한
        $('#headerCarbonView').text(IsCarbonView);    // 참조열람

        View.OpinionCount(); //결재의견 존재여부

        $("#zeoModalContact").draggable({ disabled: false });

        __addEventListener(document, "click", View.click);  //for,ContextMenu (첨부파일,근거문서)

    });
});

View.GetContentHtmlList = function () {

    if ($("#htaFormHTML").val() == "") {
        var msg = "<div style='font-size:12px;font-weight:bold;color:#FF7E00;padding:20px 0px 0px 0px;text-align:center;'>"
        msg += R_Msg_AlarmError;
        msg += "</div>";
        $("#htaFormHTML").val(msg);
    }

    // 양식로드
    ifrmForm.document.open();
    ifrmForm.document.write($("#htaFormHTML").val());
    ifrmForm.document.close();

    // ZA인장 이미지 새로고침
    View.GetSeq(ChildDocID);

    // 문서 수정불가 처리.
    View.SetInActive($('#ifrmForm').get());
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 이전결재라인 중 ZA인장 이미지 새로고침 (합의불가->결재취소->합의 할 경우 인장이 바뀌지 않는 현상 때문)
***************************************************/
View.GetSeq = function (_docID) {
    if (_docID == 0) return;

    var url = "/DeskPlusEIP/Approval/Ws/FormFlow/Common/Common.asmx/GetSeq";
    ParamArray = { 'docID': _docID };
    var obj = eval("View.SuccessGetSeq");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
View.SuccessGetSeq = function (data, status) {
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

// 수정불가 처리
View.SetInActive = function (_targetFrm) {  //alert(_targetFrm);
    if (_targetFrm == null) return;

    try {
        // xscript처리.
        var delObj = $(_targetFrm).contents().find('xscript');
        $.each(delObj, function (i, o) {
            o.innerHTML = "";
        });
    } catch (e) { }

    // input,textarea 제어 (readonly)
    $(_targetFrm).contents().find('input,textarea').attr('readonly', "true");
    $(_targetFrm).contents().find('input').attr('contenteditable', "false");
    $(_targetFrm).contents().find('div').attr('contenteditable', "false");
    // 그외객체제어 (disable) - input(button,image,submit,reset),select,checkbox, ...
    $(_targetFrm).contents().find('input:checkbox,input:button,input:image,input:submit,input:reset').attr('disabled', "true");
    $(_targetFrm).contents().find('select').attr('disabled', "true");
    // 그외객체제어 (disable-선택적) - input(radio)
    var actObj = $("input:radio", _targetFrm);
    $.each(actObj, function (i, o) {
        var subobj = _targetFrm.getElementsByName(o.name);
        for (var i = 0; i < subobj.length; i++) {
            if (!subobj[i].checked) {
                subobj[i].setAttribute("disabled", "true");
            }
        }
    });

    $(_targetFrm).contents().find('input:radio').attr('disabled', "true");
}

/**********************************************************************************************
다국어 처리
**********************************************************************************************/
View.Resource = function () {

    if ($("#j_selectall").length > 0) {
        $("#j_selectall").html(SelectAll);
    }
}

/**************************************************
목록으로 가기 
***************************************************/
View.List = function () {

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
View.ShowImage = function () {
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
첨부 파일  리스트 호출
**********************************************************************************************/
View.GetAttachFileList = function () {

    ParamArray = { 'childDocID': ChildDocID };
    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessAttachFile");
    var url = "/DeskPlusEIP/Approval/Ws/Status/Common/View.asmx/GetAttachFileList";

    AjaxService(url, data, obj);
}
View.SuccessAttachFile = function (data, status) {

    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Approval/Jt/Status/Common/JTAttachFile.htm?seq=' + Guids());
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
첨부파일 다운로드
**********************************************************************************************/
View.FileDownload = function (filePath, fileName, fileGuid) {
    var attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + filePath;
    window.open(attachFilePath);
}

/**********************************************************************************************
첨부 파일 카운트 호출
**********************************************************************************************/
View.GetAttachFileCnt = function () {

    ParamArray = { 'childDocID': ChildDocID };
    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessAttachFileCnt");
    var url = "/DeskPlusEIP/Approval/Ws/Status/Common/View.asmx/GetAttachFileCnt";

    AjaxService(url, data, obj);
}
View.SuccessAttachFileCnt = function (data, status) {
    $('#headerAttachCnt').text(data.d);
    if (data.d > 0) { $('#headerAttachCnt').attr("style", "cursor:pointer"); }

    // PDF일괄다운로드 버튼 viewing처리 (첨부파일이 없으면 설명삭제)
    if (data.d == "0") {
        $("#download").css("display", "none");
        $("#solbatch").attr("title", "");
        $("#pdfbatch").css("display", "none");
        $("#zipline1").css("display", "none");
        $("#zipline2").css("display", "none");
    }
}

/**********************************************************************************************
근거문서  리스트 호출
**********************************************************************************************/
View.GetBaseDocList = function () {

    ParamArray = { 'childDocID': ChildDocID };
    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessBaseDoc");
    var url = "/DeskPlusEIP/Approval/Ws/Status/Common/View.asmx/GetBaseDocList";

    AjaxService(url, data, obj);
}
View.SuccessBaseDoc = function (data, status) {

    $('#placeholder_baseDoc').setTemplateURL('/DeskPlusEIP/Approval/Jt/Status/Common/JTBaseDoc.htm?seq=' + Guids());
    $('#placeholder_baseDoc').processTemplate(data.d);
}

/**********************************************************************************************
근거문서 보기
**********************************************************************************************/
View.ViewBaseDoc = function (_baseDocumnetID) {

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
View.GetBaseDocCnt = function () {

    ParamArray = { 'childDocID': ChildDocID };
    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessBaseDocCnt");
    var url = "/DeskPlusEIP/Approval/Ws/Status/Common/View.asmx/GetBaseDocCnt";

    AjaxService(url, data, obj);
}
View.SuccessBaseDocCnt = function (data, status) {
    $('#headerBaseDocCnt').text(data.d);
    if (data.d > 0) { $('#headerBaseDocCnt').attr("style", "cursor:pointer"); }
}

/**********************************************************************************************
회람하기
**********************************************************************************************/
View.Cir = function () {

    if (DocumentID != "") {

        ParamArray = {
            'childDocID': ChildDocID,
            'menuName': "DocProcess"
        };

        CallEncrypt(CallGetParam());

        var url = "/DeskPlusEIP/Approval/Folder/Dept/CirMemo.aspx?p=" + GetEncryptValue;
        PopupWin(800, 500, url, "-500", "", true);
    }
    else {
        alert(R_MSG_Cir);
    }
}

/**********************************************************************************************
결재의견 추가 (열람여부체크)
**********************************************************************************************/
View.OpinionAdd = function () {

    //View.ConfirmOpinion(); 결재의견 누락으로 인해, 의견추가 직전, content를 바인딩.
    ParamArray = { 'childDocID': ChildDocID };
    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessOpinionAdd");
    var url = "/DeskPlusEIP/Approval/Ws/Status/Common/View.asmx/GetDocCont";

    AjaxService(url, data, obj);

    //if (DocumentID == "") { return; }
    //View.IsAccess();     // Doc Viewing check
}
View.SuccessOpinionAdd = function (data, status) { //alert(data.d);
    if (data.d != "") {
        $("#htaFormHTML").val(data.d);
        View.GetContentHtmlList();
    }
    setTimeout("View.ConfirmOpinion();", 600);
}

/*
// viewing 체크
View.IsAccess = function () {    //alert(_documentID);

var url = "/DeskPlusEIP/Approval/Ws/Status/Common/View.asmx/GetViewing";
ParamArray = { 'documentID': DocumentID };

CallEncrypt(CallGetParam());
var data = "{p:'" + GetEncryptValue + "'}";
var obj = eval("View.SuccessIsAccess");

AjaxService(url, data, obj);
}
View.SuccessIsAccess = function (data, status) {    //alert(FuncName)

if (data.d.toUpperCase() == "OK") {
View.ConfirmOpinion();      // 결재의견 입력페이지 호출.
}
else {
alert("[" + data.d + "] " + R_MSG_Processing); return; //님이 수정 또는 결재중입니다.
}
}
*/

/**************************************************
결재의견 추가 (페이지호출)
***************************************************/
View.ConfirmOpinion = function () {

    var url = "/DeskPlusEIP/Approval/Process/Opinion/ConfirmOpinion.aspx";

    if ($.browser.msie) {
        WindowObjectReference = PopupModalWinOpen(url, "600", "350");
        return WindowObjectReference;
    }
    else { //모바일,태블리에서  window.showModalDialog 이 동작하지 않음

        ParamArray = { 'p': 'p' };
        CallEncrypt(CallGetParam());

        var url = url + "?p=" + GetEncryptValue;
        PopupWin(600, 340, url, "", "", true);
    }
}
AddOpinion = function () {

    var content = RemoveScriptTag($("HTML", ifrmForm.document).html());

    // 문서내용(Html)처리
    $("#htaFormHTML").val("<HTML>" + content + "</HTML>");
    $("#htaFormTEXT").val($("HTML", ifrmForm.document).text() + GetInputVal(ifrmForm.document));

    __doPostBack('lbtnSubmit', "ADD");
}

/**********************************************************************************************
PDF생성유무 체크
**********************************************************************************************/
View.PDFDownLoad = function () {

    if (DocumentID != "") {

        ParamArray = {
            'module': 'APPROVAL'
            , 'listID': DocumentID
        };
        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("View.SuccessPDFDownLoad");
        var url = "/DeskPlusEIP/Common/Ws/PDF/PDF.asmx/GetPDFCreateCheck";
        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
PDF다운로드
**********************************************************************************************/
View.SuccessPDFDownLoad = function (data, status) {
    var vStatus = data.d;

    if (vStatus == "Y") {
        //pdf파일이 존재할 경우
        //var iscreate = (confirm("새로 생성하시겠습니까?")) ? "1" : "0";
        ParamArray = {
            'module': 'APPROVAL'
            , 'listID': DocumentID
        };
        CallEncrypt(CallGetParam());
        $("#ifrm")[0].src = "/DeskPlusEIP/Common/PDF/PDFDownLoad.aspx?p=" + GetEncryptValue;

    }
    else {
        //pdf파일이 존재하지 않을경우 pdf파일 생성후 다운로드
        ParamArray = {
            'module': 'APPROVAL'
            , 'listID': ChildDocID
            , 'num2': Number2
            , 'iscreate': '1'
        };
        CallEncrypt(CallGetParam());

        $("#ifrm")[0].src = "/DeskPlusEIP/Common/PDF/PDFCreate.aspx?p=" + GetEncryptValue;
    }
}

/**********************************************************************************************
ZIP선택다운로드
**********************************************************************************************/
View.ZIPDownLoad = function () {

    if (DocumentID != "") {

        var fileIDList = "";
        fileIDList = View.CheckedValue();

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


var oContextMenuCtrl = null;
var AttachFileInfoList = null;  //첨부파일 리스트
var BaseDocInfoList = null;     //근거문서 리스트
/**************************************************
클릭 이벤트 분기
***************************************************/
View.click = function (e) {

    var evt = new __event(e);
    var el = evt.target;
    var action = el.getAttribute("action");

    switch (action) {
        case "attach": View.ShowCtxMenuAttach(e); break;    //첨부파일 건수클릭
        case "basedoc": View.ShowCtxMenuBase(e); break;     //근거문서 건수클릭
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}

/**********************************************************************************************
첨부파일 건수 클릭 시 : headerAttachCnt
**********************************************************************************************/
View.ShowCtxMenuAttach = function (e) {

    if ($("#headerAttachCnt").text() == "0") return;

    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;

    // 첨부파일 리스트 가져오기
    View.GetAttachFileListMenu(ChildDocID);

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
        oItem = oContextMenuCtrl.addItem(R_AllDownLoad, View.TotalDownLoad, ImagePath + "/Icon/folders.gif");
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

            var oItem = oContextMenuCtrl.addItem(attachFileName, View.DownLoad, "EXT");
            oItem.txt.setAttribute("arg", attachFilePath, 0);
        }
        oContextMenuCtrl.show(e, null, e.clientX, e.clientY);
    }

}
/**********************************************************************************************
첨부파일 리스트 조회(웹서비스)
**********************************************************************************************/
View.GetAttachFileListMenu = function (p_childDocID) {

    var url = "/DeskPlusEIP/Approval/Ws/Status/Common/View.asmx/GetAttachFileListMenu";
    var data = "childDocID=" + p_childDocID;
    var obj = eval("View.SuccessAttFileListMenu");
    CallEncrypt(data);
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}
View.SuccessAttFileListMenu = function (data, status) {
    AttachFileInfoList = data.d;
}
/**********************************************************************************************
첨부파일 열기
**********************************************************************************************/
View.DownLoad = function (_e) {
    var evt = new __event(_e);
    if (evt.target) {
        var arg = evt.target.getAttribute("arg");
        window.open(arg);
    }
}
/**********************************************************************************************
첨부파일 전체 다운로드
**********************************************************************************************/
View.TotalDownLoad = function (_e) {

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
View.ShowCtxMenuBase = function (e) {

    if ($("#headerBaseDocCnt").text() == "0") return;

    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;

    // 근거문서 리스트 가져오기
    View.GetBaseDocListMenu(ChildDocID);

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

            var oItem = oContextMenuCtrl.addItem(objnm, View.BaseDocLoad, "EXT");
            oItem.txt.setAttribute("arg", objid, 0);
        }

        oContextMenuCtrl.show(e, null, e.clientX, e.clientY);
    }
}
/**********************************************************************************************
근거문서 리스트 조회(웹서비스)
**********************************************************************************************/
View.GetBaseDocListMenu = function (p_childDocID) {

    var url = "/DeskPlusEIP/Approval/Ws/Status/Common/View.asmx/GetBaseDocListMenu";
    var data = "childDocID=" + p_childDocID;
    var obj = eval("View.SuccessGetBaseDocListMenu");
    CallEncrypt(data);
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}
View.SuccessGetBaseDocListMenu = function (data, status) {
    BaseDocInfoList = data.d;
}
/**********************************************************************************************
근거문서보기
**********************************************************************************************/
View.BaseDocLoad = function (_e) {
    var evt = new __event(_e);
    if (evt.target) {
        var arg = evt.target.getAttribute("arg");
        View.ViewBaseDoc(arg);  //근거문서보기
    }
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 선택된 값 가져오기
***************************************************/
View.CheckedValue = function () {
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
View.checkedAllValue = function () {
    if ($("#cbxAll").is(":checked")) {
        $('.cbxFile').attr("checked", true);
    }
    else {
        $('.cbxFile').attr("checked", false);
    }
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2022-01-05
작성목적 : 결재의견 존재 여부
**********************************************************************************************/
View.OpinionCount = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Common/Popup/OpinionView.asmx/GetOpinionCount";

    ParamArray = {
        'companyID': CompanyID,
        'childDocID': ChildDocID
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessOpinionCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2022-01-05
작성목적 : 결재의견 존재 여부 이벤트
**********************************************************************************************/
View.SuccessOpinionCount = function (data, status) {

    var count = parseInt(data.d || 0, 10);

    if (count > 0) {
        View.OpinionInfo();
    }

    var baseText = $("#cbtnOpinionInfo").text();

    $("#cbtnOpinionInfo").text(baseText + " (" + count + "건)");
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2022-01-05
작성목적 : 결재의견 조회
**********************************************************************************************/
View.OpinionInfo = function () {
    ParamArray = {
        'companyID': CompanyID,
        'childDocID': ChildDocID
    };
    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Common/Popup/OpinionView.aspx?p=" + GetEncryptValue;
    PopupWin(600, 540, url, "-500", "", true);
}