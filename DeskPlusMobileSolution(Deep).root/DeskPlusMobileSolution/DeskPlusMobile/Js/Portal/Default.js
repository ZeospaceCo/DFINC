
/**************************************************
작성자   : 이현수
작성일자 : 2010-06-14
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {
    $("#submit").click(function (e) {
        e.preventDefault();
        LoginProcess();
    });

    //검색입력필드 엔터 이벤트 기술
    $("#tbLoginID, #tbPassword").keydown(function (e) {
        if (e.keyCode == 13) {
            e.preventDefault();
            LoginProcess();
        }
    });

    // 회사 선택시 
    $('#ddlCompany').change(function () {
        ChangeCompany();
    });

    ChangeCompany();

    if (!!window.applicationCache) {
        //console.log('applicationCache 지원');

        window.applicationCache.addEventListener('updateready', function () {
            //console.log('이벤트 처리');
            //showMessage('리소스 다운로드중입니다.');

            window.applicationCache.swapCache();
        }, false);
    }

    /*
    $("#spAndApp").hide();
    $("#spIOSApp").hide();

    var mobileKeyWords = new Array('Android');
    for (var word in mobileKeyWords) {
        if (navigator.userAgent.match(mobileKeyWords[word]) != null) {
            $("#spAndApp").show();
            break;
        }
    }


    var mobileKeyWords = new Array('iPhone', 'iPad');
    for (var word in mobileKeyWords) {
        if (navigator.userAgent.match(mobileKeyWords[word]) != null) {
            $("#spIOSApp").show();
            break;
        }
    }
    */
});

/**************************************************
작성자   : 이현수
작성일자 : 2010-06-14
작성목적 : 로그인 프로세스
***************************************************/
LoginProcess = function () {

    var loginId = $("#tbLoginID").val();
    var password = $("#tbPassword").val();
    var companyId = $("#ddlCompany").val();

    if (loginId == "") {
        $("#txtMessage").html(R_InputLoginID);
        return;
    }

    if (password == "") {
        $("#txtMessage").html(R_InputPwd);
        return;
    }

    var remember = "0";
    var reCompany = "0";

    if ($("#chkRemember").is(":checked")) {
        remember = "1";
    }

    if ($("#chkCompany").is(":checked")) {
        reCompany = "1";
    }


    var url = "/DeskPlusMobile/Portal/Ws/GetDefault.asmx/GetLoginCheck";
    //var data = "{_loginID : '" + encodeURIComponent(loginId) + "',_password : '" + encodeURIComponent(password) + "',_remember : '" + remember + "',_reCompany : '" + reCompany + "',_companyId : '" + companyId + "',guid : '" + Guids() + "'}";
    //var data = "{_loginID : '" + loginId + "',_password : '" + password + "',_remember : '" + remember + "',_reCompany : '" + reCompany + "',_companyId : '" + companyId + "',guid : '" + Guids() + "'}";
    var data = "{_loginID : '" + loginId + "',_password : '" + password + "',_remember : '" + remember + "',_reCompany : '" + reCompany + "',_companyId : '" + companyId + "',_logType : '0',guid : '" + Guids() + "'}";
    var success = eval("SuccessCheckDate");
    AjaxService(url, data, success);
}

/**************************************************
작성자   : 이현수
작성일자 : 2010-06-14
작성목적 : UserID / Password Focus 처리
***************************************************/
function SuccessCheckDate(result, status) {
    if (result.d != "!NULL" && result != "undefined") {
        var rtnMsg = result.d;
        if (rtnMsg.indexOf("AUTHFAIL") == 0) {
            $("#txtMessage").html(R_AuthFail);
        }
        else if (rtnMsg.indexOf("ACCESSRIGHT") == 0) {
            $("#txtMessage").html(R_AccessRight);
        }
        else if (rtnMsg.indexOf("WAIT") == 0) {
            $("#txtMessage").html(R_AuthWait);
        }
        else {
            //if (console) console.log('로그인성공');
            location.href = "/DeskPlusMobile/Portal/Portal.aspx";
        }
    } else {
        $("#txtMessage").html(R_ComfirmAuth);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2013-03-15
작성목적 : 회사 로고 이미지 가져오기 (웹서비스 호출)
***************************************************/
ChangeCompany = function () {
    var companyID = $('#ddlCompany option:selected').val();

    var url = "/DeskPlusMobile/Portal/Ws/GetDefault.asmx/GetLogoImage";
    var data = "{_companyId : '" + companyID + "'}";
    var success = eval("SuccessGetLogoImage");
    AjaxService(url, data, success);
}

/**************************************************
작성자   : famo23
작성일자 : 2013-03-21
작성목적 : 회사 로고 이미지 가져오기 (웹서비스 값 받기)
***************************************************/
function SuccessGetLogoImage(result, status) {

    var response = result.d;

    var arrVal = response.split('^');

    $(".logo").find("img").attr("src", arrVal[0]);

    /*
    $(".login").css({
    'background': 'url(' + arrVal[1] + ') no-repeat center center fixed;',
    '-webkit-background-size': 'cover',
    '-moz-background-size': 'cover',
    '-o-background-size': 'cover',
    'background-size': 'cover',
    'display': '-ms-flexbox',
    'display': 'flex',
    '-ms-flex-align': 'center',
    'align-items': 'center',
    'padding-top': '40px',
    'padding-bottom': '40px',
    'background-color': '#f9f9f9'
    })
    */

}