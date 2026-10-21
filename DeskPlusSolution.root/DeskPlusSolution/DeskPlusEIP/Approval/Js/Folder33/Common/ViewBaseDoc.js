ViewBaseDoc = {}

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

        // 결재정보
        $('#cbtnDocInfo').click(function (e) { e.preventDefault(); ViewBaseDoc.DocInfo(); });

        //이미지 보기(첨부파일)
        $('#cbtnShowImage').click(function (e) { e.preventDefault(); ViewBaseDoc.ShowImage(); });

        //회람
        $('#cbtnCir').click(function (e) { e.preventDefault(); ViewBaseDoc.Cir(); });

        //백업
        $('#cbtnZipFile').click(function (e) {

            e.preventDefault();

            ParamArray = {
                'module': 'Approval',
                'listID': DocumentID
            };
            CallEncrypt(CallGetParam());

            $("#ifrm")[0].src = "/DeskPlusEIP/Common/Zip/CreateZipFile.aspx?p=" + GetEncryptValue;
        });

        //메일발송
        $('#cbtnMail').click(function (e) {
            e.preventDefault();

            if (DocumentID != "") {

                ParamArray = { 'documentID': ChildDocID };
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

        if ($("#htaFormHTML").val() == "") { ChildDocID = -1; }
        ViewBaseDoc.GetContentHtmlList();
        ViewBaseDoc.GetAttachFileList(); ViewBaseDoc.GetAttachFileCnt();
        ViewBaseDoc.GetBaseDocList(); ViewBaseDoc.GetBaseDocCnt();
        ViewBaseDoc.Resource();
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
    ParamArray = { 'childDocID': ChildDocID };
    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Folder/Common/DocUserSeq.aspx?p=" + GetEncryptValue;
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
다국어 처리
**********************************************************************************************/
ViewBaseDoc.Resource = function () {

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
ViewBaseDoc.GetContentHtmlList = function () {

    if ($("#htaFormHTML").val() == "") {
        var msg = "<div style='font-size:12px;font-weight:bold;color:#FF7E00;padding:20px 0px 0px 0px;text-align:center;'>"
        msg += R_Msg_AlarmError;
        msg += "</div>";
        $("#htaFormHTML").val(msg);
    }

    //마이그레이션 문서 깨지는 문제 수정
    $("#htaFormHTML").val($("#htaFormHTML").val().replace(/DIV/gi, 'SPAN'));

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

    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Approval/Jt/Folder/Common/JTAttachFilePopUp_Old.htm?seq=' + Guids());
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

    $('#placeholder_baseDoc').setTemplateURL('/DeskPlusEIP/Approval/Jt/Folder/Common/JTBaseDocPopUp_Old.htm?seq=' + Guids());
    $('#placeholder_baseDoc').processTemplate(data.d);
}


/**********************************************************************************************
근거문서 보기
**********************************************************************************************/
ViewBaseDoc.ViewBaseDoc = function (_baseDocumnetID) {

    ParamArray = { 'documentID': _baseDocumnetID };
    CallEncrypt(CallGetParam());

    //var url = "/DeskPlusEIP/Approval/Folder/Common/ViewBaseDoc.aspx";

    var url = "";

    if (_baseDocumnetID < 800000) {
        url = "/DeskPlusEIP/Approval/Folder33/Common/ViewBaseDoc.aspx";

        var param = "p=" + GetEncryptValue;
        WindowObjectReference = PopupWinOpen(url, param, "ViewBaseDoc", "1024", "700");
    }
    else {
        url = "/DeskPlusEIP/Approval/Folder/Common/ViewBaseDoc.aspx";

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