List = {}

/**************************************************
작성자   : famo23
작성일자 : 2015-03-30
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    $("#tbSearch").val(SearchValue);

    if (BPage == "request") {
        $('#listTitle').text(R_Request);
    }
    else if (BPage == "receive") {
        $('#listTitle').text(R_Receive);
    }
    else if (BPage == "reference") {
        $('#listTitle').text(R_Reference);
    }
    else if (BPage == "forward") {
        $('#listTitle').text(R_Forward);
    }

    // 문서 리스트 가져오기
    setTimeout("List.getList();", 400);
    setTimeout("PageSize = 10;", 400);            //기본10개

    $('#more').click(function (e) {
        e.preventDefault();
        List.moreList();
    });

    $('#listTitle').click(function (e) {
        location.href = "Folder.aspx";        
    });

    $('#search').click(function (e) {
        e.preventDefault();
        List.search();
    });
});


/**************************************************
작성자   : famo23
작성일자 : 2015-03-30
작성목적 : 협업 리스트 가져오기 (웹서비스 호출)
***************************************************/
List.getList = function () {
    //LoadingShow(true);
    var url = "/DeskPlusMobile/Coop/Ws/GetCoop.asmx/GetList";

    //파라미터값 배열에 담기
    ParamArray = {
        'BPage': BPage,
        'PageNo': PageNo,
        'PageSize': PageSize,
        'Sv': SearchValue
    };

    var obj = eval("List.succeeded");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-03-30
작성목적 : 협업 리스트 가져오기 (웹서비스 값 받기)
***************************************************/
List.succeeded = function (data, status) {

    $('#phTemp').setTemplateURL('/DeskPlusMobile/Coop/Jt/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phTemp').processTemplate(data.d);

    if (data.d == null || data.d == "") {
        $('#phEmpty').html($('#phTemp').html());
    } else {
        $('#phCoop').append($('#phTemp').html());
    }

    PageNo = parseInt(PageNo) + 1;

    if (MoreNo > 1) {
        PageNo = MoreNo;
        MoreNo = 1;
    }

    setTimeout(function () {
        List.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-03-30
작성목적 : 다국어 처리
**********************************************************************************************/
List.Resource = function () {

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-03-30
작성목적 : 더보기 선택시 
***************************************************/
List.moreList = function () {
    List.getList();
    MoreSize = parseInt(MoreSize) + 10;
}

/**************************************************
작성자   : famo23
작성일자 : 2015-03-30
작성목적 : 문서 보기 화면으로 가기 
***************************************************/
List.viewList = function (p_listID, p_groupID, p_status, p_forwardListID) {

    var url = "View";
    if (BPage == "forward") {
        url = "ForwardView";
    }

    ParamArray = {
        'BPage': BPage,
        'MoreSize': MoreSize,
        'PageNo': PageNo,
        'ListID': p_listID,
        'GroupID': p_groupID,
        'ForwardListID':p_forwardListID,
        'Status': p_status,
        'Sv': SearchValue
    };

    CallEncrypt(CallGetParam());
    document.location.href = url + ".aspx?p=" + GetEncryptValue;
}



/**************************************************
작성자   : famo23
작성일자 : 2015-03-30
작성목적 : 검색
***************************************************/
List.search = function () {

    var selVale = $("#tbSearch").val();
    SearchValue = selVale.split("-").join("");
    SearchValue = SearchValue.split("'").join("");

    $('#phCoop').empty();
    $('#phEmpty').empty();

    PageNo = 1;
    MoreNo = 1;
    MoreSize = 10;

    setTimeout("List.getList();", 400);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-03-30
작성목적 : 검색
***************************************************/
List.SetSearch = function () {

    List.search();

    return false;
}

