Proxy = {}

/**************************************************
작성자   : famo23
작성일자 : 2011-07-07
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    // 문서 리스트 가져오기
    setTimeout("Proxy.getList();", 400);

    $('#listTitle').click(function (e) {
        location.href = "Folder.aspx";
    });

});


/**************************************************
작성자   : famo23
작성일자 : 2011-07-07
작성목적 : 대결자 리스트 가져오기 (웹서비스 호출)
***************************************************/
Proxy.getList = function () {
    //LoadingShow(true);
    var url = "/DeskPlusMobile/Approval/Ws/GetApproval.asmx/GetProxy";

    //파라미터값 배열에 담기
    ParamArray = {
        'CompanyID': CompanyID,
        'UserID': UserID
    };

    var obj = eval("Proxy.succeeded");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2011-07-07
작성목적 : 대결자 리스트 가져오기 (웹서비스 값 받기)
***************************************************/
Proxy.succeeded = function (data, status) {

    $('#phProxy').setTemplateURL('/DeskPlusMobile/Approval/Jt/JTProxyList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phProxy').processTemplate(data.d);

    setTimeout(function () {
        Proxy.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-03-30
작성목적 : 다국어 처리
**********************************************************************************************/
Proxy.Resource = function () {

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoProxy;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2011-07-07
작성목적 : 대결자 문서 리스트 화면으로 가기 
***************************************************/
Proxy.list = function (proxyUID, proxyDID) {

    ParamArray = {
        'BPage': 'APP',
        'TargetUID': proxyUID,
        'TargetDID': proxyDID
    };

    CallEncrypt(CallGetParam());

    location.href = "List.aspx?p=" + GetEncryptValue;
}
