FavoriteAdd = {}

FavoriteAdd.TreeFrame = parent.frmLeft;
FavoriteAdd.ContentFrame = parent.frmContent;

/**************************************************
작성자 : famo23
작성일자 : 2011-12-26
작성목적 : 초기화 
***************************************************/
$(function () {
    $(document).ready(function () {
        //확인
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            FavoriteAdd.Add();
        });

        //다국이름
        $('#MultiName').click(function (e) {
            e.preventDefault();
            CallMultiName($("#ctbMultiName").val(), "GetMultiName", MultiLang);
        });

        $('#ctbMultiName').attr("disabled", true);
    });
});

/**************************************************
작성자 : famo23
작성일자 : 2011-12-26
작성목적 : 추가
***************************************************/
FavoriteAdd.Add = function () {

    $('#ctbMultiName').attr("disabled", false);
    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbName", R_InputFavoriteFolderName, true);
    if (!validator.validate()) return;

    __doPostBack('lbtnConfirm', '');
}

/**************************************************
작성자 : famo23
작성일자 : 2011-12-26
작성목적 : 추가, 수정 후 트리 새로고침
***************************************************/
FavoriteAdd.GoTree = function () {

    ParamArray = {
        'accActive': 4
    };

    CallEncrypt(CallGetParam());

    var goUrl = "/DeskPlusEIP/WebHard/Folder/Tree.aspx?p=" + GetEncryptValue;
    if (FavoriteAdd.TreeFrame) FavoriteAdd.TreeFrame.location.href = goUrl;

}


/**************************************************
작성자 : famo23
작성일자 : 2011-12-26
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

    $("#ctbMultiName").val(multiNames);
}