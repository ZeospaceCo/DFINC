View = {}

/**********************************************************************************************
작성자   :
작성일자 :
작성목적 : 초기화
**********************************************************************************************/

$(function () {
    $(document).ready(function () {
        //리스트
        $('#cbtnList').click(function (e) {
            e.preventDefault();

            ParamArray = {
                'listCount': ListCount,
                'currentPage': CurrentPage,
                'searchStatus': SearchStatus,
                'searchField': SearchField,
                'searchValue': SearchValue,
                'searchDateValue': SearchDateValue,
                'order': Order,
                'itemID': ItemID,
                'itemSubID': ItemSubID
            };

            CallEncrypt(CallGetParam());
            location = "List.aspx?p=" + GetEncryptValue;
        });

        //인쇄
        $('#cbtnPrint').click(function (e) {
            e.preventDefault();
            View.printElem({ leaveOpen: false, printMode: 'popup' });
        });

        View.Resource();
    });
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
    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoAttachData); }
    if ($(".NoBaseListData") != undefined) { $(".NoBaseListData").text(R_NoBaseListData); }
    if ($(".BtnView") != undefined) { $(".BtnView").attr('src', ImagePath + '/Main/icon_search.gif'); }
    if ($(".BtnViews") != undefined) { $(".BtnViews").attr('src', ImagePath + '/Icon/icon_search.gif'); }
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