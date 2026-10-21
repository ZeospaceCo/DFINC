List = {}

/**************************************************
작성자   : famo23
작성일자 : 2012-11-06
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

    $('#btAdd').click(function (e) {
        List.add();
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
List.getList = function () {

    var url = "/DeskPlusMobile/MyTalk/Ws/GetMyTalk.asmx/GetMyTalkList";

    //파라미터값 배열에 담기
    ParamArray = {
        'PageNo': PageNo,
        'PageSize': PageSize
    };

    var obj = eval("List.succeeded");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2012-11-06
작성목적 : 마이톡 리스트 가져오기 (웹서비스 값 받기)
***************************************************/
List.succeeded = function (data, status) {

    $('#phMyTalk').setTemplateURL('/DeskPlusMobile/MyTalk/Jt/JtSNSList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phMyTalk').processTemplate(data.d);

    setTimeout(function () {
        List.Resource();
    }, 100);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-11-06
작성목적 : 다국어 처리
**********************************************************************************************/
List.Resource = function () {
    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }

    $("[id^=touchslider]").each(function () {
        $(this).touchSlider({
            duration: 350,
            flexible: true,
            paging: false,
            controls: false,
            counter: function (e) {
                $(this).next().html(e.current + "/" + e.total);
            }
        });
    })
}

/**************************************************
작성자   : famo23
작성일자 : 2012-11-06
작성목적 : 마이톡 상세보기 화면으로 가기 
***************************************************/
List.ViewList = function (_groupID) {

    ParamArray = {
        'GroupID': _groupID,
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
List.add = function () {

    ParamArray = {
        'PageNo': PageNo,
        'RUserID': UserID
    };

    CallEncrypt(CallGetParam());

    location.href = "Add.aspx?p=" + GetEncryptValue;
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
            'PageNo': PageNo
        };

        CallEncrypt(CallGetParam());

        location.href = "List.aspx?p=" + GetEncryptValue;
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
            'PageNo': PageNo
        };

        CallEncrypt(CallGetParam());

        location.href = "List.aspx?p=" + GetEncryptValue;
    }
}
