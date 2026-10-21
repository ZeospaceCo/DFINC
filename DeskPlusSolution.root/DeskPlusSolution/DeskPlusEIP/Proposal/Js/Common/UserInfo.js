UserInfo = {}

$(function () {

    $(document).ready(function () {
        // 페이저 초기화
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });


    });
});


UserInfo.OpenSNS = function (type, id, pwd) {
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
UserInfo.PopupShortReplyList = function (p_userID) {

    ParamArray = {
        'rUserID': p_userID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Pim/ShortReply/ShortReplyList.aspx";
    var param = "p=" + GetEncryptValue;
    WindowObjectReference = PopupWinOpen(url, param, "ShortReplyList", "800", "500");
}