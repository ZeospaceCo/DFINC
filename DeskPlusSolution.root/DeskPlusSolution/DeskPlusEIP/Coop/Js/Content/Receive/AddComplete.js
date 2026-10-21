Add = {}

/**********************************************************************************************
작성자   :
작성일자 :
작성목적 : 초기화
**********************************************************************************************/

$(function () {
    $(document).ready(function () {
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

        //flash 첨부파일 추가
        $('#cbtnSwfFileAdd').click(function (e) {
            e.preventDefault();
            Add.AttachSwfFile();
        });

        //완료하기
        $('#cbtnComplete').click(function (e) {
            e.preventDefault();
            Add.Save();
        });

        // 전자결재 문서추가
        $('#cbtnDocAdd').click(function (e) {
            e.preventDefault();
            Add.DocAdd();
        });

        // 업무협업 근거문서 추가
        $('#cbtnCoopAdd').click(function (e) {
            e.preventDefault();
            Add.CoopAdd();
        });

        if (NonActiveX == "T") {
            xfe.setHtmlValue($("#tbHiddenMime_contents").val());
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


        $("#ctbStartDate").datepick($.datepicker.regional[PreLang]);
        $("#ctbEndDate").datepick($.datepicker.regional[PreLang]);

        $('#ctbStartDate').focus(function (e) {
            $('#tdDate').css('height', '220px');
        });
        $('#ctbEndDate').focus(function (e) {
            $('#tdDate').css('height', '220px');
        });

        Add.GetSpoolFileList();
    });
});

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : flash 첨부파일 추가
***************************************************/
Add.AttachSwfFile = function () {
    ParamArray = {
        'swfUploadGUID': SwfUploadGUID,
        'fileType': FileType,
        'fileTypeDesc': FileTypeDesc,
        'moduleName': 'COOP',
        'callBackEvent': "Add.GetSpoolFileList"
    };

    CallEncrypt(CallGetParam());
    //var url = "/DeskPlusEIP/Upload/DPUpload/Main.aspx?p=" + GetEncryptValue;
    //PopupWinOpen(680, 510, url, "", "", true);

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
Add.GetSpoolFileList = function () {
    ParamArray = {
        'swfUploadGUID': SwfUploadGUID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Add.SuccessSpoolFileList");
    var url = "/DeskPlusEIP/Common/Ws/SpoolFile.asmx/GetSpoolFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : SPOOL 첨부파일 리스트 보여주기
**********************************************************************************************/
Add.SuccessSpoolFileList = function (data, status) {
    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Common/Jt/Attach/JTSpoolFile.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate(data.d);

    setTimeout(function () {
        Add.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 다국어 처리
**********************************************************************************************/
Add.Resource = function () {
    $("#j_spool_fileName")[0].innerHTML = R_FileName;
    $("#j_spool_fileType")[0].innerHTML = R_FileType;
    $("#j_spool_fileLen")[0].innerHTML = R_FileLen;
    $("#j_spool_filedelete")[0].innerHTML = R_FileDel;

    if ($("#j_spool_screenshot")[0] != undefined) {
        $("#j_spool_screenshot")[0].innerHTML = R_ScreenShot;
    }

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoAttachData); }
    if ($(".NoBaseListData") != undefined) { $(".NoBaseListData").text(R_NoContent); }
    if ($(".NoBaseCoopListData") != undefined) { $(".NoBaseCoopListData").text(R_NoContent); }
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
        var obj = eval("Add.SuccessSpoolFileList");
        var url = "/DeskPlusEIP/Common/Ws/SetCommon.asmx/DelSpoolFileList";

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
[저장]
**********************************************************************************************/
Add.Save = function () {
    var validator = new Validator($("#form1"));
    //validator.checkRequired("tbxTitle", R_InputTitle, true);
    validator.checkRequired("ctbStartDate", R_InputStartDate, true);
    validator.checkRequired("ctbEndDate", R_InputEndDate, true);
    if (!validator.validate()) return;

    ///전자결재 근거문서
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

    var getStartDate = $("#ctbStartDate").val();
    var getEndDate = $("#ctbEndDate").val();
    var dtResponse = CompareDT(getStartDate, getEndDate, "lessequal", PreLangReal);

    if (dtResponse == false) {
        alert(R_StartDateEndDate);
        return false;
    }

    if (BaseDocIDs != "" || CoopBaseIDs != "") {
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
    $("#cbtnComplete").attr('onclick', '').unbind('click');

    __doPostBack('lbtnAdd', '');
}

/**********************************************************************************************
[근거문서 - 문서추가]
**********************************************************************************************/
Add.DocAdd = function () {
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
Add.CoopAdd = function () {
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