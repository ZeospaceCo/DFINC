LinkPim = {};

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //확인
        $('#CbtnConfirm').click(function (e) {
            e.preventDefault();
            LinkPim.Confirm();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        //취소
        $('#CbtnClose').click(function (e) {
            e.preventDefault();

            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        //가져오기
        $('#cbtnGetGoogle').click(function (e) {
            e.preventDefault();

            if ($("#lblGoogle").text().length == 0) {
                alert(GoogleInfo_Input); //GoogleInfo_Input
                return;
            }
            if ($('#ddlSearchKind').val() == "U") {
                if ($('#startDate').val().length == 0) {
                    alert(Inquery_Input); //Inquery_Input
                    return;
                }
            }
            LinkPim.SyncGoogle();
        });

        $('#PopupLink').click(function (e) {
            e.preventDefault();
            LinkPim.CallSNS();            
        });


        //기간 //
        $('#ddlSearchKind').change(function (e) {
            e.preventDefault();
            LinkPim.DateSearch();
            //ScheduleMain.SearchComboSelect($('#ddlSearchKind').val());
        });

        $('#tbLicenseKey').focus();
        $("#startDate").datepick($.datepicker.regional[PreLang]);

    });
});

/**************************************************
작성자   : 
작성일자 : 2011-04-11
작성목적 : SNS 호출
***************************************************/
LinkPim.CallSNS = function () {
/*
    ParamArray = {
        'userID': UserID,
        'facebook': $("#tbFacebook").val(),
        'facebookPwd': $("#tbFacebookPwd").val(),
        'twitter': $("#tbTwitter").val(),
        'twitterPwd': $("#tbTwitterPwd").val(),
        'me2day': $("#tbMe2day").val(),
        'me2dayPwd': $("#tbMe2dayPwd").val(),
        'me2day': $("#tbMe2day").val(),
        'me2dayPwd': $("#tbMe2dayPwd").val(),
        'google': $("#tbGoogle").val(),
        'googlePwd': $("#tbGooglePwd").val(),
        'NaverBlog': $("#tbNaverBlog").val(),
        'NaverBlogPwd': $("#tbNaverBlogPwd").val()
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Account/MyInfo/SNS.aspx?p=" + GetEncryptValue;
*/
    var url = "/DeskPlusEIP/Account/MyInfo/Mod.aspx";
    var WindowObjectReference = PopupWinOpen(url, "", "MYINFO", "1000", "618");
}


/**************************************************
작성자   : 
작성일자 :
작성목적 :
***************************************************/
LinkPim.Confirm = function () {
    
    if ($("#lblGoogle").text().length == 0) {
        alert(GoogleInfo_Input);
        return;
    }
    __doPostBack('lbtnMod', '');
    //$("#lblGoogle").text()

}



/**************************************************
작성자   : daniel  
작성일자 : 2012-02-15 
작성목적 : 검색기간 설정 
***************************************************/

LinkPim.DateSearch = function () {
    var selVal = $('#ddlSearchKind').val();

    if (selVal == "0") {
        $("#spanDate").hide();
    }
    if (selVal == "WEEK") {
        $("#spanDate").hide();
    }
    if (selVal == "MON") {
        $("#spanDate").hide();
    }
    if (selVal == "U") {
        $("#spanDate").show();
    }
}

/**************************************************
작성자   : daniel  
작성일자 : 2012-02-15 
작성목적 : 구글 연동
***************************************************/
LinkPim.SyncGoogle = function () {

    var url = "/DeskPlusEIP/Pim/Ws/Schedule/LinkPim.asmx/GetGoogleCalendarSingle";
   // alert($('#ddlSearchKind').val());
    ParamArray = {
        'Period': $('#ddlSearchKind').val(),
        'DATE': $('#startDate').val()
    }
    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("LinkPim.SuccessGoogle");
    AjaxService(url, data, obj);
}

LinkPim.SuccessGoogle = function (data, status) {

    
    if (data.d == "AccountErr") {
        alert(GoogleAccErr);
        return;
    }
    if (data.d == "False") {
        alert(GoogleAccErr);
        return;
    }
    if (data.d == "Success") {
        alert(SuccessSync);
        return;
    }
     
}