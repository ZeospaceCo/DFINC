InOutStep1 = {}

$(function () {
    $(document).ready(function () {
        //확인
        $('#cbtnGetCSV').click(function (e) {
            e.preventDefault();
            InOutStep1.Save();
        });

        //csv주소록
        $('#csvContact').click(function (e) {
            e.preventDefault();
            location = "/DeskPlusEIP/Address/Outlook/InOutStep1.aspx";
        });

        //google주소록
        $('#googleContact').click(function (e) {            
            e.preventDefault();
            InOutStep1.GoogleContact();
        });

        //주소록 내보내기
        $('#exportCsv').click(function (e) {
            e.preventDefault();
            location = "/DeskPlusEIP/Address/Outlook/ExportCSV.aspx";
        });

    });
});


/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : CSV 주소록 가져오기
***************************************************/
InOutStep1.Save = function () {
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


    if (gubun == "") {
        alert(R_SelectOutLookFormat);
        return;
    }

    $("#tbTypes").val(gubun);

    __doPostBack("lbtnSumbit", "");
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 구글 주소록 페이지 새창띄우기
***************************************************/
InOutStep1.GoogleContact = function () {    
//    var urlpath = "/DeskPlusEIP/Address/Outlook/InGoogleStep1.aspx";
//    WindowObjectReference = window.open(urlpath, "GoogleContact", "menubar=no,location=no,resizable=no,scrollbars=yes,status=no");

    var url = "/DeskPlusEIP/Address/Outlook/InGoogleStep1.aspx";
    var param = "";
    WindowObjectReference = PopupWinOpen(url, param, "GoogleContact", "800", "600")
}