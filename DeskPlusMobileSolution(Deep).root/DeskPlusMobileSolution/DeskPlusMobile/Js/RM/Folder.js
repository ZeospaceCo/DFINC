Folder = {}

/**************************************************
작성자   : 방영환
작성일자 : 2011-06-24
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    // 승인대기 카운트 가져오기
    setTimeout("Folder.getCount();", 400);

    $('#aReserve').click(function (e) {
        e.preventDefault();
        Folder.goList('Reserve');
    });

    $('#aMyReserve').click(function (e) {
        e.preventDefault();
        Folder.goList('MyReserve');
    });

    $('#aApprove').click(function (e) {
        e.preventDefault();
        Folder.goList('Usable0');
    });

    $('#add').click(function (e) {
        Folder.add();
    });

});


/**************************************************
작성자   : 방영환
작성일자 : 2011-06-24
작성목적 : 자원관리 카운트 가져오기 (웹서비스 호출)
***************************************************/
Folder.getCount = function () {
    //LoadingShow(true);
    var url = "/DeskPlusMobile/RM/Ws/GetRM.asmx/GetCount";

    //파라미터값 배열에 담기
    ParamArray = {};

    var obj = eval("Folder.succeeded");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : 방영환
작성일자 : 2011-06-24
작성목적 : 자원관리 카운트 가져오기 (웹서비스 값 받기)
***************************************************/
Folder.succeeded = function (data, status) {
    /*
    var tmpCount = data.d.split('^');
    $("#app").text("(" + tmpCount[0] + ")");    
    */

    var tmpCount = data.d.split('^');
    $("#app").text("(" + tmpCount + ")");
}

/**************************************************
작성자   : 방영환
작성일자 : 2011-06-27
작성목적 : 문서 리스트 보기 페이지 이동
***************************************************/
Folder.goList = function (arg) {

    var url = "List.aspx";
    if (arg == "Usable0") {
        url = "UsableList.aspx";
    }

    ParamArray = {
        'BPage': arg
    };

    CallEncrypt(CallGetParam());

    location.href = url + "?p=" + GetEncryptValue;
}


/**************************************************
작성자   : 방영환
작성일자 : 2011-06-24
작성목적 : 자원예약
***************************************************/
Folder.add = function () {

    ParamArray = {
        'BPage': "Folder"
    };

    CallEncrypt(CallGetParam());

    location.href = "Add.aspx?p=" + GetEncryptValue;

}