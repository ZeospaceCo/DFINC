View = {}

$(function () {

    $(document).ready(function () {

        //인쇄
        $('#cbtnPrint').click(function (e) {
            e.preventDefault();

            /*
            ParamArray = { 'childDocID': ChildDocID };
            CallEncrypt(CallGetParam());
            var url = "/DeskPlusEIP/Approval/Common/Popup/Print.aspx?p=" + GetEncryptValue;

            if ($.browser.msie) {
            WindowObjectReference = PopupModalWinOpen(url, "800", "640")
            } else {
            PopupWin("800", "640", url, "", "", true);
            }
            */

            if ($("#OPINION", ifrmForm.document).length > 0) {
                //$('#OPINION', ifrmForm.document).hide();
            }

            newWin = window.open();
            newWin.document.write(window.frames["ifrmForm"].document.documentElement.outerHTML);
            newWin.document.close();
            newWin.focus();

            if ($("#OPINION", ifrmForm.document).length > 0) {
                $('#OPINION', ifrmForm.document).show();
            }

            setTimeout("newWin.print(); newWin.close();", 1000);
        });

        // [닫기]
        $('#cbtnClose').click(function (e) { e.preventDefault(); self.close(); });
        // [이미지 보기] - 첨부파일
        $('#cbtnShowImage').click(function (e) { e.preventDefault(); View.ShowImage(); });
        // [수정내역]
        $('#cbtnHistory').click(function (e) { e.preventDefault(); View.History(); });
        // [결재정보]
        $('#cbtnDocInfo').click(function (e) { e.preventDefault(); View.DocInfo(); });
        //삭제
        $('#cbtnDel').click(function (e) { e.preventDefault(); View.Del(); });

        //강제 Viewing초기화
        $('#cbtnViewingForce').click(function (e) { e.preventDefault(); View.UpdViewing(); });

        // [ZIP선택다운로드]
        $('#cbtnZIP').click(function (e) { e.preventDefault(); View.ZIPDownLoad(); });

        // [PDF일괄다운로드]
        $('#cbtnPDF').click(function (e) { e.preventDefault(); View.PDFDownLoad(); });

        //열람등급
        $('#headerAccessTitleID').click(function (e) { e.preventDefault(); View.AccessTitle(); });

        //열람등급
        $('#AccessTitleID').click(function (e) {
            e.preventDefault();

            ParamArray = {
            };

            CallEncrypt(CallGetParam());
            var url = "/DeskPlusEIP/Approval/Folder/Common/AccessGrade.aspx?p=" + GetEncryptValue;
            PopupWin(600, 400, url, "-500", "", true);

        });

        $('#headerAccessTitleID').text(AccessTitleName);    // 열람권한

        if ($("#htaFormHTML").val() == "") { ChildDocID = -1; }
        View.GetContentHtmlList();
        View.GetAttachFileList(); View.GetAttachFileCnt();
        View.GetBaseDocList(); View.GetBaseDocCnt();
        View.Resource();

        $("#zeoModalContact").draggable({ disabled: false });

        __addEventListener(document, "click", View.click);  //for,ContextMenu (첨부파일,근거문서)
    });
});



/**********************************************************************************************
다국어 처리
**********************************************************************************************/
View.Resource = function () {

    $("#j_fileName")[0].innerHTML = R_FileName;
    $("#j_fileType")[0].innerHTML = R_FileType;
    $("#j_fileLen")[0].innerHTML = R_FileLen;
    $("#j_download")[0].innerHTML = R_FileDown;

    //$("#j_docInfo")[0].innerHTML = R_DocInfo;
    $("#j_docNumber")[0].innerHTML = R_DocNumber;
    $("#j_subject")[0].innerHTML = R_Subject;
    $("#j_viewDoc")[0].innerHTML = R_ViewDoc;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}


/**********************************************************************************************
문서 호출
**********************************************************************************************/
View.GetContentHtmlList = function () {

    if ($("#htaFormHTML").val() == "") {
        var msg = "<div style='font-size:12px;font-weight:bold;color:#FF7E00;padding:20px 0px 0px 0px;text-align:center;'>"
        msg += R_Msg_AlarmError;
        msg += "</div>";
        $("#htaFormHTML").val(msg);
    }

    ifrmForm.document.open();
    ifrmForm.document.write($("#htaFormHTML").val());
    ifrmForm.document.close();

    var strOld = "";
    var zTags = $("input", ifrmForm.document);
    $.each(zTags, function (i, o) {
        if (o.id.indexOf("SUBJ") > 0) {
            strOld = o.id.split("_")[0];
            strOld = strOld + "_";
        }
    });

    var GroupNumber2 = "";
    if ($("#" + strOld + "ZD0", ifrmForm.document).length > 0) {
        var arrBodyChild = $("#" + strOld + "ZD0", ifrmForm.document).find('img');
        var arrFileName = $(arrBodyChild, ifrmForm.document).prop("src").split("/");

        GroupNumber2 = arrFileName[8];
    }

    //이미지 경로 설정
    if ($("#" + strOld + "EDITOR", ifrmForm.document).length > 0) {
        var arrBodyChild = $("#" + strOld + "EDITOR", ifrmForm.document).find('img');

        $.each(arrBodyChild, function (i, o) {
            if ($(o, ifrmForm.document).prop("tagName").toLowerCase() == "img") {
                var arrFileName = $(o, ifrmForm.document).prop("src").split("/");

                var imgpath = "http://intra.di.co.kr/DeskPlusEIP/FileData/1/approval/complete/" + GroupNumber2 + "/" + GroupID + "/editor/";

                $(o, ifrmForm.document).prop("src", imgpath + arrFileName[arrFileName.length - 1]);
            }
        });
    }

    // 문서 수정불가 처리.
    View.SetInActive($('#ifrmForm').get());
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
    $(_targetFrm).contents().find('input').attr('contentEditable', "false");
    $(_targetFrm).contents().find('div').attr('contentEditable', "false");
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
첨부파일 다운로드
**********************************************************************************************/
View.FileDownload = function (filePath, fileName, fileGuid) {
    var attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + filePath;
    window.open(attachFilePath);
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

    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Approval/Jt/Status/Common/JTAttachFile_Old.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate(data.d);
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

    $('#placeholder_baseDoc').setTemplateURL('/DeskPlusEIP/Approval/Jt/Status/Common/JTBaseDoc_Old.htm?seq=' + Guids());
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
[수정내역]
**********************************************************************************************/
View.History = function () {

    ParamArray = {
        'docID': DocumentID,
        'childDocID': ChildDocID,
        'menuName': "DocProcess"
    };
    CallEncrypt(CallGetParam());

    isRtnFunc = false;
    var url = "/DeskPlusEIP/Approval/Folder/Common/HistoryDoc.aspx?p=" + GetEncryptValue;
    //PopupWin(600, 400, url, "-500", "", true);

    var popWidth = 600; //팝업 가로
    var popHeight = 400; //팝업 세로

    // 팝업 화면 중앙에 오도록
    var x = (window.screen.width - popWidth) / 2;
    var y = (window.screen.height - popHeight) / 2;

    WindowObjectReference = window.open(url, "openHistory", "left=" + x + ", top=" + y + ", width=" + popWidth + ", height=" + popHeight + ", menubar=no, location=no, resizable=no, scrollbars=no, status=no");
    WindowObjectReference.focus();
}

/**********************************************************************************************
[결재정보]
**********************************************************************************************/
View.DocInfo = function () {
    ParamArray = {
        'documentID': DocumentID,
        'childDocID': ChildDocID
    };
    CallEncrypt(CallGetParam());

    isRtnFunc = false;
    var url = "/DeskPlusEIP/Approval/Status/Common/DocUserSeq.aspx?p=" + GetEncryptValue;
    //PopupWin(600, 530, url, "-500", "", true);

    var popWidth = 600; //팝업 가로
    var popHeight = 530; //팝업 세로

    // 팝업 화면 중앙에 오도록
    var x = (window.screen.width - popWidth) / 2;
    var y = (window.screen.height - popHeight) / 2;

    WindowObjectReference = window.open(url, "openDocInfo", "left=" + x + ", top=" + y + ", width=" + popWidth + ", height=" + popHeight + ", menubar=no, location=no, resizable=no, scrollbars=no, status=no");
    WindowObjectReference.focus();
}

/**********************************************************************************************
[삭제]
**********************************************************************************************/
View.Del = function () {

    if (window.confirm(R_MSG_Del2)) {

        // 커스텀연동 처리
        if (CustomFilePath != "" && ChildDocID != -1) {
            ifrmProcessing.document.location.href = CustomFilePath + "?seq=" + Guids();
        } else {
            ApproveDel();
        }
    }
}
ApproveDel = function () {

    if (cAccountTag.indexOf("ZP") == -1) {
        if ((cAccountTag.indexOf("ZQ") == -1) && (DocumentID != cGroupID)) {
            //이관 받은 문서 
            var url = "/DeskPlusEIP/Approval/Ws/Admin/Document/ViewDocProc.asmx/DelDocIDProcess";
            ParamArray = {
                'docID': DocumentID
            };

            CallEncrypt(CallGetParam());
            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("View.SuccessApproveDel");

            AjaxService(url, data, obj);

        }
        else {
            var url = "/DeskPlusEIP/Approval/Ws/Admin/Document/ViewDocProc.asmx/DelDocProcess";
            ParamArray = {
                'docID': cGroupID
            };

            CallEncrypt(CallGetParam());
            var data = "{p:'" + GetEncryptValue + "'}";
            var obj = eval("View.SuccessApproveDel");

            AjaxService(url, data, obj);
        }
    }
    else {  // 파란수신이면
        var url = "/DeskPlusEIP/Approval/Ws/Admin/Document/ViewDocProc.asmx/DelDocIDProcess";
        ParamArray = {
            'docID': DocumentID
        };

        CallEncrypt(CallGetParam());
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("View.SuccessApproveDel");

        AjaxService(url, data, obj);
    }
}
View.SuccessApproveDel = function (data, status) {

    if (data.d == true) {
        alert(R_MSG_Del4);
        //opener.location.reload();
        opener.List.LoadData("search");
        self.close();
    }
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
[Viewing초기화]
**********************************************************************************************/
View.UpdViewing = function () {

    if (window.confirm(R_MSG_ViewingForceMsg)) {

        var url = "/DeskPlusEIP/Approval/Ws/Admin/Document/ViewDocProc.asmx/UpdViewing";
        ParamArray = {
            'docID': DocumentID
        };

        CallEncrypt(CallGetParam());
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("View.SuccessUpdViewing");

        AjaxService(url, data, obj);
    }
}
View.SuccessUpdViewing = function (data, status) {
    if (data.d) { alert(R_MSG_SUCCESS); }
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
        if ($.browser.msie) {
            if (attachArr.length > 1) {
                oItem = oContextMenuCtrl.addItem(R_AllDownLoad, View.TotalDownLoad, ImagePath + "/Icon/folders.gif");
                oItem.txt.setAttribute("arg", AttachFileInfoList, 0);
                oContextMenuCtrl.addBlank();
            }
        }
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
