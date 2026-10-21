var View = {};

/**************************************************************
1차 심사 페이지로 이동
**************************************************************/
View.FirstProcess = function () {
    var listPage = "../Admin/FirstProcess.aspx";
    ParamArray = { 'menuID': "FirstProcess" };
    CallEncrypt(CallGetParam());
    parent.frmContent.location = listPage + "?p=" + GetEncryptValue;
}

/**************************************************************
1차 심사완료 페이지로 이동
**************************************************************/
View.FirstResult = function () {
    var listPage = "../Admin/FirstProcess.aspx";
    ParamArray = { 'menuID': "FirstResult" };
    CallEncrypt(CallGetParam());
    parent.frmContent.location = listPage + "?p=" + GetEncryptValue;
}