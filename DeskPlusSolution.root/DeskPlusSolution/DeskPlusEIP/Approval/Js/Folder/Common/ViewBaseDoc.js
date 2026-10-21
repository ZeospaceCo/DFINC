ViewBaseDoc = {}

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

        // 결재정보
        $('#cbtnDocInfo').click(function (e) { e.preventDefault(); ViewBaseDoc.DocInfo(); });

        //이미지 보기(첨부파일)
        $('#cbtnShowImage').click(function (e) { e.preventDefault(); ViewBaseDoc.ShowImage(); });

        //회람
        $('#cbtnCir').click(function (e) { e.preventDefault(); ViewBaseDoc.Cir(); });

        //백업
        $('#cbtnZipFile').click(function (e) {

            e.preventDefault();

            if (DocState == "C") {
                ParamArray = {
                    'module': 'Approval',
                    'listID': DocumentID
                };
            }
            else {
                ParamArray = {
                    'module': 'APPROVALPROCESS',
                    'listID': DocumentID
                };
            }

            CallEncrypt(CallGetParam());

            $("#ifrm")[0].src = "/DeskPlusEIP/Common/Zip/CreateZipFile.aspx?p=" + GetEncryptValue;
        });

        //메일발송
        $('#cbtnMail').click(function (e) {
            e.preventDefault();

            if (DocumentID != "") {

                if (DocState == "C") {
                    ParamArray = { 'documentID': ChildDocID };
                }
                else {
                    ParamArray = {
                        'documentID': ChildDocID,
                        'isComplete': '0'
                    };
                }

                CallEncrypt(CallGetParam());

                var url = "/DeskPlusEIP/Approval/Folder/Dept/SendMail.aspx?p=" + GetEncryptValue;
                PopupWin(800, 500, url, "-500", "", true);
            }
            else {
                alert(R_Msg_Mail);
            }
        });

        // [ZIP선택다운로드]
        $('#cbtnZIP').click(function (e) { e.preventDefault(); ViewBaseDoc.ZIPDownLoad(); });

        //결재의견 보기
        $('#cbtnOpinionInfo').click(function (e) { e.preventDefault(); ViewBaseDoc.OpinionInfo(); });

        if ($("#htaFormHTML").val() == "") { ChildDocID = -1; }
        ViewBaseDoc.GetContentHtmlList();
        ViewBaseDoc.GetAttachFileList(); ViewBaseDoc.GetAttachFileCnt();
        ViewBaseDoc.GetBaseDocList(); ViewBaseDoc.GetBaseDocCnt();
        ViewBaseDoc.Resource();

        ViewBaseDoc.OpinionCount(); //결재의견 존재여부

        $("#zeoModalContact").draggable({ disabled: false });

        __addEventListener(document, "click", ViewBaseDoc.click);  //for,ContextMenu (첨부파일,근거문서)

    });
});

/**************************************************
인쇄
***************************************************/
ViewBaseDoc.printElem = function (options) {
    //$('#toPrint').printElement(options);

    var divToPrint = document.getElementById('ifrmForm');
    var html = divToPrint.contentWindow.document.body.innerHTML;

    newWin = window.open();
    newWin.document.write(html);
    newWin.location.reload();
    newWin.focus();
    newWin.print();
    newWin.close();
}

/**********************************************************************************************
[결재정보]
**********************************************************************************************/
ViewBaseDoc.DocInfo = function () {
    ParamArray = {
        'childDocID': ChildDocID,
        'documentID': DocumentID
    };
    CallEncrypt(CallGetParam());

    var url = "";

    if (DocState == "C") {
        url = "/DeskPlusEIP/Approval/Folder/Common/DocUserSeq.aspx?p=" + GetEncryptValue;
    }
    else {
        url = "/DeskPlusEIP/Approval/Status/Common/DocUserSeq.aspx?p=" + GetEncryptValue;
    }

    PopupWin(600, 530, url, "-500", "", true);
}

/**********************************************************************************************
다국어 처리
**********************************************************************************************/
ViewBaseDoc.Resource = function () {

    if ($("#j_selectall").length > 0) {
        $("#j_selectall").html(SelectAll);
    }
}


/**********************************************************************************************
문서 호출
**********************************************************************************************/
ViewBaseDoc.GetContentHtmlList = function () {

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
    ViewBaseDoc.SetInActive($('#ifrmForm').get());
}

// 수정불가 처리
ViewBaseDoc.SetInActive = function (_targetFrm) {  //alert(_targetFrm);
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
ViewBaseDoc.ShowImage = function () {
    ParamArray = {
        'childDocID': ChildDocID,
        'menuName': "Doc"
    };
    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Status/Common/GallerySubList.aspx";
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "ShowImage1", "800", "600")
}

/**********************************************************************************************
첨부파일 다운로드
**********************************************************************************************/
ViewBaseDoc.FileDownload = function (filePath, fileName, fileGuid) {
    var attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + filePath;
    window.open(attachFilePath);
}

/**********************************************************************************************
첨부 파일  리스트 호출
**********************************************************************************************/
ViewBaseDoc.GetAttachFileList = function () {

    ParamArray = { 'childDocID': ChildDocID };
    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ViewBaseDoc.SuccessAttachFile");
    var url = "/DeskPlusEIP/Approval/Ws/Folder/Common/View.asmx/GetAttachFileList";

    AjaxService(url, data, obj);
}
ViewBaseDoc.SuccessAttachFile = function (data, status) {

    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Approval/Jt/Folder/Common/JTAttachFilePopUp.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate(data.d);

}

/**********************************************************************************************
첨부 파일 카운트 호출
**********************************************************************************************/
ViewBaseDoc.GetAttachFileCnt = function () {

    ParamArray = { 'childDocID': ChildDocID };
    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ViewBaseDoc.SuccessAttachFileCnt");
    var url = "/DeskPlusEIP/Approval/Ws/Status/Common/View.asmx/GetAttachFileCnt";

    AjaxService(url, data, obj);
}
ViewBaseDoc.SuccessAttachFileCnt = function (data, status) {
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
ViewBaseDoc.GetBaseDocList = function () {

    ParamArray = { 'childDocID': ChildDocID };
    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ViewBaseDoc.SuccessBaseDoc");
    var url = "/DeskPlusEIP/Approval/Ws/Folder/Common/View.asmx/GetBaseDocList";

    AjaxService(url, data, obj);
}
ViewBaseDoc.SuccessBaseDoc = function (data, status) {

    $('#placeholder_baseDoc').setTemplateURL('/DeskPlusEIP/Approval/Jt/Folder/Common/JTBaseDocPopUp.htm?seq=' + Guids());
    $('#placeholder_baseDoc').processTemplate(data.d);
}


/**********************************************************************************************
근거문서 보기
**********************************************************************************************/
ViewBaseDoc.ViewBaseDoc = function (_baseDocumnetID) {

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
ViewBaseDoc.GetBaseDocCnt = function () {

    ParamArray = { 'childDocID': ChildDocID };
    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ViewBaseDoc.SuccessBaseDocCnt");
    var url = "/DeskPlusEIP/Approval/Ws/Status/Common/View.asmx/GetBaseDocCnt";

    AjaxService(url, data, obj);
}
ViewBaseDoc.SuccessBaseDocCnt = function (data, status) {
    $('#headerBaseDocCnt').text(data.d);
    if (data.d > 0) { $('#headerBaseDocCnt').attr("style", "cursor:pointer"); }
}

/**********************************************************************************************
회람하기
**********************************************************************************************/
ViewBaseDoc.Cir = function () {

    if (DocumentID != "") {

        ParamArray = {
            'childDocID': ChildDocID,
            'menuName': "Doc"
        };

        CallEncrypt(CallGetParam());

        var url = "/DeskPlusEIP/Approval/Folder/Dept/CirMemo.aspx?p=" + GetEncryptValue;
        PopupWin(800, 500, url, "-500", "", true);
    }
    else {
        alert(R_MSG_Cir);
    }
}



var oContextMenuCtrl = null;
var AttachFileInfoList = null;  //첨부파일 리스트
var BaseDocInfoList = null;     //근거문서 리스트
/**************************************************
클릭 이벤트 분기
***************************************************/
ViewBaseDoc.click = function (e) {

    var evt = new __event(e);
    var el = evt.target;
    var action = el.getAttribute("action");

    switch (action) {
        case "attach": ViewBaseDoc.ShowCtxMenuAttach(e); break;    //첨부파일 건수클릭
        case "basedoc": ViewBaseDoc.ShowCtxMenuBase(e); break;     //근거문서 건수클릭
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}

/**********************************************************************************************
첨부파일 건수 클릭 시 : headerAttachCnt
**********************************************************************************************/
ViewBaseDoc.ShowCtxMenuAttach = function (e) {

    if ($("#headerAttachCnt").text() == "0") return;

    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;

    // 첨부파일 리스트 가져오기
    ViewBaseDoc.GetAttachFileListMenu(ChildDocID);

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
        oItem = oContextMenuCtrl.addItem(R_AllDownLoad, ViewBaseDoc.TotalDownLoad, ImagePath + "/Icon/folders.gif");
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

            var oItem = oContextMenuCtrl.addItem(attachFileName, ViewBaseDoc.DownLoad, "EXT");
            oItem.txt.setAttribute("arg", attachFilePath, 0);
        }
        oContextMenuCtrl.show(e, null, e.clientX, e.clientY);
    }

}
/**********************************************************************************************
첨부파일 리스트 조회(웹서비스)
**********************************************************************************************/
ViewBaseDoc.GetAttachFileListMenu = function (p_childDocID) {

    var url = "/DeskPlusEIP/Approval/Ws/Folder/Common/View.asmx/GetAttachFileListMenu";
    var data = "childDocID=" + p_childDocID;
    var obj = eval("ViewBaseDoc.SuccessAttFileListMenu");
    CallEncrypt(data);
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}
ViewBaseDoc.SuccessAttFileListMenu = function (data, status) {
    AttachFileInfoList = data.d;
}
/**********************************************************************************************
첨부파일 열기
**********************************************************************************************/
ViewBaseDoc.DownLoad = function (_e) {
    var evt = new __event(_e);
    if (evt.target) {
        var arg = evt.target.getAttribute("arg");
        window.open(arg);
    }
}
/**********************************************************************************************
첨부파일 전체 다운로드
**********************************************************************************************/
ViewBaseDoc.TotalDownLoad = function (_e) {

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
ViewBaseDoc.ShowCtxMenuBase = function (e) {

    if ($("#headerBaseDocCnt").text() == "0") return;

    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;

    // 근거문서 리스트 가져오기
    ViewBaseDoc.GetBaseDocListMenu(ChildDocID);

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

            var oItem = oContextMenuCtrl.addItem(objnm, ViewBaseDoc.BaseDocLoad, "EXT");
            oItem.txt.setAttribute("arg", objid, 0);
        }

        oContextMenuCtrl.show(e, null, e.clientX, e.clientY);
    }
}
/**********************************************************************************************
근거문서 리스트 조회(웹서비스)
**********************************************************************************************/
ViewBaseDoc.GetBaseDocListMenu = function (p_childDocID) {

    var url = "/DeskPlusEIP/Approval/Ws/Folder/Common/View.asmx/GetBaseDocListMenu";
    var data = "childDocID=" + p_childDocID;
    var obj = eval("ViewBaseDoc.SuccessGetBaseDocListMenu");
    CallEncrypt(data);
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}
ViewBaseDoc.SuccessGetBaseDocListMenu = function (data, status) {
    BaseDocInfoList = data.d;
}
/**********************************************************************************************
근거문서보기
**********************************************************************************************/
ViewBaseDoc.BaseDocLoad = function (_e) {
    var evt = new __event(_e);
    if (evt.target) {
        var arg = evt.target.getAttribute("arg");
        ViewBaseDoc.ViewBaseDoc(arg);  //근거문서보기
    }
}

/**********************************************************************************************
ZIP선택다운로드
**********************************************************************************************/
ViewBaseDoc.ZIPDownLoad = function () {

    if (DocumentID != "") {

        var fileIDList = "";
        fileIDList = ViewBaseDoc.CheckedValue();

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
ViewBaseDoc.CheckedValue = function () {
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
ViewBaseDoc.checkedAllValue = function () {
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
ViewBaseDoc.OpinionCount = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Common/Popup/OpinionView.asmx/GetOpinionCount";

    ParamArray = {
        'companyID': CompanyID,
        'childDocID': ChildDocID
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ViewBaseDoc.SuccessOpinionCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2022-01-05
작성목적 : 결재의견 존재 여부 이벤트
**********************************************************************************************/
ViewBaseDoc.SuccessOpinionCount = function (data, status) {

    var count = parseInt(data.d || 0, 10);

    if (count > 0) {
        ViewBaseDoc.OpinionInfo();
    }

    var baseText = $("#cbtnOpinionInfo").text();

    $("#cbtnOpinionInfo").text(baseText + " (" + count + "건)");
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2022-01-05
작성목적 : 결재의견 조회
**********************************************************************************************/
ViewBaseDoc.OpinionInfo = function () {
    ParamArray = {
        'companyID': CompanyID,
        'childDocID': ChildDocID
    };
    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Common/Popup/OpinionView.aspx?p=" + GetEncryptValue;
    PopupWin(600, 540, url, "-500", "", true);
}