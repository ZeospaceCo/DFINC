var GetMemberInfo = {};


/**************************************************
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //닫기
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        //수정
        $('#cbtnMod').click(function (e) {
            e.preventDefault();
            GetMemberInfo.Mod();
        });

        //탈퇴
        $('#cbtnDrop').click(function (e) {
            e.preventDefault();
            GetMemberInfo.Drop();
        });

    });
});


/**************************************************
작성자 : famo23
작성일자 : 2012-06-12
작성목적 : 자기 소개 수정 (웹서비스 값 호출) 
***************************************************/
GetMemberInfo.Mod = function () {

    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbMemo", R_InputDes, true);
    if (!validator.validate()) return;

    var myDescription = $("#ctbMemo").val();   // 자기소개

    ParamArray = {
        'communityID': CommunityID,
        'description': myDescription
    };

    CallEncrypt(CallGetParam())

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("GetMemberInfo.SuccessMod");
    var url = "/DeskPlusEIP/Community/Ws/Site/Main/Search/Common/GetMemberInfo.asmx/UserMod";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-12
작성목적 : 자기 소개 수정 (웹서비스 값 받기)
**********************************************************************************************/
GetMemberInfo.SuccessMod = function (data, status) {

    if (data.d) {
        alert(R_CompleteConfirm);
        return;
    }
    else {
        alert(R_ProcessingError);
        return;
    }
}

/**************************************************
작성자 : famo23
작성일자 : 2012-06-12
작성목적 : 회원 탈퇴 (웹서비스 값 호출) 
***************************************************/
GetMemberInfo.Drop = function () {

    if (window.confirm(R_ConfirmDrop)) {
        ParamArray = {
            'communityID': CommunityID,
            'status': '0'
        };

        CallEncrypt(CallGetParam())

        var data = "{p:'" + GetEncryptValue + "'}";
        var obj = eval("GetMemberInfo.SuccessDrop");
        var url = "/DeskPlusEIP/Community/Ws/Site/Main/Search/Common/GetMemberInfo.asmx/UserDrop";

        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-06-12
작성목적 : 회원 탈퇴 (웹서비스 값 받기)
**********************************************************************************************/
GetMemberInfo.SuccessDrop = function (data, status) {

    if (data.d) {
        alert(R_CompleteConfirm);
        parent.parent.location.href = "/DeskPlusEIP/Community/Default.aspx";
        return;
    }
    else {
        alert(R_ProcessingError);
        return;
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-07-13
작성목적 : SNS 처리
**********************************************************************************************/
GetMemberInfo.OpenSNS = function (type, id, pwd) {
    var opt = "menubar=yes,location=yes,resizable=yes,scrollbars=yes,status=yes"
    switch (type.toLowerCase()) {
        case "facebook":
            var url = "http://www.facebook.com/" + id;
            var param = "";
            PopupWinOpen(url, param, 'facebook', 1024, 768, opt);
            break;
        case "twitter":
            var url = "http://twitter.com/" + id;
            var param = "";
            PopupWinOpen(url, param, 'facebook', 1024, 768, opt);
            break;
        case "me2day":
            var url = "http://me2day.net/" + id;
            var param = "";
            PopupWinOpen(url, param, 'facebook', 1024, 768, opt);
            break;
        case "naverblog":
            var url = "http://blog.naver.com/" + id;
            var param = "";
            PopupWinOpen(url, param, 'naverBlog', 1024, 768, opt);
            break;
    }
}