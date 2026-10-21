/// <reference path="../../../../Html/Js/jQuery/jquery.js" />


ListUserDept = {}


/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //관리자 페이지 이동
        $('#cbtnAdmin').click(function (e) {
            e.preventDefault();
            ListUserDept.Admin();
        });

    });
});

/**************************************************
작성자   : kkzz52
작성일자 : 2018-04-18
작성목적 : 관리자 도구
***************************************************/
ListUserDept.Admin = function () {
    var url = "../Admin/Default.aspx";
    parent.location.href = url;
}

/**************************************************
작성자   : kkzz52
작성일자 : 2019-04-03
작성목적 : 겸직자 기본 부서 저장하기
***************************************************/
ListUserDept.Add = function (_index, _userID) {

    var row = $("#ddlBaseDept" + _index);

    var url = "/DeskPlusEIP/HR/Ws/Content/ListUserDept.asmx/AddUserDept";

    ParamArray = {
        'userID': _userID,
        'deptID': row.val()
    };

    var obj = eval("ListUserDept.SuccessAdd");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : kkzz52
작성일자 : 2019-04-03
작성목적 : 
**********************************************************************************************/
ListUserDept.SuccessAdd = function (data, status) {
    if (data.d == true) {
        alert(R_MSG_SUCCESS);

        var url = "/DeskPlusEIP/HR/Menu/Tree.aspx";
        ParamArray = { 'defaultMenuID': "ListUserDept", 'accActive': "1" };

        CallEncrypt(CallGetParam());
        parent.frmLeft.location.href = url + "?p=" + GetEncryptValue;
    }
    else {
        alert(R_ProcessingError);
    }
}
