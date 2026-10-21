BoardList = {}

/**************************************************
작성자   : famo23
작성일자 : 2012-07-10
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {


    // 문서 리스트 가져오기
    setTimeout("BoardList.getList('');", 400);

    // 검색
    $('#search').click(function (e) {
        e.preventDefault();
        BoardList.search();
    });

    $('#pvNum').click(function (e) {
        e.preventDefault();
        BoardList.pvNum();
    });

    $('#nxNum').click(function (e) {
        e.preventDefault();
        BoardList.nxNum();
    });

    $('#listTitle').click(function (e) {
        ParamArray = {
            'CommunityID': CommunityID
        };

        CallEncrypt(CallGetParam());

        location.href = "BoardFolder.aspx?p=" + GetEncryptValue;
    });

    $("#tbSearch").attr("placeholder", R_InputCom);

});

/**************************************************
작성자   : famo23
작성일자 : 2012-07-10
작성목적 : 선택한 게시판 리스트 가져오기 (웹서비스 호출)
***************************************************/
BoardList.getList = function (params) {

    var url = "/DeskPlusMobile/Community/Ws/GetCommunity.asmx/GetBoardList";

    //파라미터값 배열에 담기
    ParamArray = {
        'CommunityID': CommunityID,
        'MenuID': MenuID,
        'PageNo': PageNo,
        'PageSize': PageSize,
        'SearchValue': SearchValue.securityText()
    };


    var obj = eval("BoardList.succeededBoardList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2012-07-10
작성목적 : 선택한 게시판 리스트 가져오기 (웹서비스 값 받기)
***************************************************/
BoardList.succeededBoardList = function (data, status) {

    $('#phBoardList').setTemplateURL('/DeskPlusMobile/Community/Jt/JTBoardList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phBoardList').processTemplate(data.d);

    setTimeout(function () {
        BoardList.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-07-10
작성목적 : 다국어 처리
**********************************************************************************************/
BoardList.Resource = function () {

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2012-07-10
작성목적 : 검색 
***************************************************/
BoardList.search = function () {

    SearchValue = $("#tbSearch").val();

    ParamArray = {
        'CommunityID': CommunityID,
        'MenuID': MenuID,
        'Sv': SearchValue.securityText()
    };

    CallEncrypt(CallGetParam());

    location.href = "BoardList.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2012-07-10
작성목적 : 게시글 보기 
***************************************************/
BoardList.viewList = function (boardID) {

    ParamArray = {
        'CommunityID': CommunityID,
        'MenuID': MenuID,
        'BoardID': boardID,
        'PageNo': PageNo,
        'Sv': SearchValue.securityText()
    };

    CallEncrypt(CallGetParam());

    location.href = "BoardView.aspx?p=" + GetEncryptValue;

}


/**************************************************
작성자   : famo23
작성일자 : 2013-02-04
작성목적 : 검색
***************************************************/
BoardList.SetSearch = function () {

    BoardList.search();

    return false;
}


/**************************************************
작성자   : famo23
작성일자 : 2015-10-27
작성목적 : 이전 페이지 이동
***************************************************/
BoardList.pvNum = function () {

    if (parseInt(PageNo) > 1) {
        PageNo = parseInt(PageNo) - 1;

        ParamArray = {
            'CommunityID': CommunityID,
            'MenuID': MenuID,
            'PageNo': PageNo,
            'Sv': SearchValue.securityText()
        };

        CallEncrypt(CallGetParam());

        location.href = "BoardList.aspx?p=" + GetEncryptValue;
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2015-10-27
작성목적 : 다음 페이지 이동
***************************************************/
BoardList.nxNum = function () {

    var tmpTotalNum = $("#TotalNum").html();

    if (parseInt(PageNo) < parseInt(tmpTotalNum)) {
        PageNo = parseInt(PageNo) + 1;

        ParamArray = {
            'CommunityID': CommunityID,
            'MenuID': MenuID,
            'PageNo': PageNo,
            'Sv': SearchValue.securityText()
        };

        CallEncrypt(CallGetParam());

        location.href = "BoardList.aspx?p=" + GetEncryptValue;
    }
}



