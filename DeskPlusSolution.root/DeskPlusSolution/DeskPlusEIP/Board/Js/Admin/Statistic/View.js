View = {}

$(function () {
    $(document).ready(function () {
        //추가
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            View.Add();
        });

        //답변
        $('#cbtnReply').click(function (e) {
            e.preventDefault();
            View.Reply();
        });

        //조회
        $('#cbtnRead').click(function (e) {
            e.preventDefault();
            View.ReadList();
        });

        //평가
        $('#cbtnRate').click(function (e) {
            e.preventDefault();
            View.Rate();
        });

        //삭제
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            View.Del();
        });

        //수정
        $('#cbtnMod').click(function (e) {
            e.preventDefault();
            View.Mod();
        });

        //인쇄
        $('#cbtnPrint').click(function (e) {
            e.preventDefault();
            View.Print();
        });

        //취소
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            View.Cancel();
        });

        //댓글 추가
        $('#cbtnShortReply').click(function (e) {
            e.preventDefault();
            View.AddShortReply();
        });

        if (Option_Reple != "False") {
            View.GetShortReplyList();
        }

        View.GetAttachFileList();
        View.Resource();
    });
});

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 다국어 처리
**********************************************************************************************/
View.Resource = function () {
    $("#j_fileName")[0].innerHTML = R_FileName;
    $("#j_fileType")[0].innerHTML = R_FileType;
    $("#j_fileLen")[0].innerHTML = R_FileLen;
    $("#j_download")[0].innerHTML = R_FileDown;

    if (Option_Reple != "False") {
        $("#j_Name")[0].innerHTML = R_Name;
        $("#j_Content")[0].innerHTML = R_Contents;
        $("#j_RegDate")[0].innerHTML = R_RegDate;
    }

    if ($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 첨부파일
***************************************************/
View.AttachFile = function () {
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 목록으로 가기
***************************************************/
View.Cancel = function () {
    ParamArray = {
        'catID': CatID,
        'categoryID': CategoryID,
        'currentPage': CurrentPage,
        'searchGroup': SearchGroup,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    CallEncrypt(CallGetParam());
    location.href = "ReadCount.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 삭제
***************************************************/
View.Del = function () {
    if (window.confirm(R_ApplyDelete)) {
        __doPostBack("LBTNProcess", "DEL");
    }
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 수정
***************************************************/
View.Mod = function () {
    ParamArray = {
        'catID': CatID,
        'categoryID': CategoryID,
        'listID': ListID,
        'currentPage': CurrentPage,
        'searchGroup': SearchGroup,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    CallEncrypt(CallGetParam());
    location.href = "Mod.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 추가
***************************************************/
View.Add = function () {
    ParamArray = {
        'catID': CatID,
        'categoryID': CategoryID
    };

    CallEncrypt(CallGetParam());
    location.href = "AddN.aspx?p=" + GetEncryptValue;

}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 답변
***************************************************/
View.Reply = function () {
    ParamArray = {
        'catID': CatID,
        'categoryID': CategoryID,
        'listID': ListID,
        'currentPage': CurrentPage,
        'searchGroup': SearchGroup,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    CallEncrypt(CallGetParam());
    location.href = "ReplyN.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 조회현황
***************************************************/
View.ReadList = function () {
    ParamArray = {
        'categoryID': CategoryID,
        'listID': ListID
    };

    CallEncrypt(CallGetParam())
    var url = "/DeskPlusEIP/Board/Content/ReadList.aspx?p=" + GetEncryptValue;

    PopupWin(500, 400, url, "", "", true);
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 댓글
***************************************************/
View.AddShortReply = function () {
    ParamArray = {
        'categoryID': CategoryID,
        'listID': ListID
    };

    CallEncrypt(CallGetParam())

    var url = "ShortReply.aspx?p=" + GetEncryptValue;
    PopupWin(500, 200, url, "-500", "-220", false);
}

/**************************************************
작성자   : 이현수
작성일자 : 2011-04-11
작성목적 : 평가
***************************************************/
View.Rate = function () {
    ParamArray = {
        'categoryID': CategoryID,
        'listID': ListID
    };

    CallEncrypt(CallGetParam())
    var url = "/DeskPlusEIP/Board/Content/Rate.aspx?p=" + GetEncryptValue;

    PopupWin(600, 450, url, "", "", true);
}

/**************************************************
작성자   : 이현수
작성일자 : 2011-04-11
작성목적 : 인쇄
***************************************************/
View.Print = function () {
}

/**********************************************************************************************
작성자   : daniel
작성일자 : 2011-04-20
작성목적 : 댓글  리스트 호출
**********************************************************************************************/
View.GetShortReplyList = function () {
    ParamArray = {
        'categoryID': CategoryID,
        'listID': ListID
    };

    CallEncrypt(CallGetParam())

    data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessShortReplyList");
    var url = "/DeskPlusEIP/Board/Ws/Admin/Statistic/View.asmx/GetShortReplyList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : daniel
작성일자 : 2011-04-14
작성목적 : 댓글 리스트 성공시 페이지 표시
**********************************************************************************************/
View.SuccessShortReplyList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Board/Jt/Content/JTReple.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 첨부파일 다운로드
**********************************************************************************************/
View.FileDownload = function (filePath, fileName, fileGuid) {
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
        'categoryID': CategoryID,
        'listID': ListID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessAttachFile");
    var url = "/DeskPlusEIP/Board/Ws/Admin/Statistic/View.asmx/GetAttachFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : daniel
작성일자 : 2011-04-14
작성목적 : 첨부파일 리스트 보여주기
**********************************************************************************************/
View.SuccessAttachFile = function (data, status) {
    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Board/Jt/Content/JTAttachFile.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate(data.d);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-22
작성목적 : 댓글 수정화면 보여주기
**********************************************************************************************/
View.ModShortReply = function (shortReplyID, categoryID, listID) {
    ParamArray = {
        'shortReplyID': shortReplyID,
        'categoryID': categoryID,
        'listID': listID
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
View.DelReple = function (shortReplyID) {
    if (confirm(R_ReallyApplyDelete)) {
        ParamArray = {
            'categoryID': CategoryID,
            'listID': ListID,
            'ShortReplyID': shortReplyID
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("View.SuccessDelShortReply");
        var url = "/DeskPlusEIP/Board/Ws/Admin/Statistic/View.asmx/DelShortReply";
        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : daniel
작성일자 : 2011-04-14
작성목적 : 댓글 삭제 성공
**********************************************************************************************/
View.SuccessDelShortReply = function (data, status) {
    if (data.d) {
        View.GetShortReplyList();
        View.Resource();
    }
}