RepeatDel = {}

/**************************************************
작성자   : 방영환
작성일자 : 2011-06-21
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {


    $('#btCancel').click(function (e) {
        RepeatDel.goBefore();
    });

    $('#btConfirm').click(function (e) {
        RepeatDel.confirm();
    });
});



/**************************************************
작성자   : 방영환
작성일자 : 2011-06-21
작성목적 : 자원예약 저장 버튼 클릭시 
***************************************************/
RepeatDel.confirm = function () {

    var RepeatSel = "";

    if ($("#rdRepeat1").is(":checked")) {
        RepeatSel = "1";
    }
    else if ($("#rdRepeat2").is(":checked")) {
        RepeatSel = "2";
    }
    else {
        RepeatSel = "3";
    }


    LoadingShow(true);

    var url = "/DeskPlusMobile/RM/Ws/SetRM.asmx/Del";

    //파라미터값 배열에 담기
    ParamArray = {
        'reservationID': ReservationID,
        'parentReservationID': ParentReservationID,
        'writeYM': WriteYM,
        'repeatSel': RepeatSel,
        'startDate': StartDate
    };

    var obj = eval("RepeatDel.successConfirm");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);

}

/**********************************************************************************************
작성자   : 방영환
작성일자 : 2012-02-16
작성목적 : 자원을 사용할 수 있는 지 확인 (웹서비스 값 받기)
**********************************************************************************************/
RepeatDel.successConfirm = function (data, status) {
    if (data.d) {
        LoadingShow(false);
        RepeatDel.goBefore();
    }
    else {
        NoticeShow("!", "에러");
        LoadingShow(false);
    }
}




/**************************************************
작성자   : 방영환
작성일자 : 2011-06-24
작성목적 : 자원예약 취소
***************************************************/
RepeatDel.goBefore = function () {

    var url = "List.aspx";
    if (BPage == "Usable0") {
        url = "UsableList.aspx";
    }

    ParamArray = {
        'OpenDate': TmpDate,
        'PageNo': PageNo,
        'Sv': SearchValue,
        'BPage': BPage
    };

    CallEncrypt(CallGetParam());

    location.href = url + "?p=" + GetEncryptValue;
}

