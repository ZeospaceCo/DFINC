var Title = {};


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
            Title.Confirm();
        });

        Title.setTitleColorValue(TitleColor);
        Title.setTitleImage(TitleImage);
    });
});


/**************************************************
작성자 :  famo23
작성일자 : 2012-05-29
작성목적 : 타이틀 색상변경
***************************************************/
Title.setTitleColorValue = function (_arg) {
    $("#titleTextColorSmaple").css("background", _arg);
    $("#titleTextColor").text(_arg);
    $("#myCommunityTitleColor1").css("color", _arg);
    $("#myCommunityTitleColor2").css("color", _arg);
    $("#myCommunityTitleColor3").css("color", _arg);
    $("#myCommunityTitleColor4").css("color", _arg);

    TitleColor = _arg;    
}

/**************************************************
작성자 :  famo23
작성일자 : 2012-05-29
작성목적 : 타이틀 이미지 변경
***************************************************/
Title.setTitleImage = function (_arg) {

    for (var i = 1; i < 23; i++) {
        $("#imgitem" + i).attr("class", "item" + i);
    }
    $("#img" + _arg).attr("class", _arg + " on");

    $("#myCommunityTitleBG").css("background", "url(/DeskPlusEIP/Design/C/Img/Community/" + _arg + ".jpg)");

    TitleImage = _arg;
}



/**************************************************
작성자 : famo23
작성일자 : 2012-05-29
작성목적 : 확인 (웹서비스 호출)
***************************************************/
Title.Confirm = function () {

    var url = "/DeskPlusEIP/Community/Ws/Site/Admin/Function/Skin/Title.asmx/ModStyle";
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
    var obj = eval("Title.SuccessModTitle");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-05-29
작성목적 : 확인 (웹서비스 값 받기)
**********************************************************************************************/
Title.SuccessModTitle = function (data, status) {

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
작성목적 : 메뉴화면으로 이동
**********************************************************************************************/
Title.goUrl = function () {

    ParamArray = {
        'communityID': CommunityID
    };

    CallEncrypt(CallGetParam());
    document.location.href = "Menu.aspx?p=" + GetEncryptValue;

}

