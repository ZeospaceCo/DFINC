InGoogleStep1 = {}

$(function () {
    $(document).ready(function () {
        $('#register').click(function (e) {
            e.preventDefault();
            InGoogleStep1.GetGoogleContact();
        });

    });
});



/**************************************************
작성자   : 심재운
작성목적 : google 주소록 전체 가져오기
***************************************************/
InGoogleStep1.GetGoogleContact = function () {

    var validator = new Validator($("#form1"));

    validator.checkRequired("Id", R_InputGoogleMail, true);
    validator.checkRequired("Pwd", R_InputGooglePW, true);

    if (!validator.validate()) return;

    var url = "/DeskPlusEIP/Address/Ws/GoogleContactService.asmx/AddGoogleContact";

    var id = $("#Id").val();
    var pwd = $("#Pwd").val();

    //파라미터값 배열에 담기
    ParamArray = {
        'username': id,
        'userpwd': pwd,
        'accessToken': AccessToken
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("InGoogleStep1.SuccessGetGoogleContact");

    AjaxService(url, data, obj);    
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , google 주소록 전체 가져오기
**********************************************************************************************/
InGoogleStep1.SuccessGetGoogleContact = function (data, status) {
    if (data.d == true) {
        location = "InGoogleStep2.aspx";
    }
    else {
        alert(R_AfterProcessingError);
    }
}