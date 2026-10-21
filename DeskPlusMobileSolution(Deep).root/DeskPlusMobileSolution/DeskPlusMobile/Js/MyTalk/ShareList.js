ShareList = {}

/**************************************************
작성자   : famo23
작성일자 : 2012-11-06
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {  

    // 문서 리스트 가져오기
    setTimeout("ShareList.getList();", 400);
    setTimeout("PageSize = 10;", 400);            //기본10개
    
    $('#more').click(function (e) {
        e.preventDefault();
        ShareList.moreList();
    });

    $('#btAdd').click(function (e) {
        ShareList.add();
    })

    $('#address').click(function (e) {
        location.href = "/DeskPlusMobile/Address/List.aspx";
    })

});


/**************************************************
작성자   : famo23
작성일자 : 2012-11-06
작성목적 : 마이톡 리스트 가져오기 (웹서비스 호출)
***************************************************/
ShareList.getList = function () {

    var url = "/DeskPlusMobile/MyTalk/Ws/GetMyTalk.asmx/GetMyTalkList";

    //파라미터값 배열에 담기
    ParamArray = {
        'PageNo': PageNo,
        'PageSize': PageSize
    };

    var obj = eval("ShareList.succeeded");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2012-11-06
작성목적 : 마이톡 리스트 가져오기 (웹서비스 값 받기)
***************************************************/
ShareList.succeeded = function (data, status) {

    $('#phTemp').setTemplateURL('/DeskPlusMobile/MyTalk/Jt/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phTemp').processTemplate(data.d);

    if (data.d == null || data.d == "") {
        $('#phEmpty').html($('#phTemp').html());
    } else {
        $('#phMyTalk').append($('#phTemp').html());
    }

    PageNo = parseInt(PageNo) + 1;

    if (MoreNo > 1) {
        PageNo = MoreNo;
        MoreNo = 1;
    }

    setTimeout(function () {
        ShareList.Resource();
    }, 100);

}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-11-06
작성목적 : 다국어 처리
**********************************************************************************************/
ShareList.Resource = function () {

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }

    if ($("#SecurityData")[0] != undefined) {
        $("#SecurityData")[0].innerHTML = R_SecurityMsg;
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2012-11-06
작성목적 : 더보기 선택시 
***************************************************/
ShareList.moreList = function () {

    ShareList.getList();
    MoreSize = parseInt(MoreSize) + 10;

}

/**************************************************
작성자   : famo23
작성일자 : 2012-11-06
작성목적 : 마이톡 상세보기 화면으로 가기 
***************************************************/
ShareList.ViewList = function (_groupID) {

    ParamArray = {
        'GroupID': _groupID,
        'MoreSize': MoreSize,
        'PageNo': PageNo
    };

    CallEncrypt(CallGetParam());

    location.href = "View.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2012-11-06
작성목적 : 마이톡 댓글 등록 화면으로 가기 
***************************************************/
ShareList.add = function () {

    ParamArray = {
        'MoreSize': MoreSize,
        'PageNo': PageNo,
        'RUserID': UserID
    };

    CallEncrypt(CallGetParam());

    location.href = "Add.aspx?p=" + GetEncryptValue;
}
