Mod = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //flash 첨부파일 추가
        $('#cbtnSwfFileAdd').click(function (e) {
            e.preventDefault();
            Mod.AttachSwfFile();
        });

        //추가
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            Mod.Save();
        });

        // 전자결재 문서추가
        $('#cbtnDocAdd').click(function (e) {
            e.preventDefault();
            Mod.DocAdd();
        });

        // 업무협업 근거문서 추가
        $('#cbtnCoopAdd').click(function (e) {
            e.preventDefault();
            Mod.CoopAdd();
        });

        //리스트
        $('#cbtnList').click(function (e) {
            e.preventDefault();

            ParamArray = {
                'listCount': ListCount,
                'currentPage': CurrentPage,
                'searchStatus': SearchStatus,
                'searchField': SearchField,
                'searchValue': SearchValue,
                'searchDateValue': SearchDateValue,
                'order': Order,
                'itemID': ItemID,
                'itemSubID': ItemSubID
            };

            CallEncrypt(CallGetParam());
            location = "List.aspx?p=" + GetEncryptValue;
        });


        if (NonActiveX == "T") {
            xfe.setHtmlValue($("#tbHiddenMime_contents").val());
            $('#ActiveX').click(function (e) {
                e.preventDefault();
                if (confirm(ConfirmEditorTab)) {
                    location.href = "ModReply.aspx?" + ClientQueryString;
                }
            });
        } else {
            $('#NonActiveX').click(function (e) {
                e.preventDefault();
                if (confirm(ConfirmEditorTab)) {
                    location.href = "ModReplyN.aspx?" + ClientQueryString;
                }
            });
        }

        Mod.GetSpoolFileList();

        __addEventListener(document, "click", Mod.click);

        Mod.Init();
    });
});

/**************************************************
작성자   : 이현수
작성일자 : 2012-04-12
작성목적 : 클릭 이벤트 분기 (컨텍스트 메뉴 변경)
***************************************************/
Mod.click = function (e) {
    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "del": Add.ShowDelContextMenu(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}

var oContextMenuCtrl = null;

/**************************************************
작성자   : 이현수
작성일자 : 2008-08-29
작성목적 : 컨텍스트 메뉴 생성
***************************************************/
Mod.ShowDelContextMenu = function (e) {
    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;

    var gubun = el.getAttribute("type"); //구분자(부서:G, 사용자 M)
    var oid = el.getAttribute("oid"); //코드값
    var objID = el.getAttribute("objID"); //코드값

    oContextMenuCtrl = new DeskPlusEIP.Component.ContextMenuCtrl();
    oContextMenuCtrl.init();

    oContextMenuCtrl.addNameItem(R_Del, "Mod.DelItem('" + gubun + "_" + oid + "_" + objID + "')", ImagePath + "/Icon/ContextMenu/del.png");
    oContextMenuCtrl.show(e);
}

/**************************************************
작성자   : 이현수
작성일자 : 2012-04-12
작성목적 : 부서/사용자 삭제
***************************************************/
Mod.DelItem = function (id) {
    $("#" + id).remove();
    /*
    var oEl = document.getElementById(id);
    if (oEl) {
    oEl.removeNode(true);
    }
    */
}

/**********************************************************************************************
초기화
**********************************************************************************************/
Mod.Init = function () {
    //수정모드일경우
    if (ListID != "0") {
        Mod.GetAttachFileList();
        Mod.GetBaseList();
        Mod.GetBaseCoopList();
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-20
작성목적 : 파일  리스트 호출
**********************************************************************************************/
Mod.GetAttachFileList = function () {
    ParamArray = {
        'listID': ListID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Mod.SuccessAttachFile");
    var url = "/DeskPlusEIP/Coop/Ws/Content/Request/ModReply.asmx/GetAttachFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 첨부파일 리스트 보여주기
**********************************************************************************************/
Mod.SuccessAttachFile = function (data, status) {
    $('#placeholder_fileList').setTemplateURL('/DeskPlusEIP/Coop/Jt/Content/Request/JTDelFile.htm?seq=' + Guids());
    $('#placeholder_fileList').processTemplate(data.d);

    setTimeout(function () {
        Mod.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-20
작성목적 : 근거문서  리스트 호출
**********************************************************************************************/
Mod.GetBaseList = function () {
    ParamArray = {
        'listID': ListID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Mod.SuccessGetBaseList");
    var url = "/DeskPlusEIP/Coop/Ws/Content/Request/ModReply.asmx/GetBaseList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 근거문서 리스트 보여주기
**********************************************************************************************/
Mod.SuccessGetBaseList = function (data, status) {
    $('#placeholder_basedocList').setTemplateURL('/DeskPlusEIP/Coop/Jt/Content/Request/JTDelListBase.htm?seq=' + Guids());
    $('#placeholder_basedocList').processTemplate(data.d);

    setTimeout(function () {
        Mod.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 게시판 실제 근거문서 삭제 처리
**********************************************************************************************/
function DelBase(p_listID, p_documentID) {
    if (window.confirm(R_ApplyDelete)) {
        ParamArray = {
            'listID': p_listID,
            'documentID': p_documentID
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Mod.SuccessBaseList");
        var url = "/DeskPlusEIP/Coop/Ws/Content/Request/ModReply.asmx/DelBaseList";

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 근거문서 리스트 보여주기
**********************************************************************************************/
Mod.SuccessBaseList = function (data, status) {
    if (data.d == true) {
        Mod.GetBaseList();
    }
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-20
작성목적 : 협업 근거문서  리스트 호출
**********************************************************************************************/
Mod.GetBaseCoopList = function () {
    ParamArray = {
        'listID': ListID,
        'groupID': GroupID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Mod.SuccessGetBaseCoopList");
    var url = "/DeskPlusEIP/Coop/Ws/Content/Request/Mod.asmx/GetBaseCoopList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 협업 근거문서 리스트 보여주기
**********************************************************************************************/
Mod.SuccessGetBaseCoopList = function (data, status) {
    $('#placeholder_baseCoopList').setTemplateURL('/DeskPlusEIP/Coop/Jt/Content/Request/JTDelListBaseCoop.htm?seq=' + Guids());
    $('#placeholder_baseCoopList').processTemplate(data.d);

    setTimeout(function () {
        Mod.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 게시판 실제 근거문서 삭제 처리
**********************************************************************************************/
function DelBaseCoop(p_listID, p_groupID, p_baseCoopID) {
    if (window.confirm(R_ApplyDelete)) {
        ParamArray = {
            'listID': p_listID,
            'groupID': p_groupID,
            'baseCoopID': p_baseCoopID
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Mod.SuccessBaseCoopList");
        var url = "/DeskPlusEIP/Coop/Ws/Content/Request/Mod.asmx/DelBaseCoopList";

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 근거문서 리스트 보여주기
**********************************************************************************************/
Mod.SuccessBaseCoopList = function (data, status) {
    if (data.d == true) {
        Mod.GetBaseCoopList();
    }
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 게시판 실제 파일 삭제 처리
**********************************************************************************************/
function DelFile(p_attachFileID) {
    if (window.confirm(R_ApplyDelete)) {
        ParamArray = {
            'listID': ListID,
            'attachID': p_attachFileID
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Mod.SuccessFileList");
        var url = "/DeskPlusEIP/Coop/Ws/Content/Request/ModReply.asmx/DelAttachFile";

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 첨부파일 리스트 보여주기
**********************************************************************************************/
Mod.SuccessFileList = function (data, status) {
    if (data.d == true) {
        Mod.GetAttachFileList();
    }
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : flash 첨부파일 추가
***************************************************/
Mod.AttachSwfFile = function () {
    ParamArray = {
        'swfUploadGUID': SwfUploadGUID,
        'fileType': FileType,
        'fileTypeDesc': FileTypeDesc,
        'moduleName': 'COOP',
        'callBackEvent': "Mod.GetSpoolFileList"
    };

    CallEncrypt(CallGetParam());
    //    var url = "/DeskPlusEIP/Upload/DPUpload/Main.aspx?p=" + GetEncryptValue;
    //    PopupWin(680, 410, url, "", "", true);

    //    var url;
    //    if ($.browser.msie) {
    //        url = "/DeskPlusEIP/Upload/DPUpload/Main.aspx";
    //    } else {
    //        url = "/DeskPlusEIP/Upload/SilverUpload/Main.aspx";
    //    }

    var url = GetAttachPathURL();
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "DPUpload", "680", "510")
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-20
작성목적 : SPOOL 파일  리스트 호출
**********************************************************************************************/
Mod.GetSpoolFileList = function () {
    ParamArray = {
        'swfUploadGUID': SwfUploadGUID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Mod.SuccessSpoolFileList");
    var url = "/DeskPlusEIP/Common/Ws/SpoolFile.asmx/GetSpoolFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : SPOOL 첨부파일 리스트 보여주기
**********************************************************************************************/
Mod.SuccessSpoolFileList = function (data, status) {
    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Common/Jt/Attach/JTSpoolFile.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate(data.d);

    setTimeout(function () {
        Mod.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-14
작성목적 : 근거문서 추가
**********************************************************************************************/
Mod.DocAdd = function () {
    ParamArray = {
        'folderName': "dept",
        'baseDocIDs': BaseDocIDs
    };
    CallEncrypt(CallGetParam());

    var popWidth = 1000; //팝업 가로
    var popHeight = 600; //팝업 세로

    // 팝업 화면 중앙에 오도록
    var x = (window.screen.width - popWidth) / 2;
    var y = (window.screen.height - popHeight) / 2;

    var url = "/DeskPlusEIP/Approval/FormFlow/Basic/GetBaseDoc.aspx?p=" + GetEncryptValue;
    WindowObjectReference = window.open(url, "AttachBaseDoc", "left=" + x + ", top=" + y + ", width=" + popWidth + ", height=" + popHeight + ", menubar=no, location=yes, resizable=yes, scrollbars=yes, status=no");
    WindowObjectReference.focus();
}

/**********************************************************************************************
업무협업 근거문서 추가
**********************************************************************************************/
Mod.CoopAdd = function () {
    ParamArray = {
        'folderName': "dept",
        'coopBaseIDs': CoopBaseIDs
    };
    CallEncrypt(CallGetParam());

    var popWidth = 1000; //팝업 가로
    var popHeight = 600; //팝업 세로

    // 팝업 화면 중앙에 오도록
    var x = (window.screen.width - popWidth) / 2;
    var y = (window.screen.height - popHeight) / 2;

    var url = "/DeskPlusEIP/Coop/Content/Request/GetListBase.aspx?p=" + GetEncryptValue;
    WindowObjectReference = window.open(url, "AttachCoopBaseDoc", "left=" + x + ", top=" + y + ", width=" + popWidth + ", height=" + popHeight + ", menubar=no, location=yes, resizable=yes, scrollbars=yes, status=no");
    WindowObjectReference.focus();
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 다국어 처리
**********************************************************************************************/
Mod.Resource = function () {
    $("#j_spool_fileName")[0].innerHTML = R_FileName;
    $("#j_spool_fileType")[0].innerHTML = R_FileType;
    $("#j_spool_fileLen")[0].innerHTML = R_FileLen;
    $("#j_spool_filedelete")[0].innerHTML = R_FileDel;

    $("#j_fileName")[0].innerHTML = R_FileName;
    $("#j_fileType")[0].innerHTML = R_FileType;
    $("#j_fileLen")[0].innerHTML = R_FileLen;
    $("#j_filedelete")[0].innerHTML = R_FileDel;

    $("#j_basetitle")[0].innerHTML = R_Title;
    $("#j_basedel")[0].innerHTML = R_Del;

    $("#j_baseCooptitle")[0].innerHTML = R_Title;
    $("#j_baseCoopdel")[0].innerHTML = R_Del;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoAttachData); }
    if ($(".NoBaseListData") != undefined) { $(".NoBaseListData").text(R_NoContent); }
    if ($(".NoBaseCoopListData") != undefined) { $(".NoBaseCoopListData").text(R_NoContent); }
    if ($(".BtnView") != undefined) { $(".BtnView").attr('src', ImagePath + '/Icon/icon_search.gif'); }
    if ($(".BtnUp") != undefined) { $(".BtnUp").attr('src', ImagePath + '/Btn/sort_down.gif'); }
    if ($(".BtnDown") != undefined) { $(".BtnDown").attr('src', ImagePath + '/Btn/sort_on.gif'); }
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 임시파일 삭제 처리
**********************************************************************************************/
function DelSpoolPath(fileName) {
    if (window.confirm(R_ApplyDelete)) {
        ParamArray = {
            'fileName': fileName,
            'swfUploadGUID': SwfUploadGUID
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Mod.SuccessSpoolFileList");
        var url = "/DeskPlusEIP/Common/Ws/SetCommon.asmx/DelSpoolFileList";

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
[저장]
**********************************************************************************************/
Mod.Save = function () {
    //    var validator = new Validator($("#form1"));
    //    validator.checkRequired("tbxTitle", R_InputTitle, true);
    //    if (!validator.validate()) return;

    //전자결재 근거문서
    $("#tbBaseDocIDs").val(BaseDocIDs);

    //업무협업 근거문서
    $("#tbBaseCoopIDs").val(CoopBaseIDs);

    if (NonActiveX == "T") {
        $("#tbHiddenHTML_contents").val(xfe.getHtmlValue());
        $("#tbHiddenMime_contents").val("");
        $("#tbHiddenText_contents").val(xfe.getTextValue());
    } else {
        if (!EditorSecurity(form1.twe)) return;

        var strHtml = form1.twe.HtmlValue;
        if (strHtml.indexOf(".js") > -1 || strHtml.indexOf('=\"xss:expression') > -1) {
            strHtml = strHtml.replace(".js\"", "\"");
            strHtml = strHtml.replace('=\"xss:expression', "");

            form1.twe.HtmlValue = strHtml;

            alert(R_DoubtStcipt);
        }

        $("#tbHiddenHTML_contents").val(form1.twe.HtmlValue);
        $("#tbHiddenMime_contents").val(form1.twe.MimeEnValue);
        $("#tbHiddenText_contents").val(form1.twe.TextValue);
    }

    var contents = $("#tbHiddenText_contents").val();
    var contentHtml = $("#tbHiddenHTML_contents").val();
    if (contentHtml.indexOf("<IMG") > -1) {
        contents = "<img"; //dummy (if you save only image in editor)
    }

    if (contents == "") {
        alert(R_NoContent);
        return;
    }

    //2번 이상 클릭 불가
    $("#cbtnAdd").attr('onclick', '').unbind('click');

    __doPostBack('lbtnAdd', '');
}