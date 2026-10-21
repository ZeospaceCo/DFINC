CirDocList = {}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-23
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    if (BPage == "CirRecv") {
        $('#listTitle').html("<i class='fas fa-file-signature  dropbtn '></i>" + R_CirRecv);
    }
    else if (BPage == "CirSend") {
        $('#listTitle').html("<i class='fas fa-file-signature  dropbtn '></i>" + R_CirSend);
    }

    // 문서 리스트 가져오기
    setTimeout("CirDocList.getList();", 400);

    $('#pvNum').click(function (e) {
        e.preventDefault();
        CirDocList.pvNum();
    });

    $('#nxNum').click(function (e) {
        e.preventDefault();
        CirDocList.nxNum();
    });

    $('#listTitle').click(function (e) {
        location.href = "Folder.aspx";
    });

    $('#search').click(function (e) {
        e.preventDefault();
        CirDocList.search();
    });

    $("#tbSearch").attr("placeholder", R_InputApp);

});


/**************************************************
작성자   : famo23
작성일자 : 2015-10-23
작성목적 : 문서 리스트 가져오기 (웹서비스 호출)
***************************************************/
CirDocList.getList = function () {
    //LoadingShow(true);

    var url = "/DeskPlusMobile/Approval/Ws/GetApproval.asmx/GetCirDocList";

    //파라미터값 배열에 담기
    ParamArray = {
        'BPage': BPage,
        'PageNo': PageNo,
        'PageSize': PageSize,
        'Sv': SearchValue.securityText()
    };

    var obj = eval("CirDocList.succeeded");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-23
작성목적 : 문서 리스트 가져오기 (웹서비스 값 받기)
***************************************************/
CirDocList.succeeded = function (data, status) {

    $('#phApproval').setTemplateURL('/DeskPlusMobile/Approval/Jt/JTCirDocList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phApproval').processTemplate(data.d);

    setTimeout(function () {
        CirDocList.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-10-23
작성목적 : 다국어 처리
**********************************************************************************************/
CirDocList.Resource = function () {

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-23
작성목적 : 문서 보기 화면으로 가기 
***************************************************/
CirDocList.viewList = function (cirDocID, cirUserID, docID) {

    ParamArray = {
        'BPage': BPage,
        'PageNo': PageNo,
        'Sv': SearchValue.securityText(),
        'CirDocID': cirDocID,
        'CirUserID': cirUserID,
        'dID': docID
    };

    CallEncrypt(CallGetParam());
    document.location.href = "CirDocView.aspx?p=" + GetEncryptValue;
}



/**************************************************
작성자   : famo23
작성일자 : 2015-10-23
작성목적 : 검색
***************************************************/
CirDocList.search = function () {

    SearchValue = $("#tbSearch").val();

    ParamArray = {
        'BPage': BPage,
        'PageNo': 1,
        'Sv': SearchValue.securityText()
    };

    CallEncrypt(CallGetParam());

    location.href = "CirDocList.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-23
작성목적 : 검색
***************************************************/
CirDocList.SetSearch = function () {

    CirDocList.search();

    return false;
}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-27
작성목적 : 이전 페이지 이동
***************************************************/
CirDocList.pvNum = function () {

    if (parseInt(PageNo) > 1) {
        PageNo = parseInt(PageNo) - 1;

        ParamArray = {
            'BPage': BPage,
            'PageNo': PageNo,
            'Sv': SearchValue.securityText()
        };

        CallEncrypt(CallGetParam());

        location.href = "CirDocList.aspx?p=" + GetEncryptValue;
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2015-10-27
작성목적 : 다음 페이지 이동
***************************************************/
CirDocList.nxNum = function () {

    var tmpTotalNum = $("#TotalNum").html();

    if (parseInt(PageNo) < parseInt(tmpTotalNum)) {
        PageNo = parseInt(PageNo) + 1;

        ParamArray = {
            'BPage': BPage,
            'PageNo': PageNo,
            'Sv': SearchValue.securityText()
        };

        CallEncrypt(CallGetParam());

        location.href = "CirDocList.aspx?p=" + GetEncryptValue;
    }
}