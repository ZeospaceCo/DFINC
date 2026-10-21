ModPassword = {}

$(function () {
    $(document).ready(function () {

        // 저장
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();

            ModPassword.Confirm();
            //txtRepeat
        });
        //닫기

        $('#cbtnClose').click(function (e) {
            e.preventDefault();

            ModPassword.Close();
        });

        $("#ctbPassword").focus();
    });
});

/*************************
작성자 : 심재운 
작성일 : 2012.07.20
용도: 패스워드 저장
**************************/
ModPassword.Confirm = function () {

    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbPassword", R_InputNowPWd, true);
    validator.checkRequired("ctbNewPassword", R_InputNewPWd, true);
    validator.checkRequired("ctbNewPassword1", R_InputNewPWd1, true);

    if (!validator.validate()) return false;

    if ($("#ctbNewPassword").val() != $("#ctbNewPassword1").val()) {
        alert(R_InputComparePwd);
        return;
    }
    else {

        /*if ($("#ctbNewPassword").val() == PwdFirst || $("#ctbNewPassword").val().indexOf(PwdFirst) > -1) {
        alert(R_PwdRegex);
        return;
        }*/

        var arrPwdFirst = PwdFirst.split(',');
        for (var i = 0; i < arrPwdFirst.length; i++) {            
            if ($("#ctbNewPassword").val() == arrPwdFirst[i] || $("#ctbNewPassword").val().indexOf(arrPwdFirst[i]) > -1) {
                alert(R_PwdRegex);
                return;
            }
        }


        var passed = validatePassword($("#ctbNewPassword").val(), {
            length: [PwdLength, Infinity],
            lower: PwdLower,
            upper: PwdUpper,
            numeric: PwdNumeric,
            special: PwdSpecial,
            badWords: ["password", "!@#123"],
            badSequenceLength: 0
        });

        if (passed == true) {
            __doPostBack("lbtnSubmit", "");
        }
        else {
            alert(R_PwdRegex);
        }
    }
}


/*************************
작성자: 
작성일 
용도: 저장 : 창 닫기
**************************/
ModPassword.Close = function () {
    $("#tbPassword", parent.document).val('');
    $("#tbPassword", parent.document).focus();
    if (parent.disablePopup) { parent.disablePopup(); } else {
        location.href = "http://ep.di.co.kr";
    }
}


