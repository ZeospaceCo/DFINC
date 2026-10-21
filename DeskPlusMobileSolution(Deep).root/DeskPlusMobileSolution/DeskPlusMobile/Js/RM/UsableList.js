UsableList = {}

/**************************************************
작성자   : 방영환
작성일자 : 2011-07-12
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    setTimeout("UsableList.getList('');", 400);

    $('#search').click(function (e) {
        e.preventDefault();
        UsableList.search();
    });

    $('#pvNum').click(function (e) {
        e.preventDefault();
        UsableList.pvNum();
    });

    $('#nxNum').click(function (e) {
        e.preventDefault();
        UsableList.nxNum();
    });

    $('#listTitle').click(function (e) {
        location.href = "Folder.aspx";
    });

    $('#add').click(function (e) {
        UsableList.add();
    });

    $("#tbSearch").attr("placeholder", R_InputRM);

});


/**************************************************
작성자   : 방영환
작성일자 : 2011-07-12
작성목적 : 리스트 가져오기 (웹서비스 호출)
***************************************************/
UsableList.getList = function (params) {

    var url = "/DeskPlusMobile/RM/Ws/GetRM.asmx/GetUsalbleList";

    //파라미터값 배열에 담기
    ParamArray = {
        'PageNo': PageNo,
        'PageSize': PageSize,
        'Usable': '0',
        'Sv': SearchValue.securityText()
    };

    var obj = eval("UsableList.succeeded");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : 방영환
작성일자 : 2011-07-12
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
***************************************************/
UsableList.succeeded = function (data, status) {

    $('#phRM').setTemplateURL('/DeskPlusMobile/RM/Jt/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phRM').processTemplate(data.d);

    setTimeout(function () {
        UsableList.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : 방영환
작성일자 : 2012-03-30
작성목적 : 다국어 처리
**********************************************************************************************/
UsableList.Resource = function () {

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }
}


/**************************************************
작성자   : 방영환
작성일자 : 2011-06-17
작성목적 : 예약자원 보기
***************************************************/
UsableList.view = function (ReservationID) {

    ParamArray = {
        'ReservationID': ReservationID,
        'Sv': SearchValue.securityText(),
        'PageNo': PageNo,
        'BPage': BPage
    };

    CallEncrypt(CallGetParam());

    location.href = "View.aspx?p=" + GetEncryptValue;

}

/**************************************************
작성자   : 방영환
작성일자 : 2011-07-13
작성목적 : 검색 
***************************************************/
UsableList.search = function () {

    SearchValue = $("#tbSearch").val();

    ParamArray = {
        'Sv': SearchValue.securityText()
    };

    CallEncrypt(CallGetParam());

    location.href = "UsableList.aspx?p=" + GetEncryptValue;

}

/**************************************************
작성자   : 방영환
작성일자 : 2013-02-04
작성목적 : 검색
***************************************************/
UsableList.SetSearch = function () {

    UsableList.search();

    return false;
}


/**************************************************
작성자   : 방영환
작성일자 : 2015-11-02
작성목적 : 이전 페이지 이동
***************************************************/
UsableList.pvNum = function () {

    if (parseInt(PageNo) > 1) {
        PageNo = parseInt(PageNo) - 1;

        ParamArray = {
            'PageNo': PageNo,
            'Sv': SearchValue.securityText(),
            'BPage': BPage
        };


        CallEncrypt(CallGetParam());

        location.href = "UsableList.aspx?p=" + GetEncryptValue;
    }
}


/**************************************************
작성자   : 방영환
작성일자 : 2015-11-02
작성목적 : 다음 페이지 이동
***************************************************/
UsableList.nxNum = function () {

    var tmpTotalNum = $("#TotalNum").html();

    if (parseInt(PageNo) < parseInt(tmpTotalNum)) {
        PageNo = parseInt(PageNo) + 1;

        ParamArray = {
            'PageNo': PageNo,
            'Sv': SearchValue.securityText(),
            'BPage': BPage
        };

        CallEncrypt(CallGetParam());

        location.href = "UsableList.aspx?p=" + GetEncryptValue;
    }
}

