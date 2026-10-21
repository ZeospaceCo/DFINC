View = {}

/**************************************************
작성자   : freeman
작성일자 : 2011-07-09
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    setTimeout("View.GetView();", 400);

    $('#before').click(function (e) {
        View.goBefore();
    });

    $('#listTitle').click(function (e) {
        View.goBefore();
    });
});



/**************************************************
작성자   : freeman
작성일자 : 2011-07-14
작성목적 : 주소 정보 가져오기 (웹서비스 호출)
***************************************************/
View.GetView = function () {
    //LoadingShow(true);
    var url = "/DeskPlusMobile/Address/Ws/GetAddress.asmx/GetAddressInfo";

    //파라미터값 배열에 담기
    ParamArray = {
        'gubun': Gubun,
        'ownerID': OwnerID
    };

    var obj = eval("View.SucceededAddressInfo");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}


/**************************************************
작성자   : freeman
작성일자 : 2011-07-14
작성목적 : 주소록 가져오기 (웹서비스 값 받기)
***************************************************/
View.SucceededAddressInfo = function (data, status) {
    var schCont = eval('(' + data.d + ')');

    $("#userInfo").text(schCont.GetContent[0].Name + " " + schCont.GetContent[0].TitleName);

    $("#callMobile").html("<img src='/DeskPlusMobile/Content/ko/Base/Img/hp.png' onclick=\"javascript:View.CallTel('" + schCont.GetContent[0].MobilePhone + "')\" />");
    $("#scallMobile").html("<a href='#' onclick=\"javascript:View.CallTel('" + schCont.GetContent[0].MobilePhone + "')\">" + schCont.GetContent[0].MobilePhone + "</a>");

    $("#smsMobile").html("<img src='/DeskPlusMobile/Content/ko/Base/Img/talk.png' onclick=\"javascript:View.SMSTel('" + schCont.GetContent[0].MobilePhone + "')\" />");
    $("#ssmsMobile").html("<a href='#' onclick=\"javascript:View.SMSTel('" + schCont.GetContent[0].MobilePhone + "')\">" + schCont.GetContent[0].MobilePhone + "</a>");

    $("#callOffice").html("<img src='/DeskPlusMobile/Content/ko/Base/Img/office.png' onclick=\"javascript:View.CallTel('" + schCont.GetContent[0].OfficePhone + "')\" />");
    $("#scallOffice").html("<a href='#' onclick=\"javascript:View.CallTel('" + schCont.GetContent[0].OfficePhone + "')\">" + schCont.GetContent[0].OfficePhone + "</a>");

    $("#mail").html("<img src='/DeskPlusMobile/Content/ko/Base/Img/email.png' onclick=\"javascript:View.CallMail('" + schCont.GetContent[0].Mail + "')\" />");
    $("#smail").html("<a href='#' onclick=\"javascript:View.CallMail('" + schCont.GetContent[0].Mail + "')\">" + schCont.GetContent[0].Mail + "</a>");

    if (Gubun == "G" && MyTalkCheck == "Y") {
        $("#mytalk").html("<img src='/DeskPlusMobile/Content/ko/Base/Img/talk.png' onclick=\"javascript:View.Add(" + schCont.GetContent[0].UserID + ")\" />");
    }

    if (Gubun == "G") {
        $("#deptEtc").text(schCont.GetContent[0].DeptEtc);
    }

    $("#deptName").text(schCont.GetContent[0].DeptName);

    $("div.mppt img").attr("src", schCont.GetContent[0].PhotoPath);

    //LoadingShow(false);

}


/**************************************************
작성자   : freeman
작성일자 : 2011-07-14
작성목적 : 전화걸기
***************************************************/
View.CallTel = function (arg) {
    if (arg != "") {
        if (MobileType == "ios") {
            var url = "tel:" + arg;
            //window.webkit.messageHandlers.open_safari.postMessage(url);
            openSafari(url);
        }
        else {
            location.href = "tel:" + arg;
        }
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2016-06-02
작성목적 : 문자발송
***************************************************/
View.SMSTel = function (arg) {
    if (arg != "") {
        if (MobileType == "ios") {
            var url = "sms:" + arg;
            //window.webkit.messageHandlers.open_safari.postMessage(url);
            openSafari(url);
        }
        else {
            location.href = "sms:" + arg;
        }
    }
}


/**************************************************
작성자   : freeman
작성일자 : 2011-07-14
작성목적 : 메일 발송
***************************************************/
View.CallMail = function (arg) {
    if (arg != "") {
        if (MailCheck == "Y") {

            ParamArray = {
                'Mail': arg,
                'gubun': Gubun,
                'ownerID': OwnerID,
                'PageNo': PageNo,
                'sv': SearchValue.securityText()

            };

            CallEncrypt(CallGetParam());

            location.href = "/DeskPlusMobile/Mail/Send.aspx?p=" + GetEncryptValue;
        }
        else {
            location.href = "mailto:" + arg;
        }
    }
}


/**************************************************
작성자   : freeman
작성일자 : 2011-07-14
작성목적 : 문자 보내기
***************************************************/
View.CallSMS = function (arg) {
    if (arg != "") {
        location.href = "sms:" + arg;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2011-07-28
작성목적 : 목록 페이지 이동
***************************************************/
View.goBefore = function () {

    ParamArray = {
        'gubun': Gubun,
        'PageNo': PageNo,
        'FastPath': FastPath,
        'FirstChar': FirstChar,
        'sv': SearchValue.securityText()
    };

    CallEncrypt(CallGetParam());

    location.href = "List.aspx?p=" + GetEncryptValue;

}

/**************************************************
작성자   : famo23
작성일자 : 2012-11-06
작성목적 : 마이톡 등록
***************************************************/
View.Add = function (_rUserID) {

    ParamArray = {
        'gubun': Gubun,
        'ownerID': OwnerID,
        'PageNo': PageNo,
        'FastPath': FastPath,
        'FirstChar': FirstChar,
        'sv': SearchValue.securityText(),
        'RUserID': _rUserID
    };

    CallEncrypt(CallGetParam());

    location.href = "Add.aspx?p=" + GetEncryptValue;

}

