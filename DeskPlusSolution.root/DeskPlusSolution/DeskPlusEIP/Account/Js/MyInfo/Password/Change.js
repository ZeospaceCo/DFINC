Change = {};

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //확인
        $('#CbtnConfirm').click(function (e) {
            e.preventDefault();
            Change.Confirm();
        });

        //취소
        $('#CbtnClose').click(function (e) {
            e.preventDefault();
            parent.disablePopup();
        });

        $('#ctbPwd').focus();
    });
});


/**************************************************
작성자   : freeman
작성일자 : 2011-11-21
작성목적 : 패스워드 변경
***************************************************/
Change.Confirm = function () {

    var url = "/DeskPlusEIP/Account/Ws/MyInfo/Password/Change.asmx/PasswordChange";


    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbPwd", R_InputPassword, true);
    validator.checkRequired("ctbPwdConfirm", R_InputPassword, true);
    if (!validator.validate()) return;

    var password = $("#ctbPwd").val();
    var passwordconfirm = $("#ctbPwdConfirm").val();

    var passedPwd = validatePassword(password, {
        length: [PwdLength, Infinity],
        lower: PwdLower,
        upper: PwdUpper,
        numeric: PwdNumeric,
        special: PwdSpecial,
        badWords: ["password", "!@#123"],
        badSequenceLength: 0
    });

    if (passedPwd == false) {
        alert(R_PwdRegex);
        $("#ctbPwd").focus();
        return;
    }

    //두개의 패스워드가 동일하지 않을 경우
    if (password != passwordconfirm) {
        alert(R_NoMatchPwd);
        return;
    }

    //초기패스워드와 동일할 경우 
    /*if (password == PwdFirst || password.indexOf(PwdFirst) > -1) {
        alert(R_PwdRegex);
        return;
    }*/
    
    var arrPwdFirst = PwdFirst.split(',');
    for (var i = 0; i < arrPwdFirst.length; i++) {

        if (password == arrPwdFirst[i] || password.indexOf(arrPwdFirst[i]) > -1) {
            alert(R_PwdRegex);
            return;
        }        
    }

    var data = "{password : '" + password + "'}";
    var success = eval("Change.SuccessPassword");
    AjaxService(url, data, success);
}

/**********************************************************************************************
작성자   : freeman
작성일자 : 2011-11-21
작성목적 : 패스워드 변경 성공처리
**********************************************************************************************/
Change.SuccessPassword = function (data, status) {

    if (data.d == true) {
        alert(R_Admin_UserManager_MSG_SUCCESS);
    }
    else
    {
        alert(R_DenyConfirm);
    }
    
    parent.disablePopup();
}
