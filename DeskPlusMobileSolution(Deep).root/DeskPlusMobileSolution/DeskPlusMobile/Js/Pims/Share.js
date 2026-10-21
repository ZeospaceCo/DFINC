Share = {}

/**************************************************
작성자   : famo23
작성일자 : 2011-06-24
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    // 공유자 가져오기
    setTimeout("Share.getShare();", 400);

    $('#listTitle').click(function (e) {
        Share.goList();
    });
});


/**************************************************
작성자   : famo23
작성일자 : 2011-06-24
작성목적 : 공유자 가져오기 (웹서비스 호출)
***************************************************/
Share.getShare = function () {
    //LoadingShow(true);
    var url = "/DeskPlusMobile/Pims/Ws/GetPims.asmx/GetShareList";

    //파라미터값 배열에 담기
    ParamArray = {
        'UserID': UserID
    };

    var obj = eval("Share.succeededShare");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2011-06-24
작성목적 : 공유자 가져오기 (웹서비스 값 받기)
***************************************************/
Share.succeededShare = function (data, status) {
    $('#phShare').setTemplateURL('/DeskPlusMobile/Pims/Jt/JTShare.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phShare').processTemplate(data.d);

    setTimeout(function () {
        Share.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-03-30
작성목적 : 다국어 처리
**********************************************************************************************/
Share.Resource = function () {

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoShare;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2011-06-24
작성목적 : 나의 일정가기
***************************************************/
Share.goList = function () {

    location.href = "List.aspx";
}

/**************************************************
작성자   : famo23
작성일자 : 2011-06-24
작성목적 : 공유자 일정 보기 
***************************************************/
Share.viewShare = function (OpenID, OpenName) {

    ParamArray = {
        'OpenID': OpenID,
        'OpenName': OpenName
    };

    CallEncrypt(CallGetParam());

    location.href = "List.aspx?p=" + GetEncryptValue;

}

