View = {}

/**************************************************
작성자   : 방영환
작성일자 : 2011-06-17
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    if (ReservationID != 0) {

        setTimeout("View.getData();", 400);
        setTimeout("View.getReply();", 400);
        setTimeout("View.getReject();", 400);
    }

    $('#btDel').click(function (e) {
        View.del();
    });

    $('#btReply').click(function (e) {
        View.reply();
    });

    $('#btApproved').click(function (e) {
        View.modUsable("1");
    });

    $('#btUnApproved').click(function (e) {
        //View.modUsable("2");
        View.reject();
    });

    $('#listTitle').click(function (e) {
        View.goBefore();
    });

    $('#before').click(function (e) {
        View.goBefore();
    });

    //경고창 닫기
    $('.modal_close').click(function (e) {
        e.preventDefault();
        action_popup.close(this);
    });

    if (window.history && window.history.pushState) {
        window.history.replaceState(null, null, '');
        $(window).on('popstate', function (event) {
            LoadingShow(false);
        });
    }
});


/**************************************************
작성자   : 방영환
작성일자 : 2011-06-17
작성목적 : 자원예약내용 가져오기 (웹서비스 호출)
***************************************************/
View.getData = function () {
    var url = "/DeskPlusMobile/RM/Ws/GetRM.asmx/GetReserve";

    //파라미터값 배열에 담기
    ParamArray = {
        'ReservationID': ReservationID
    };

    var obj = eval("View.succeededData");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**************************************************
작성자   : 방영환
작성일자 : 2011-06-17
작성목적 : 자원예약내용 가져오기 가져오기 (웹서비스 값 받기)
***************************************************/
View.succeededData = function (data, status) {


    var reserveCont = eval('(' + data.d + ')');


    $("#title").append("[" + reserveCont.GetReserve[0].Title + "] " + reserveCont.GetReserve[0].Subject);
    $("#date").append(R_ReserveDate + " : " + reserveCont.GetReserve[0].ReserveDate);
    $("#etc").append(reserveCont.GetReserve[0].ModifyUserInfo + "<BR>" + R_ModifyDate + " : " + reserveCont.GetReserve[0].ModifyDateStr + "<BR> " + R_SecuritySetting + " : " + reserveCont.GetReserve[0].IsGuardStr + " <font class='tViewS'>[" + reserveCont.GetReserve[0].UsableStr + "]</font>");
    //"<font class='istf'> [" + View.getStatus(conObj.dsInfo[0].StatusLast) + "]</font>"
    $("#content").append(reserveCont.GetReserve[0].bodyNoStyle);

    ParentReservationID = reserveCont.GetReserve[0].ParentReservationID;
    WriteYM = reserveCont.GetReserve[0].WriteYM;
    StartDate = reserveCont.GetReserve[0].StartDateStr;

    //버튼처리
    /*
    - 댓글은 항상 활성화
    1. 작성자와 본인이 같은경우
    - 삭제, 수정

    2. 본인이 담당자인 경우
    - 삭제, 수정
    해당자원이 승인되어 있으면 거부
    미승인인 경우 승인

    3. 해당 자원이 삭제 되었을 경우 수정불가 Active=0
    - 삭제

    4. 해당 자원이 담당자 승인해야 하는 자원인지 확인 필요.
    */

    if (UserID == reserveCont.GetReserve[0].WriteUserID) {
        //본인이 작성자일 경우
        $("#btDel").show();
    }

    if (UserID == reserveCont.GetReserve[0].ManagerUserID) {
        //본인이 담당자일 경우
        $("#btDel").show();

        if (reserveCont.GetReserve[0].Authority == "1") {
            if (reserveCont.GetReserve[0].Usable == "0") {
                $("#btApproved").show();
                $("#btUnApproved").show();
            }
            else if (reserveCont.GetReserve[0].Usable == "2") {
                $("#btApproved").show();
            }
            else if (reserveCont.GetReserve[0].Usable == "1") {
                $("#btUnApproved").show();
            }
        }
    }

}



/**************************************************
작성자   : 방영환
작성일자 : 2011-06-20
작성목적 : 이전 페이지 이동
***************************************************/
View.goBefore = function () {
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


/**************************************************
작성자   : 방영환
작성일자 : 2012-11-07
작성목적 : 일정 댓글 가져오기 (웹서비스 호출)
***************************************************/
View.getReply = function () {
    var url = "/DeskPlusMobile/RM/Ws/GetRM.asmx/GetReply";

    //파라미터값 배열에 담기
    ParamArray = {
        'ReservationID': ReservationID
    };

    var obj = eval("View.succeededReply");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}


/**************************************************
작성자   : 방영환
작성일자 : 2012-11-07
작성목적 : 일정 댓글  가져오기 (웹서비스 값 받기)
***************************************************/
View.succeededReply = function (data, status) {
    var srObj = data.d;

    var tmpSR = "";

    if (srObj.length > 0) {
        jQuery.each($("#reply"), function () { this.style.display = ""; });
    }

    for (var i = 0; i < srObj.length; i++) {
        tmpSR += "<div class='title'><div>";
        tmpSR += "<table border='0' cellpadding='0' cellspacing='0' style='width:100%'>";
        tmpSR += "<tr><td align='center'>&nbsp;</td><td class='lif'>";
        tmpSR += srObj[i].UserName + "&nbsp;| " + srObj[i].InputDate;
        tmpSR += "<br />";
        tmpSR += srObj[i].Content + " " + srObj[i].ShowNew;
        tmpSR += "</td>";
        tmpSR += "<td>&nbsp;</td></tr><tr>"
        tmpSR += "<td height='1' colspan='3' align='center' bgcolor='#CCCCCC'></td></tr></table>";
        tmpSR += "</table>";
        tmpSR += "</div></div>";
    }

    $("#reply").empty();
    $("#reply").append(tmpSR);

}


/**************************************************
작성자   : 방영환
작성일자 : 2012-11-07
작성목적 : 거부사유 가져오기 (웹서비스 호출)
***************************************************/
View.getReject = function () {
    var url = "/DeskPlusMobile/RM/Ws/GetRM.asmx/GetReject";

    //파라미터값 배열에 담기
    ParamArray = {
        'ReservationID': ReservationID
    };

    var obj = eval("View.succeededReject");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}


/**************************************************
작성자   : 방영환
작성일자 : 2012-11-07
작성목적 : 거부사유 가져오기 (웹서비스 값 받기)
***************************************************/
View.succeededReject = function (data, status) {
    var srObj = data.d;

    var tmpSR = "";

    if (srObj.length > 0) {
        jQuery.each($("#reject"), function () { this.style.display = ""; });
    }

    for (var i = 0; i < srObj.length; i++) {
        tmpSR += "<div class='title'><div>";
        tmpSR += "<table border='0' cellpadding='0' cellspacing='0' style='width:100%'>";
        tmpSR += "<tr><td align='center'>&nbsp;</td><td class='lif'>";
        tmpSR += "<B>" + R_RejectStr + "</B><br />";
        tmpSR += srObj[i].UserName + "&nbsp;| " + srObj[i].InputDate;
        tmpSR += "<br />";
        tmpSR += srObj[i].Content;
        tmpSR += "</td>";
        tmpSR += "<td>&nbsp;</td></tr><tr>"
        tmpSR += "<td height='1' colspan='3' align='center' bgcolor='#CCCCCC'></td></tr></table>";
        tmpSR += "</table>";
        tmpSR += "</div></div>";
    }

    $("#reject").empty();
    $("#reject").append(tmpSR);

}

/**************************************************
작성자   : 방영환
작성일자 : 2011-06-20
작성목적 : 자원예약 삭제하기
***************************************************/
View.del = function () {
    action_popup.confirm(R_DelList, function (res) {
        if (res) {
            if (ReservationID != ParentReservationID) {
                //반복일정 삭제 페이지로 이동
                View.repeatDel();
            }
            else {
                //반복일정인지 체크
                View.getRepeat();
            }
        }
    })
}

/**************************************************
작성자   : 방영환
작성일자 : 2011-06-20
작성목적 : 자원예약 삭제하기 (웹서비스 호출)
***************************************************/
View.delReserve = function () {
    var url = "/DeskPlusMobile/RM/Ws/SetRM.asmx/Del";

    //파라미터값 배열에 담기
    ParamArray = {
        'reservationID': ReservationID,
        'parentReservationID': ParentReservationID,
        'writeYM': WriteYM
    };

    var obj = eval("View.successDel");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**************************************************
작성자   : 방영환
작성일자 : 2011-06-20
작성목적 : 일정 삭제하기 (웹서비스 값 받기)
***************************************************/
View.successDel = function (data, status) {
    View.goBefore();
}


/**************************************************
작성자   : 방영환
작성일자 : 2011-06-20
작성목적 : 반복일정인지 체크 (웹서비스 호출)
***************************************************/
View.getRepeat = function () {
    var url = "/DeskPlusMobile/RM/Ws/GetRM.asmx/GetRepeat";

    //파라미터값 배열에 담기
    ParamArray = {
        'reservationID': ReservationID
    };

    var obj = eval("View.successGetRepeat");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**************************************************
작성자   : 방영환
작성일자 : 2011-06-20
작성목적 : 반복일정인지 체크 (웹서비스 값 받기)
***************************************************/
View.successGetRepeat = function (data, status) {
    if (data.d) {
        //반복일정일 경우 반복일정 페이지로 이동.
        View.repeatDel();
    }
    else {
        View.delReserve();
    }
}


/**************************************************
작성자   : 방영환
작성일자 : 2011-06-17
작성목적 : 댓글페이지로 이동
***************************************************/
View.reply = function () {

    ParamArray = {
        'ReservationID': ReservationID,
        'ParentReservationID': ParentReservationID,
        'OpenDate': TmpDate,
        'PageNo': PageNo,
        'Sv': SearchValue,
        'BPage': BPage
    };

    CallEncrypt(CallGetParam());

    location.href = "ReplyAdd.aspx?p=" + GetEncryptValue;

}

/**************************************************
작성자   : 방영환
작성일자 : 2011-06-17
작성목적 : 거부사유 작성 페이지로 이동
***************************************************/
View.reject = function () {

    ParamArray = {
        'ReservationID': ReservationID,
        'ParentReservationID': ParentReservationID,
        'OpenDate': TmpDate,
        'PageNo': PageNo,
        'Sv': SearchValue,
        'BPage': BPage
    };

    CallEncrypt(CallGetParam());

    location.href = "RejectAdd.aspx?p=" + GetEncryptValue;

}


/**************************************************
작성자   : 방영환
작성일자 : 2011-06-20
작성목적 : 승인 or 거부 (웹서비스 호출)
***************************************************/
View.modUsable = function (_usable) {

    var url = "/DeskPlusMobile/RM/Ws/SetRM.asmx/ModUsable";

    //파라미터값 배열에 담기
    ParamArray = {
        'reservationID': ReservationID,
        'usable': _usable
    };

    var obj = eval("View.successModUsable");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**************************************************
작성자   : 방영환
작성일자 : 2011-06-20
작성목적 : 승인 or 거부 (웹서비스 값 받기)
***************************************************/
View.successModUsable = function (data, status) {
    if (data.d) {
        NoticeShow("*", R_Complete);
        View.goBefore();
    }
}



/**************************************************
작성자   : 방영환
작성일자 : 2011-06-17
작성목적 : 반복일정삭제 페이지로 이동
***************************************************/
View.repeatDel = function () {

    ParamArray = {
        'ReservationID': ReservationID,
        'ParentReservationID': ParentReservationID,
        'OpenDate': TmpDate,
        'PageNo': PageNo,
        'Sv': SearchValue,
        'BPage': BPage,
        'startDate': StartDate,
        'writeYM': WriteYM
    };

    CallEncrypt(CallGetParam());

    location.href = "RepeatDel.aspx?p=" + GetEncryptValue;

}