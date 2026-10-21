View = {}

$(function () {
    //인쇄
    $('#cbtnPrint').click(function (e) {
        e.preventDefault();
        View.printElem({ leaveOpen: false, printMode: 'popup' });
    });

    //닫기
    $('#cbtnList').click(function (e) {
        e.preventDefault();
        View.List();
    });

    View.Resource();

    $("#zeoModalContact").draggable({ disabled: false });
});

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 인쇄
***************************************************/
View.printElem = function (options) {
    $('#toPrint').printElement(options);
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 다국어 처리
**********************************************************************************************/
View.Resource = function () {
    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
    if ($(".NoBaseListData") != undefined) { $(".NoBaseListData").text(R_NoBaseListData); }

    if ($(".BtnNew") != undefined) { $(".BtnNew").attr('src', ImagePath + '/btn/i_new.gif'); }
    if ($(".BtnView") != undefined) { $(".BtnView").attr('src', ImagePath + '/Icon/icon_search.gif'); }
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 닫기
***************************************************/
View.List = function () {
    self.close();
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