Default = {}
var ResourceArray = [];

/**************************************************
작성자   : famo23
작성일자 : 2012-06-05
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //주소록
        /*
        $('.addressbtn').click(function (e) {
        e.preventDefault();
        var winOpt = "width=790,height=560,toolbar=no,status=no,scrollbars=no,resizable=no";
        var winLink = "./Address/Default.aspx";
        window.open(winLink, "openAddress", winOpt);
        });
        */

        //메일보내기
        $('#cbtnSend').click(function (e) {
            e.preventDefault();
            Default.Send();
        });

        //취소
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            Default.Cancel();
        });

        //파일추가
        $('#cbtnAddFile').click(function (e) {
            e.preventDefault();
            Default.AddFile();
        });

        //받는 사람 주소록
        $('#imgTo').click(function (e) {
            e.preventDefault();
            Default.GoAddress('TO');
        });

        //참조 주소록
        $('#imgCc').click(function (e) {
            e.preventDefault();
            Default.GoAddress('CC');
        });

        //숨은 참조 주소록
        $('#imgBcc').click(function (e) {
            e.preventDefault();
            Default.GoAddress('BCC');
        });

        //첨부파일 초기화
        Default.FileList();

        if ($("#tbHiddenMime_contents")) {
            xfe.setHtmlValue($("#tbHiddenMime_contents").val());
        }
        else {
            xfe.setHtmlValue(form1.tbHiddenMime_contents.value);
        }
        /*
        //ByFreeman 20120106 X-Free Editor - 템플릿 추가
        if (NonActiveX == "T") {

            if ($("#tbHiddenMime_contents")) {
                xfe.setHtmlValue($("#tbHiddenMime_contents").val());
            }
            else {
                xfe.setHtmlValue(form1.tbHiddenMime_contents.value);
            }
            var agent = navigator.userAgent.toLowerCase();
            if ((navigator.appName == 'Netscape' && navigator.userAgent.search('Trident') != -1) || (agent.indexOf("msie") != -1)) {

            }
            else {
                $("#ActiveX").hide();
            }
            $('#ActiveX').click(function (e) {
                e.preventDefault();
                if (confirm(R_ConfirmEditorTab)) {
                    location.href = "Default.aspx?" + ClientQueryString;
                }
            });
        } else {
            $('#NonActiveX').click(function (e) {
                e.preventDefault();
                if (confirm(R_ConfirmEditorTab)) {
                    location.href = "DefaultN.aspx?" + ClientQueryString;
                }
            });
        }
        */

        Default.AutoGrow();
    });
});

/**********************************************************************************************
작성자   : freeman
작성일자 : 2012-06-29
작성목적 : Textarea 자등증가표시
**********************************************************************************************/
Default.AutoGrow = function () {

    $('#ctbTo').autogrow(); //받는 사람 자동 높이 조절
    $('#ctbCc').autogrow(); //참조 사람 자동 높이 조절
    $('#ctbBcc').autogrow(); //숨은 참조 사람 자동 높이 조절
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-05
작성목적 : 첨부파일 리스트 초기화 보여주기
**********************************************************************************************/
Default.FileList = function () {

    $('#phFile').setTemplateURL('/DeskPlusEIP/Community/Jt/Site/Main/Mail/JTSpoolFile.htm?seq=' + Guids());
    $('#phFile').processTemplate("");

}


/**************************************************
작성자   : famo23  
작성일자 : 2012-06-05
작성목적 : 첨부파일 추가
***************************************************/
Default.AddFile = function () {

    ParamArray = {
        'swfUploadGUID': SwfUploadGUID,
        'fileType': FileType,
        'fileTypeDesc': FileTypeDesc,
        'moduleName': 'Community',
        'callBackEvent': "Default.GetSpoolFileList"
    };

    CallEncrypt(CallGetParam());

    var url = GetAttachPathURL();
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "DPUpload", "680", "510")
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-05
작성목적 : SPOOL 파일  리스트 호출
**********************************************************************************************/
Default.GetSpoolFileList = function () {

    ParamArray = {
        'swfUploadGUID': SwfUploadGUID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Default.SuccessSpoolFileList");
    var url = "/DeskPlusEIP/Common/Ws/SpoolFile.asmx/GetSpoolFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-05
작성목적 : SPOOL 첨부파일 리스트 보여주기
**********************************************************************************************/
Default.SuccessSpoolFileList = function (data, status) {

    $('#phFile').setTemplateURL('/DeskPlusEIP/Community/Jt/Site/Main/Mail/JTSpoolFile.htm?seq=' + Guids());
    $('#phFile').processTemplate(data.d);

}


/**********************************************************************************************
작성자   : famo23
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
        var obj = eval("Default.SuccessSpoolFileList");
        var url = "/DeskPlusEIP/Common/Ws/SetCommon.asmx/DelSpoolFileList";

        AjaxService(url, data, obj);
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2012-06-07
작성목적 : 메일보내기
***************************************************/
Default.Send = function () {

    if (BtnCheck == "Y") return;

    //에디터 처리
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


    //제목 입력 필수
    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbTitle", R_InputTitle, true);
    if (!validator.validate()) return;

    //받는 사람 필수
    validator.checkRequired("ctbTo", R_InputTo, true);
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
    
    Default.MailToCheck();
}

/***********************************************************************
작성자   : famo23
작성일자 : 2012-06-07
작성목적 : 받는 사람 메일 체크 (웹서비스 호출)
************************************************************************/
Default.MailToCheck = function () {

    ParamArray = {
        'mail': $("#ctbTo").val()
    };

    CallEncrypt(CallGetParam());

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Default.SuccessMailToCheck");
    var url = "/DeskPlusEIP/Community/Ws/Site/Main/Mail/Default.asmx/MailList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-07
작성목적 : 받는 사람 메일 체크 (웹서비스 값 받기)
**********************************************************************************************/
Default.SuccessMailToCheck = function (data, status) {

    if (data.d != "") {
        var tmpVal = data.d.split('^');

        if (jQuery.trim(tmpVal[1]) != "") {
            alert(tmpVal[1] + "\n" + R_ErrorMailType);
            return;
        }

        $("#tbTo").val(tmpVal[0]);

        if (jQuery.trim($("#ctbCc").val()) != "") {
            Default.MailCcCheck();
        }
        else {
            if (jQuery.trim($("#ctbBcc").val()) != "") {
                Default.MailBccCheck();
            }
            else {
                BtnCheck = "Y";
                __doPostBack('lbtnConfirm', '');
            }
        }
    }
    else {
        alert(R_ProcessingError);
        return;
    }
}

/***********************************************************************
작성자   : famo23
작성일자 : 2012-06-07
작성목적 : 참조 메일 체크 (웹서비스 호출)
************************************************************************/
Default.MailCcCheck = function () {

    ParamArray = {
        'mail': $("#ctbCc").val()
    };

    CallEncrypt(CallGetParam());

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Default.SuccessMailCcCheck");
    var url = "/DeskPlusEIP/Community/Ws/Site/Main/Mail/Default.asmx/MailList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-07
작성목적 : 참조 메일 체크 (웹서비스 값 받기)
**********************************************************************************************/
Default.SuccessMailCcCheck = function (data, status) {

    if (data.d != "") {
        var tmpVal = data.d.split('^');

        if (jQuery.trim(tmpVal[1]) != "") {
            alert(tmpVal[1] + "\n" + R_ErrorMailType);
            return;
        }

        $("#tbCc").val(tmpVal[0]);

        if (jQuery.trim($("#ctbBcc").val()) != "") {
            Default.MailBccCheck();
        }
        else {
            BtnCheck = "Y";
            __doPostBack('lbtnConfirm', '');
        }
    }
    else {
        alert(R_ProcessingError);
        return;
    }
}

/***********************************************************************
작성자   : famo23
작성일자 : 2012-06-07
작성목적 : 숨은 참조 메일 체크 (웹서비스 호출)
************************************************************************/
Default.MailBccCheck = function () {

    ParamArray = {
        'mail': $("#ctbBcc").val()
    };

    CallEncrypt(CallGetParam());

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Default.SuccessMailBccCheck");
    var url = "/DeskPlusEIP/Community/Ws/Site/Main/Mail/Default.asmx/MailList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-07
작성목적 : 숨은 참조 메일 체크 (웹서비스 값 받기)
**********************************************************************************************/
Default.SuccessMailBccCheck = function (data, status) {

    if (data.d != "") {
        var tmpVal = data.d.split('^');

        if (jQuery.trim(tmpVal[1]) != "") {
            alert(tmpVal[1] + "\n" + R_ErrorMailType);
            return;
        }

        $("#tbBcc").val(tmpVal[0]);

        BtnCheck = "Y";
        __doPostBack('lbtnConfirm', '');
    }
    else {
        alert(R_ProcessingError);
        return;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2012-06-07
작성목적 : 주소록 
***************************************************/
Default.GoAddress = function (_gubun) {

    ParamArray = {
        'gubun': _gubun
    };

    CallEncrypt(CallGetParam());

    var url = "./Address/Default.aspx";
    var param = "p=" + GetEncryptValue;
    PopupWinOpen(url, param, "openAddress", "900", "650");
}


/**************************************************
작성자   : famo23
작성일자 : 2012-06-08
작성목적 : 취소
***************************************************/
Default.Cancel = function () {

    ParamArray = {
        'communityID': CommunityID,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'recordPerPage': RecordPerPage
    };

    CallEncrypt(CallGetParam());    

    location.href = "/DeskPlusEIP/Community/Site/Main/Search/List.aspx?p=" + GetEncryptValue;
}

