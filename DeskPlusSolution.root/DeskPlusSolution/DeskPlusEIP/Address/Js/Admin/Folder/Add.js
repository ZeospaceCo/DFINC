Add = {}
Add.TreeFrame = parent.frmLeft;
Add.ContentFrame = parent.frmContent;

$(function () {
    $(document).ready(function () {
        //확인
        $('#CbtnConfirm').click(function (e) {
            e.preventDefault();
            Add.Add();
        });

        //하위그룹 순서 변경
        $('#cbtnFolderOrder').click(function (e) {
            e.preventDefault();
            Add.FolderOrder();
        });

        //하위그룹 이동 변경
        $('#cbtnFolderMove').click(function (e) {
            e.preventDefault();
            Add.FolderMove();
        });

        //다국이름
        $('#MultiName').click(function (e) {
            e.preventDefault();
            CallMultiName($("#CTBEnglishName").val(), "GetMultiName", MultiLang);
        });

        $('#CTBEnglishName').attr("disabled", true);

    });
});

/**************************************************
추가
***************************************************/
Add.Add = function () {

    $('#CTBEnglishName').attr("disabled", false);
    var validator = new Validator($("#form1"));
    validator.checkRequired("CTBName", R_InputFolderName, true);
    if (!validator.validate()) return;

    __doPostBack('lbtnConfirm', '');
}


/**************************************************
작성자   : 심재운
작성일자 :2011-06-15
작성목적 : 하위그룹 순서 변경
***************************************************/
Add.FolderOrder = function () {

    ParamArray = {
        'categoryID': CategoryID,
        'types': Types
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Address/Admin/Content/UserFolder.aspx?p=" + GetEncryptValue;
    PopupWin(400, 400, url, "", "", true);
}


/**************************************************
작성자   : 심재운
작성일자 :2011-06-15
작성목적 : 하위그룹 이동 변경
***************************************************/
Add.FolderMove = function () {

    ParamArray = {
        'categoryID': CategoryID,
        'types': Types
    };

    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Address/Admin/Folder/Move.aspx?p=" + GetEncryptValue;
    PopupWin(400, 400, url, "", "", true);
}

/**************************************************
추가, 수정 후 트리 새로고침
***************************************************/
Add.GoTree = function () {
    parent.frmLeft.location.href = "/DeskPlusEIP/Address/Admin/Menu/Menu.aspx";
}


/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 다국이름값 가져와서 설정하기
***************************************************/
function GetMultiName(name_base, name_ko, name_en, name_ja, name_fr, name_zhcn, name_zhtw) {

    name_base = decodeURIComponent(name_base);
    name_ko = decodeURIComponent(name_ko);
    name_en = decodeURIComponent(name_en);
    name_ja = decodeURIComponent(name_ja);
    name_fr = decodeURIComponent(name_fr);
    name_zhcn = decodeURIComponent(name_zhcn);
    name_zhtw = decodeURIComponent(name_zhtw);

    var multiNames = name_ko + "," + name_en + "," + name_ja + "," + name_fr + "," + name_zhcn + "," + name_zhtw;

    $("#CTBEnglishName").val(multiNames);
}