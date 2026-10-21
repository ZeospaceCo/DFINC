BoardFolder = {}

/**************************************************
작성자   : famo23
작성일자 : 2012-07-09
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    $('#listTitle').click(function (e) {
        location.href = "Folder.aspx";
    });


    BoardFolder.getList();
});

/**************************************************
작성자   : famo23
작성일자 : 2012-07-09
작성목적 : 게시판 가져오기 (웹서비스 호출)
***************************************************/
BoardFolder.getList = function () {
    var url = "/DeskPlusMobile/Community/Ws/GetCommunity.asmx/GetBoardMenu";

    //파라미터값 배열에 담기
    ParamArray = {
        'communityID': CommunityID
    };

    var obj = eval("BoardFolder.succeededGetBoardMenu");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2012-07-09
작성목적 : 게시판 가져오기 (웹서비스 값 받기)
***************************************************/
BoardFolder.succeededGetBoardMenu = function (data, status) {
    
    $('#phTemp').setTemplateURL('/DeskPlusMobile/Community/Jt/JTBoardFolder.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phTemp').processTemplate(data.d);

    if (data.d == null || data.d == "") {
        $('#phEmpty').html($('#phTemp').html());
    } else {
        $('#phBoardFolder').append($('#phTemp').html());
    }

    setTimeout(function () {
        BoardFolder.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-07-09
작성목적 : 다국어 처리
**********************************************************************************************/
BoardFolder.Resource = function () {

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2012-07-10
작성목적 : 선택한 게시판 리스트로 이동
***************************************************/
BoardFolder.goList = function (community, menuID, readAuth) {

    // 읽기 권한  체크
    if (Number(Grade) <= Number(readAuth)) {

        ParamArray = {
            'CommunityID': community,
            'MenuID': menuID
        };

        CallEncrypt(CallGetParam());

        location.href = "BoardList.aspx?p=" + GetEncryptValue;
    }
    else {
        NoticeShow("!", R_NoReadAuth);
        return;
    }
}

