
/**************************************************
작성자   : famo23
작성일자 : 2019-02-26
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
});

/**************************************************
작성자   : famo23
작성일자 : 2019-02-26
작성목적 : 기기정보 가져오기 
***************************************************/
function get_value() {
    var keyData = "";
    var callbackData = "responseGetToken";

    var message = {
        "command": "get_token",
        "data": {
            "key": keyData,
            "callback": callbackData
        }
    }

    sendZeospaceMessage(message);
}


function responseGetToken(data) {

    // Token Response Callbac Funtion
    if (data) {

        var jsonData = JSON.parse(data);

        UUID = jsonData.deviceId; //아이폰 유니크 아이디
        RegID = jsonData.token; //서버에서 발급받은 토큰값

        //저장된 아이디가 있을 경우에 자동로그인체크
        if (SaveLID != "") {
            GetLoginCheck();
        }
    }

}

function set_value(phone, ios_uuid, token) {

    if ((token == "") && (parseInt(RegCnt) < 7)) {
        RegCnt = parseInt(RegCnt) + 1;
        setTimeout("get_value();", 1000);
        return;
    }

    if ((ios_uuid == "") && (parseInt(RegCnt) == 7)) {
        $("#txtMessage").html("기기의 고유값을 가져오는데 지연이 되고 있습니다.앱을 종료후에 다시 접속부탁드립니다.");
        return;
    }

    UUID = ios_uuid; //아이폰 유니크 아이디
    RegID = token; //서버에서 발급받은 토큰값

    //저장된 아이디가 있을 경우에 자동로그인체크
    if (SaveLID != "") {
        GetLoginCheck();
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2019-02-26
작성목적 : 자동로그인 체크 
***************************************************/
GetLoginCheck = function () {

    $("#tbLoginID").val(SaveLID);

    ParamArray = {
        'cID': $("#ddlCompany").val(),
        'lID': SaveLID,
        'uuID': UUID
    };

    CallEncrypt(CallGetParam());

    $.ajax({
        type: "POST",
        url: "/DeskPlusMobile/Portal/Ws/GetDefault.asmx/GetEnv",
        data: "{'p': '" + GetEncryptValue + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {

            if (msg.d != "!NULL") {
                var tmpVal = msg.d;
                if (tmpVal != "") {
                    var arrVal = tmpVal.split("@");

                    ApproveMobile = arrVal[0];
                    LoginType = arrVal[1];
                    AccessRight = arrVal[2];

                    if (AccessRight == "2") {
                        $("#txtMessage").html("관리자가 접속불가설정을 해 놓았습니다 ");
                        return;
                    }

                    if (ApproveMobile == "0") {
                        $("#txtMessage").html("접속승인대기중입니다")
                    }
                    else if (ApproveMobile == "1") {
                        SetLogin();
                    }
                    else if (ApproveMobile == "2") {
                        $("#txtMessage").html("관리자가 접속불가설정을 해 놓았습니다 ");
                    }
                    else if (ApproveMobile == "3") {
                        $("#txtMessage").html("관리자가 접속불가설정을 해 놓았습니다 ");
                    }
                    else {
                        $("#txtMessage").html("인터넷환경 확인후 재접속부탁드립니다");
                    }
                }
                else {
                    $("#txtMessage").empty();
                }

            }
            else {
                $("#txtMessage").html("인터넷환경 확인후 재접속부탁드립니다");
            }


        },
        error: function (msg) {
            $("#txtMessage").html("인터넷환경 확인후 접속부탁드립니다.");
        }
    });
}


/**************************************************
작성자   : famo23
작성일자 : 2019-02-26
작성목적 : 로그인 프로세스
***************************************************/
LoginProcess = function () {

    var companyId = $("#ddlCompany").val();
    var loginId = $("#tbLoginID").val();
    var password = $("#tbPassword").val();
    password = password.split("&").join("~zeo~");

    if (loginId == "") {
        $("#txtMessage").html("아이디를 입력해주세요");
        return;
    }

    if (password == "") {
        $("#txtMessage").html("비밀번호를 입력해주세요");
        return;
    }

    if (UUID == "") {
        $("#txtMessage").html("기기의 고유값을 가져오는데 지연이 되고 있습니다.앱을 종료후에 다시 접속부탁드립니다.");
        return;
    }

    ParamArray = {
        'loginID': loginId,
        'password': password,
        'companyID': companyId,
        'uuID': UUID,
        'mobileType': 'ios',
        'regID': RegID
    };

    CallEncrypt(CallGetParam());

    $.ajax({
        type: "POST",
        url: "/DeskPlusMobile/Portal/Ws/GetDefault.asmx/GetCheckConfirm",
        data: "{'p': '" + GetEncryptValue + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {

            if (msg.d == "WAIT") {
                $("#txtMessage").html("접속승인대기중입니다")
            }
            else if (msg.d == "OK") {
                ParamArray = {
                    'uID': loginId,
                    'uCID': companyId,
                    'uuID': UUID,
                    'regID': RegID
                };

                CallEncrypt(CallGetParam());

                location.href = "/DeskPlusMobile/Portal/Ws/GetDefault.asmx/GetLoginOutSideApp?p=" + GetEncryptValue;
            }
            else if (msg.d == "ACCESSRIGHT") {
                $("#txtMessage").html("권한이 없습니다 관리자에게 문의 하세요 ");
            }
            else if (msg.d == "NOAUTH") {
                $("#txtMessage").html("관리자가 접속불가설정을 해 놓았습니다 ");
            }
            else {
                $("#txtMessage").html("아이디&비밀번호를 확인해 주세요 ");
            }

        },
        error: function (msg) {
            $("#txtMessage").html("인터넷환경 확인후 접속부탁드립니다.");
        }
    });
}

/**************************************************
작성자   : famo23
작성일자 : 2019-02-26
작성목적 : 로그인 페이지 이동 
***************************************************/
SetLogin = function () {
    // 0 - 아이디+패스워드 , 1-패스워드 , 2-자동인증
    if (LoginType == "1") {
        $("#txtMessage").empty();
    }
    else if (LoginType == "2") {

        ParamArray = {
            'uID': SaveLID,
            'uCID': $("#ddlCompany").val(),
            'uuID': UUID,
            'regID': RegID
        };

        CallEncrypt(CallGetParam());

        location.href = "/DeskPlusMobile/Portal/Ws/GetDefault.asmx/GetLoginOutSideApp?p=" + GetEncryptValue;
    }
    else {
        SaveLID = "";
        $("#tbLoginID").val("");
        $("#txtMessage").empty();
    }
}



/**************************************************
작성자   : famo23
작성일자 : 2019-02-26
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
작성일자 : 2019-02-26
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

