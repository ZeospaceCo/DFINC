var Menu = {};


/**************************************************
작성자 : famo23
작성일자 : 2012-05-29
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //확인
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            Menu.Confirm();
        });

        $("#MenuTotal").css("background", MenuColor1);
        $("#TopMenu").css("background", MenuColor2);
        $("#BotMenu").css("background", MenuColor3);

        $(".tit").css("color", MenuBlock1TextClass);
        $(".tit1").css("color", MenuBlock2TextClass);

    });
});


/**************************************************
작성자 :  famo23
작성일자 : 2012-05-29
작성목적 : 메뉴 색상변경
***************************************************/
Menu.setTitleColorValue = function (_arg) {
    var menuIndex = $('input[name="rbMenu"]:checked').val();

    if (menuIndex == "1") {
        $("#MenuTotal").css("background", _arg);
        MenuColor1 = _arg;
    }
    else if (menuIndex == "2") {
        $("#TopMenu").css("background", _arg);
        MenuColor2 = _arg;
    }
    else if (menuIndex == "3") {
        $("#BotMenu").css("background", _arg);
        MenuColor3 = _arg;
    }

}

/**************************************************
작성자 :  famo23
작성일자 : 2012-05-29
작성목적 : 메뉴 글자색 반전 변경
***************************************************/
Menu.setTextClass = function (_arg) {

    var tmpColor1 = "#000000";
    var tmpColor2 = "#000000";

    if ($("#cbBlock1").is(":checked")) {
        tmpColor1 = "#FFFFFF";
    }

    if ($("#cbBlock2").is(":checked")) {
        tmpColor2 = "#FFFFFF";
    }

    if (_arg == "1") {
        $(".tit").css("color", tmpColor1);
        MenuBlock1TextClass = tmpColor1;
    }
    else if (_arg == "2") {
        $(".tit1").css("color", tmpColor2);
        MenuBlock2TextClass = tmpColor2;
    }
}


/**************************************************
작성자 : famo23
작성일자 : 2012-05-29
작성목적 : 확인 (웹서비스 호출)
***************************************************/
Menu.Confirm = function () {

    var url = "/DeskPlusEIP/Community/Ws/Site/Admin/Function/Skin/Menu.asmx/ModStyle";
    ParamArray = {
        'communityID': CommunityID,
        'titleColor': TitleColor,
        'titleImage': TitleImage,
        'menuColor1' : MenuColor1,
        'menuColor2' : MenuColor2,
        'menuColor3' : MenuColor3,
        'menuBlock1TextClass' : MenuBlock1TextClass,
        'menuBlock2TextClass' : MenuBlock2TextClass
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Menu.SuccessModMenu");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-05-29
작성목적 : 확인 (웹서비스 값 받기)
**********************************************************************************************/
Menu.SuccessModMenu = function (data, status) {

    if (data.d) {
        alert(R_CompleteConfirm);
        return;
    }
    else {
        alert(R_ProcessingError);
        return;
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-05-29
작성목적 : 타이틀 화면으로 이동
**********************************************************************************************/
Menu.goUrl = function () {

    ParamArray = {
        'communityID': CommunityID
    };

    CallEncrypt(CallGetParam());
    document.location.href = "Title.aspx?p=" + GetEncryptValue;

}
