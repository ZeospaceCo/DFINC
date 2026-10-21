Menu = {}

$(function () {
    $(document).ready(function () {
        
    })
});

Menu.GoListPage = function (menuID) {
    
    var listPage = "Mod.aspx";
    if (menuID == "") { menuID = "manager"; }

    switch (menuID.toUpperCase()) {
        case "MANAGER": //관리자
            listPage = "Mod.aspx";
            menuID = "manager";
            break;
        case "LIST": //리스트
            listPage = "List.aspx";
            menuID = "list";
            break;
    }

    if (listPage != "") {
        ParamArray = { 'menuID': menuID };
        CallEncrypt(CallGetParam());
        parent.frmFolderContent.location.href = listPage + "?p=" + GetEncryptValue;
    }
}