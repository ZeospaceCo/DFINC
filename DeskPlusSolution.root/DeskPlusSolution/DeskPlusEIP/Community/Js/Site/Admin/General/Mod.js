var Mod = {};


/**************************************************
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //확인
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            Mod.Confirm();
        });

        //닫기
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        $('#cbtnMember').click(function (e) {
            e.preventDefault();
            Mod.GetMember();
        });

        $('#ctbUserInfo').attr("disabled", true);

        Mod.LoadData();
    });
});

/**************************************************
작성자 : shine
작성일자 : 2012-05-30
작성목적 :  
***************************************************/
Mod.LoadData = function () {
    var url = "/DeskPlusEIP/Community/Ws/Site/Admin/General/Mod.asmx/GetList";

    ParamArray = {
        'communityID': CommunityID
    };

    CallEncrypt(CallGetParam());
    var obj = eval("Mod.SuccessGetList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**************************************************
작성자 : shine
작성일자 : 2012-05-30
작성목적 : 리스트 가져오기 (웹서비스 값 받기)
***************************************************/
Mod.SuccessGetList = function (data, state) {
    //$("#lbManager").text(data.d[0].Dept + " " + data.d[0].Title + " " + data.d[0].UserName);
    if (data.d[0]) {
        $("#lbManager").text(data.d[0].Dept + " " + data.d[0].Title + " " + data.d[0].UserName);
    }
}

/**************************************************
작성자 : shine
작성일자 : 2012-05-30
작성목적 : 확인 
***************************************************/
Mod.Confirm = function () {
    var url = "/DeskPlusEIP/Community/Ws/Site/Admin/General/Mod.asmx/ModGrade";

    if (UserID == "") {
        alert(R_CheckManager);
        return;
    }

    if (window.confirm(R_ConfirmChange)) {
        ParamArray = {
            'communityID': CommunityID,
            'userID': UserID
        };

        CallEncrypt(CallGetParam());
        var obj = eval("Mod.SuccessMod");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }
}

/**************************************************
작성자 : shine
작성일자 : 2012-05-30
작성목적 : 확인 
***************************************************/
Mod.SuccessMod = function (data, state) {
    if (data.d == true) {
        alert(R_ConfirmManager);

        if (AdminCheck == "Y") {
            UserID = 0;
            $('#ctbUserInfo').attr("disabled", false);
            $('#ctbUserInfo').val("");
            $('#ctbUserInfo').attr("disabled", true);
            Mod.LoadData();            
        }
        else {            
            parent.location.href = "/DeskPlusEIP/Community/Default.aspx";
        }
    }
    else {
        alert(R_ProcessingError);
    }
}

/**************************************************
작성자 : shine
작성일자 : 2012-05-30
작성목적 : 확인 
***************************************************/
Mod.GetMember = function () {
    ParamArray = {
        'communityID': CommunityID
    };

    CallEncrypt(CallGetParam());

    var url = "./Common/GetMember.aspx?p=" + GetEncryptValue;
    PopupWin(500, 400, url, "50", "", true);
}

/**************************************************
작성자 : shine
작성일자 : 2012-05-30
작성목적 : 변경할 시샵인 정보 바인딩 
***************************************************/
Mod.UserSetting = function (info) {
    UserID = info.split("┐")[0];
    var userInfo = info.split("┐")[1];

    $("#ctbUserInfo").val(userInfo);
}