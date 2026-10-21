SendSMS = {}

var AuthArray = []; //db 에서 권한 값을 권한배열에 담는다.

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 데이터 초기화 // 데이터베이스로부터 출력할 데이터를 가져와서 출력하는 메서드
**********************************************************************************************/

$(function () {
    $(document).ready(function () {
        //특수문자
        $('#SpecialChar').click(function (e) {
            e.preventDefault();

            var url = "SpecialChar.aspx";

            //PopupWin(300, 200, url, "", "", true);

            var width = "300";
            var height = "200";
            var left = Math.ceil((window.screen.availWidth - width) / 2);
            var top = Math.ceil((window.screen.availHeight - height) / 2);

            window.open("SpecialChar.aspx", "SpecialChar", "toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=no, width=" + width + ",height=" + height + ",top=" + top + ",left=" + left);
        });

        //문자저장
        $('#SaveChar').click(function (e) {
            e.preventDefault();
            SendSMS.SaveChar();
        });

        //문자호출
        $('#CallSaveChar').click(function (e) {
            e.preventDefault();

            var url = "CharList.aspx";
            //PopupWin(400, 310, url, "", "", true);

            var width = "400";
            var height = "310";
            var left = Math.ceil((window.screen.availWidth - width) / 2);
            var top = Math.ceil((window.screen.availHeight - height) / 2);

            window.open("CharList.aspx", "CharList", "toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=no, width=" + width + ",height=" + height + ",top=" + top + ",left=" + left);
        });

        //주소록 호출
        $('#CallAddress').click(function (e) {
            e.preventDefault();

            window.open("../SMS/AddressBook.aspx", "OpenAddressBook", "toolbar=no,width=800,height=580,directories=no,status=no,scrollbars=no,resize=no,menubar=no");
        });

        //최신전화번호
        $('#RecentNumber').click(function (e) {
            e.preventDefault();
            var url = "RecentNumber.aspx";
            //PopupWin(400, 310, url, "", "", true);

            var width = "400";
            var height = "310";
            var left = Math.ceil((window.screen.availWidth - width) / 2);
            var top = Math.ceil((window.screen.availHeight - height) / 2);

            window.open("RecentNumber.aspx", "RecentNumber", "toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=no, width=" + width + ",height=" + height + ",top=" + top + ",left=" + left);
        });

        //수동 전화번호 추가
        $('#AddPhone').click(function (e) {
            e.preventDefault();

            var content = $("#quickSearch").val().securityText();

            if (content != "") {
                SendSMS.AddCompareArray(content, "");
                SendSMS.SendPhoneHTML();
            }
        });

        //예약전송
        $('#ChkReservSend').click(function (e) {
            e.preventDefault();

            if ($("#ChkReservSend").attr("class") == "label on") {
                $("#ChkReservSend").attr("class", "label off");
                $("#ChkReservSendInfo").hide();
            }
            else if ($("#ChkReservSend").attr("class") == "label off") {
                $("#ChkReservSend").attr("class", "label on");
                $("#ChkReservSendInfo").show();
            }
        });

        //전송하기
        $('#SendMessage').click(function (e) {
            e.preventDefault();

            SendSMS.SendSMS();
        });

        //문자초기화
        $('#ClearMessage').click(function (e) {
            e.preventDefault();

            $("#txtMessage").val("");
            SendSMS.CheckLen();
        });

        //예약팝업닫기
        $('#ReservClose').click(function (e) {
            e.preventDefault();
            $("#ChkReservSendInfo").hide();
        });

        $("#ReservYYYYMMDD").datepick($.datepicker.regional[PreLang]);
        $("#ChkReservSendInfo").hide();

        SendSMS.SendPhoneHTML();

        SendSMS.Init();
    });
});

/**********************************************************************************************
작성자   : shimpark
작성목적 : 초기화
**********************************************************************************************/
SendSMS.Init = function () {
    //예약일자 설정하기
    var d = new Date()
    $("#ReservHour").val(d.getHours());
    $("#ReservMinute").val(0);
    var min = d.getMinutes();
    if (min > 0) {
        var j = Math.round(min / 5);
        $("#ReservMinute").val(j * 5);
    }

    GetSMSUser(ParamPhoneNumber);
}

/**********************************************************************************************
작성자   : shimpark
작성목적 : 폰글자 초기화
**********************************************************************************************/
SendSMS.Reset = function () {
    if ($("#txtMessage").val() == INPUT_PHONEMSG_MSG) {
        $("#txtMessage").val("");
    }

    $("#txtLen").val(0);
}

/**********************************************************************************************
작성자   : shimpark
작성목적 : 글자 크기 구하기
**********************************************************************************************/
SendSMS.CheckLen = function () {
    $("#txtLen").css("color", "Black");
    var msgtext = $("#txtMessage").val();

    var i = 0, l = 0;
    var temp, lastl;

    //길이를 구한다.
    while (i < msgtext.length) {
        temp = msgtext.charAt(i);

        if (escape(temp).length > 4)
            l += 2;
        else if (temp != '\r')
            l++;

        //        if (temp == '\r' && l > 90) //			if (temp=='\r' && l>400)
        //        {
        //            $("#txtMessage").val($("#txtMessage").val().substr(0, i));
        //        }

        // OverFlow
        if (l > 80) { $("#txtLen").css("color", "Red"); }

        // OverFlow
        /*
        if (l > 80) {
        alert(R_LIMIT_PHONEMSG_MSG);
        temp = $("#txtMessage").val().substr(0, i);
        $("#txtMessage").val(temp);
        l = lastl;
        i = msgtext.length;
        $("#txtLen").text(l + "/80");

        return;
        break;
        }
        */

        lastl = l;
        i++;
    }

    $("#txtLen").text(l + "/80");
}

/**********************************************************************************************
작성자   : shimpark
작성목적 : 이모티콘,특수문자에서 비주얼폰으로 옮기기
**********************************************************************************************/
SendSMS.AddChar = function (ch) {
    $("#txtMessage").append(ch);
    SendSMS.CheckLen();
}

/**********************************************************************************************
작성자   : shimpark
작성목적 : 문자저장
**********************************************************************************************/
SendSMS.SaveChar = function () {
    var content = $("#txtMessage").val();
    var confirmAlert = "[" + content + "] " + R_ShallWeInputSave;

    if (confirm(confirmAlert)) {
        var url = "/DeskPlusEIP/Portlet/Ws/Package/SMSNew/SendSMS.asmx/SaveContent";

        //파라미터값 배열에 담기
        //auth 관리자페이지 : 1; 일반인페이지 : 0
        ParamArray = {
            'id': 0,
            'gubun': 'insert',
            'content': escape(content)
        };

        var obj = eval("SendSMS.SuccessSaveChar");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : shimpark
작성목적 : 문자저장, 완료
**********************************************************************************************/
SendSMS.SuccessSaveChar = function (data, status) {
    if (data.d == true) {
        alert(R_SMS_SMSConfig_Success);
    }
}

/**************************************************
작성자   : shimpark
작성목적 : 배열값 삭제하기 id :전화번호; name : 이름
***************************************************/
SendSMS.DelAuthArray = function (keyID, keyName) {
    for (var i = 0; i < AuthArray.length; i++) {
        if (AuthArray[i].id == keyID) {
            AuthArray.splice(i, 1);
            break;
        }
    }

    SendSMS.SendPhoneHTML();
}

/**************************************************
작성자   : shimpark
작성목적 : 배열값 추가하기, id :전화번호; name : 이름
***************************************************/
SendSMS.AddAuthArray = function (keyID, keyName) {
    AuthArray.push({
        'id': keyID,
        'name': keyName
    });
}

/**************************************************
작성자   : shimpark
작성목적 : 추가할 값을 배열에서 중복 체크하여 추가하기 id :전화번호; name : 이름
***************************************************/
SendSMS.AddCompareArray = function (keyID, keyName) {
    var response = 0; //비교값이 없을 경우, 그냥 추가한다.

    if (AuthArray.length > 0) {
        for (var i = 0; i < AuthArray.length; i++) {
            //alert("AuthArray[i].id : [" + i + "]" + v.id + " | compArr.id : " + compArr.id);

            if (AuthArray[i].id == keyID) {
                response = 1; //동일한 값이 존재할 경우
                break;
            }
        }

        if (response == 0) {
            //중복되지 않으면 권한값 할당하기
            SendSMS.AddAuthArray(keyID, keyName);
        }
    }
    else {
        SendSMS.AddAuthArray(keyID, keyName);
    }
}

/**************************************************
작성자   : shimpark
작성목적 : 추가할 값을 배열에서 중복 체크하여 추가하기 id :전화번호; name : 이름
***************************************************/
SendSMS.SendPhoneHTML = function () {
    var response = "";
    for (var i = 0; i < AuthArray.length; i++) {
        response += "<li id='rcpt" + i + "'>";
        response += "<em>" + Number(i + 1) + "</em>";
        response += "<span id='ncell" + i + "'>" + AuthArray[i].name + "</span>";
        //response += "<input id='cell" + i + "' type='text' class='text' value='" + AuthArray[i].id + "' />";
        response += "<span style='width:80px'>" + AuthArray[i].id + "</span>";
        response += "<button type=\"button\" class=\"delete\" title=\"\" onclick=\"SendSMS.DelAuthArray('" + AuthArray[i].id + "','" + AuthArray[i].name + "')\">삭제</button>";
        response += "</li>";
    }

    $("#rcptUl").html("");
    $("#rcptUl").html(response);
}

//==================================================================
// GetSMSUser : SMS 전송할 사용자 정보 가져오기
// 이름|전화번호$이름|전화번호$
//==================================================================
function GetSMSUser(p_user) {
    var rtnVal = p_user

    if (rtnVal != "") {
        if (rtnVal.indexOf("$") > -1) {
            var wholeSplit = rtnVal.split("$");
            for (var i = 0; i < wholeSplit.length; i++) {
                if (wholeSplit[i] != "") {
                    var valueSplit = wholeSplit[i].split("|");
                    var phoneNumber = valueSplit[1];
                    var phoneName = valueSplit[0];

                    SendSMS.AddCompareArray(phoneNumber, phoneName);
                }
            }
        }
        else {
            var valueSplit = rtnVal.split("|");
            var phoneNumber = valueSplit[1];
            var phoneName = valueSplit[0];

            SendSMS.AddCompareArray(phoneNumber, phoneName);
        }

        SendSMS.SendPhoneHTML();
    }
}

/**********************************************************************************************
작성자   : shimpark
작성목적 : 문자 발송
**********************************************************************************************/
SendSMS.SendSMS = function () {
    var content = $("#txtMessage").val();
    var sendNumber = $("#SendNumber").val();
    var receiveNumber = ""; //id: 전화번호; name: 이름
    var status = "0";
    var reservYYYYMMDD = "";
    var reservHour = "";
    var reservMinute = "";
    var R_SendMsg = "문자를 지금 전송 하시겠습니까?";

    if (content == "") {
        alert("문자를 입력해 주세요.");
        return;
    }

    if (sendNumber == "") {
        alert("보내는사람 휴대폰 번호가 존재하지 않습니다. 나의 정보에서 휴대폰 번호를 입력해 주시기 바랍니다.");
        return;
    }

    var sendNumberMatch = sendNumber.match(RegExp("-", 'g'));
    if (sendNumberMatch == null) {
        alert("보내는사람 휴대폰번호를 (010-111-1111, 02-1111-2222) 형태로 기재해 주세요.");
        return;
    }

    //하이픈 - 이 두개 여야 한다.
    if (sendNumberMatch.length != 2) {
        alert("보내는사람 휴대폰번호를 (010-111-1111, 02-1111-2222) 형태로 기재해 주세요.");
        return;
    }

    //보내는 번호가 9자리 이하면 경고창 띄운다. (- 하이픈 포함) 이정웅 
    if (sendNumber.length < 10) {
        alert("보내는사람 휴대폰 번호에 지역번호를 기재해 주세요.");
        return;
    }

    for (var i = 0; i < AuthArray.length; i++) {
        //전화번호: 이름; 전화번호: 이름;
        receiveNumber += AuthArray[i].id + ":" + AuthArray[i].name + ",";
    }

    if (receiveNumber == "") {
        alert("전송할 휴대폰 번호가 존재하지 않으므로 입력해 주시기 바랍니다.");
        return;
    }

    if ($("#ChkReservSend").attr("class") == "label on") {
        status = "1"; //예약
        R_SendMsg = "문자를 예약 전송 하시겠습니까?";
        reservYYYYMMDD = $("#ReservYYYYMMDD").val();
        reservHour = $("#ReservHour").val();
        reservMinute = $("#ReservMinute").val();

        if (reservYYYYMMDD == "") {
            alert("예약 발송 일자를 선택해 주세요.");
            return;
        }

        if (reservHour == "") {
            alert("예약 발송 시(hour)를 선택해 주세요.");
            return;
        }

        if (reservMinute == "") {
            alert("예약 발송 분(minite)을 선택해 주세요.");
            return;
        }
    }

    if (window.confirm(R_SendMsg)) {
        var url = "/DeskPlusEIP/Portlet/Ws/Package/SMSNew/SendSMS.asmx/AddSMS";

        //파라미터값 배열에 담기
        //auth 관리자페이지 : 1; 일반인페이지 : 0
        ParamArray = {
            'msg': escape(content),
            'status': status,
            'yyyymmdd': reservYYYYMMDD,
            'hh': reservHour,
            'mm': reservMinute,
            'replyNumber': sendNumber,
            'receiveNumber': receiveNumber
        };

        var obj = eval("SendSMS.SuccessSendSMS");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : shimpark
작성목적 : 문자 전송, 완료
**********************************************************************************************/
SendSMS.SuccessSendSMS = function (data, status) {
    if (data.d == true) {
        alert(R_SMS_SMSConfig_Success);
    }
}