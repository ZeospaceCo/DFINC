View = {}

$(function () {
    $(document).ready(function () {
        //인쇄
        $('#cbtnPrint').click(function (e) {
            e.preventDefault();
            View.printElem({ leaveOpen: false, printMode: 'popup' });
        });

        //닫기
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            self.close();
        });

        //이전
        $('#pvWeek').click(function (e) {
            e.preventDefault();

            var folderID = $('#ddlFolder').val();
            ParamArray = {
                'folderID': folderID,
                'startDate': BeforeStartDate
            };

            CallEncrypt(CallGetParam());
            location = "ViewPT.aspx?p=" + GetEncryptValue;
        });

        //다음
        $('#nxWeek').click(function (e) {
            e.preventDefault();

            var folderID = $('#ddlFolder').val();
            ParamArray = {
                'folderID': folderID,
                'startDate': NextStartDate
            };

            CallEncrypt(CallGetParam());
            location = "ViewPT.aspx?p=" + GetEncryptValue;
        });

        //댓글 추가
        $('#cbtnShortReply').click(function (e) {
            e.preventDefault();
            View.AddShortReply();
            View.Resource();
        });

        //분류별 가져오기
        $('#ddlFolder').change(function (e) {
            e.preventDefault();

            var folderID = $('#ddlFolder').val();
            ParamArray = {
                'folderID': folderID,
                'startDate': StartDate
            };

            CallEncrypt(CallGetParam());
            location = "ViewPT.aspx?p=" + GetEncryptValue;
        });

        View.GetShortReplyList();
        View.GetAttachFileList();
        View.Resource();

        $("#zeoModalContact").draggable({ disabled: false });

        //큰 이미지 줄이기
        $(".shrink-me").resize({ maxWidth: 800 });
    });
});

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 인쇄
***************************************************/
View.printElem = function (options) {
    $('#toPrint').append("<link rel='stylesheet' type='text/css' href='" + FullDomainPath + "/DeskPlusEIP/Util/Print/Print.css'>"); //크롬에서 배경 그래픽 적용
    $('#toPrint').printElement(options);
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 다국어 처리
**********************************************************************************************/
View.Resource = function () {
//    $("#j_fileName")[0].innerHTML = R_FileName;
//    $("#j_fileType")[0].innerHTML = R_FileType;
//    $("#j_fileLen")[0].innerHTML = R_FileLen;
//    $("#j_download")[0].innerHTML = R_FileDown;

//    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }

//    $("#j_Name").text(R_Name);
//    $("#j_Content").text(R_Contents);
//    $("#j_RegDate").text(R_RegDate);
//    if ($(".r_mod") != undefined) { $(".r_mod").text(R_Modify); }
//    if ($(".r_del") != undefined) { $(".r_del").text(R_Del); }
    if ($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }
    if ($(".ModMsg") != undefined) { $(".ModMsg").text(R_Modify); }
    if ($(".DelMsg") != undefined) { $(".DelMsg").text(R_Del); }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-20
작성목적 : 댓글  리스트 호출
**********************************************************************************************/
View.GetShortReplyList = function () {
    ParamArray = {
        'listID': ListID
    };

    CallEncrypt(CallGetParam())

    data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessShortReplyList");
    var url = "/DeskPlusEIP/Report/Ws/Content/View.asmx/GetShortReplyList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 댓글 리스트 성공시 페이지 표시
**********************************************************************************************/
View.SuccessShortReplyList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Report/Jt/Content/JTReple.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
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
작성일자 : 2011-04-20
작성목적 : 파일  리스트 호출
**********************************************************************************************/
View.GetAttachFileList = function () {
    ParamArray = {
        'listID': ListID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessAttachFile");
    var url = "/DeskPlusEIP/Report/Ws/Content/Mod.asmx/GetAttachFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 첨부파일 리스트 보여주기
**********************************************************************************************/
View.SuccessAttachFile = function (data, status) {
    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Report/Jt/Content/JTAttachFile.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate(data.d);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-20
작성목적 : 댓글  리스트 호출
**********************************************************************************************/
View.GetShortReplyList = function () {
    if (ListID == "0") { $('#cbtnShortReply').hide(); }

    ParamArray = {
        'listID': ListID
    };

    CallEncrypt(CallGetParam())

    data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessShortReplyList");
    var url = "/DeskPlusEIP/Report/Ws/Content/View.asmx/GetShortReplyList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 댓글 리스트 성공시 페이지 표시
**********************************************************************************************/
View.SuccessShortReplyList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Report/Jt/Content/JTReple.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 댓글
***************************************************/
View.AddShortReply = function () {
    ParamArray = {
        'listID': ListID,
        'gubun': 'View'
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
        'gubun': 'View'
    };

    CallEncrypt(CallGetParam())
    var url = "ShortReplyMod.aspx?p=" + GetEncryptValue;
    PopupWin(500, 200, url, "-500", "", true);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-22
작성목적 : 댓글 삭제
**********************************************************************************************/
DelReple = function (shortReplyID) {
    if (confirm(R_ApplyDelete)) {
        ParamArray = {
            'listID': ListID,
            'ShortReplyID': shortReplyID
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("View.SuccessDelShortReply");
        var url = "/DeskPlusEIP/Report/Ws/Content/View.asmx/DelShortReply";
        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 댓글 삭제 성공
**********************************************************************************************/
View.SuccessDelShortReply = function (data, status) {
    if (data.d) {
        View.GetShortReplyList();
        View.Resource();
    }
}