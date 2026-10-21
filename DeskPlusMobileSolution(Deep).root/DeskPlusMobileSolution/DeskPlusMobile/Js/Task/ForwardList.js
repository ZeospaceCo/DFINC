List = {}

/**************************************************
작성자   : famo23
작성일자 : 2015-05-13
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {
    // 문서 리스트 가져오기
    setTimeout("List.getList();", 400);

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

    $('#search').click(function (e) {
        e.preventDefault();
        List.search();
    });

    $("#tbSearch").attr("placeholder", R_InputTask);
});


/**************************************************
작성자   : famo23
작성일자 : 2015-05-13
작성목적 : 협업 리스트 가져오기 (웹서비스 호출)
***************************************************/
List.getList = function () {
    //LoadingShow(true);
    var url = "/DeskPlusMobile/Task/Ws/GetTask.asmx/GetForwardList";

    //파라미터값 배열에 담기
    ParamArray = {
        'PageNo': PageNo,
        'PageSize': PageSize,
        'Sv': SearchValue.securityText()
    };

    var obj = eval("List.succeeded");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-05-13
작성목적 : 협업 리스트 가져오기 (웹서비스 값 받기)
***************************************************/
List.succeeded = function (data, status) {

    $('#phTask').setTemplateURL('/DeskPlusMobile/Task/Jt/JTForwardList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phTask').processTemplate(data.d);

    setTimeout(function () {
        List.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-05-13
작성목적 : 다국어 처리
**********************************************************************************************/
List.Resource = function () {

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-05-13
작성목적 : 문서 보기 화면으로 가기 
***************************************************/
List.viewList = function (p_listID, p_groupID, p_forwardID) {

    ParamArray = {
        'PageNo': PageNo,
        'ListID': p_listID,
        'GroupID': p_groupID,
        'ForwardID': p_forwardID,
        'Sv': SearchValue.securityText()
    };

    CallEncrypt(CallGetParam());
    document.location.href = "ForwardView.aspx?p=" + GetEncryptValue;
}



/**************************************************
작성자   : famo23
작성일자 : 2015-05-13
작성목적 : 검색
***************************************************/
List.search = function () {

    SearchValue = $("#tbSearch").val();

    ParamArray = {
        'PageNo': 1,
        'Sv': SearchValue.securityText()
    };

    CallEncrypt(CallGetParam());

    location.href = "ForwardList.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2015-05-13
작성목적 : 검색
***************************************************/
List.SetSearch = function () {

    List.search();

    return false;
}


/**************************************************
작성자   : famo23
작성일자 : 2015-10-28
작성목적 : 이전 페이지 이동
***************************************************/
List.pvNum = function () {

    if (parseInt(PageNo) > 1) {
        PageNo = parseInt(PageNo) - 1;

        ParamArray = {
            'PageNo': PageNo,
            'Sv': SearchValue.securityText()
        };

        CallEncrypt(CallGetParam());

        location.href = "ForwardList.aspx?p=" + GetEncryptValue;
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2015-10-28
작성목적 : 다음 페이지 이동
***************************************************/
List.nxNum = function () {

    var tmpTotalNum = $("#TotalNum").html();

    if (parseInt(PageNo) < parseInt(tmpTotalNum)) {
        PageNo = parseInt(PageNo) + 1;

        ParamArray = {
            'PageNo': PageNo,
            'Sv': SearchValue.securityText()
        };

        CallEncrypt(CallGetParam());

        location.href = "ForwardList.aspx?p=" + GetEncryptValue;
    }
}
