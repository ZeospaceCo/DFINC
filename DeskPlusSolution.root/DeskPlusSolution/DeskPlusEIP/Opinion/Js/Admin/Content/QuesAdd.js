QuesAdd = {}

$(function () {
    $(document).ready(function () {
        //이벤트

        //날짜
        $("#ctbSDate").datepick($.datepicker.regional[PreLang]);
        $("#ctbEDate").datepick($.datepicker.regional[PreLang]);
        $('#ctbSDate').focus(function (e) { $('#tdDate').css('height', '220px'); });
        $('#ctbEDate').focus(function (e) { $('#tdDate').css('height', '220px'); });

        // [복사본(신규설문)]
        $('#cbtnCopy').click(function (e) { e.preventDefault(); QuesAdd.Copy(); });

        // [확인]
        $('#cbtnConfirm').click(function (e) { e.preventDefault(); QuesAdd.Add(); });

        // [취소]
        $('#cbtnCancel').click(function (e) { e.preventDefault(); QuesAdd.Cancel(); });

        // [주관부서변경]
        $('#cbtnUptSuper').click(function (e) { e.preventDefault(); QuesAdd.UptSuper(); });

        // [첨부파일-등록]
        $('#cbtnFileAdd').click(function (e) { e.preventDefault(); QuesAdd.AttachSwfFile(); });

        $('#cbIsSecret').click(function (e) {

            if ($('#cbIsSecret').is(':checked')) {
                $('#cbIsVoteUser').attr('checked', false);
                $('#cbIsVoteUser').attr('disabled', true);
            }
            else {
                $('#cbIsVoteUser').removeAttr('disabled');
                $('#cbIsVoteUser').attr('checked', true);
            }
        });

        QuesAdd.GetSpoolFileList();

        /*
        //ByFreeman 20120106 X-Free Editor - 템플릿 추가
        if (NonActiveX == "T") {
        var agent = navigator.userAgent.toLowerCase();
        if ((navigator.appName == 'Netscape' && navigator.userAgent.search('Trident') != -1) || (agent.indexOf("msie") != -1)) {

        }
        else {
        $("#ActiveX").hide();
        }
        $('#ActiveX').click(function (e) {
        e.preventDefault();
        if (confirm(ConfirmEditorTab)) {
        location.href = "QuesAdd.aspx?" + ClientQueryString;
        }
        });
        } else {
        $('#NonActiveX').click(function (e) {
        e.preventDefault();
        if (confirm(ConfirmEditorTab)) {
        location.href = "QuesAddN.aspx?" + ClientQueryString;
        }
        });
        }
        */

    });
});

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : [복사본(신규설문)]
***************************************************/
QuesAdd.Copy = function () {
    alert("Copy");
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : [확인]
***************************************************/
QuesAdd.Add = function () {

    //에디터 내용 히든박스에 저장
    //ByFreeman 20120106 NonActiveX Test
    if (NonActiveX == "T") {
        $("#tbHiddenHTML_contents").val(xfe.getHtmlValue());
        $("#tbHiddenMime_contents").val("");
    } else {
        if (!EditorSecurity(form1.twe)) return;

        var strHtml = form1.twe.HtmlValue;
        if (strHtml.indexOf(".js") > -1 || strHtml.indexOf('=\"xss:expression') > -1) {
            strHtml = strHtml.replace(".js\"", "\"");
            strHtml = strHtml.replace('=\"xss:expression', "");

            form1.twe.HtmlValue = strHtml;
            alert(R_Msg_ErrCont);
        }

        $("#tbHiddenHTML_contents").val(form1.twe.HtmlValue);
        $("#tbHiddenMime_contents").val(form1.twe.MimeEnValue);
    }

    //필수입력
    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbTitle", R_Msg_ErrTitle, true);
    validator.checkRequired("ctbSDate", R_Msg_ErrSDate, true);
    validator.checkRequired("ctbEDate", R_Msg_ErrEDate, true);
    validator.checkRequired("ctbDeptNm", R_Msg_ErrSupDept, true);
    if (!validator.validate()) return;

    //Sript체크

    //날짜체크
    var arr1 = $("#ctbSDate").val().split("-");
    var arr2 = $("#ctbEDate").val().split("-");
    var d1 = new Date(arr1[0], arr1[1], arr1[2]).valueOf();
    var d2 = new Date(arr2[0], arr2[1], arr2[2]).valueOf();
    if ((d2 - d1) < 0) { alert(R_Msg_ErrDate); return; }

    $("#tbDeptID").val($("#ctbDeptID").val());

    __doPostBack('lbtnSubmit', "ADD");
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : [목록/취소]
***************************************************/
QuesAdd.Cancel = function () {
    ParamArray = {
        'menuID': MenuID
        , 'pageSize': PageSize
        , 'pageNo': PageNo
        , 'sKey': SKey
        , 'sVal': SVal
        , 'sPeriod': SPeriod
        , 'sPeriodVal': SPeriodVal
        , 'order': Order
    };
    CallEncrypt(CallGetParam());
    location.href = "QuesList.aspx?p=" + GetEncryptValue;
}


/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : [주관부서변경]
***************************************************/
QuesAdd.UptSuper = function () {
    var sDeptID = $("#lbDeptID").text();
    var sDeptNm = $("#ctbDeptNm").val();

    ParamArray = {
        'deptID': sDeptID
        , 'deptNm': sDeptNm
    };
    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Opinion/Folder/GetDept.aspx";
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "UptSuper", "500", "400")
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : [첨부파일-등록]
***************************************************/
QuesAdd.AttachSwfFile = function () {

    ParamArray = {
        'swfUploadGUID': SwfUploadGUID,
        'fileType': FileType,
        'fileTypeDesc': FileTypeDesc,
        'moduleName': 'OPINION',
        'callBackEvent': "QuesAdd.GetSpoolFileList"
    };

    CallEncrypt(CallGetParam());

    var url = GetAttachPathURL();
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "DPUpload", "680", "510")
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : SPOOL 파일  리스트 호출
***************************************************/
QuesAdd.GetSpoolFileList = function () {

    ParamArray = {
        'swfUploadGUID': SwfUploadGUID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("QuesAdd.SuccessSpoolFileList");
    var url = "/DeskPlusEIP/Common/Ws/SpoolFile.asmx/GetSpoolFileList";

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : SPOOL 첨부파일 리스트 보여주기
***************************************************/
QuesAdd.SuccessSpoolFileList = function (data, status) {

    $('#phFile').setTemplateURL('/DeskPlusEIP/Common/Jt/Attach/JTSpoolFile.htm?seq=' + Guids());
    $('#phFile').processTemplate(data.d);

    QuesAdd.Resource();
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 임시파일 삭제 처리
***************************************************/
function DelSpoolPath(fileName) {

    if (window.confirm(R_Delete)) {
        ParamArray = {
            'fileName': fileName,
            'swfUploadGUID': SwfUploadGUID
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("QuesAdd.GetSpoolFileList");
        var url = "/DeskPlusEIP/Common/Ws/SetCommon.asmx/DelSpoolFileList";

        AjaxService(url, data, obj);
    }
}

/**************************************************
작성자   : withsuki
작성일자 : 
작성목적 : 다국어처리
***************************************************/
QuesAdd.Resource = function () {
    //첨부파일
    //    $("#j_spool_fileName")[0].innerHTML = R_FileName;
    //    $("#j_spool_fileType")[0].innerHTML = R_FileType;
    //    $("#j_spool_fileLen")[0].innerHTML = R_FileLen;
    //    $("#j_spool_filedelete")[0].innerHTML = R_FileDel;

    if ($(".ModMsg") != undefined) { $(".ModMsg").text(R_Modify); }
    if ($(".DelMsg") != undefined) { $(".DelMsg").text(R_Del); }

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_JTNoData); }
}

