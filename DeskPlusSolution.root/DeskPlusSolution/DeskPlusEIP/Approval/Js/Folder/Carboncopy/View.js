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

        //목록
        $('#cbtnList').click(function (e) { e.preventDefault(); View.List(); });

        //백업
        $('#cbtnZipFile').click(function (e) {

            e.preventDefault();
            if (CirDocumentID != "") {
                ParamArray = {
                    'module': 'Approval',
                    'cirDocID': CirDocumentID
                };
                CallEncrypt(CallGetParam());

                $("#ifrm")[0].src = "/DeskPlusEIP/Common/Zip/CreateZipFile.aspx?p=" + GetEncryptValue;
            } else {
                alert(R_MSG_Backup2);
            }
        });

        //메일발송
        $('#cbtnMail').click(function (e) {
            e.preventDefault();

            if (CirDocumentID != "") {
                ParamArray = { 'cirDocID': CirDocumentID };
                CallEncrypt(CallGetParam());

                var url = "/DeskPlusEIP/Approval/Folder/Carboncopy/SendMail.aspx?p=" + GetEncryptValue;
                PopupWin(800, 480, url, "-500", "", true);
            }
            else {
                alert(R_MSG_Mail3);
            }
        });

        //결재정보 보기
        $('#cbtnDocInfo').click(function (e) {
            e.preventDefault();

            ParamArray = { 'cirDocID': CirDocumentID };
            CallEncrypt(CallGetParam());
            var url = "/DeskPlusEIP/Approval/Folder/Carboncopy/CirDocUser.aspx?p=" + GetEncryptValue;
            PopupWin(700, 480, url, "-500", "", true);
        });

        //의견 보기
        $('#cbtnOpinion').click(function (e) {
            e.preventDefault();

            ParamArray = { 'cirDocID': CirDocumentID };
            CallEncrypt(CallGetParam());
            var url = "/DeskPlusEIP/Approval/Folder/Carboncopy/CirDocUser.aspx?p=" + GetEncryptValue;
            PopupWin(700, 480, url, "-500", "", true);
        });


        //회람의견보기
        $('#cbtnOpinion').click(function (e) {
            e.preventDefault();

            ParamArray = { 'cirDocID': CirDocumentID };
            CallEncrypt(CallGetParam());

            var url = "/DeskPlusEIP/Approval/Folder/Carboncopy/CirOpinion.aspx?p=" + GetEncryptValue;
            PopupWin(600, 205, url, "-500", "", true);
        });

        //결재의견 보기
        $('#cbtnOpinionInfo').click(function (e) { e.preventDefault(); View.OpinionInfo(); });

        // [ZIP선택다운로드]
        $('#cbtnZIP').click(function (e) { e.preventDefault(); View.ZIPDownLoad(); });

        if ($("#htaFormHTML").val() == "") { CirDocumentID = -1; }
        View.GetContentHtmlList();
        View.GetAttachFileList();
        View.GetBaseDocList();
        View.Resource();

        View.OpinionCount(); //결재의견 존재여부

        $("#zeoModalContact").draggable({ disabled: false });

        if ($('#headerAttachCnt').text() != "0") {
            $('#headerAttachCnt').attr("style", "cursor:pointer");
        }

        if ($('#headerBaseDocCnt').text() != "0") {
            $('#headerBaseDocCnt').attr("style", "cursor:pointer");
        }

        __addEventListener(document, "click", View.click);  //for,ContextMenu (첨부파일,근거문서)
    });
});


/**********************************************************************************************
다국어 처리
**********************************************************************************************/
View.Resource = function () {

    if ($("#j_selectall").length > 0) {
        $("#j_selectall").html(SelectAll);
    }
}

/**********************************************************************************************
목록으로 가기 
**********************************************************************************************/
View.List = function () {

    ParamArray = {
        'recordPerPage': RecordPerPage,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'folderStatusID': FolderStatusID,
        'cabinetID': CabinetID,
        'types': Types,
        'formID': FormID,
        'yyyy': YYYY,
        'yyyymm': YYYYMM,
        'headerName': HeaderName
    };

    CallEncrypt(CallGetParam());
    location.href = TargetPage + "?p=" + GetEncryptValue;
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

    if (DocumentID < 800000) {
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

                    var imgpath = "http://wpms.busangas.co.kr/DeskPlusEIP/FileData/1/approval/circulationold/" + GroupNumber2 + "/" + DocumentID + "/editor/";

                    $(o, ifrmForm.document).prop("src", imgpath + arrFileName[arrFileName.length - 1]);
                }
            });
        }
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
이미지 보기 (첨부파일)
**********************************************************************************************/
View.ShowImage = function () {
    ParamArray = { 'cirDocumentID': CirDocumentID };
    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Folder/Carboncopy/GallerySubList.aspx";
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

    ParamArray = { 'cirDocumentID': CirDocumentID };
    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessAttachFile");
    var url = "/DeskPlusEIP/Approval/Ws/Folder/Carboncopy/View.asmx/GetAttachFileList";

    AjaxService(url, data, obj);
}
View.SuccessAttachFile = function (data, status) {

    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Approval/Jt/Folder/Carboncopy/JTAttachFile.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate(data.d);

    if (data.d.length == 0) {
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
View.GetBaseDocList = function () {

    ParamArray = { 'cirDocumentID': CirDocumentID };
    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessBaseDoc");
    var url = "/DeskPlusEIP/Approval/Ws/Folder/Carboncopy/View.asmx/GetBaseDocList";

    AjaxService(url, data, obj);
}
View.SuccessBaseDoc = function (data, status) {

    $('#placeholder_baseDoc').setTemplateURL('/DeskPlusEIP/Approval/Jt/Folder/Carboncopy/JTBaseDoc.htm?seq=' + Guids());
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
    View.GetAttachFileListMenu(CirDocumentID);

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
View.GetAttachFileListMenu = function (_cirDocumentID) {

    var url = "/DeskPlusEIP/Approval/Ws/Folder/Carboncopy/View.asmx/GetAttachFileListMenu";
    var data = "cirDocumentID=" + _cirDocumentID;
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
    View.GetBaseDocListMenu(CirDocumentID);

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
View.GetBaseDocListMenu = function (_cirDocumentID) {

    var url = "/DeskPlusEIP/Approval/Ws/Folder/Carboncopy/View.asmx/GetBaseDocListMenu";
    var data = "cirDocumentID=" + _cirDocumentID;
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

/**********************************************************************************************
ZIP선택다운로드
**********************************************************************************************/
View.ZIPDownLoad = function () {

    if (CirDocumentID != "") {

        var fileIDList = "";
        fileIDList = View.CheckedValue();

        if (fileIDList != "") {
            ParamArray = {
                'module': 'APPROVALCIR'
                , 'listID': CirDocumentID
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
        'childDocID': LastDocID
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
        'childDocID': LastDocID
    };
    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Common/Popup/OpinionView.aspx?p=" + GetEncryptValue;
    PopupWin(600, 540, url, "-500", "", true);
}