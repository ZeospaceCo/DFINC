Mod = {}

$(function () {

    $(document).ready(function () {
        //확인
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            Mod.Add();
        });

        //취소
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            Mod.Cancel();
        });

        //flash 첨부파일 추가
        $('#cbtnSwfFileAdd').click(function (e) {
            e.preventDefault();
            Mod.AttachSwfFile();
        });

        xfe.setHtmlValue($("#contents").val());
        /*
        //ByFreeman 20120106 X-Free Editor - 템플릿 추가
        if (NonActiveX == "T") {
            xfe.setHtmlValue($("#contents").val());
            var agent = navigator.userAgent.toLowerCase();
            if ((navigator.appName == 'Netscape' && navigator.userAgent.search('Trident') != -1) || (agent.indexOf("msie") != -1)) {

            }
            else {
                $("#ActiveX").hide();
            }
            $('#ActiveX').click(function (e) {
                e.preventDefault();
                if (confirm(ConfirmEditorTab)) {
                    location.href = "Mod.aspx?" + ClientQueryString;
                }
            });
        } else {
            $('#NonActiveX').click(function (e) {
                e.preventDefault();
                if (confirm(ConfirmEditorTab)) {
                    location.href = "ModN.aspx?" + ClientQueryString;
                }
            });
        }
        */
    });
});

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 수정 취소시
***************************************************/
Mod.Cancel = function () {
    ParamArray = {
        'categoryID': CategoryID,
        'listID': ListID,
        'currentPage': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    CallEncrypt(CallGetParam());
    location.href = "View.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 수정
***************************************************/
Mod.Add = function () {
    //에디터 내용 히든박스에 저장
    //ByFreeman 20120106 NonActiveX Test
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

    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbName", Add_InputTitle, true);
    if (!validator.validate()) return;

    var contents = $("#tbHiddenText_contents").val();
    var contentHtml = $("#tbHiddenHTML_contents").val();
    if (contentHtml.indexOf("<IMG") > -1) {
        contents = "<img"; //dummy (if you save only image in editor)
    }

    if (contents == "") {
        alert(Add_InputContent);
        return;
    }

    var imageRex = false;
    var screenShotVal = $(".radioSpoolFile:checked").val();

    if (screenShotVal != "" && screenShotVal != undefined) {
        var arr = new Array('.bmp', '.tif', '.jpeg', '.jpg', '.gif', '.png', '.pjpeg', '.pjpg');

        for (i = 0; i <= arr.length - 1; i++) {
            if (arr[i] == screenShotVal.toLowerCase().indexOf(arr[i]) != -1) {
                imageRex = true;
                break;
            }
        }

        if (imageRex == true) {
            $("#tbScreenShot").val(screenShotVal);
        }
        else {
            alert(R_NotImageFormat);
            return;
        }
    }

    //alert($("#tbScreenShot").val());

    __doPostBack('lbtnWrite', '');
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-20
작성목적 : 파일  리스트 호출
**********************************************************************************************/
Mod.GetAttachFileList = function () {
    ParamArray = {
        'categoryID': CategoryID,
        'listID': ListID,
        'screenShot': ScreenShot
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Mod.SuccessAttachFile");
    var url = "/DeskPlusEIP/Board/Ws/Temp/Mod.asmx/GetMyAttachFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 첨부파일 리스트 보여주기
**********************************************************************************************/
Mod.SuccessAttachFile = function (data, status) {
    $('#placeholder_fileList').setTemplateURL('/DeskPlusEIP/Board/Jt/Content/JTDelScreenFile.htm?seq=' + Guids());
    $('#placeholder_fileList').processTemplate(data.d);

    setTimeout(function () {
        Mod.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 다국어 처리
**********************************************************************************************/
Mod.Resource = function () {

    if ($(".j_screenshot") != undefined) { $(".j_screenshot").text(R_ScreenShot); }

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
        'moduleName': 'BOARD',
        'callBackEvent': "Mod.GetSpoolFileList"
    };

    CallEncrypt(CallGetParam());

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
    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Board/Jt/Content/JTSpoolFile.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate(data.d);

    setTimeout(function () {
        Mod.Resource();
    }, 100);
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
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 임시 실제 파일 삭제 처리
**********************************************************************************************/
function DelFile(p_attachFileID) {
    if (window.confirm(DelAttach)) {

        ParamArray = {
            'categoryID': CategoryID,
            'listID': ListID,
            'attachFileID': p_attachFileID
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("Mod.SuccessFileList");
        var url = "/DeskPlusEIP/Board/Ws/Temp/Mod.asmx/DelMyAttachFile";

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