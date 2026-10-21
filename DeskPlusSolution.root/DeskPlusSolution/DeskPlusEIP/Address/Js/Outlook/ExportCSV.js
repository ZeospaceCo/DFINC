ExportCSV = {}

$(function () {
    $(document).ready(function () {

        //확인
        $('#cbtnExportCSV').click(function (e) {
            e.preventDefault();
            ExportCSV.Save();
        });

        //csv주소록  페이지 이동
        $('#csvContact').click(function (e) {
            e.preventDefault();
            location = "/DeskPlusEIP/Address/Outlook/InOutStep1.aspx";
        });

        //google주소록
        $('#googleContact').click(function (e) {
            e.preventDefault();
            ExportCSV.GoogleContact();
        });

        //주소록 내보내기 페이지 이동
        $('#exportCsv').click(function (e) {
            e.preventDefault();
            location = "/DeskPlusEIP/Address/Outlook/ExportCSV.aspx";
        });

    });
});


/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 구글 주소록 페이지 새창띄우기
***************************************************/
ExportCSV.GoogleContact = function () {
    //    var urlpath = "/DeskPlusEIP/Address/Outlook/InGoogleStep1.aspx";
    //    WindowObjectReference = window.open(urlpath, "GoogleContact", "menubar=no,location=no,resizable=no,scrollbars=yes,status=no");

    var url = "/DeskPlusEIP/Address/Outlook/InGoogleStep1.aspx";
    var param = "";
    WindowObjectReference = PopupWinOpen(url, param, "GoogleContact", "800", "600")
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 주소록 내보내기 처리하기
***************************************************/
ExportCSV.Save = function () {

    $("#tbTypes").val("");

    var gubun = "";

    if ($("#csvDaum").is(":checked")) {
        gubun = $("#csvDaum").val();
    }

    if ($("#csvNaver").is(":checked")) {
        gubun = $("#csvNaver").val();
    }

    if ($("#csvGoogle").is(":checked")) {
        gubun = $("#csvGoogle").val();
    }

    if ($("#csvOutlook").is(":checked")) {
        gubun = $("#csvOutlook").val();
    }

    if ($("#csvExpress").is(":checked")) {
        gubun = $("#csvExpress").val();
    }

    //daum,naver,google,outlook,express
    if (gubun == "") {
        alert(R_SelectOutLookFormat);
        return;
    }

    $("#tbTypes").val(gubun);

    __doPostBack('lbtnSumbit', "");
}