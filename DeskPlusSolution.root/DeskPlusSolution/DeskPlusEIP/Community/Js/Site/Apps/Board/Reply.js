
/// <reference path="../../../Html/Js/jQuery/jquery.js" />


Reply = {}

/**************************************************
작성자   : shine
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //게시물 추가
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            Reply.Write("Register");
        });

        //취소
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            Reply.Cancel();
        });


        //flash 첨부파일 추가
        $('#cbtnSwfFileAdd').click(function (e) {
            e.preventDefault();
            Reply.AttachSwfFile();
        });

        Reply.GetSpoolFileList();

        xfe.setHtmlValue($("#tbHiddenMime_contents").val());
        /*
        //ByFreeman 20120106 X-Free Editor - 템플릿 추가
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
                    location.href = "Reply.aspx?" + ClientQueryString;
                }
            });
        } else {
            $('#NonActiveX').click(function (e) {
                e.preventDefault();
                if (confirm(ConfirmEditorTab)) {
                    location.href = "ReplyN.aspx?" + ClientQueryString;
                }
            });
        }
        */
    });
});

/**************************************************
작성자   : shine
작성일자 : 2012-06-05
작성목적 : 게시물 추가
***************************************************/
Reply.Write = function (_arg) {

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

            alert(R_EditorScript);
        }

        $("#tbHiddenHTML_contents").val(form1.twe.HtmlValue);
        $("#tbHiddenMime_contents").val(form1.twe.MimeEnValue);
        $("#tbHiddenText_contents").val(form1.twe.TextValue);
    }

    var validator = new Validator($("#form1"));
    validator.checkRequired("tbTitle", R_InputContent, true);
    if (!validator.validate()) return;

    var contents = $("#tbHiddenText_contents").val();
    var contentHtml = $("#tbHiddenHTML_contents").val();
    if (contentHtml.indexOf("<IMG") > -1) {
        contents = "<img"; //dummy (if you save only image in editor)
    }

    if (contents == "") {
        alert(R_InputContent);
        return;
    }

    if (TrySaveCnt == 0) {
        __doPostBack('lbtnWrite', _arg);
        TrySaveCnt += 1;
    }
}



/**************************************************
작성자   : shine
작성일자 : 2012-06-05
작성목적 : 게시물 추가 취소시 
***************************************************/
Reply.Cancel = function () {

    ParamArray = {
        'communityID': CommunityID,
        'menuID': MenuID,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'grade': Grade,
        'recordPerPage': RecordPerPage
    };

    CallEncrypt(CallGetParam());
    location.href = "List.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : shine
작성일자 : 2012-06-05
작성목적 : flash 첨부파일 추가
***************************************************/
Reply.AttachSwfFile = function () {

    ParamArray = {
        'swfUploadGUID': SwfUploadGUID,
        'fileType': FileType,
        'fileTypeDesc': FileTypeDesc,
        'moduleName': 'COMMUNITY',
        'callBackEvent': "Reply.GetSpoolFileList"
    };

    CallEncrypt(CallGetParam());
    
    var url = GetAttachPathURL();
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "DPUpload", "680", "510")
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-06-05
작성목적 : SPOOL 파일  리스트 호출
**********************************************************************************************/
Reply.GetSpoolFileList = function () {

    ParamArray = {
        'swfUploadGUID': SwfUploadGUID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Reply.SuccessSpoolFileList");
    var url = "/DeskPlusEIP/Common/Ws/SpoolFile.asmx/GetSpoolFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shine
작성일자 : 2012-06-05
작성목적 : SPOOL 첨부파일 리스트 보여주기
**********************************************************************************************/
Reply.SuccessSpoolFileList = function (data, status) {

    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Common/Jt/Attach/JTSpoolFile.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate(data.d);

}



/**********************************************************************************************
작성자   : shine
작성일자 : 2012-06-05
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
        var obj = eval("Reply.SuccessSpoolFileList");
        var url = "/DeskPlusEIP/Common/Ws/SetCommon.asmx/DelSpoolFileList";

        AjaxService(url, data, obj);
    }
}