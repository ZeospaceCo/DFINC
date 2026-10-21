View = {}

/**************************************************
작성자   : famo23
작성일자 : 2012-02-22
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {
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

        //목록
        $('#cbtnList').click(function (e) {
            e.preventDefault();
            View.GoList();
        });

        //이미지 파일
        View.GetFileList();

        //자원 담당자
        View.GetUserList();
    });
});

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-22
작성목적 : 이미지 파일 리스트 (웹서비스 호출)
**********************************************************************************************/
View.GetFileList = function () {
    ParamArray = {
        'listID': ListID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessFileList");
    var url = "/DeskPlusEIP/RM/Ws/Admin/Content/View.asmx/GetFileList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-22
작성목적 : 이미지 파일 리스트 (웹서비스 값 받기)
**********************************************************************************************/
View.SuccessFileList = function (data, status) {
    $('#phFile').setTemplateURL('/DeskPlusEIP/RM/Jt/Admin/Content/JTFile.htm?seq=' + Guids());
    $('#phFile').processTemplate(data.d);

}



/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-22
작성목적 : 첨부파일 다운로드
**********************************************************************************************/
View.FileDownload = function (filePath) {
    var attachFilePath = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p=" + filePath;
    window.open(attachFilePath);
    //WindowObjectReference = window.open(attachFilePath, "FileDownload", "menubar=no,location=no,resizable=no,scrollbars=no,status=no");
}

/**************************************************************
작성자 : famo23
작성일자 : 2012-02-22
작성목적 : 목록
**************************************************************/
View.GoList = function () {
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
    location.href = "List.aspx?p=" + GetEncryptValue;
}

/**************************************************************
작성자 : famo23
작성일자 : 2012-02-22
작성목적 : 자원 삭제 (웹서비스 호출)
**************************************************************/
View.Del = function () {
    if (confirm(R_DelList)) {
        __doPostBack('lbtnConfirm', '');
    }
}

/**************************************************************
작성자 : famo23
작성일자 : 2012-02-22
작성목적 : 자원 수정
**************************************************************/
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
    location.href = "Mod.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-28
작성목적 : 자원담당자 리스트 (웹서비스 호출)
**********************************************************************************************/
View.GetUserList = function () {
    ParamArray = {
        'listID': ListID
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("View.SuccessUserList");
    var url = "/DeskPlusEIP/RM/Ws/Admin/Content/View.asmx/GetUserList";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-28
작성목적 : 자원담당자 리스트 (웹서비스 값 받기)
**********************************************************************************************/
View.SuccessUserList = function (data, status) {
    $('#phUser').setTemplateURL('/DeskPlusEIP/RM/Jt/Admin/Content/JTUser.htm?seq=' + Guids());
    $('#phUser').processTemplate(data.d);

}


/**************************************************
작성자   : famo23
작성일자 : 2012-03-16
작성목적 : 인쇄
***************************************************/
View.printElem = function (options) {
    $('#toPrint').append("<link rel='stylesheet' type='text/css' href='" + FullDomainPath + "/DeskPlusEIP/Util/Print/Print.css'>"); //크롬에서 배경 그래픽 적용
    $('#toPrint').printElement(options);
}