List = {}

/**************************************************
작성자   : famo23
작성일자 : 2011-06-27
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    if (BPage == "REP") {
        $('#listTitle').html("<i class='fas fa-file-signature  dropbtn '></i>" + R_Report);
    }
    else if (BPage == "REL") {
        $('#listTitle').html("<i class='fas fa-file-signature  dropbtn '></i>" + R_Related);
    }
    else if (BPage == "APP") {
        $('#listTitle').html("<i class='fas fa-file-signature  dropbtn '></i>" + R_Approved);
    }

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
        if (UserID == TargetUID) {
            location.href = "Folder.aspx";
        }
        else {
            location.href = "Proxy.aspx";
        }
    });

    $('#search').click(function (e) {
        e.preventDefault();
        List.search();
    });

    $("#tbSearch").attr("placeholder", R_InputApp);

});




/**************************************************
작성자   : famo23
작성일자 : 2011-06-27
작성목적 : 문서 리스트 가져오기 (웹서비스 호출)
***************************************************/
List.getList = function () {
    //LoadingShow(true);
    var url = "/DeskPlusMobile/Approval/Ws/GetApproval.asmx/GetList";

    //파라미터값 배열에 담기
    ParamArray = {
        'BPage': BPage,
        'PageNo': PageNo,
        'PageSize': PageSize,
        'TargetUID': TargetUID,
        'Sv': SearchValue.securityText()
    };

    var obj = eval("List.succeeded");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2011-06-27
작성목적 : 문서 리스트 가져오기 (웹서비스 값 받기)
***************************************************/
List.succeeded = function (data, status) {

    $('#phApproval').setTemplateURL('/DeskPlusMobile/Approval/Jt/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phApproval').processTemplate(data.d);

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
작성일자 : 2011-06-28
작성목적 : 문서 보기 화면으로 가기 
***************************************************/
List.viewList = function (dID, stat, aTag, accTag, num2, seq, aDeptID) {

    /*
    $("#hdPoint").val(getNowScroll().Y);
    $("#hdSize").val(MoreSize);
    $("#hdNo").val(PageNo);
    */

    //$('html, body').animate({ scrollTop: getNowScroll().Y }, 500);

    ParamArray = {
        'BPage': BPage,
        'PageNo': PageNo,
        'DID': dID,
        'Stat': stat,
        'ATag': aTag,
        'AccTag': accTag,
        'Num2': num2,
        'Seq': seq,
        'TargetUID': TargetUID,
        'TargetDID': aDeptID,
        'Sv': SearchValue.securityText()
    };

    CallEncrypt(CallGetParam());
    document.location.href = "View.aspx?p=" + GetEncryptValue;
}



/**************************************************
작성자   : famo23
작성일자 : 2014-08-26
작성목적 : 검색
***************************************************/
List.search = function () {

    SearchValue = $("#tbSearch").val();

    ParamArray = {
        'BPage': BPage,
        'PageNo': 1,
        'TargetUID': TargetUID,
        'Sv': SearchValue.securityText()
    };

    CallEncrypt(CallGetParam());

    location.href = "List.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2014-08-26
작성목적 : 검색
***************************************************/
List.SetSearch = function () {

    List.search();

    return false;
}

/**************************************************
작성자   : famo23
작성일자 : 2014-09-16
작성목적 : 이전 페이지 이동
***************************************************/
List.pvNum = function () {

    if (parseInt(PageNo) > 1) {
        PageNo = parseInt(PageNo) - 1;

        ParamArray = {
            'BPage': BPage,
            'PageNo': PageNo,
            'TargetUID': TargetUID,
            'Sv': SearchValue.securityText()
        };

        CallEncrypt(CallGetParam());

        location.href = "List.aspx?p=" + GetEncryptValue;
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2014-09-16
작성목적 : 다음 페이지 이동
***************************************************/
List.nxNum = function () {

    var tmpTotalNum = $("#TotalNum").html();

    if (parseInt(PageNo) < parseInt(tmpTotalNum)) {
        PageNo = parseInt(PageNo) + 1;

        ParamArray = {
            'BPage': BPage,
            'PageNo': PageNo,
            'TargetUID': TargetUID,
            'Sv': SearchValue.securityText()
        };

        CallEncrypt(CallGetParam());

        location.href = "List.aspx?p=" + GetEncryptValue;
    }
}

