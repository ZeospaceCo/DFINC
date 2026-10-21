var AdminTree = {};

/**************************************************
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

    });
});

/**************************************************************
 리스트 페이지 호출하기
**************************************************************/
AdminTree.GoListPage = function (menuID) {

    var listPage = "AdminRole.aspx";
    if (menuID == "") { menuID = "ADMINROLE"; }

    switch (menuID.toUpperCase()) {
        case "ADMINROLE": //양식 관리
            listPage = "AdminRole.aspx";
            menuID = "AdminRole";
            break;
        default:
            break;
    }

    if (listPage != "") {
        ParamArray = { 'menuID': menuID };
        CallEncrypt(CallGetParam());
        parent.frmContent.location.href = listPage + "?p=" + GetEncryptValue;
    }
}
