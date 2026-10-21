
GetSkinMail = {}

$(function () {
    $(document).ready(function () {

        //확인
        $('#CbtnConfirm').click(function (e) {
            e.preventDefault();
            GetSkinMail.Confirm();
        });

        //닫기
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        //메일설정
        $('#cbtnGW').click(function (e) {
            e.preventDefault();

            ParamArray = { 'tabID': TabID };
            CallEncrypt(CallGetParam());

            var url = "GetSkin.aspx?p=" + GetEncryptValue;
            location = url;
        });

    });
});

/**************************************************
작성자:
작성일:2012-07-14
작성목적 : 
***************************************************/
GetSkinMail.SetSkin = function (color) { 
    SkinColor = color;
    $("#ColorSample").attr("src", ImagePath + "/Package/mailskin_" + color + ".gif");
    //alert($("#ColorSample").attr("src"))
}


/**************************************************
작성자: shine
작성일:2011-11-14
작성목적 : 스킨컬러 저장
***************************************************/
GetSkinMail.Confirm = function () {

    var url = "/DeskPlusEIP/EIP/Ws/Portal/GetSkinMail.asmx/ModSkin";
    ParamArray = { 'skinColor': SkinColor };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("GetSkinMail.Success");

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-18
작성목적 : 저장 성공 메시지
**********************************************************************************************/
GetSkinMail.Success = function (data, status) {
    alert(R_ConfirmApply);
    opener.parent.location.reload();
    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
    return;
}