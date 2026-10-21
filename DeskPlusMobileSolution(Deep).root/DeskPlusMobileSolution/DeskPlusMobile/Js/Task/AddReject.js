AddReject = {}

/**************************************************
작성자   : famo23
작성일자 : 2015-05-14
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    $('#btCancel').click(function (e) {
        AddReject.viewList();
    });

    $('#btAdd').click(function (e) {
        AddReject.add();
    });

    $('#before').click(function (e) {
        AddReject.goBefore();
    });

    $('#listTitle').click(function (e) {
        AddReject.viewList();
    });

    //경고창 닫기
    $('.modal_close').click(function (e) {
        e.preventDefault();
        action_popup.close(this);
    });

});

/**************************************************
작성자   : famo23
작성일자 : 2015-05-14
작성목적 : 목록 페이지 이동
***************************************************/
AddReject.goBefore = function () {

    if (Alarm == "Y") {
        ParamArray = {
            'PageNo': PageNo
        };

        CallEncrypt(CallGetParam());

        location.href = "/DeskPlusMobile/MDM/List.aspx?p=" + GetEncryptValue;
    }
    else {
        ParamArray = {
            'BPage': BPage,
            'PageNo': PageNo,
            'Sv': SearchValue.securityText()
        };

        CallEncrypt(CallGetParam());

        location.href = "List.aspx?p=" + GetEncryptValue;
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2015-05-14
작성목적 : 취소 버튼 클릭시
***************************************************/
AddReject.viewList = function () {

    if (Alarm == "Y") {
        ParamArray = {
            'BPage': BPage,
            'BType': BType,
            'PageNo': PageNo,
            'listID': ListID,
            'groupID': GroupID,
            'Status': Status
        };

        CallEncrypt(CallGetParam());

        location.href = "AlarmView.aspx?p=" + GetEncryptValue;
    }
    else {
        ParamArray = {
            'BPage': BPage,
            'BType': BType,
            'PageNo': PageNo,
            'ListID': ListID,
            'GroupID': GroupID,
            'Status': Status,
            'Sv': SearchValue.securityText()
        };

        CallEncrypt(CallGetParam());

        location.href = "View.aspx?p=" + GetEncryptValue;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-05-14
작성목적 : 재검토요청 (웹서비스 호출)
***************************************************/
AddReject.add = function () {

    action_popup.confirm(R_ShallWeReject, function (res) {
        if (res) {

            if (BtnClickCheck != "") {
                NoticeShow("!", R_ProcessingWait);
                return;
            }

            LoadingShow(true);

            var tmpContent = $("#memo").val();

            BtnClickCheck = "Y";
            var url = "/DeskPlusMobile/Task/Ws/SetTask.asmx/Reject";

            //파라미터값 배열에 담기
            ParamArray = {
                'ListID': ListID,
                'Content': encodeURIComponent(tmpContent)
            };

            var obj = eval("AddReject.succeeded");
            CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
            data = "{p:'" + GetEncryptValue + "'}";
            AjaxAsyncService(url, data, obj);
        }
    })
}

/**************************************************
작성자   : famo23
작성일자 : 2015-05-14
작성목적 : 접수하기 등록 (웹서비스 값 받기)
***************************************************/
AddReject.succeeded = function (data, status) {

    if (data.d) {
        //NoticeShow("*", R_Register);
        Status = "5";
        AddReject.viewList();
        LoadingShow(false);
    }
    else {
        NoticeShow("!", R_ProcessingError);
        LoadingShow(false);
    }
}
