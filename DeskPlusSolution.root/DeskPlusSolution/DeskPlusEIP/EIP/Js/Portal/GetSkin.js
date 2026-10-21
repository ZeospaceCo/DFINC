
GetSkin = {}

$(function () {
    $(document).ready(function () {

        //확인
        $('#CbtnConfirm').click(function (e) {
            e.preventDefault();
            GetSkin.Confirm();
        });

        //닫기
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        //메일설정
        $('#cbtnMail').click(function (e) {
            e.preventDefault();

            ParamArray = { 'tabID': TabID };
            CallEncrypt(CallGetParam());

            var url = "GetSkinMail.aspx?p=" + GetEncryptValue;
            location = url;
        });
        
        // Deep메일만 보임
        if (MailProduct.indexOf("DEEP") < 0) {
            $(".clsmail").attr("style", "display:none");
        }
    });
});

/**************************************************
작성자:
작성일:2012-07-14
작성목적 : 
***************************************************/
GetSkin.SetSkin = function (color) {
    SkinColor = color;
    $("#ColorSample").attr("src", ImagePath + "/Package/colorsample" + color + ".bmp");
}


/**************************************************
작성자: shine
작성일:2011-11-14
작성목적 : 스킨컬러 저장
***************************************************/
GetSkin.Confirm = function () {

    var url = "/DeskPlusEIP/EIP/Ws/Portal/GetSkin.asmx/ModSkin";
    ParamArray = { 'skinColor': SkinColor };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("GetSkin.Success");

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : shine
작성일자 : 2011-04-18
작성목적 : 저장 성공 메시지
**********************************************************************************************/
GetSkin.Success = function (data, status) {
    alert(R_ConfirmApply);
    opener.parent.location.reload();
    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
    return;
}