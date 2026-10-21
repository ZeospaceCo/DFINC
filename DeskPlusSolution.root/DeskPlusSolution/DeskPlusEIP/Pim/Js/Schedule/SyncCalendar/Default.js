Default = {};

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //취소
        $('#CbtnClose').click(function (e) {
            e.preventDefault();

            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        //구글로그인
        $('#cbtnSignIn').click(function (e) {
            e.preventDefault();

            Default.SignIn();
        });

        //구글로그아웃
        $('#cbtnSignOut').click(function (e) {
            e.preventDefault();

            Default.SignOut();
        });

        //나의 정보 
        $('#PopupLink').click(function (e) {
            e.preventDefault();
            Default.CallSNS();
        });

        //가져오기
        $('#cbtnGetCalendar').click(function (e) {
            e.preventDefault();

            if ($('#ddlSearchFilter').val() == "U") {
                if ($('#ctbDate').val().length == 0) {
                    alert(Inquery_Input); //Inquery_Input
                    return;
                }
            }
            Default.SyncGoogle();
        });

        //기간 //
        $('#ddlSearchFilter').change(function (e) {
            e.preventDefault();
            Default.DateFilter();
            //ScheduleMain.SearchComboSelect($('#ddlSearchFilter').val());
        });

        $("#ctbDate").datepick($.datepicker.regional[PreLang]);

    });
});

/**************************************************
작성자   : 
작성일자 : 2011-04-11
작성목적 : SNS 호출
***************************************************/
Default.CallSNS = function () {
    var url = "/DeskPlusEIP/Account/MyInfo/Mod.aspx";
    var WindowObjectReference = PopupWinOpen(url, "", "MYINFO", "1000", "618");
}

/**************************************************
작성자   : freeman
작성일자 : 2015-06-11
작성목적 : 구글 로그인
***************************************************/
Default.SignIn = function () {

    if ($("#ctbGoogle").val().length == 0) {
        alert(GoogleInfo_Input);
        $("#ctbGoogle").focus();
        return;
    }
    __doPostBack('lbtnSignIn', '');
}


/**************************************************
작성자   : freeman
작성일자 : 2015-06-11
작성목적 : 구글 로그아웃
***************************************************/
Default.SignOut = function () {
    __doPostBack('lbtnSignOut', '');
}


/**************************************************
작성자   :   
작성일자 : 
작성목적 : 구글 연동
***************************************************/
Default.SyncGoogle = function () {

    __doPostBack('lbtnGetCalendar', '');
}

/**************************************************
작성자   :   
작성일자 :  
작성목적 : 검색기간 설정 
***************************************************/

Default.DateFilter = function () {
    var selVal = $('#ddlSearchFilter').val();

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