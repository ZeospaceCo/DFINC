RegisterInform = {}

$(function () {
    $(document).ready(function () {
        // 금주 일정
        $('#cbtnBringNow').click(function (e) {
            e.preventDefault();

            if (window.confirm(R_ShallAddNowJob)) {
                NowNext = "now";
                RegisterInform.Bring();
                $("#tbthisWeek").focus();
            };
        });

        // 차주 일정
        $('#cbtnBringNext').click(function (e) {
            e.preventDefault();

            if (window.confirm(R_ShallAddNextJob)) {
                NowNext = "next";
                RegisterInform.Bring();
                $("#tbNextWeek").focus();
            }
        });

        //저장
        $('#cbtnReport').click(function (e) {
            e.preventDefault();
            RegisterInform.Add();
        });

        //취소
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            RegisterInform.Cancel();
        });

        //보고내용 가져오기
        $('#cbtnBringReporter').click(function (e) {
            e.preventDefault();

            ParamArray = {
                'startDate': NowStartDate,
                'gubun': '2'
            };

            CallEncrypt(CallGetParam());
            var url = "ConfirmUserList.aspx?p=" + GetEncryptValue;
            PopupWin(400, 400, url, "", "", true);
        });

        //보고체계지정 가져오기
        $('#ddlFolder').change(function (e) {
            e.preventDefault();

            var folderID = $('#ddlFolder').val();

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
                'order': Order,
                'startDate': StartDate,
                'endDate': EndDate,
                'listID': ListID,
                'folderID': folderID
            };

            CallEncrypt(CallGetParam())

            location = "RegisterInform.aspx?p=" + GetEncryptValue;
        });

        //PT 보기
        $('#cbtnPT').click(function (e) {
            e.preventDefault();

            var folderID = $('#ddlFolder').val();

            ParamArray = {
                'folderID': folderID,
                'startDate': NowStartDate
            };

            CallEncrypt(CallGetParam());

            var url = "ViewPT.aspx";
            var param = "p=" + GetEncryptValue;
            WindowObjectReference = PopupWinOpen(url, param, "PT" + Guids(), "1200", "800", 'menubar=no,location=no,resizable=yes,scrollbars=yes,status=no');
        });

        //댓글 추가
        $('#cbtnShortReply').click(function (e) {
            e.preventDefault();
            RegisterInform.AddShortReply();
        });

        //flash 첨부파일 추가
        $('#cbtnSwfFileAdd').click(function (e) {
            e.preventDefault();
            RegisterInform.AttachSwfFile();
        });

        var thisWeekH = $('#tbthisWeek').prop("scrollHeight");
        if (thisWeekH > 150) {
            $('#tbthisWeek').height(20 + thisWeekH);
            $('#tbNextWeek').height(20 + thisWeekH);
        } else {
            $('#tbthisWeek').height(150);
            $('#tbNextWeek').height(150);
        }

        var issueH = $('#tbIssue').prop("scrollHeight");
        if (issueH > 150) { $('#tbIssue').height(20 + issueH); } else { $('#tbIssue').height(150); }

        RegisterInform.GetAttachFileList();
        RegisterInform.GetSpoolFileList();
        RegisterInform.GetShortReplyList();
    });
});

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 나의 일정 가져오기
***************************************************/
RegisterInform.Bring = function () {
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

    var obj = eval("RegisterInform.SuccessBring");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 나의 일정 가져오기
**********************************************************************************************/
RegisterInform.SuccessBring = function (data, status) {
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
작성목적 : 수정 취소시
***************************************************/
RegisterInform.Cancel = function () {
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
작성목적 : 추가 및 수정
***************************************************/
RegisterInform.Add = function () {
    var validator = new Validator($("#form1"));
    validator.checkRequired("tbthisWeek", R_ThisWeek, true);
    //validator.checkRequired("tbNextWeek", R_NextWeek, true);
    if (!validator.validate()) return;

    var folderID = $("#ddlFolder").val();
    if (folderID == "") { alert(R_SelInformSystem); return; }

    __doPostBack('lbtnWrite', '');
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-20
작성목적 : 파일  리스트 호출
**********************************************************************************************/
RegisterInform.GetAttachFileList = function () {
    ParamArray = {
        'listID': ListID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("RegisterInform.SuccessAttachFile");
    var url = "/DeskPlusEIP/Report/Ws/Content/Mod.asmx/GetAttachFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 첨부파일 리스트 보여주기
**********************************************************************************************/
RegisterInform.SuccessAttachFile = function (data, status) {
    $('#placeholder_fileList').setTemplateURL('/DeskPlusEIP/Report/Jt/Content/JTDelFile.htm?seq=' + Guids());
    $('#placeholder_fileList').processTemplate(data.d);

    setTimeout(function () {
        RegisterInform.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 다국어 처리
**********************************************************************************************/
RegisterInform.Resource = function () {
//    $("#j_fileName")[0].innerHTML = R_FileName;
//    $("#j_fileType")[0].innerHTML = R_FileType;
//    $("#j_fileLen")[0].innerHTML = R_FileLen;
//    $("#j_filedelete")[0].innerHTML = R_Del;

//    if ($("#j_screenshot")[0] != undefined) {
//        $("#j_screenshot")[0].innerHTML = R_ScreenShot;
//    }

//    $("#j_Name").text(R_Name);
//    $("#j_Content").text(R_Contents);
//    $("#j_RegDate").text(R_RegDate);
//    if ($(".r_mod") != undefined) { $(".r_mod").text(R_Modify); }
//    if ($(".r_del") != undefined) { $(".r_del").text(R_Del); }
    if ($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }
    if ($(".ModMsg") != undefined) { $(".ModMsg").text(R_Modify); }
    if ($(".DelMsg") != undefined) { $(".DelMsg").text(R_Del); }

//    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
//    if ($("#j_spool_fileName") != undefined) { $("#j_spool_fileName").text(R_FileName); }
//    if ($("#j_spool_fileType") != undefined) { $("#j_spool_fileType").text(R_FileType); }
//    if ($("#j_spool_fileLen") != undefined) { $("#j_spool_fileLen").text(R_FileLen); }
//    if ($("#j_spool_filedelete") != undefined) { $("#j_spool_filedelete").text(R_Del); }
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : flash 첨부파일 추가
***************************************************/
RegisterInform.AttachSwfFile = function () {
    ParamArray = {
        'swfUploadGUID': SwfUploadGUID,
        'fileType': FileType,
        'fileTypeDesc': FileTypeDesc,
        'moduleName': 'REPORT',
        'callBackEvent': "RegisterInform.GetSpoolFileList"
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
RegisterInform.GetSpoolFileList = function () {
    ParamArray = {
        'swfUploadGUID': SwfUploadGUID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("RegisterInform.SuccessSpoolFileList");
    var url = "/DeskPlusEIP/Common/Ws/SpoolFile.asmx/GetSpoolFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : SPOOL 첨부파일 리스트 보여주기
**********************************************************************************************/
RegisterInform.SuccessSpoolFileList = function (data, status) {
    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Common/Jt/Attach/JTSpoolFile.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate(data.d);

    RegisterInform.Resource();
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
        var obj = eval("RegisterInform.SuccessSpoolFileList");
        var url = "/DeskPlusEIP/Common/Ws/SetCommon.asmx/DelSpoolFileList";

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 실제 파일 삭제 처리
**********************************************************************************************/
function DelFile(p_attachFileID) {
    if (window.confirm(R_DelAttach)) {
        ParamArray = {
            'listID': ListID,
            'attachFileID': p_attachFileID
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("RegisterInform.SuccessFileList");
        var url = "/DeskPlusEIP/Report/Ws/Content/Mod.asmx/DelAttachFile";

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 첨부파일 다운로드
**********************************************************************************************/
FileDownload = function (filePath, fileName, fileGuid) {
    var attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + filePath;
    window.open(attachFilePath);
    //WindowObjectReference = window.open(attachFilePath, "FileDownload", "menubar=no,location=no,resizable=no,scrollbars=no,status=no");
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 첨부파일 리스트 보여주기
**********************************************************************************************/
RegisterInform.SuccessFileList = function (data, status) {
    if (data.d == true) {
        RegisterInform.GetAttachFileList();
    }
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 선택한 보고내역자 정보 가져오기
**********************************************************************************************/
function GetReportUser(userList) {
    $.each(userList, function (key, values) {
        RegisterInform.BringReportUser(values);
    });
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 보고내역 가져오기
***************************************************/
RegisterInform.BringReportUser = function (p_listID) {
    var url = "/DeskPlusEIP/Report/Ws/Content/Register.asmx/GetReporter";

    ParamArray = {
        'listID': p_listID
    };

    var obj = eval("RegisterInform.SuccessBringReportUser");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 보고내역 가져오기
**********************************************************************************************/
RegisterInform.SuccessBringReportUser = function (data, status) {
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

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 조회현황
***************************************************/
RegisterInform.ReadList = function () {
    ParamArray = {
        'listID': ListID
    };

    CallEncrypt(CallGetParam())

    var url = "/DeskPlusEIP/Report/Content/ReadList.aspx?p=" + GetEncryptValue;

    PopupWin(500, 400, url, "", "", true);
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 댓글
***************************************************/
RegisterInform.AddShortReply = function () {
    ParamArray = {
        'listID': ListID,
        'gubun': 'RegisterInform'
    };

    CallEncrypt(CallGetParam())

    var url = "ShortReply.aspx?p=" + GetEncryptValue;
    PopupWin(500, 200, url, "-500", "-220", false);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-22
작성목적 : 댓글 수정화면 보여주기
**********************************************************************************************/
ModShortReply = function (shortReplyID, listID) {
    ParamArray = {
        'shortReplyID': shortReplyID,
        'listID': listID,
        'gubun': 'RegisterInform'
    };

    CallEncrypt(CallGetParam())
    var url = "ShortReplyMod.aspx?p=" + GetEncryptValue;
    PopupWin(500, 200, url, "-500", "", true);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-20
작성목적 : 댓글  리스트 호출
**********************************************************************************************/
RegisterInform.GetShortReplyList = function () {
    if (ListID == "0") { $('#cbtnShortReply').hide(); }

    ParamArray = {
        'listID': ListID
    };

    CallEncrypt(CallGetParam())

    data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("RegisterInform.SuccessShortReplyList");
    var url = "/DeskPlusEIP/Report/Ws/Content/View.asmx/GetShortReplyList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 댓글 리스트 성공시 페이지 표시
**********************************************************************************************/
RegisterInform.SuccessShortReplyList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Report/Jt/Content/JTReple.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-22
작성목적 : 댓글 삭제
**********************************************************************************************/
DelReple = function (shortReplyID) {
    if (confirm(R_ReallyApplyDelete)) {
        ParamArray = {
            'listID': ListID,
            'ShortReplyID': shortReplyID
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("RegisterInform.SuccessDelShortReply");
        var url = "/DeskPlusEIP/Report/Ws/Content/View.asmx/DelShortReply";
        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 댓글 삭제 성공
**********************************************************************************************/
RegisterInform.SuccessDelShortReply = function (data, status) {
    if (data.d) {
        RegisterInform.GetShortReplyList();
        RegisterInform.Resource();
    }
}