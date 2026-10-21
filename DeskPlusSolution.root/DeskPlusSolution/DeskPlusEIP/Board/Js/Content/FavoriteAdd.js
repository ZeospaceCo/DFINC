FavoriteAdd = {}

FavoriteAdd.TreeFrame = parent.frmLeft;
FavoriteAdd.ContentFrame = parent.frmContent;

$(function () {
    $(document).ready(function () {
        //확인
        $('#CbtnConfirm').click(function (e) {
            e.preventDefault();
            FavoriteAdd.Add();
        });

        //다국이름
        $('#MultiName').click(function (e) {
            e.preventDefault();
            CallMultiName($("#ctbEnglishName").val(), "GetMultiName", MultiLang);
        });

        $('#ctbEnglishName').attr("disabled", true);
    });
});

/**************************************************
추가
***************************************************/
FavoriteAdd.Add = function () {
    $('#ctbEnglishName').attr("disabled", false);
    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbName", R_InputFolderName, true);

    if (!validator.validate()) return;

    __doPostBack('lbtnConfirm', '');
}

/**************************************************
추가, 수정 후 트리 새로고침
***************************************************/
FavoriteAdd.GoTree = function () {
    ParamArray = {
        'dcparm': Dcparm,
        'menuCatID': MenuCatID,
        'catID': CatID,
        'accActive': 2
    };

    CallEncrypt(CallGetParam());

    var goUrl = "/DeskPlusEIP/Board/Category/Tree.aspx?p=" + GetEncryptValue;
    if (FavoriteAdd.TreeFrame) FavoriteAdd.TreeFrame.location.href = goUrl;
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

    $("#ctbEnglishName").val(multiNames);
}