Register = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        // 금주 일정
        $('#cbtnBringNow').click(function (e) {
            e.preventDefault();

            if (window.confirm(R_ShallAddNowJob)) {
                NowNext = "now";
                Register.Bring();
                $("#tbthisWeek").focus();
            };
        });

        // 차주 일정
        $('#cbtnBringNext').click(function (e) {
            e.preventDefault();

            if (window.confirm(R_ShallAddNextJob)) {
                NowNext = "next";
                Register.Bring();
                $("#tbNextWeek").focus();
            }
        });

        //확인요청
        $('#cbtnRequest').click(function (e) {
            e.preventDefault();
            Register.Write("2");
        });

        //저장
        $('#cbtnReport').click(function (e) {
            e.preventDefault();
            Register.Write("1");
        });

        //취소
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            Register.Cancel()
        });

        //보고내용 가져오기
        $('#cbtnBringReporter').click(function (e) {
            e.preventDefault();

            ParamArray = {
                'startDate': NowStartDate,
                'gubun': '1'
            };

            CallEncrypt(CallGetParam());
            var url = "ConfirmUserList.aspx?p=" + GetEncryptValue;
            PopupWin(400, 400, url, "", "", true);
        });

        //flash 첨부파일 추가
        $('#cbtnSwfFileAdd').click(function (e) {
            e.preventDefault();
            Register.AttachSwfFile();
        });

        Register.GetSpoolFileList();
    });
});

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 나의 일정 가져오기
***************************************************/
Register.Bring = function () {
    var url = "/DeskPlusEIP/Report/Ws/Content/Register.asmx/GetPimMySche";

    if (NowNext == "now") {
        ParamArray = {
            'startDate': NowStartDate
        };
    } else if (NowNext == "next") {
        ParamArray = {
            'startDate': NextStartDate
        };
    }

    var obj = eval("Register.SuccessBring");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2014-10-28
작성목적 : 성공시 게시판 리스트 표시
**********************************************************************************************/
Register.SuccessBring = function (data, status) {
    if (data.d == null) return;
    var objList = data.d;

    setTimeout(function () {
        $.each(objList, function (index, info) {
            if (NowNext == "now") {
                $("#tbthisWeek").append("[" + info.StartDate + "] : " + info.Title);
                $("#tbthisWeek").append("\r");
            } else if (NowNext == "next") {
                $("#tbNextWeek").append("[" + info.StartDate + "] : " + info.Title);
                $("#tbNextWeek").append("\r");
            }
        });
    }, 1);
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 취소
***************************************************/
Register.Cancel = function () {
    ParamArray = {
        'years': Years,
        'months': Months,
        'targetUserID': TargetUserID,
        'targetDeptID ': TargetDeptID,
        'weeks': Weeks,
        'flag': Flag,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'listType': ListType,
        'order': Order
    };

    CallEncrypt(CallGetParam());
    location.href = ListType + ".aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 보고
***************************************************/
Register.Write = function (_arg) {
    var authUser = $("#lblName").text();

    if (jQuery.trim(authUser) == "" && _arg == "2") {
        alert(R_CheckAuthUser);
        return;
    }

    var validator = new Validator($("#form1"));
    validator.checkRequired("tbthisWeek", R_ThisWeek, true);
    //validator.checkRequired("tbNextWeek", R_NextWeek, true);
    if (!validator.validate()) return;

    __doPostBack('lbtnWrite', _arg);
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : flash 첨부파일 추가
***************************************************/
Register.AttachSwfFile = function () {
    ParamArray = {
        'swfUploadGUID': SwfUploadGUID,
        'fileType': FileType,
        'fileTypeDesc': FileTypeDesc,
        'moduleName': 'REPORT',
        'callBackEvent': "Register.GetSpoolFileList"
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
Register.GetSpoolFileList = function () {
    ParamArray = {
        'swfUploadGUID': SwfUploadGUID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Register.SuccessSpoolFileList");
    var url = "/DeskPlusEIP/Common/Ws/SpoolFile.asmx/GetSpoolFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2014-10-28
작성목적 : SPOOL 첨부파일 리스트 보여주기
**********************************************************************************************/
Register.SuccessSpoolFileList = function (data, status) {
    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Common/Jt/Attach/JTSpoolFile.htm?seq=' + Guids());

    $('#placeholder_file').processTemplate(data.d);

    setTimeout(function () {
        Register.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2014-10-28
작성목적 : 다국어 처리
**********************************************************************************************/
Register.Resource = function () {
    $("#j_spool_fileName")[0].innerHTML = R_FileName;
    $("#j_spool_fileType")[0].innerHTML = R_FileType;
    $("#j_spool_fileLen")[0].innerHTML = R_FileLen;
    $("#j_spool_filedelete")[0].innerHTML = R_FileDel;

    if ($("#j_spool_screenshot")[0] != undefined) {
        $("#j_spool_screenshot")[0].innerHTML = R_ScreenShot;
    }

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2014-10-28
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
        var obj = eval("Register.SuccessSpoolFileList");
        var url = "/DeskPlusEIP/Common/Ws/SetCommon.asmx/DelSpoolFileList";

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2014-10-28
작성목적 : 선택한 보고내역자 정보 가져오기
**********************************************************************************************/
function GetReportUser(userList) {
    $.each(userList, function (key, values) {
        Register.BringReportUser(values);
    });
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 보고내역 가져오기
***************************************************/
Register.BringReportUser = function (p_listID) {
    var url = "/DeskPlusEIP/Report/Ws/Content/Register.asmx/GetReporter";

    ParamArray = {
        'listID': p_listID
    };

    var obj = eval("Register.SuccessBringReportUser");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2014-10-28
작성목적 : 성공시 보고내역 가져오기
**********************************************************************************************/
Register.SuccessBringReportUser = function (data, status) {
    if (data.d == null) return;
    var objList = data.d;

    setTimeout(function () {
        $.each(objList, function (index, info) {
            if (info.NowJob != "") {
                $("#tbthisWeek").append("\r");
                $("#tbthisWeek").append("== [" + info.TitleBase + " " + info.NameBase + "] == \r");
                $("#tbthisWeek").append("\r");
                $("#tbthisWeek").append(info.NowJob);
                $("#tbthisWeek").append("\r\r");
            }

            if (info.NextJob != "") {
                $("#tbNextWeek").append("\r");
                $("#tbNextWeek").append("== [" + info.TitleBase + " " + info.NameBase + "] == \r");
                $("#tbNextWeek").append("\r");
                $("#tbNextWeek").append(info.NextJob);
                $("#tbNextWeek").append("\r\r");
            }

            if (info.Issue != "") {
                $("#tbIssue").append("\r");
                $("#tbIssue").append("== [" + info.TitleBase + " " + info.NameBase + "] == \r");
                $("#tbIssue").append("\r");
                $("#tbIssue").append(info.Issue);
                $("#tbIssue").append("\r\r");
            }
        });
    }, 1);
}