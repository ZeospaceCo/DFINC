SubmitList = {}

/**************************************************
작성자   : famo23
작성일자 : 2019-07-11
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    // 문서 리스트 가져오기
    setTimeout("SubmitList.getList();", 400);

    $('#pvNum').click(function (e) {
        e.preventDefault();
        SubmitList.pvNum();
    });

    $('#nxNum').click(function (e) {
        e.preventDefault();
        SubmitList.nxNum();
    });


    $('#listTitle').click(function (e) {
        location.href = "WFolder.aspx";
    });

    $('#search').click(function (e) {
        e.preventDefault();
        SubmitList.search();
    });

    $("#tbSearch").attr("placeholder", R_Subject);

});


/**************************************************
작성자   : famo23
작성일자 : 2019-07-11
작성목적 : 문서 리스트 가져오기 (웹서비스 호출)
***************************************************/
SubmitList.getList = function () {
    //LoadingShow(true);
    var url = "/DeskPlusMobile/Approval/Ws/GetApproval.asmx/GetSubmitList";

    //파라미터값 배열에 담기
    ParamArray = {
        'PageNo': PageNo,
        'PageSize': PageSize,
        'Sv': SearchValue.securityText()
    };

    var obj = eval("SubmitList.succeeded");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2019-07-11
작성목적 : 문서 리스트 가져오기 (웹서비스 값 받기)
***************************************************/
SubmitList.succeeded = function (data, status) {

    $('#phApproval').setTemplateURL('/DeskPlusMobile/Approval/Jt/JTSubmitList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phApproval').processTemplate(data.d);

    setTimeout(function () {
        SubmitList.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2019-07-11
작성목적 : 다국어 처리
**********************************************************************************************/
SubmitList.Resource = function () {

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2019-07-11
작성목적 : 상신 화면으로 가기 
***************************************************/
SubmitList.Add = function (dID) {

    ParamArray = {
        'DID': dID,
        'PageNo': PageNo,
        'Sv': SearchValue.securityText()
    };

    CallEncrypt(CallGetParam());
    document.location.href = "SubmitAdd.aspx?p=" + GetEncryptValue;
}


/**************************************************
작성자   : famo23
작성일자 : 2019-07-11
작성목적 : 검색
***************************************************/
SubmitList.search = function () {

    SearchValue = $("#tbSearch").val();

    ParamArray = {
        'PageNo': 1,
        'Sv': SearchValue.securityText()
    };

    CallEncrypt(CallGetParam());

    location.href = "SubmitList.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2019-07-11
작성목적 : 검색
***************************************************/
SubmitList.SetSearch = function () {

    SubmitList.search();

    return false;
}

/**************************************************
작성자   : famo23
작성일자 : 2019-07-11
작성목적 : 이전 페이지 이동
***************************************************/
SubmitList.pvNum = function () {

    if (parseInt(PageNo) > 1) {
        PageNo = parseInt(PageNo) - 1;

        ParamArray = {
            'PageNo': PageNo,
            'Sv': SearchValue.securityText()
        };

        CallEncrypt(CallGetParam());

        location.href = "SubmitList.aspx?p=" + GetEncryptValue;
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2019-07-11
작성목적 : 다음 페이지 이동
***************************************************/
SubmitList.nxNum = function () {

    var tmpTotalNum = $("#TotalNum").html();

    if (parseInt(PageNo) < parseInt(tmpTotalNum)) {
        PageNo = parseInt(PageNo) + 1;

        ParamArray = {
            'PageNo': PageNo,
            'Sv': SearchValue.securityText()
        };

        CallEncrypt(CallGetParam());

        location.href = "SubmitList.aspx?p=" + GetEncryptValue;
    }
}

