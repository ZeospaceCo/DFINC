Default = {}

$(function () {
    $(document).ready(function () {
        var csrf_token = $('meta[name="csrf-token"]').attr('content');

        function csrfSafeMethod(method) {
            // these HTTP methods do not require CSRF protection
            return (/^(GET|HEAD|OPTIONS)$/.test(method));
        }

        $.ajaxSetup({
            beforeSend: function (xhr, settings) {
                if (!csrfSafeMethod(settings.type) && !this.crossDomain) {
                    xhr.setRequestHeader("anti-csrf-token", csrf_token);
                }
            }
        });

        $("#Submit1").click(function (e) {
            e.preventDefault();
            Default.LoginProcess();
        });
        //엔터 이벤트 기술
        $("#tbLoginID, #tbPassword").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                Default.LoginProcess();
            }
        });

        $("#searchPassword").click(function (e) {
            Default.SearchPassword();
        });

        $("#Otp").click(function (e) {
            e.preventDefault();
            Default.SetOTP();            
        });

        /*
        if (top.opener) {
        try {
        top.opener.location = "/DeskPlusEIP/Site/Auth/Default.aspx";
        self.close();
        }
        catch (Error) {
        }
        }
        */

        $('#ddlCompany').change(function () {
            Default.GetLogoImage();
        });

        /*
        if (!!parent.frmTop == true || !!parent.frmContent == true || !!parent.parent.frmTop == true || !!parent.parent.frmContent == true) {
        top.location = "/DeskPlusEIP/Site/Auth/Default.aspx";
        return;
        }
        */

        Default.GetLogoImage();

        if ($("#tbLoginID").val() == "") {
            $("#tbPassword").attr("placeholder", R_InputPwd);
            $("#tbLoginID").css('ime-mode', 'inactive').focus();
        }
        else {
            $("#tbPassword").focus();
        }

        $('input, textarea').placeholder();

    });
});

/**************************************************
작성자   : freeman
작성일자 : 2013-03-20
작성목적 : 로고 이미지 변경
***************************************************/
Default.GetLogoImage = function () {

    var companyID = $('#ddlCompany option:selected').val();
    var url = "/DeskPlusEIP/Ws/Site/Auth/Default.asmx/GetLogoImage";
    var data = "{companyID : '" + companyID + "'}";
    var success = eval("Default.SuccessGetLogoImage");
    AjaxService(url, data, success);
}

/**************************************************
작성자   : freeman
작성일자 : 2013-03-20
작성목적 : 로고 이미지 변경
***************************************************/
Default.SuccessGetLogoImage = function (data, status) {
    var response = data.d;

    var arrVal = response.split('^');
    $("#loginImg").attr("src", arrVal[0]);
    $("#VloginImg").attr("src", arrVal[1]);


    $(".login").css({
        'background': 'url(' + arrVal[2] + ') no-repeat center center fixed;',
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
        'background-color': '#000'
    })

}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 체크
***************************************************/
Default.LoginProcess = function () {

    var validator = new Validator($("#form1"));
    validator.checkRequired("tbLoginID", R_CheckIDandPwd, true);
    validator.checkRequired("tbPassword", R_CheckIDandPwd, true);

    if (!validator.validate()) return false;

    Default.PasswordError();    
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 날짜 체크
***************************************************/
Default.CheckDate = function () {
    var companyID = $("#ddlCompany").val();
    var loginID = $("#tbLoginID").val();
    var password = $("#tbPassword").val();

    var url = "/DeskPlusEIP/Ws/Site/Auth/Default.asmx/CheckPasswordDate";
    var data = "{companyID : '" + companyID + "',loginID : '" + loginID + "',password : '" + password + "'}";
    var success = eval("Default.SuccessCheckDate");
    AjaxService(url, data, success);
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 패스워드 날짜 문제 없을 경우, 인증 처리
***************************************************/
Default.SuccessCheckDate = function (data, status) {
    var response = data.d;

    var rtnV = response.split("^")[0];
    OTPYN = response.split("^")[1];

    if (rtnV == "1") {
        if (OTPYN == "N") {
            __doPostBack("lbtnSubmit", "");
            return true; ;
        }
        else {
            Default.LoadOTP();
        }
    }
    else if (rtnV == "0") {
        Default.ModPassword(); //패스워드 변경 Alert 테스트
        return false;
    }
    else if (rtnV == "-1") {
        //1. 인증된 사용자가 아님
        alert(R_InvalidUserIDPassword);
        return false;
    }
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 패스워드 틀릴 경우 카운트
***************************************************/
Default.PasswordError = function () {
    var companyID = $("#ddlCompany").val();
    var loginID = $("#tbLoginID").val();
    var password = $("#tbPassword").val();

    var url = "/DeskPlusEIP/Ws/Site/Auth/Default.asmx/PasswordError";
    var data = "{companyID : '" + companyID + "',loginID : '" + loginID + "',password : '" + password + "'}";
    var success = eval("Default.SuccessPasswordError");
    AjaxService(url, data, success);
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 패스워드 틀릴 경우, 성공시.
***************************************************/
Default.SuccessPasswordError = function (data, status) {
    var response = data.d;

    if (response == "0") {
        Default.CheckDate();
    }
    else if (response == "-2") {
        alert(R_CheckAuthDept);
    }
    else if (response == "-3") { //패스워드 횟수 틀린 후 패스워드가 기억나도 로그인안됨
        Default.SendPassword();
    }
    else {

        if (response == MaxCnt) {
            Default.SendPassword();
        }
        else {
            alert("(" + response + ") " + R_PasswordFaultCount + "\n" + R_PasswordFaultTotalCount);
        }
    }
}

/**************************************************
작성자   : 심재운
작성일자 :
작성목적 : 패스워드 변경 페이지 호출
***************************************************/
Default.ModPassword = function () {

    var cid = $("#ddlCompany").val();
    var id = $("#tbLoginID").val();

    ParamArray = {
        'companyID': cid,
        'loginID': id
    };

    CallEncryptLogin(CallGetParam());
    var url = "ModPassword.aspx?p=" + GetEncryptValue;
    PopupWin(550, 430, url, "50", "", true);
}

/*****************************
* METHOD : 암호화 메소드 호출
*****************************/
CallEncryptLogin = function (param) {
    param = escape(param);
    var url = "/DeskPlusEIP/Ws/Site/Auth/Default.asmx/AjaxGetEncrypt";
    var data = "{data :'" + param + "'}";
    var obj = eval("SuccessCallEncryptLogin");
    AjaxService(url, data, obj);
}

/*****************************
* METHOD : 암호화 값 가져오기
*****************************/
SuccessCallEncryptLogin = function (data, status) {
    GetEncryptValue = encodeURIComponent(data.d);
}


/*****************************
* METHOD : 인증메일(발송자,수신자2차) 체크후 발송
*****************************/
Default.SendPassword = function () {
    var cid = $("#ddlCompany").val();
    var id = $("#tbLoginID").val();

    var url = "/DeskPlusEIP/Ws/Site/Auth/Default.asmx/CheckEmail";
    var data = "{companyID : '" + cid + "',loginID : '" + id + "'}";
    var success = eval("Default.SuccessCheckEmail");
    AjaxService(url, data, success);

}

Default.SuccessCheckEmail = function (data, status) {
    if (data.d == "1") {
        var cid = $("#ddlCompany").val();
        var id = $("#tbLoginID").val();

        ParamArray = {
            'companyID': cid,
            'loginID': id
        };

        CallEncryptLogin(CallGetParam());
        var url = "SendPassword.aspx?p=" + GetEncryptValue;
        PopupWin(550, 350, url, "50", "", true);
    }
    else if (data.d == "-1") {
        alert(R_PasswordOver);
        return;
    }
    else if (data.d == "-2") {
        alert(R_NoAuthEmail);
        return;
    }

}

/*****************************
* METHOD : 패스워드 찾기
*****************************/
Default.SearchPassword = function () {
    var cid = $("#ddlCompany").val();
    var gubun = "search";

    ParamArray = {
        'companyID': cid,
        'gubun': gubun
    };

    CallEncryptLogin(CallGetParam());
    var url = "SendPassword.aspx?p=" + GetEncryptValue;
    PopupWin(550, 350, url, "50", "", true);
};


/**************************************************************************************************************/
Default.SetOTP = function () {

    var validator = new Validator($("#form1"));
    validator.checkRequired("tbLoginID", "로그인아이디를 입력해주세요", true);
    validator.checkRequired("tbPassword", "비밀번호를 입력해주세요", true);

    if (!validator.validate()) return false;

    var cid = $("#ddlCompany").val();
    var id = $("#tbLoginID").val();
    var pwd = $("#tbPassword").val();

    var url = "/DeskPlusEIP/Ws/Site/Auth/Default.asmx/GetOTP";
    var data = "{companyID : '" + cid + "',loginID : '" + id + "',password : '" + pwd + "'}";
    var success = eval("Default.SuccessSetOTP");
    AjaxService(url, data, success);
}

/**************************************************************************************************************/
Default.SuccessSetOTP = function (data, status) {

    if (data.d > 0) {

        var cid = $("#ddlCompany").val();
        var id = $("#tbLoginID").val();

        ParamArray = {
            'cID': cid,
            'lID': id
        };
        CallEncryptLogin(CallGetParam());
        var url = "/DeskPlusOTP/OTP.aspx";

        PopupWinOpen(url, "p=" + GetEncryptValue, "OTP", 550, 400);

    }
    else if (data.d == -1) {
        alert("해당 사용자가 없습니다. \n로그인아이디 & 비밀번호를 확인 부탁드립니다");
        return;
    }
    else if (data.d == -2) {
        alert("해당 사용자의 부서가 없습니다. \n관리자에게 문의 부탁드립니다.");
        return;
    }
    else if (data.d == -3) {
        alert("OTP가 이미 등록되어 있습니다");
        return;
    }
}

/**************************************************************************************************************/
Default.LoadOTP = function () {

    var cid = $("#ddlCompany").val();
    var id = $("#tbLoginID").val();

    ParamArray = {
        'cID': cid,
        'lID': id
    };
    CallEncryptLogin(CallGetParam());
    var url = "/DeskPlusOTP/PopOTP.aspx";

    PopupWinOpen(url, "p=" + GetEncryptValue, "PopOTP", 550, 300);
}


function RetunOTP(_rtn) {

    if (_rtn == "1") {
        __doPostBack("lbtnSubmit", "");
        return true; ;
    }
    else if (_rtn == "2") {
        alert("구글 OTP 인증이 되지 않았습니다");
        return;
    }
    else if (_rtn == "3") {
        alert("구글 OTP 등록 부탁드립니다.");
        return;
    }    
}