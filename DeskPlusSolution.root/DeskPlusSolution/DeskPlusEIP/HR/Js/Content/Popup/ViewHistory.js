ViewHistory = {}

$(function () {

    $(document).ready(function () {

        //닫기
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        ViewHistory.Init();
        //데이터 로드
        ViewHistory.LoadData("");
    });
});


/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-21
작성목적 : 초기화
***************************************************/
ViewHistory.Init = function () {

}

/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 리스트 초기화
***************************************************/
ViewHistory.LoadData = function (gubun) {

    var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";

    $('#loading').html(loadingPath);

    setTimeout(function () {
        ViewHistory.GetPageList();
    }, 100);
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 리스트 호출
**********************************************************************************************/
ViewHistory.GetPageList = function () {
    var url = "/DeskPlusEIP/HR/Ws/Content/Popup/ViewHistory.asmx/GetAttendanceHistory";

    ParamArray = {
        'attID': AttID
    };

    var obj = eval("ViewHistory.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 성공시  리스트 표시
**********************************************************************************************/
ViewHistory.SuccessPageList = function (data, status) {

    $('#placeholder').setTemplateURL('/DeskPlusEIP/HR/Jt/Content/Popup/JTListViewHistory.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    ViewHistory.Resource();
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
ViewHistory.Resource = function () {

    $("#r_WSTime")[0].innerHTML = R_WSTime;     //출근시간
    $("#r_WSConn")[0].innerHTML = R_WSConn;     //접속
    $("#r_WCTime")[0].innerHTML = R_WCTime;     //퇴근시간
    $("#r_WCConn")[0].innerHTML = R_WSConn;     //접속
    $("#r_LateRemark")[0].innerHTML = R_LateRemark;     //지각사유
    $("#r_EarlyRemark")[0].innerHTML = R_EarlyRemark;     //조태사유
    $("#r_ModifyName")[0].innerHTML = R_ModifyName;     //수정자
    $("#r_ModifyDate")[0].innerHTML = R_ModifyDate;     //수정일
    $("#r_ModifyReason")[0].innerHTML = R_ModifyReason;  //수정사유

    //데이터없음
    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}