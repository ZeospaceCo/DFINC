DocList = {}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-21
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    if (BPage == "Private") {
        $('#listTitle').html("<i class='fas fa-file-signature  dropbtn '></i>" + R_PrivateFolder);
    }
    else if (BPage == "Approval") {
        $('#listTitle').html("<i class='fas fa-file-signature  dropbtn '></i>" + R_ApprovalFolder);
    }
    else if (BPage == "Reject") {
        $('#listTitle').html("<i class='fas fa-file-signature  dropbtn '></i>" + R_RejectFolder);
    }
    else if (BPage == "Arbitrary") {
        $('#listTitle').html("<i class='fas fa-file-signature  dropbtn '></i>" + R_ArbitraryFolder);
    }
    else if (BPage == "Forward") {
        $('#listTitle').html("<i class='fas fa-file-signature  dropbtn '></i>" + R_ForwardFolder);
    }

    // 문서 리스트 가져오기
    setTimeout("DocList.getList();", 400);

    $('#pvNum').click(function (e) {
        e.preventDefault();
        DocList.pvNum();
    });

    $('#nxNum').click(function (e) {
        e.preventDefault();
        DocList.nxNum();
    });


    $('#listTitle').click(function (e) {
        location.href = "Folder.aspx";
    });

    $('#search').click(function (e) {
        e.preventDefault();
        DocList.search();
    });

    $("#tbSearch").attr("placeholder", R_InputApp);

});


/**************************************************
작성자   : famo23
작성일자 : 2015-10-21
작성목적 : 문서 리스트 가져오기 (웹서비스 호출)
***************************************************/
DocList.getList = function () {
    //LoadingShow(true);

    var url = "/DeskPlusMobile/Approval/Ws/GetApproval.asmx/GetDocList";

    //파라미터값 배열에 담기
    ParamArray = {
        'BPage': BPage,
        'PageNo': PageNo,
        'PageSize': PageSize,
        'Sv': SearchValue.securityText()
    };

    var obj = eval("DocList.succeeded");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-21
작성목적 : 문서 리스트 가져오기 (웹서비스 값 받기)
***************************************************/
DocList.succeeded = function (data, status) {

    $('#phApproval').setTemplateURL('/DeskPlusMobile/Approval/Jt/JTDocList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phApproval').processTemplate(data.d);

    setTimeout(function () {
        DocList.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-10-21
작성목적 : 다국어 처리
**********************************************************************************************/
DocList.Resource = function () {

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2015-10-21
작성목적 : 문서 보기 화면으로 가기 
***************************************************/
DocList.viewList = function (dID, keyID, key2ID) {

    ParamArray = {
        'BPage': BPage,
        'PageNo': PageNo,
        'DID': dID,
        'Sv': SearchValue.securityText(),
        'KeyID': keyID,
        'key2ID': key2ID
    };

    CallEncrypt(CallGetParam());
    document.location.href = "DocView.aspx?p=" + GetEncryptValue;
}



/**************************************************
작성자   : famo23
작성일자 : 2015-10-21
작성목적 : 검색
***************************************************/
DocList.search = function () {

    SearchValue = $("#tbSearch").val();

    ParamArray = {
        'BPage': BPage,
        'Sv': SearchValue.securityText()
    };

    CallEncrypt(CallGetParam());

    location.href = "DocList.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-21
작성목적 : 검색
***************************************************/
DocList.SetSearch = function () {

    DocList.search();

    return false;
}


/**************************************************
작성자   : famo23
작성일자 : 2014-09-16
작성목적 : 이전 페이지 이동
***************************************************/
DocList.pvNum = function () {

    if (parseInt(PageNo) > 1) {
        PageNo = parseInt(PageNo) - 1;

        ParamArray = {
            'BPage': BPage,
            'PageNo': PageNo,
            'Sv': SearchValue.securityText()
        };

        CallEncrypt(CallGetParam());

        location.href = "DocList.aspx?p=" + GetEncryptValue;
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2014-09-16
작성목적 : 다음 페이지 이동
***************************************************/
DocList.nxNum = function () {

    var tmpTotalNum = $("#TotalNum").html();

    if (parseInt(PageNo) < parseInt(tmpTotalNum)) {
        PageNo = parseInt(PageNo) + 1;

        ParamArray = {
            'BPage': BPage,
            'PageNo': PageNo,
            'Sv': SearchValue.securityText()
        };

        CallEncrypt(CallGetParam());

        location.href = "DocList.aspx?p=" + GetEncryptValue;
    }
}

