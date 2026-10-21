AddSimple = {}

$(function () {
    $(document).ready(function () {
        //확인
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            AddSimple.Confirm();
        });

        //닫기
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });
    });
});

/**************************************************
작성자   : shmipark
작성일자 : 2011-04-11
작성목적 : 간략 주소록 저장
***************************************************/
AddSimple.Confirm = function () {

    var validator = new Validator($("#form1"));

    validator.checkRequired("ctbName", R_InputFitstName, true);
    validator.checkRequired("ctbMail", R_InputEmail, true);
    validator.checkEmail("ctbMail", R_NotEmailFormat, true);

    var name = $("#ctbName").val();
    var mail = $("#ctbMail").val();
    var mobile1 = $("#ctbMobile1").val();
    var mobile2 = $("#ctbMobile2").val();
    var mobile3 = $("#ctbMobile3").val();
    var company = $("#ctbCompany").val();
    var deptName = $("#ctbDeptName").val();
    var companyPhone1 = $("#ctbPhone1").val();
    var companyPhone2 = $("#ctbPhone2").val();
    var companyPhone3 = $("#ctbPhone3").val();

    if (!validator.validate()) return;

    ParamArray = {
        'categoryID': CategoryID,
        'types': Types,
        'name': name,
        'mobile1': mobile1,
        'mobile2': mobile2,
        'mobile3': mobile3,
        'company': company,
        'deptName': deptName,
        'companyPhone1': companyPhone1,
        'companyPhone2': companyPhone2,
        'companyPhone3': companyPhone3
    };
    
    var url = "/DeskPlusEIP/Address/Ws/Content/AddSimple.asmx/AddPrivate";
    var obj = eval("AddSimple.SuccessAddPrivate");
    CallEncrypt(CallGetParam());
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 간편주소록 성공시 페이지 표시
**********************************************************************************************/
AddSimple.SuccessAddPrivate = function (data, status) {
//    parent.List.GetCount();
//    parent.List.GetPageList();
//    parent.disablePopup();

    if (parent.disablePopup) {
        parent.List.GetCount(); parent.List.GetPageList(); parent.disablePopup();
    } else { opener.List.GetCount(); opener.List.GetPageList(); self.close(); }
}
