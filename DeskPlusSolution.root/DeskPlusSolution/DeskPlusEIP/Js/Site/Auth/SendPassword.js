SendPassword = {}

$(function () {
    $(document).ready(function () {

        if (Gubun == "search") {
            $('#cbtnConfirm').hide(); //확인버튼 숨김
            $('#trEmail').hide();
            $('#txtSendGuide').hide();

        }
        else {
            $('#trSearchEmail').hide();
            $('#trSearchLoginID').hide();
            $('#txtSearchGuide').hide();
            $('#cbtnCheckConfirm').hide();
        }

        //확인(로그인5번틀리면 나옴)
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            SendPassword.Confirm();
        });


        //전송(체크후전송 PW찾기기능)
        $('#cbtnCheckConfirm').click(function (e) {
            e.preventDefault();
            SendPassword.CheckConfirm();
        });


        //로그인ID로 2차외부메일 검색
        $('#cbtnLoginCheck').click(function (e) {
            e.preventDefault();
            SendPassword.SearchEmail();
        });


        //닫기
        $('#cbtnClose').click(function (e) {
            e.preventDefault();

            SendPassword.Close();
        });

    });
});

/*************************
작성자: 
작성일 
용도: 저장 : 창 닫기
**************************/
SendPassword.Close = function () {
    $("#tbPassword", parent.document).val('');
    $("#tbPassword", parent.document).focus();
    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}

/*************************
작성자: 
작성일 
용도: 저장 : 저장
**************************/
SendPassword.Confirm = function () {

    var companyID = CompanyID;
    var loginID = LoginID;
    var lang = PreLangs;

    var url = "/DeskPlusEIP/Ws/Site/Auth/Default.asmx/SendPassword";
    var data = "{companyID : '" + companyID + "',loginID : '" + loginID + "',langType : '" + lang + "'}";
    var success = eval("SendPassword.SuccessSendPassword");
    AjaxService(url, data, success);

}
SendPassword.SuccessSendPassword = function (data, status) {
    if (data.d == "1") {
        alert(R_Success);
        if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
    }
    else if (data.d == "NoPattern") {
        alert(R_CheckEmail);
        if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
    }
}


///로그인ID, Email 체크 후 전송
SendPassword.CheckConfirm = function () {

    if ($("#ctbCheckLoginID").val() == "" || $("#tbEmail").val() == "") {
        alert("입력한 값이 없습니다._리소스");
        return;
    }

    var companyID = CompanyID;
    var loginID = $("#ctbCheckLoginID").val();
    var email = $("#tbEmail").val();
    var lang = PreLangs;

    var url = "/DeskPlusEIP/Ws/Site/Auth/Default.asmx/CheckSendPassword";
    var data = "{companyID : '" + companyID + "',loginID : '" + loginID + "',email :'" + email + "',langType : '" + lang + "'}";
    var success = eval("SendPassword.SuccessCheckConfirm");
    AjaxService(url, data, success);
}

SendPassword.SuccessCheckConfirm = function (data, status) {
    if (data.d == "1") {
        alert(R_Success);
        if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
    }
    else if (data.d == "-1") {
        alert("관리자가 없습니다.");
        if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
    }
    else if (data.d == "0") {
        alert("등록된 메일주소와 입력한 메일주소가 다릅니다.");
        if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
    }
    else if (data.d == "NoPattern") {
        alert("이메일 형식에 맞지 않습니다.");
        if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
    }
}


//로그인ID로 2차 메일 검색
SendPassword.SearchEmail = function () {
    if ($("#ctbCheckLoginID").val() == "") {
        alert("로그인 ID를 입력 해 주세요");
        return;
    }

    var companyID = CompanyID;
    var loginID = $("#ctbCheckLoginID").val();

    var url = "/DeskPlusEIP/Ws/Site/Auth/Default.asmx/SearchEmail";
    var data = "{companyID : '" + companyID + "',loginID : '" + loginID + "'}";
    var success = eval("SendPassword.SuccessSearchEmail");
    AjaxService(url, data, success);

}

SendPassword.SuccessSearchEmail = function (data, status) {
    if (data.d == "-2") { //2차메일 없음
        alert("2차메일없음");
        $('#tbEmail').val("");
    }
    else {
        divAuthEmail.innerText = data.d; //2차메일이 있으면 하단에 뿌려줌 Hidden값에도 넣어줌
        $('#tbEmail').val(data.d);
    }
}