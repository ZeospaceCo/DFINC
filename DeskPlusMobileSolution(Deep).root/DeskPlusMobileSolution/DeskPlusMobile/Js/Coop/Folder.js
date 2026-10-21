Folder = {}

/**************************************************
작성자   : famo23
작성일자 : 2015-03-30
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    // 문서 카운트 가져오기
    setTimeout("Folder.getCount();", 400);

    $('#request').click(function (e) {
        Folder.goList('request');
    });

    $('#receive').click(function (e) {
        Folder.goList('receive');
    });

    $('#reference').click(function (e) {
        Folder.goList('reference');
    });

    $('#forward').click(function (e) {
        Folder.goList('forward');
    });

});


/**************************************************
작성자   : famo23
작성일자 : 2015-03-30
작성목적 : 업무협업 카운트 가져오기 (웹서비스 호출)
***************************************************/
Folder.getCount = function () {

    var url = "/DeskPlusMobile/Coop/Ws/GetCoop.asmx/GetCount";

    //파라미터값 배열에 담기
    ParamArray = {
        'CompanyID': CompanyID,
        'UserID': UserID
    };

    var obj = eval("Folder.succeeded");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-03-30
작성목적 : 업무협업 카운트 가져오기 (웹서비스 값 받기)
***************************************************/
Folder.succeeded = function (data, status) {

    var tmpCount = data.d.split('^');
    $("#req").text(tmpCount[0]);
    $("#rec").text(tmpCount[1]);
    $("#ref").text(tmpCount[2]);
    $("#for").text(tmpCount[3]);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-03-30
작성목적 : 업무협업 리스트 페이지 이동
***************************************************/
Folder.goList = function (arg) {

    ParamArray = {
        'BPage': arg
    };

    CallEncrypt(CallGetParam());

    location.href = "List.aspx?p=" + GetEncryptValue;
}


