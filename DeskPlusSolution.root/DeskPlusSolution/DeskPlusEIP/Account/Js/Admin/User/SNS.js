SNS = {}



/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //확인
        $('#CbtnConfirm').click(function (e) {
            e.preventDefault();
            SNS.Confirm();
        });

        //닫기
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        if (UserID == "0") {
            $("#modPwdComment").hide();
        }
        else {
            $("#modPwdComment").show();
        }
    });
});


var SNSArray = []

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 처리하기
***************************************************/
SNS.Confirm = function () {    

    SNSArray = {
        'facebook': $("#ctbFacebook").val(),
        'facebookPwd': $("#ctbFacebookPwd").val(),
        'twitter': $("#ctbTwitter").val(),
        'twitterPwd': $("#ctbTwitterPwd").val(),
        'me2day': $("#ctbMe2day").val(),
        'me2dayPwd': $("#ctbMe2dayPwd").val(),
        'google': $("#ctbGoogle").val(),
        'googlePwd': $("#ctbGooglePwd").val(),
        'NaverBlog': $("#ctbNaverBlog").val(),
        'NaverBlogPwd': $("#ctbNaverBlogPwd").val()
    };

    parent.SetSNS(SNSArray);

    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}