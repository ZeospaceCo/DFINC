AccountInfo = {}

$(function () {

    $(document).ready(function () {
        //닫기        
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        //저장
        $('#cbtnSave').click(function (e) {
            e.preventDefault();
            AccountInfo.Confirm();
        });

        $("#ctbJoinDate").datepick($.datepicker.regional[PreLang]);
        $("#ctbRetireDate").datepick($.datepicker.regional[PreLang]);

        $("#ctbJoinDate").keydown(function (e) { return false; });
        $("#ctbRetireDate").keydown(function (e) { return false; });

    });
});

/**************************************************
작성자   : kkzz52
작성일자 : 2019-02-18
작성목적 : 입사일, 퇴사일 수정
***************************************************/
AccountInfo.Confirm = function () {
    if (jQuery.trim($("#ctbJoinDate").val()) == "") {
        alert(R_Admin_User_Input_JoinDate);
        return;
    }

    if (ActiveUser != "1") {
        if (jQuery.trim($("#ctbRetireDate").val()) == "") {
            alert(R_Admin_User_Input_RetireDate);
            return;
        }
    }

    var url = "/DeskPlusEIP/HR/Ws/Content/Popup/AccountInfo.asmx/Mod";

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'userID': UserID,
        'joinDate': $("#ctbJoinDate").val(),
        'retireDate': $("#ctbRetireDate").val()
    };

    var obj = eval("AccountInfo.SuccessConfirm");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2019-02-18
작성목적 : 입사일, 퇴사일 수정 성공 시
**********************************************************************************************/
AccountInfo.SuccessConfirm = function (data, status) {
    if (data.d == "1") {
        alert(R_MSG_SUCCESS);
        if (parent.disablePopup) {
            parent.disablePopup();
        } else { self.close(); }
    }
    else {
        alert(R_ProcessingError);
    }
}

AccountInfo.OpenSNS = function (type, id, pwd) {
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

/**********************************************************************************************
작성자   : shine  
작성일자 : 2011-11-24
작성목적 : 사람에게 댓글쓰기 팝업
**********************************************************************************************/
AccountInfo.PopupShortReplyList = function (p_userID) {

    ParamArray = {
        'rUserID': p_userID
    };

    CallEncrypt(CallGetParam());

    //var url = "/DeskPlusEIP/Pim/ShortReply/ShortReplyList.aspx";
    var url = "/DeskPlusEIP/SNS/Common/MyTalkGroup.aspx";
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "ShortReplyList", "700", "520");
    //WindowObjectReference = PopupWinOpen(url, param, "ShortReplyList", "800", "500");
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 편지쓰기
***************************************************/
AccountInfo.SendMail = function () {

    var objPopupMail = window.open("", "WriteMail", "width=1100px,height=700px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");

    document.getElementById("frmMail").action = URL_GateUrl;
    document.getElementById("frmMail").submit();
}