Add = {};

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

       
        //권한 추가
        $('#cbtnADDAuth').click(function (e) {
            e.preventDefault();
            Add.AddAuth();
        });

        Add.MenuRoleList();        

    });
});

/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-01-17
작성목적 : 권한접근제어 리스트
**********************************************************************************************/
Add.MenuRoleList = function () {
    var url = "/DeskPlusEIP/MDM/Ws/MenuAuth/Add.asmx/GetMenuRoleList";

    //파라미터값 배열에 담기
    ParamArray = {
        'companyID': CompanyID,
        'menuID': MenuID
    };

    var obj = eval("Add.SuccessMenuRoleList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-01-17
작성목적 : 성공시, 권한접근제어 리스트
**********************************************************************************************/
Add.SuccessMenuRoleList = function (data, status) {

    $('#placeholder_Role').setTemplateURL('/DeskPlusEIP/MDM/Jt/MenuAuth/JTRole.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder_Role').processTemplate(data.d);

    Add.Resource();
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-01-17
작성목적 : 리소스
**********************************************************************************************/
Add.Resource = function () {

    $("#r_authName")[0].innerHTML = R_AuthName;
    $("#r_del")[0].innerHTML = R_Del;

    if ($(".btn_file_del")[0] != undefined) {
        $(".btn_file_del").text(R_Del);
    }

    if ($(".NoListData")[0] != undefined) {
        $(".NoListData")[0].innerHTML = R_NoContent;
    }
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-01-17
작성목적 : 권한 추가  
**********************************************************************************************/
Add.AddAuth = function () {

    ParamArray = {
        'menuID': MenuID
    };

    CallEncrypt(CallGetParam());
    var url = "AddAuth.aspx?p=" + GetEncryptValue;

    PopupWin(800, 500, url, "-500", "", true);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-01-17
작성목적 : 권한접근제어 삭제
**********************************************************************************************/
Add.DelMenuRole = function (p_id) {

    if (window.confirm(R_ConfirmDel)) {
        var url = "/DeskPlusEIP/MDM/Ws/MenuAuth/Add.asmx/DelMenuRole";

        //파라미터값 배열에 담기
        ParamArray = {
            'companyID': CompanyID,
            'menuRoleID': p_id
        };

        var obj = eval("Add.SuccessDelMenuRole");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-01-17
작성목적 : 성공시, 권한접근제어 삭제
**********************************************************************************************/
Add.SuccessDelMenuRole = function (data, status) {

    if (data.d == true) {
        Add.MenuRoleList();
    }
}



