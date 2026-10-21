ModList = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        // 첨부파일 추가
        $('#cbtnSwfFileAdd').click(function (e) {
            e.preventDefault();
            ModList.AttachSwfFile();
        });

        //수정
        $('#cbtnMod').click(function (e) {
            e.preventDefault();
            ModList.Save();
        });

        // 전자결재 근거문서 추가
        $('#cbtnAddApp').click(function (e) {
            e.preventDefault();
            ModList.AddApp();
        });

        // 업무협업 근거문서 추가
        $('#cbtnAddTask').click(function (e) {
            e.preventDefault();
            ModList.AddTask();
        });

        // 취소하기
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();

            ParamArray = {
                'currentPage': CurrentPage,
                'searchStatus': SearchStatus,
                'searchField': SearchField,
                'searchValue': SearchValue,
                'searchDateValue': SearchDateValue,
                'listID': GroupID,
                'groupID': GroupID,
                'itemID': ItemID,
                'itemSubID': ItemSubID,
                'impoUserYN': ImpoUserYN,
                'isGubun': IsGubun,
                'isPage': IsPage,
                'recentPageNo': RecentPageNo,
                'processPageNo': ProcessPageNo,
                'delayPageNo': DelayPageNo,                
                'comPageNo': ComPageNo,
                'ccPageNo': CCPageNo,
                'writeCommentPageNo': WriteCommentPageNo,
                'impoPageNo': ImpoPageNo,
                'year': Year
            };

            CallEncrypt(CallGetParam());

            if (IsPage == "") {
                if (IsType == "RE") {
                    location = "/DeskPlusEIP/Task/Content/Request/ViewN.aspx?p=" + GetEncryptValue;
                }
                else if (IsType == "TO") {
                    location = "/DeskPlusEIP/Task/Content/Receive/ViewN.aspx?p=" + GetEncryptValue;
                }
                else if (IsType == "CC") {
                    location = "/DeskPlusEIP/Task/Content/Reference/ViewN.aspx?p=" + GetEncryptValue;
                }
                else if (IsType == "FO") {
                    location = "/DeskPlusEIP/Task/Content/Forward/ViewN.aspx?p=" + GetEncryptValue;
                }
                else {
                    location = "/DeskPlusEIP/Task/Content/Total/ViewN.aspx?p=" + GetEncryptValue;
                }
            }
            else {
                location = "/DeskPlusEIP/Task/Home/ViewN.aspx?p=" + GetEncryptValue;
            }

        });

        $("#tbxCompleteRequestDate").datepick($.datepicker.regional[PreLang]);
        $('#tbxCompleteRequestDate').focus(function (e) {
            $('#tdDate').css('height', '220px');
        });

        setTimeout(function () {

            xfe.setHtmlValue($("#tbHiddenMime_contents").val());
            /*
            if (NonActiveX == "T") {
                xfe.setHtmlValue($("#tbHiddenMime_contents").val());
                var agent = navigator.userAgent.toLowerCase();
                if ((navigator.appName == 'Netscape' && navigator.userAgent.search('Trident') != -1) || (agent.indexOf("msie") != -1)) {

                }
                else {
                    $("#ActiveX").hide();
                }
                $('#ActiveX').click(function (e) {
                    e.preventDefault();
                    if (confirm(ConfirmEditorTab)) {
                        location.href = "ModList.aspx?" + ClientQueryString;
                    }
                });
            } else {
                $('#NonActiveX').click(function (e) {
                    e.preventDefault();
                    if (confirm(ConfirmEditorTab)) {
                        location.href = "ModListN.aspx?" + ClientQueryString;
                    }
                });
            }
            */

        }, 1000);

        __addEventListener(document, "click", ModList.click);

        ModList.Init();
    });
});

/**********************************************************************************************
초기화
**********************************************************************************************/
ModList.Init = function () {

    ModList.SetSpoolFile();
    ModList.GetAttachList();
    ModList.GetBaseAppList();
    //ModList.GetBaseTaskList();

}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-20
작성목적 : 다국어 처리
**********************************************************************************************/
ModList.SetSpoolFile = function () {

    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Common/Jt/Attach/JTSpoolFile.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate("");
}


/**************************************************
작성자   : famo23
작성일자 : 2015-04-20
작성목적 : 첨부파일 추가
***************************************************/
ModList.AttachSwfFile = function () {
    ParamArray = {
        'swfUploadGUID': SwfUploadGUID,
        'fileType': FileType,
        'fileTypeDesc': FileTypeDesc,
        'moduleName': 'TASK',
        'callBackEvent': "ModList.GetSpoolFileList"
    };

    CallEncrypt(CallGetParam());

    var url = GetAttachPathURL();
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "DPUpload", "680", "510")
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-20
작성목적 : SPOOL 파일  리스트 호출
**********************************************************************************************/
ModList.GetSpoolFileList = function () {
    ParamArray = {
        'swfUploadGUID': SwfUploadGUID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ModList.SuccessSpoolFileList");
    var url = "/DeskPlusEIP/Common/Ws/SpoolFile.asmx/GetSpoolFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-20
작성목적 : SPOOL 첨부파일 리스트 보여주기
**********************************************************************************************/
ModList.SuccessSpoolFileList = function (data, status) {
    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Common/Jt/Attach/JTSpoolFile.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate(data.d);

    /*
    setTimeout(function () {
        ModList.Resource();
    }, 100);
    */
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-20
작성목적 : 다국어 처리
**********************************************************************************************/
ModList.Resource = function () {
    /*
    $("#j_spool_fileName")[0].innerHTML = R_FileName;
    $("#j_spool_fileType")[0].innerHTML = R_FileType;
    $("#j_spool_fileLen")[0].innerHTML = R_FileLen;
    $("#j_spool_filedelete")[0].innerHTML = R_FileDel;

    $("#j_fileName")[0].innerHTML = R_FileName;
    $("#j_fileType")[0].innerHTML = R_FileType;
    $("#j_fileLen")[0].innerHTML = R_FileLen;
    $("#j_filedelete")[0].innerHTML = R_FileDel;

    $("#j_basetitle")[0].innerHTML = R_DocNumber;
    $("#j_basedel")[0].innerHTML = R_Del;

    //$("#j_baseTasktitle")[0].innerHTML = R_DocNumber;
    //$("#j_baseTaskdel")[0].innerHTML = R_Del;    

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoAttachData); }
    if ($(".NoBaseAppListData") != undefined) { $(".NoBaseAppListData").text(R_NoContent); }
    if ($(".NoBaseTaskListData") != undefined) { $(".NoBaseTaskListData").text(R_NoContent); }
    if ($(".BtnView") != undefined) { $(".BtnView").attr('src', ImagePath + '/Icon/icon_search.gif'); }
    if ($(".BtnViews") != undefined) { $(".BtnViews").attr('src', ImagePath + '/Icon/doc_view.gif'); }
    */

    if ($(".BtnUp") != undefined) { $(".BtnUp").attr('src', ImagePath + '/Btn/sort_down.gif'); }
    if ($(".BtnDown") != undefined) { $(".BtnDown").attr('src', ImagePath + '/Btn/sort_on.gif'); }

    if ($(".fa-times-circle").length > 0) {
        $(".fa-times-circle").attr("title", R_FileDel);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-20
작성목적 : 파일  리스트 호출
**********************************************************************************************/
ModList.GetAttachList = function () {
    ParamArray = {
        'listID': ListID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ModList.SuccessGetAttachList");
    var url = "/DeskPlusEIP/Task/Ws/Common/ModList.asmx/GetAttachList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-20
작성목적 : 첨부파일 리스트 보여주기
**********************************************************************************************/
ModList.SuccessGetAttachList = function (data, status) {

    $('#placeholder_fileList').setTemplateURL('/DeskPlusEIP/Task/Jt/Common/JTFile.htm?seq=' + Guids());
    $('#placeholder_fileList').processTemplate(data.d);

    setTimeout(function () {
        ModList.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-20
작성목적 : 실제 파일 삭제 처리
**********************************************************************************************/
function DelFile(p_attachID) {
    if (window.confirm(R_ApplyDelete)) {
        ParamArray = {
            'listID': ListID,
            'attachID': p_attachID
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("ModList.SuccessDelFile");
        var url = "/DeskPlusEIP/Task/Ws/Common/ModList.asmx/DelAttach";

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-20
작성목적 : 실제 파일 삭제 처리
**********************************************************************************************/
ModList.SuccessDelFile = function (data, status) {
    if (data.d == true) {
        ModList.GetAttachList();
    }
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-20
작성목적 : 전자결재 근거문서  리스트 호출
**********************************************************************************************/
ModList.GetBaseAppList = function () {
    ParamArray = {
        'listID': ListID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ModList.SuccessGetBaseAppList");
    var url = "/DeskPlusEIP/Task/Ws/Common/ModList.asmx/GetBaseAppList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-20
작성목적 : 전자결재 근거문서 리스트 보여주기
**********************************************************************************************/
ModList.SuccessGetBaseAppList = function (data, status) {
    $('#placeholder_baseAppList').setTemplateURL('/DeskPlusEIP/Task/Jt/Common/JTBaseApp.htm?seq=' + Guids());
    $('#placeholder_baseAppList').processTemplate(data.d);

    setTimeout(function () {
        ModList.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-20
작성목적 : 전자결재 근거문서 삭제 처리
**********************************************************************************************/
function DelBaseApp(p_listID, p_documentID) {
    if (window.confirm(R_ApplyDelete)) {
        ParamArray = {
            'listID': p_listID,
            'documentID': p_documentID
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("ModList.SuccessDelBaseApp");
        var url = "/DeskPlusEIP/Task/Ws/Common/ModList.asmx/DelBaseApp";

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-20
작성목적 : 전자결재 근거문서 삭제 처리
**********************************************************************************************/
ModList.SuccessDelBaseApp = function (data, status) {
    if (data.d == true) {
        ModList.GetBaseAppList();
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-20
작성목적 : 협업 근거문서  리스트 호출
**********************************************************************************************/
ModList.GetBaseTaskList = function () {
    ParamArray = {
        'listID': ListID,
        'groupID': GroupID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ModList.SuccessGetBaseTaskList");
    var url = "/DeskPlusEIP/Task/Ws/Common/ModList.asmx/GetBaseTaskList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-20
작성목적 : 협업 근거문서 리스트 보여주기
**********************************************************************************************/
ModList.SuccessGetBaseTaskList = function (data, status) {
    $('#placeholder_baseTaskList').setTemplateURL('/DeskPlusEIP/Task/Jt/Common/JTBaseTask.htm?seq=' + Guids());
    $('#placeholder_baseTaskList').processTemplate(data.d);

    setTimeout(function () {
        ModList.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-20
작성목적 : 협업 근거문서 삭제 처리
**********************************************************************************************/
function DelBaseTask(p_listID, p_groupID, p_baseTaskID) {
    if (window.confirm(R_ApplyDelete)) {
        ParamArray = {
            'listID': p_listID,
            'groupID': p_groupID,
            'baseTaskID': p_baseTaskID
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("ModList.SuccessDelBaseTask");
        var url = "/DeskPlusEIP/Task/Ws/Common/ModList.asmx/DelBaseTask";

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-20
작성목적 : 협업 근거문서 리스트 보여주기
**********************************************************************************************/
ModList.SuccessDelBaseTask = function (data, status) {
    if (data.d == true) {
        ModList.GetBaseTaskList();
    }
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-20
작성목적 : 근거문서 추가
**********************************************************************************************/
ModList.AddApp = function () {
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
    WindowObjectReference = window.open(url, "AttachBaseApp", "left=" + x + ", top=" + y + ", width=" + popWidth + ", height=" + popHeight + ", menubar=no, location=yes, resizable=yes, scrollbars=yes, status=no");
    WindowObjectReference.focus();
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-20
작성목적 : 업무협업 근거문서 추가
**********************************************************************************************/
ModList.AddTask = function () {
    ParamArray = {
        'baseTasks': BaseTasks,
        'groupID': GroupID
    };
    CallEncrypt(CallGetParam());

    var popWidth = 1000; //팝업 가로
    var popHeight = 600; //팝업 세로

    // 팝업 화면 중앙에 오도록
    var x = (window.screen.width - popWidth) / 2;
    var y = (window.screen.height - popHeight) / 2;

    var url = "/DeskPlusEIP/Task/Common/BaseTaskList.aspx?p=" + GetEncryptValue;
    WindowObjectReference = window.open(url, "AttachBaseTask", "left=" + x + ", top=" + y + ", width=" + popWidth + ", height=" + popHeight + ", menubar=no, location=yes, resizable=yes, scrollbars=yes, status=no");
    WindowObjectReference.focus();
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-20
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
        var obj = eval("ModList.SuccessDelSpoolFileList");
        var url = "/DeskPlusEIP/Common/Ws/SetCommon.asmx/DelSpoolFileList";

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-01-08
작성목적 : SPOOL 첨부파일 리스트 보여주기
**********************************************************************************************/
ModList.SuccessDelSpoolFileList = function (data, status) {
    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Common/Jt/Attach/JTSpoolFile.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate(data.d);

    /*
    setTimeout(function () {
        ModList.Resource();
    }, 100);

    var pos = document.body.scrollHeight;
    $("html, body").animate({ scrollTop: pos }, 100);
    */
}


var oContextMenuCtrl = null;

/**************************************************
작성자   : famo23
작성일자 : 2015-04-20
작성목적 : 클릭 이벤트 분기 (컨텍스트 메뉴 변경)
***************************************************/
ModList.click = function (e) {
    var evt = new __event(e);
    var el = evt.target;

    var action = el.getAttribute("action");
    switch (action) {
        case "del": ModList.ShowDelContextMenu(e); break;
        default: if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem(e); break;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-20
작성목적 : 컨텍스트 메뉴 생성
***************************************************/
ModList.ShowDelContextMenu = function (e) {
    if (oContextMenuCtrl != null) oContextMenuCtrl.clearItem();

    var evt = new __event(e);
    var el = evt.target;
    var tmpID = el.id;

    oContextMenuCtrl = new DeskPlusEIP.Component.ContextMenuCtrl();
    oContextMenuCtrl.init();

    oContextMenuCtrl.addNameItem(R_Del, "ModList.DelItem('" + tmpID + "')", ImagePath + "/Icon/ContextMenu/del.png");
    oContextMenuCtrl.show(e);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-20
작성목적 : 부서/사용자 삭제
***************************************************/
ModList.DelItem = function (id) {
    $("#" + id).remove();
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-20
작성목적 : 수정하기
**********************************************************************************************/
ModList.Save = function () {

    if (BtnCheck == "Y") {
        alert(R_MsgProcessing);
        return;
    }

    //전자결재 근거문서
    $("#tbBaseApps").val(BaseDocIDs);

    //업무협업 근거문서
    $("#tbBaseTasks").val(BaseTasks);

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
    BtnCheck = "Y";

    __doPostBack('lbtnAdd', "MOD");
}