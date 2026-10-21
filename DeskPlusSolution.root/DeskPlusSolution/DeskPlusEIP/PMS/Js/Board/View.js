View = {}

/**************************************************
초기화
***************************************************/
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

        //조회현황
        $('#cbtnListView').click(function (e) {
            e.preventDefault();
            View.ListView();
        });

        //수정
        $('#cbtnMod').click(function (e) {
            e.preventDefault();
            View.Mod();
            // __doPostBack("LBTNProcess", "MOD");
        });

        //삭제
        $('#cbtnDel').click(function (e) {
            e.preventDefault();

            if (window.confirm(DeleteMsg)) {
                __doPostBack("LBTNProcess", "DEL");
            }
        });

        //인쇄
        $('#cbtnPrint').click(function (e) {
            e.preventDefault();
            View.Print({ leaveOpen: false, printMode: 'popup' });
        });

        //목록
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();

            //팝업창에서 목록은 닫기로~
            if (PageName.toUpperCase() == "VIEWPOPUP.ASPX") {
                self.close();
            } else {
                View.Cancel();
            }
        });

        //짧은 답글
        $('#cbtnShortReply').click(function (e) {
            e.preventDefault();
            View.ListShortReply();
        });

        View.Init();
    });
});

/**********************************************************************************************
[초기화]
**********************************************************************************************/
View.Init = function () {
    //첨부파일 리스트
    View.GetAttachFileList();

    //짧은댓글 리스트
    View.GetShortReplyList();

    setTimeout(function () {
        View.Resource();
    }, 100);
}

/**********************************************************************************************
[취소]
**********************************************************************************************/
View.Cancel = function () {
    //팝업창일 경우,
    if (Popup == "1") {
        self.close();
        return;
    }

    ParamArray = {
        'projectID': ProjectID,
        'menuID': MenuID,
        'menuType': MenuType,
        'rqProjectID': RqProjectID,
        'currentPage': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'searchTermValue': SearchTermValue,
        'searchTerm': SearchTerm,
        'recordPerPage': RecordPerPage
    };

    CallEncrypt(CallGetParam());
    location.href = "JList.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
[조회명단]
**********************************************************************************************/
View.ListView = function () {
    ParamArray = {
        'projectID': ProjectID,
        'menuID': MenuID,
        'boardID': BoardID,
        'currentPage': CurrentPage
    };

    CallEncrypt(CallGetParam());
    var url = "ReadList.aspx?p=" + GetEncryptValue;
    // WindowObjectReference = PopupWinOpen(url, "", "Register", "500", "374")

    //  var url = "/DeskPlusEIP/Board/Content/ReadList.aspx?p=" + GetEncryptValue;

    PopupWin(500, 400, url, "", "", true);
}

/**********************************************************************************************
[작성하기]
**********************************************************************************************/
View.Add = function () {
    ParamArray = {
        'projectID': ProjectID,
        'menuID': MenuID,
        'menuType': MenuType,
        'rqProjectID': RqProjectID,
        'currentPage': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'searchTermValue': SearchTermValue,
        'searchTerm': SearchTerm
    };

    CallEncrypt(CallGetParam());
    var param = "p=" + GetEncryptValue;

    self.location.href = "Add.aspx?" + param;
}

/**********************************************************************************************
[인쇄 하기]
**********************************************************************************************/
View.Print = function (options) {
    $('#toPrint').append("<link rel='stylesheet' type='text/css' href='" + FullDomainPath + "/DeskPlusEIP/Util/Print/Print.css'>"); //크롬에서 배경 그래픽 적용
    $('#toPrint').printElement(options);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-20
작성목적 : 짧은 댓글  리스트 호출
**********************************************************************************************/
View.GetShortReplyList = function () {
    ParamArray = {
        'projectID': ProjectID,
        'menuID': MenuID,
        'boardID': BoardID
    };

    CallEncrypt(CallGetParam())

    data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessShortReplyList");
    var url = "/DeskPlusEIP/PMS/Ws/Board/View.asmx/GetShortReplyList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 짧은 답글 리스트 성공시 페이지 표시
**********************************************************************************************/
View.SuccessShortReplyList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/PMS/Jt/Board/JTReple.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);

    setTimeout(function () {
        View.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-22
작성목적 : 짧은댓글 삭제
**********************************************************************************************/
View.DelReple = function (shortReplyID) {
    if (confirm(R_ReallyApplyDelete)) {
        ParamArray = {
            'projectID': ProjectID,
            'menuID': MenuID,
            'boardID': BoardID,
            'boardShortReplyID': shortReplyID
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("View.SuccessDelShortReply");
        var url = "/DeskPlusEIP/PMS/Ws/Board/View.asmx/DelShortReply";
        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 짧은댓글 삭제 성공
**********************************************************************************************/
View.SuccessDelShortReply = function (data, status) {
    if (data.d) {
        View.GetShortReplyList();
    }
}

/**********************************************************************************************
[짧은 답글 리스트]
**********************************************************************************************/
View.ListShortReply = function () {
    ParamArray = {
        'projectID': ProjectID,
        'menuID': MenuID,
        'boardID': BoardID
    };

    CallEncrypt(CallGetParam());

    var url = "ShortReply.aspx?p=" + GetEncryptValue;

    PopupWin(500, 200, url, "-500", "-220", false);
}

/**********************************************************************************************
[파일 다운로드]
**********************************************************************************************/
View.NavigateFileUrl = function (url, fileName) {
    ParamArray = {
        'filePath': escape(url),
        'fileName': escape(fileName)
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx";
    var param = url + "?p=" + GetEncryptValue;

    location.href = param;
}

/**********************************************************************************************
[답변이동]
**********************************************************************************************/
View.Reply = function () {
    ParamArray = {
        'projectID': ProjectID,
        'menuID': MenuID,
        'menuType': MenuType,
        'rqProjectID': RqProjectID,
        'currentPage': CurrentPage,
        'boardID': BoardID,
        'depth': Number(Depth) + 1,
        'listOrder': Number(ListOrder) + 1,
        'groupID': GroupID,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'searchTermValue': SearchTermValue,
        'searchTerm': SearchTerm
    };

    CallEncrypt(CallGetParam());

    location.href = "ReplyN.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
[수정이동]
**********************************************************************************************/
View.Mod = function () {
    ParamArray = {
        'projectID': ProjectID,
        'menuID': MenuID,
        'menuType': MenuType,
        'rqProjectID': RqProjectID,
        'boardID': BoardID,
        'depth': Depth,
        'listOrder': ListOrder,
        'groupID': GroupID,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'searchTermValue': SearchTermValue,
        'searchTerm': SearchTerm,
        'recordPerPage': RecordPerPage,
        'currentPage': CurrentPage
    };

    CallEncrypt(CallGetParam());

    location.href = "ModN.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 다국어 처리
**********************************************************************************************/
View.Resource = function () {

    /*
    $("#j_fileName")[0].innerHTML = R_FileName;
    $("#j_fileType")[0].innerHTML = R_FileType;
    $("#j_fileLen")[0].innerHTML = R_FileLen;
    $("#j_download")[0].innerHTML = R_FileDown;

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }

    $("#j_Name")[0].innerHTML = R_Name;
    $("#j_Content")[0].innerHTML = R_Contents;
    $("#j_RegDate")[0].innerHTML = R_RegDate;

    if ($(".r_mod") != undefined) {
        $(".r_mod").text(R_Mod);
    }

    if ($(".r_del") != undefined) {
        $(".r_del").text(R_FileDel);
    }
    */

    if ($(".ModMsg") != undefined) { $(".ModMsg").text(R_Mod); }
    if ($(".DelMsg") != undefined) { $(".DelMsg").text(R_FileDel); }

    if ($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-22
작성목적 : 짧은 댓글 수정화면 보여주기
**********************************************************************************************/
View.ModShortReply = function (p_projectID, p_menuID, p_boardID, p_shortReplyID) {
    ParamArray = {
        'projectID': p_projectID,
        'menuID': p_menuID,
        'boardID': p_boardID,
        'boardShortReplyID': p_shortReplyID
    };

    CallEncrypt(CallGetParam())
    var url = "ShortReplyMod.aspx?p=" + GetEncryptValue;
    PopupWin(500, 200, url, "-500", "", true);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-20
작성목적 : 파일  리스트 호출
**********************************************************************************************/
View.GetAttachFileList = function () {
    ParamArray = {
        'projectID': ProjectID,
        'menuID': MenuID,
        'boardID': BoardID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessAttachFile");
    var url = "/DeskPlusEIP/PMS/Ws/Board/View.asmx/GetAttachFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 첨부파일 리스트 보여주기
**********************************************************************************************/
View.SuccessAttachFile = function (data, status) {
    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/PMS/Jt/Board/JTAttachFile.htm?seq=' + Guids());
    $('#placeholder_file').processTemplate(data.d);
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