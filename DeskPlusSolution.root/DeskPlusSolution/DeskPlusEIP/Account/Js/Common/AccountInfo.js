AccountInfo = {}

$(function () {

    $(document).ready(function () {
        // 페이저 초기화
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        $('#CbtnDetail').click(function (e) {
            e.preventDefault();

            AccountInfo.OpenDetail();
        });


    });
});

AccountInfo.OpenDetail = function () {
    var opt = "directories = no, status = no, scrollbars = auto, resizable = yes, menubar = no"
    var url = "http://intra.di.co.kr/Disource/empdetail_qry.asp?EmpNo=" + UserEmpNO + "&uid=" + EmployeeNumber;
    var param = "";

    WindowObjectReference = PopupWinOpen(url, param, "OpenDetail", "1000", "730", opt);

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