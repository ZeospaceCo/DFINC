List = {}

/**************************************************
작성자   : 이중호
작성일자 : 2019-08-02
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    $('#btAll').click(function (e) {
        NoticeHide();
        List.checkAll();
    });

    $('#btDel').click(function (e) {
        NoticeHide();
        List.DelAll();
    });

    $('#btRProcessing').click(function (e) {
        NoticeHide();
        List.ReadAll();
    });

    $('#pvNum').click(function (e) {
        e.preventDefault();
        List.pvNum();
    });

    $('#nxNum').click(function (e) {
        e.preventDefault();
        List.nxNum();
    });

    List.getList();
});

/**************************************************
작성자   : 이중호
작성일자 : 2019-08-14
작성목적 : 파일 리스트 가져오기 (웹서비스 호출)
***************************************************/
List.getList = function () {
    var url = "/DeskPlusMobile/MDM/Ws/GetNotification.asmx/GetList";

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
작성자   : 이중호
작성일자 : 2019-08-14
작성목적 : 문서 리스트 가져오기 (웹서비스 값 받기)
***************************************************/
List.succeeded = function (data, status) {

    $('#phNotiList').setTemplateURL('/DeskPlusMobile/MDM/Jt/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phNotiList').processTemplate(data.d);

    setTimeout(function () {
        List.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : 이중호
작성일자 : 2019-08-14
작성목적 : 다국어 처리
**********************************************************************************************/
List.Resource = function () {

    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }
}

/**************************************************
작성목적 : 이전 페이지 이동
***************************************************/
List.pvNum = function () {

    if (parseInt(PageNo) > 1) {
        PageNo = parseInt(PageNo) - 1;

        ParamArray = {
            'PageNo': PageNo,
            'PageSize': PageSize
        };

        CallEncrypt(CallGetParam());

        location.href = "List.aspx?p=" + GetEncryptValue;
    }
}


/**************************************************
작성목적 : 다음 페이지 이동
***************************************************/
List.nxNum = function () {

    var tmpTotalNum = $("#TotalNum").html();

    if (parseInt(PageNo) < parseInt(tmpTotalNum)) {
        PageNo = parseInt(PageNo) + 1;

        ParamArray = {
            'PageNo': PageNo,
            'PageSize': PageSize
        };

        CallEncrypt(CallGetParam());

        location.href = "List.aspx?p=" + GetEncryptValue;
    }
}



/**************************************************
작성자   : 이중호
작성일자 : 2019-08-14
작성목적 : 전체선택 / 전체해제
***************************************************/
List.checkAll = function () {

    if ($("#allcheck").text() == R_SelAll) {
        if ($("input:checkbox").length > 0) {
            $("#allcheck").text(R_SelNoAll);

            for (var i = 0; i < $("input:checkbox").length; i++) {
                $("input:checkbox")[i].checked = true;
            }
        }
    }
    else {
        if ($("input:checkbox").length > 0) {
            $("#allcheck").text(R_SelAll);

            for (var i = 0; i < $("input:checkbox").length; i++) {
                $("input:checkbox")[i].checked = false;
            }
        }
    }
}

/**************************************************
작성자   : 이중호
작성일자 : 2019-08-21
작성목적 : 뷰
***************************************************/
List.view = function (notificationID, targetUrl) {

    var url = "/DeskPlusMobile/MDM/WS/SetNotification.asmx/ReadNotification";

    //파라미터값 배열에 담기
    ParamArray = {
        'NotificationID': notificationID,
        'mobileLinkUrl': targetUrl
    };

    var obj = eval("List.succeededView");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);

}

List.succeededView = function (data, status) {

    if (data.d != "false") {

        if (data.d != "") {
            document.location.href = data.d + "&PageNo=" + PageNo;
        }
        else {
            List.getList();
        }
    }

}

/**************************************************
작성자   : 이중호
작성일자 : 2019-08-22
작성목적 : 전체 읽음 처리
***************************************************/
List.ReadAll = function () {

    var NotiIDList = "";

    $("input[id^=Noti_]:checked").each(function () {
        NotiIDList += $(this).val() + ",";
    });

    NotiIDList = NotiIDList.trimEnds(",");

    if (NotiIDList.length == 0) {
        NoticeShow("!", R_ReadNoNoti);
        return;
    }

    var url = "/DeskPlusMobile/MDM/WS/SetNotification.asmx/ReadAllNotification";

    //파라미터값 배열에 담기
    ParamArray = {
        'NotiIDList': NotiIDList
    };

    var obj = eval("List.succeededReadAll");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);

}

List.succeededReadAll = function (data, status) {
    if (data.d == true) {
        List.getList();
    }
}

/**************************************************
작성자   : 이중호
작성일자 : 2019-08-22
작성목적 : 삭제 처리
***************************************************/
List.DelAll = function () {

    var NotiIDList = "";

    $("input[id^=Noti_]:checked").each(function () {
        NotiIDList += $(this).val() + ",";
    });

    NotiIDList = NotiIDList.trimEnds(",");

    if (NotiIDList.length == 0) {
        NoticeShow("!", R_DelNoNoti);
        return;
    }

    var url = "/DeskPlusMobile/MDM/WS/SetNotification.asmx/DelAllNotification";

    //파라미터값 배열에 담기
    ParamArray = {
        'NotiIDList': NotiIDList
    };

    var obj = eval("List.succeededDelAll");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);

}

List.succeededDelAll = function (data, status) {
    if (data.d == true) {

        //파라미터값 배열에 담기
        ParamArray = {
            'PageNo': PageNo,
            'PageSize': PageSize,
            'UUID': UUID
        };

        CallEncrypt(CallGetParam());

        location.href = "List.aspx?p=" + GetEncryptValue;
    }
}

