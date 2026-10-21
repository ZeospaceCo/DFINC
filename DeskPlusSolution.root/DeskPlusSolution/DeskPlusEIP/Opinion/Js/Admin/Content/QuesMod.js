QuesMod = {}

$(function () {
    $(document).ready(function () {
        //이벤트

        //날짜
        $("#ctbSDate").datepick($.datepicker.regional[PreLang]);
        $("#ctbEDate").datepick($.datepicker.regional[PreLang]);
        $('#ctbSDate').focus(function (e) { $('#tdDate').css('height', '220px'); });
        $('#ctbEDate').focus(function (e) { $('#tdDate').css('height', '220px'); });

        // [삭제]
        $('#cbtnDel').click(function (e) { e.preventDefault(); QuesMod.Del(); });

        // [확인]
        $('#cbtnConfirm').click(function (e) { e.preventDefault(); QuesMod.Add(); });

        // [취소]
        $('#cbtnCancel').click(function (e) { e.preventDefault(); QuesMod.Cancel(); });

        // [주관부서변경]
        $('#cbtnUptSuper').click(function (e) { e.preventDefault(); QuesMod.UptSuper(); });

        // [첨부파일-등록]
        $('#cbtnFileAdd').click(function (e) { e.preventDefault(); QuesMod.AttachSwfFile(); });

        // [질문리스트-질문추가]
        $('#cbtnUnitAdd').click(function (e) { e.preventDefault(); QuesMod.UnitAdd(); });

        // [질문리스트-질문순서]
        $('#cbtnUnitSeq').click(function (e) { e.preventDefault(); QuesMod.UnitModSeq(); });

        // [설문대상자]
        $('#cbtnTargetAdd').click(function (e) { e.preventDefault(); QuesMod.TargetAdd(); });

        // [설문관리자]
        $('#cbtnAdminAdd').click(function (e) { e.preventDefault(); QuesMod.AdminAdd(); });

        // [첨부파일-다운로드]
        //$('#cbtnFileDown').click(function (e) { e.preventDefault(); QuesMod.FileDown(); });

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

        if (QuesID > 0) {
            setTimeout(function () {
                QuesMod.GetAttachFileList();    //첨부파일리스트
                QuesMod.GetSpoolFileList();     //첨부파일리스트
                QuesMod.GetUnitList();          //질문리스트
                QuesMod.GetTargetList();        //대상자리스트
                QuesMod.GetAdminList();         //관리자리스트
            }, 200);
        }

        xfe.setHtmlValue($("#tbHiddenHTML_contents").val());
        /*
        //ByFreeman 20120106 X-Free Editor - 템플릿 추가
        if (NonActiveX == "T") {

        xfe.setHtmlValue($("#tbHiddenHTML_contents").val());
        var agent = navigator.userAgent.toLowerCase();
        if ((navigator.appName == 'Netscape' && navigator.userAgent.search('Trident') != -1) || (agent.indexOf("msie") != -1)) {

        }
        else {
        $("#ActiveX").hide();
        }
        $('#ActiveX').click(function (e) {
        e.preventDefault();
        if (confirm(ConfirmEditorTab)) {
        location.href = "QuesMod.aspx?" + ClientQueryString;
        }
        });
        } else {
        $('#NonActiveX').click(function (e) {
        e.preventDefault();
        if (confirm(ConfirmEditorTab)) {
        location.href = "QuesModN.aspx?" + ClientQueryString;
        }
        });
        }
        */

    });
});

/**************************************************
작성자   : withsuki
작성일자 :
작성목적 : [삭제]
***************************************************/
QuesMod.Del = function () {
    if (window.confirm(R_Delete)) {
        __doPostBack('lbtnSubmit', "DEL");
    }
}

/**************************************************
작성자   : withsuki
작성일자 :
작성목적 : [확인]
***************************************************/
QuesMod.Add = function () {
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
    __doPostBack('lbtnSubmit', "MOD");
}

/**************************************************
작성자   : withsuki
작성일자 :
작성목적 : [목록/취소]
***************************************************/
QuesMod.Cancel = function () {
    ParamArray = {
        'menuID': MenuID
        , 'pageSize': PageSize
        , 'pageNo': PageNo
        , 'sKey': SKey
        , 'sVal': SVal
        , 'sPeriod': SPeriod
        , 'sPeriodVal': SPeriodVal
        , 'order': Order
        , 'quesID': QuesID
    };
    CallEncrypt(CallGetParam());
    location.href = "QuesView.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : withsuki
작성일자 :
작성목적 : [주관부서변경]
***************************************************/
QuesMod.UptSuper = function () {
    var sDeptID = $("#lbDeptID").text();
    var sDeptNm = $("#ctbDeptNm").val();

    ParamArray = {
        'deptID': sDeptID
        , 'deptNm': sDeptNm
    };
    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Opinion/Folder/GetDept.aspx";
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "UptSuper", "500", "390")
}

/**************************************************
작성자   : withsuki
작성일자 :
작성목적 : 파일리스트 호출
***************************************************/
QuesMod.GetAttachFileList = function () {
    ParamArray = { 'quesID': QuesID };
    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("QuesMod.SuccessAttachFile");
    var url = "/DeskPlusEIP/Opinion/Ws/Admin/Content/QuesMod.asmx/GetAttachList";
    AjaxService(url, data, obj);
}

/**************************************************
작성자   : withsuki
작성일자 :
작성목적 : 첨부파일리스트 보여주기
***************************************************/
QuesMod.SuccessAttachFile = function (data, status) {
    $('#phFileList').setTemplateURL('/DeskPlusEIP/Opinion/Jt/Admin/Content/JtAttachList.htm?seq=' + Guids());
    $('#phFileList').processTemplate(data.d);
    setTimeout(function () { QuesMod.Resource('ATTACH'); }, 100);
}

/**************************************************
작성자   : withsuki
작성일자 :
작성목적 : 파일삭제처리
***************************************************/
QuesMod.DelFile = function (_attachID) {
    if (window.confirm(R_Delete)) {
        ParamArray = {
            'quesID': QuesID,
            'attachID': _attachID
        };
        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("QuesMod.GetAttachFileList");
        var url = "/DeskPlusEIP/Opinion/Ws/Admin/Content/QuesMod.asmx/DelAttach";

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : withsuki
작성일자 :
작성목적 : 첨부파일 다운로드
**********************************************************************************************/
QuesMod.FileDownload = function (filePath, fileName, fileGuid) {
    ParamArray = {
        'filePath': filePath + fileGuid,
        'fileName': fileName
    };
    CallEncrypt(CallGetParam());
    var attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + GetEncryptValue;
    window.open(attachFilePath);
    //WindowObjectReference = window.open(attachFilePath, "FileDownload", "menubar=no,location=no,resizable=no,scrollbars=no,status=no");
}

/**************************************************
작성자   : withsuki
작성일자 :
작성목적 : [첨부파일-등록]
***************************************************/
QuesMod.AttachSwfFile = function () {
    ParamArray = {
        'swfUploadGUID': SwfUploadGUID,
        'fileType': FileType,
        'fileTypeDesc': FileTypeDesc,
        'moduleName': 'OPINION',
        'callBackEvent': "QuesMod.GetSpoolFileList"
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
QuesMod.GetSpoolFileList = function () {
    ParamArray = {
        'swfUploadGUID': SwfUploadGUID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("QuesMod.SuccessSpoolFileList");
    var url = "/DeskPlusEIP/Common/Ws/SpoolFile.asmx/GetSpoolFileList";

    AjaxService(url, data, obj);
}

/**************************************************
작성자   : withsuki
작성일자 :
작성목적 : SPOOL 첨부파일 리스트 보여주기
***************************************************/
QuesMod.SuccessSpoolFileList = function (data, status) {
    $('#phFile').setTemplateURL('/DeskPlusEIP/Common/Jt/Attach/JTSpoolFile.htm?seq=' + Guids());
    $('#phFile').processTemplate(data.d);
    setTimeout(function () { QuesMod.Resource('SPOOL'); }, 100);
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
        var obj = eval("QuesMod.GetSpoolFileList");
        var url = "/DeskPlusEIP/Common/Ws/SetCommon.asmx/DelSpoolFileList";

        AjaxService(url, data, obj);
    }
}

/**************************************************
작성자   : withsuki
작성일자 :
작성목적 : 질문리스트
***************************************************/
QuesMod.GetUnitList = function () {
    ParamArray = { 'quesID': QuesID };
    CallEncrypt(CallGetParam());        //배열값을 파라미터형태로 반환

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("QuesMod.ScsUnitList");
    var url = "/DeskPlusEIP/Opinion/Ws/Admin/Content/QuesMod.asmx/GetQuesUnittList";
    AjaxService(url, data, obj);
}
QuesMod.ScsUnitList = function (data, status) {
    $('#phUnitList').setTemplateURL('/DeskPlusEIP/Opinion/Jt/Admin/Content/JtQuesUnitList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phUnitList').processTemplate(data.d);
    setTimeout(function () { QuesMod.Resource('UNIT'); }, 100);
}

/**************************************************
작성자   : withsuki
작성일자 :
작성목적 : 질문추가
***************************************************/
QuesMod.UnitAdd = function () {
    ParamArray = { 'section': '0', 'quesID': QuesID };
    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Opinion/Folder/GetUnitList.aspx";
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "UnitAdd", "550", "320")
}
/**************************************************
작성자   : withsuki
작성일자 :
작성목적 : 질문추가(웹서비스호출)
***************************************************/
UnitAddWs = function (_selv) {
    if (_selv != "") {
        ParamArray = { 'quesID': QuesID, 'arrID': _selv };
        CallEncrypt(CallGetParam());
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("QuesMod.GetUnitList");  //질문리스트
        var url = "/DeskPlusEIP/Opinion/Ws/Admin/Content/QuesMod.asmx/InsQuesUnit";
        AjaxService(url, data, obj);
    }
}

/**************************************************
작성자   : withsuki
작성일자 :
작성목적 : 질문순서변경
***************************************************/
QuesMod.UnitModSeq = function () {
    ParamArray = { 'quesID': QuesID };
    CallEncrypt(CallGetParam());
    var url = "QuesSeq.aspx";
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "QuesSeq", "500", "400");
}

/**************************************************
작성자   : withsuki
작성일자 :
작성목적 : 질문삭제(건당)
***************************************************/
QuesMod.UnitDel = function (_quesUnitID) {
    if (window.confirm(R_Delete)) {
        ParamArray = { 'quID': _quesUnitID };
        CallEncrypt(CallGetParam());
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("QuesMod.GetUnitList");  //질문리스트
        var url = "/DeskPlusEIP/Opinion/Ws/Admin/Content/QuesMod.asmx/DelQuesUnit";
        AjaxService(url, data, obj);
    }
}

/**************************************************
작성자   : withsuki
작성일자 :
작성목적 : 대상자리스트
***************************************************/
QuesMod.GetTargetList = function () {
    ParamArray = { 'quesID': QuesID };
    CallEncrypt(CallGetParam());        //배열값을 파라미터형태로 반환

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("QuesMod.ScsTargetList");
    var url = "/DeskPlusEIP/Opinion/Ws/Admin/Content/QuesMod.asmx/GetTargetList";
    AjaxService(url, data, obj);
}
QuesMod.ScsTargetList = function (data, status) {
    $('#phTarget').setTemplateURL('/DeskPlusEIP/Opinion/Jt/Admin/Content/JtQuesTargetList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phTarget').processTemplate(data.d);
    setTimeout(function () { QuesMod.Resource('TARGET'); }, 100);
}

/**************************************************
작성자   : withsuki
작성일자 :
작성목적 : 대상자추가
***************************************************/
QuesMod.TargetAdd = function () {
    ParamArray = { 'func': 'TargetAddWs' };
    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Opinion/Folder/GetUserDept.aspx";
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "TargetAdd", "500", "477");
}
/**************************************************
작성자   : withsuki
작성일자 :
작성목적 : 대상자추가(웹서비스호출)
***************************************************/
TargetAddWs = function (_selv) {
    if (_selv != "") {
        var arrID = "";
        var selStr = _selv.split(';');
        $.each(selStr, function (i, v) {
            var arrVal = v.split('|');
            arrID += arrVal[0] + "-" + arrVal[1] + ";";        /* 구분-아이디;구분-아이디; */
        });
        ParamArray = { 'quesID': QuesID, 'arrID': arrID };
        CallEncrypt(CallGetParam());
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("QuesMod.GetTargetList");  //대상리스트
        var url = "/DeskPlusEIP/Opinion/Ws/Admin/Content/QuesMod.asmx/InsTarget";
        AjaxService(url, data, obj);
    }
}

/**************************************************
작성자   : withsuki
작성일자 :
작성목적 : 대상자삭제
***************************************************/
QuesMod.TargetDel = function (_targetID) {
    if (window.confirm(R_Delete)) {
        ParamArray = { 'targetID': _targetID };
        CallEncrypt(CallGetParam());
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("QuesMod.GetTargetList");  //대상리스트
        var url = "/DeskPlusEIP/Opinion/Ws/Admin/Content/QuesMod.asmx/DelTarget";
        AjaxService(url, data, obj);
    }
}

/**************************************************
작성자   : withsuki
작성일자 :
작성목적 : 관리자리스트
***************************************************/
QuesMod.GetAdminList = function () {
    ParamArray = { 'quesID': QuesID };
    CallEncrypt(CallGetParam());        //배열값을 파라미터형태로 반환

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("QuesMod.ScsAdminList");
    var url = "/DeskPlusEIP/Opinion/Ws/Admin/Content/QuesMod.asmx/GetAdminList";
    AjaxService(url, data, obj);
}
QuesMod.ScsAdminList = function (data, status) {
    $('#phAdmin').setTemplateURL('/DeskPlusEIP/Opinion/Jt/Admin/Content/JtQuesAdminList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phAdmin').processTemplate(data.d);
    setTimeout(function () { QuesMod.Resource('ADMIN'); }, 100);
}

/**************************************************
작성자   : withsuki
작성일자 :
작성목적 : 관리자추가
***************************************************/
QuesMod.AdminAdd = function () {
    ParamArray = { 'func': 'AdminAddWs', 'NoGroup': '1' };
    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Opinion/Folder/GetUserDept.aspx";
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "AdminAdd", "500", "477");
}
/**************************************************
작성자   : withsuki
작성일자 :
작성목적 : 대상자추가(웹서비스호출)
***************************************************/
AdminAddWs = function (_selv) {
    if (_selv != "") {
        var arrID = "";
        var selStr = _selv.split(';');
        $.each(selStr, function (i, v) {
            var arrVal = v.split('|');
            arrID += arrVal[1] + ";";        /* 아이디;아이디; */
        });

        ParamArray = { 'quesID': QuesID, 'arrID': arrID };
        CallEncrypt(CallGetParam());
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("QuesMod.GetAdminList");  //관리자리스트
        var url = "/DeskPlusEIP/Opinion/Ws/Admin/Content/QuesMod.asmx/InsAdmin";
        AjaxService(url, data, obj);
    }
}
/**************************************************
작성자   : withsuki
작성일자 :
작성목적 : 관리자삭제
***************************************************/
QuesMod.AdminDel = function (_adminID) {
    if (window.confirm(R_Delete)) {
        ParamArray = { 'adminID': _adminID };
        CallEncrypt(CallGetParam());
        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("QuesMod.GetAdminList");  //관리자리스트
        var url = "/DeskPlusEIP/Opinion/Ws/Admin/Content/QuesMod.asmx/DelAdmin";
        AjaxService(url, data, obj);
    }
}

/**************************************************
작성자   : withsuki
작성일자 :
작성목적 : 다국어처리
***************************************************/
QuesMod.Resource = function (sec) {
    //    switch (sec.toUpperCase()) {
    //        case "ATTACH":  //첨부파일리스트
    //            $("#jtAttach_fileName").text(R_FileName);
    //            $("#jtAttach_fileType").text(R_FileType);
    //            $("#jtAttach_fileLen").text(R_FileLen);
    //            $("#jtAttach_filedelete").text(R_FileDel);
    //            break;
    //        case "SPOOL":   //첨부파일
    //            $("#j_spool_fileName").text(R_FileName);
    //            $("#j_spool_fileType").text(R_FileType);
    //            $("#j_spool_fileLen").text(R_FileLen);
    //            $("#j_spool_filedelete").text(R_FileDel);
    //            break;
    //        case "UNIT":
    //            $("#jtQUL_F1").text(R_JTF7); $("#jtQUL_F2").text(R_JTF8);
    //            $("#jtQUL_F3").text(R_JTF9); $("#jtQUL_F4").text(R_JTF10);
    //            break;
    //        case "TARGET":
    //            $("#jtQTL_F1").text(R_JTF11); $("#jtQTL_F2").text(R_JTF10);
    //            break;
    //        case "ADMIN":
    //            $("#jtQAL_F1").text(R_JTF12); $("#jtQAL_F2").text(R_JTF10);
    //            break;
    //    }

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_JTNoData); }
    if ($(".jtNoData") != undefined) { $(".jtNoData").text(R_JTNoData); }
}