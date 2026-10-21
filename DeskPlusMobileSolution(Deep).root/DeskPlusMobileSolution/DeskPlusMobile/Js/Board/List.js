List = {}

/**************************************************
작성자   : famo23
작성일자 : 2011-07-12
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    setTimeout("List.getList('');", 400);

    $('#search').click(function (e) {
        e.preventDefault();
        List.search();
    });

    $('#pvNum').click(function (e) {
        e.preventDefault();
        List.pvNum();
    });

    $('#nxNum').click(function (e) {
        e.preventDefault();
        List.nxNum();
    });

    $('#listTitle').click(function (e) {
        location.href = "Folder.aspx";
    });

    $('#add').click(function (e) {
        List.add();
    });

    $("#tbSearch").attr("placeholder", R_InputBoa);

});


/**************************************************
작성자   : famo23
작성일자 : 2011-07-12
작성목적 : 게시판 리스트 가져오기 (웹서비스 호출)
***************************************************/
List.getList = function (params) {
    //LoadingShow(true);

    var url = "/DeskPlusMobile/Board/Ws/GetBoard.asmx/GetList";

    //파라미터값 배열에 담기
    ParamArray = {
        'PageNo': PageNo,
        'PageSize': PageSize,
        'SearchValue': SearchValue.securityText(),
        'PCatID': PCatID
    };

    var obj = eval("List.succeeded");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2011-07-12
작성목적 : 게시판 리스트 가져오기 (웹서비스 값 받기)
***************************************************/
List.succeeded = function (data, status) {

    $('#phBoard').setTemplateURL('/DeskPlusMobile/Board/Jt/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phBoard').processTemplate(data.d);

    setTimeout(function () {
        List.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-03-30
작성목적 : 다국어 처리
**********************************************************************************************/
List.Resource = function () {

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2011-07-12
작성목적 : 문서 보기 화면으로 가기 
***************************************************/
List.viewList = function (categoryID, listID) {

    if (listID == "0") {

        ParamArray = {
            'MainCID': MainCID,
            'PCatID': categoryID,
            'Sv': SearchValue.securityText()
        };

        CallEncrypt(CallGetParam());

        location.href = "List.aspx?p=" + GetEncryptValue;
    }
    else {
        ParamArray = {
            'MainCID': MainCID,
            'PCatID': PCatID,
            'PageNo': PageNo,
            'CID': categoryID,
            'ID': listID,
            'Sv': SearchValue.securityText()
        };

        CallEncrypt(CallGetParam());

        location.href = "View.aspx?p=" + GetEncryptValue;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2011-07-13
작성목적 : 문서 보기 화면으로 가기 
***************************************************/
List.search = function () {

    SearchValue = $("#tbSearch").val();

    ParamArray = {
        'MainCID': MainCID,
        'PCatID': PCatID,
        'Sv': SearchValue.securityText()
    };

    CallEncrypt(CallGetParam());

    location.href = "List.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2013-02-04
작성목적 : 검색
***************************************************/
List.SetSearch = function () {

    List.search();

    return false;
}



/**************************************************
작성자   : famo23
작성일자 : 2015-09-04
작성목적 : 글 등록
***************************************************/
List.add = function () {

    ParamArray = {
        'MainCID': MainCID,
        'PCatID': PCatID,
        'PageNo': PageNo,
        'Sv': SearchValue.securityText()
    };

    CallEncrypt(CallGetParam());

    location.href = "Write.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2015-11-02
작성목적 : 이전 페이지 이동
***************************************************/
List.pvNum = function () {

    if (parseInt(PageNo) > 1) {
        PageNo = parseInt(PageNo) - 1;

        ParamArray = {
            'MainCID': MainCID,
            'PCatID': PCatID,
            'PageNo': PageNo,
            'Sv': SearchValue.securityText()
        };

        CallEncrypt(CallGetParam());

        location.href = "List.aspx?p=" + GetEncryptValue;
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2015-11-02
작성목적 : 다음 페이지 이동
***************************************************/
List.nxNum = function () {

    var tmpTotalNum = $("#TotalNum").html();

    if (parseInt(PageNo) < parseInt(tmpTotalNum)) {
        PageNo = parseInt(PageNo) + 1;

        ParamArray = {
            'MainCID': MainCID,
            'PCatID': PCatID,
            'PageNo': PageNo,
            'Sv': SearchValue.securityText()
        };

        CallEncrypt(CallGetParam());

        location.href = "List.aspx?p=" + GetEncryptValue;
    }
}

