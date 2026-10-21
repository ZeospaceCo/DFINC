var AddressAdd = {};
AddressAdd.Control = null;

/**************************************************
작성자 : freeice122
작성일자 : 2014-03-13
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //추가
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            AddressAdd.Confirm();
        });

        //닫기
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            self.close();
        });

        AddressAdd.Init();
    });
});

/**********************************************************************************************
작성자   : freeice122
작성일자 : 2014-03-13
작성목적 :
**********************************************************************************************/
AddressAdd.Init = function () {
    $("#ctbName").val(UserName);
    $("#ctbMail").val(UserEmail);
}

/**************************************************
작성자   : freeice122
작성일자 : 2014-03-13
작성목적 : 주소 추가
***************************************************/
AddressAdd.Confirm = function () {
    var validator = new Validator($("#form1"));

    var name = $("#ctbName").val();
    var mail = $("#ctbMail").val();
    var phone = "";
    var Types = "PG";

    var CategoryID = $("#ddlAddressBook").val();

    if (name == "") {
        alert(R_InputFitstName);
        $("#ctbName").focus();
        return;
    }

    if (mail != "") {
        validator.checkEmail("ctbMail", R_NotCorrectEmailFormat, true);
    }

    if (!validator.validate()) return;

    var url = "/DeskPlusEIP/Address/Ws/Mail/AddressAdd.asmx/AddPrivate";
    var data = "{companyID : '" + CompanyID + "',userID : '" + UserID + "',preLang : '" + PreLang + "',categoryID : '" + CategoryID + "',types : '" + Types + "',name : '" + name + "',mail : '" + mail + "',mobile : '" + phone + "'}";

    var success = eval("AddressAdd.SuccessAddPrivate");
    AjaxService(url, data, success);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2014-03-13
작성목적 : 간편주소록 성공시 페이지 표시
**********************************************************************************************/
AddressAdd.SuccessAddPrivate = function (data, status) {
    alert(R_Complete);
    self.close();
}