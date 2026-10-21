Folder = {}

/**************************************************
작성자   : famo23
작성일자 : 2012-07-09
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {
    Folder.getList();
});

/**************************************************
작성자   : famo23
작성일자 : 2012-07-09
작성목적 : 내가 가입한 커뮤니티 가져오기 (웹서비스 호출)
***************************************************/
Folder.getList = function () {
    var url = "/DeskPlusMobile/Community/Ws/GetCommunity.asmx/GetCommunityList";

    //파라미터값 배열에 담기
    ParamArray = {
        'CommunityID': CommunityID
    };

    var obj = eval("Folder.succeededCommunity");   
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2012-07-09
작성목적 : 내가 가입한 커뮤니티 가져오기 (웹서비스 값 받기)
***************************************************/
Folder.succeededCommunity = function (data, status) {

    $('#phTemp').setTemplateURL('/DeskPlusMobile/Community/Jt/JTFolder.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phTemp').processTemplate(data.d);

    if (data.d == null || data.d == "") {
        $('#phEmpty').html($('#phTemp').html());
    } else {
        $('#phCommunity').append($('#phTemp').html());
    }

    setTimeout(function () {
        Folder.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-07-09
작성목적 : 다국어 처리
**********************************************************************************************/
Folder.Resource = function () {

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }
}

/************************************************************************************************
작성자   : famo23
작성일자 : 2012-07-09
작성목적 : 선택한 커뮤니티로 이동 -> 커뮤니티 방문 시 방문 카운트, 포인트 추가
************************************************************************************************/
Folder.goList = function (communityID) {

    CommunityID = communityID;
    var url = "/DeskPlusMobile/Community/Ws/SetCommunity.asmx/AddVisitPoint";

    //파라미터값 배열에 담기
    ParamArray = {
        'CommunityID': CommunityID
    };    

    var obj = eval("Folder.succeededAddVisitPoint");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2012-07-09
작성목적 : 커뮤니티 방문 시 방문 카운트 추가 성공
***************************************************/
Folder.succeededAddVisitPoint = function (data, status) {

    if (data.d) {
        ParamArray = {
            'CommunityID': CommunityID
        };

        CallEncrypt(CallGetParam());

        location.href = "BoardFolder.aspx?p=" + GetEncryptValue;
    }
    else {
        NoticeShow("!", R_Error); return;
    }
}

