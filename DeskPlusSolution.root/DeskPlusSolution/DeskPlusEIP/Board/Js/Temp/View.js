View = {}

$(function () {
    $(document).ready(function () {
        //작성
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            View.Add();
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
            View.printElem({ leaveOpen: false, printMode: 'popup' });
        });

        //취소
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            View.Cancel();
        });

        //임시글을 게시판에 글올리기
        $('#cbtnMove').click(function (e) {
            e.preventDefault();
            View.Move();
        });

        View.GetAttachFileList();

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
        'categoryID': CategoryID,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    CallEncrypt(CallGetParam());
    location.href = GoPage + "?p=" + GetEncryptValue;
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
        'categoryID': CategoryID,
        'listID': ListID,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    CallEncrypt(CallGetParam());
    location.href = "ModN.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 추가
***************************************************/
View.Add = function () {
    ParamArray = {
        'categoryID': CategoryID,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    CallEncrypt(CallGetParam());
    location.href = "AddN.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 이동하기
***************************************************/
View.Move = function () {
    ParamArray = {
        'categoryID': CategoryID,
        'listID': ListID
    };

    CallEncrypt(CallGetParam())

    var url = "Move.aspx?p=" + GetEncryptValue;

    PopupWin(400, 375, url, "", "", true);
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-18
작성목적 : 임시저장 - 글올리기
**********************************************************************************************/
function GetSelBoardCategory(p_categoryID) {
    var url = "/DeskPlusEIP/Board/Ws/Temp/Move.asmx/MoveMyTemp";

    ParamArray = {
        'categoryID': p_categoryID,
        'listID': ListID
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("SuccessGetSelBoardCategory");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-18
작성목적 : 성공시, 임시저장 - 글올리기
**********************************************************************************************/
function SuccessGetSelBoardCategory(data, status) {
    var response = data.d;
    if (response) {
        alert(R_Processing);
    }
}

/**************************************************
작성자   : 이현수
작성일자 : 2011-04-11
작성목적 : 인쇄
***************************************************/
View.Print = function () {
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-20
작성목적 : 파일  리스트 호출
**********************************************************************************************/
View.GetAttachFileList = function () {
    ParamArray = {
        'categoryID': CategoryID,
        'listID': ListID,
        'screenShot': ''
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessAttachFile");
    var url = "/DeskPlusEIP/Board/Ws/Temp/View.asmx/GetMyAttachFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 첨부파일 리스트 보여주기
**********************************************************************************************/
View.SuccessAttachFile = function (data, status) {
    $('#placeholder_file').setTemplateURL('/DeskPlusEIP/Board/Jt/Temp/JTAttachFile.htm?seq=' + Guids());
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