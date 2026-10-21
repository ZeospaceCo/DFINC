Share= {}

/**************************************************
작성자   : famo23
작성일자 : 2012-04-17
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    // 문서 리스트 가져오기
    setTimeout("Share.getList();", 400);

    $('#listTitle').click(function (e) {
        location.href = "Folder.aspx";
    });

});


/**************************************************
작성자   : famo23
작성일자 : 2012-04-17
작성목적 : 공유자 리스트 가져오기 (웹서비스 호출)
***************************************************/
Share.getList = function () {
    var url = "/DeskPlusMobile/WebHard/Ws/GetWebHard.asmx/GetShare";

    //파라미터값 배열에 담기
    ParamArray = {
        'guid1': '1'
    };

    var obj = eval("Share.succeeded");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);

    //var obj = eval("Share.succeeded");
    //AjaxAsyncService(url, "", obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2012-04-17
작성목적 : 공유자 리스트 가져오기 (웹서비스 값 받기)
***************************************************/
Share.succeeded = function (data, status) {

    $('#phShare').setTemplateURL('/DeskPlusMobile/WebHard/Jt/JTShare.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phShare').processTemplate(data.d);

    setTimeout(function () {
        Share.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-04-17
작성목적 : 다국어 처리
**********************************************************************************************/
Share.Resource = function () {

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2012-04-18
작성목적 : 공유자 일정 보기 
***************************************************/
Share.viewShare = function (FolderID) {

    ParamArray = {
        'FolderID': FolderID,
        'ShareGubun': 'Y'
    };

    CallEncrypt(CallGetParam());

    location.href = "List.aspx?p=" + GetEncryptValue;

}

