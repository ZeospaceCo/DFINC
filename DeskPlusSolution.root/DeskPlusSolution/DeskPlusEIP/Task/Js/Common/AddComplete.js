AddComplete = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        // 첨부파일 추가
        $('#cbtnSwfFileAdd').click(function (e) {
            e.preventDefault();
            AddComplete.AttachSwfFile();
        });

        //완료하기
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            AddComplete.Save();
        });

        // 전자결재 근거문서 추가
        $('#cbtnAddApp').click(function (e) {
            e.preventDefault();
            AddComplete.AddApp();
        });

        // 업무협업 근거문서 추가
        $('#cbtnAddTask').click(function (e) {
            e.preventDefault();
            AddComplete.AddTask();
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
                'listID': ListID,
                'groupID': GroupID,
                'itemID': ItemID,
                'itemSubID': ItemSubID,
                'isType': IsType,
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
                else {
                    location = "/DeskPlusEIP/Task/Content/Receive/ViewN.aspx?p=" + GetEncryptValue;
                }
            }
            else {
                location = "/DeskPlusEIP/Task/Home/ViewN.aspx?p=" + GetEncryptValue;
            }

        });

        $("#ctbStartDate").datepick($.datepicker.regional[PreLang]);
        $("#ctbEndDate").datepick($.datepicker.regional[PreLang]);

        $('#ctbStartDate').focus(function (e) {
            $('#tdDate').css('height', '220px');
        });

        $('#ctbEndDate').focus(function (e) {
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
                        location.href = "AddComplete.aspx?" + ClientQueryString;
                    }
                });
            } else {
                $('#NonActiveX').click(function (e) {
                    e.preventDefault();
                    if (confirm(ConfirmEditorTab)) {
                        location.href = "AddCompleteN.aspx?" + ClientQueryString;
                    }
                });
            }
            */
        }, 1000);

        AddComplete.SetSpoolFile();

        __addEventListener(document, "click", AddComplete.click);

    });
});

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-20
작성목적 : 다국어 처리
**********************************************************************************************/
AddComplete.SetSpoolFile = function () {

    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Common/Jt/Attach/JTSpoolFile.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate("");
    /*
    $("#j_spool_fileName")[0].innerHTML = R_FileName;
    $("#j_spool_fileType")[0].innerHTML = R_FileType;
    $("#j_spool_fileLen")[0].innerHTML = R_FileLen;
    $("#j_spool_filedelete")[0].innerHTML = R_FileDel;
    
    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoAttachData); }
    if ($(".NoBaseAppListData") != undefined) { $(".NoBaseAppListData").text(R_NoContent); }
    if ($(".NoBaseTaskListData") != undefined) { $(".NoBaseTaskListData").text(R_NoContent); }

    if ($(".BtnView") != undefined) { $(".BtnView").attr('src', ImagePath + '/Main/icon_search.gif'); }
    if ($(".BtnViews") != undefined) { $(".BtnViews").attr('src', ImagePath + '/Icon/icon_search.gif'); }
    */
}


/**************************************************
작성자   : famo23
작성일자 : 2015-04-20
작성목적 : 첨부파일 추가
***************************************************/
AddComplete.AttachSwfFile = function () {
    ParamArray = {
        'swfUploadGUID': SwfUploadGUID,
        'fileType': FileType,
        'fileTypeDesc': FileTypeDesc,
        'moduleName': 'TASK',
        'callBackEvent': "AddComplete.GetSpoolFileList"
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
AddComplete.GetSpoolFileList = function () {
    ParamArray = {
        'swfUploadGUID': SwfUploadGUID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("AddComplete.SuccessSpoolFileList");
    var url = "/DeskPlusEIP/Common/Ws/SpoolFile.asmx/GetSpoolFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-20
작성목적 : SPOOL 첨부파일 리스트 보여주기
**********************************************************************************************/
AddComplete.SuccessSpoolFileList = function (data, status) {
    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Common/Jt/Attach/JTSpoolFile.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate(data.d);

    /*
    setTimeout(function () {
        AddComplete.Resource();
    }, 100);
    */
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-20
작성목적 : 다국어 처리
**********************************************************************************************/
AddComplete.Resource = function () {
   /*
    $("#j_spool_fileName")[0].innerHTML = R_FileName;
    $("#j_spool_fileType")[0].innerHTML = R_FileType;
    $("#j_spool_fileLen")[0].innerHTML = R_FileLen;
    $("#j_spool_filedelete")[0].innerHTML = R_FileDel;
    */
    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoAttachData); }    
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-20
작성목적 : 근거문서 추가
**********************************************************************************************/
AddComplete.AddApp = function () {
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
AddComplete.AddTask = function () {
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
        var obj = eval("AddComplete.SuccessDelSpoolFileList");
        var url = "/DeskPlusEIP/Common/Ws/SetCommon.asmx/DelSpoolFileList";

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2016-01-08
작성목적 : SPOOL 첨부파일 리스트 보여주기
**********************************************************************************************/
AddComplete.SuccessDelSpoolFileList = function (data, status) {
    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Common/Jt/Attach/JTSpoolFile.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate(data.d);

    /*
    setTimeout(function () {
        AddComplete.Resource();
    }, 100);

    var pos = document.body.scrollHeight;
    $("html, body").animate({ scrollTop: pos }, 100);
    */
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-20
작성목적 : 완료하기
**********************************************************************************************/
AddComplete.Save = function () {

    if (BtnCheck == "Y") {
        alert(R_MsgProcessing);
        return;
    }

    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbStartDate", R_InputStartDate, true);
    validator.checkRequired("ctbEndDate", R_InputEndDate, true);
    if (!validator.validate()) return;

    var getStartDate = $("#ctbStartDate").val();
    var getEndDate = $("#ctbEndDate").val();
    var dtResponse = CompareDT(getStartDate, getEndDate, "lessequal", PreLangReal);

    if (dtResponse == false) {
        alert(R_StartDateEndDate);
        return false;
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

    if (BaseDocIDs != "" || BaseTasks != "") {
        var contents = $("#tbHiddenText_contents").val();
        var contentHtml = $("#tbHiddenHTML_contents").val();
        if (contentHtml.indexOf("<IMG") > -1) {
            contents = "<img"; //dummy (if you save only image in editor)
        }

        if (contents == "") {
            alert(R_NoContent);
            return;
        }
    }

    //2번 이상 클릭 불가
    BtnCheck = "Y";

    __doPostBack('lbtnAdd', '');
}