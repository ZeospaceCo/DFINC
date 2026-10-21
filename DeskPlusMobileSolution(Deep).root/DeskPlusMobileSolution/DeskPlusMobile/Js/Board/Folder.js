Folder = {}

/**************************************************
작성자   : famo23
작성일자 : 2011-07-11
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    // 게시판 카운트 가져오기
    setTimeout("Folder.getCount();", 400);
});


/**************************************************
작성자   : famo23
작성일자 : 2012-04-20
작성목적 : 게시판 카테로기 가져오기 (웹서비스 호출)
***************************************************/
Folder.getCount = function () {
    //LoadingShow(true);
    var url = "/DeskPlusMobile/Board/Ws/GetBoard.asmx/GetFolder";

    //파라미터값 배열에 담기
    ParamArray = {
        'guid1': '1'
    };

    var obj = eval("Folder.succeeded");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);


    /*
    var obj = eval("Folder.succeeded");
    AjaxAsyncService(url, "", obj);
    */
}

/**************************************************
작성자   : famo23
작성일자 : 2012-04-20
작성목적 : 게시판 카테고리 가져오기 (웹서비스 값 받기)
***************************************************/
Folder.succeeded = function (data, status) {

    //LoadingShow(false);

    $('#phFolder').setTemplateURL('/DeskPlusMobile/Board/Jt/JTFolder.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phFolder').processTemplate(data.d);

    setTimeout(function () {
        Folder.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-04-20
작성목적 : 다국어 처리
**********************************************************************************************/
Folder.Resource = function () {

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2011-07-12
작성목적 : 게시판 리스트 가기
***************************************************/
Folder.goList = function (mainCID, pCatID) {

    ParamArray = {
        'MainCID': mainCID,
        'PCatID': pCatID
    };

    CallEncrypt(CallGetParam());

    location.href = "List.aspx?p=" + GetEncryptValue;
}