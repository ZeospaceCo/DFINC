Add = {}
Add.TreeFrame = parent.frmLeft;
Add.ContentFrame = parent.frmContent;

/**************************************************
작성자 : famo23
작성일자 : 2011-10-18
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //확인
        $('#CbtnConfirm').click(function (e) {
            e.preventDefault();
            Add.Add();
        });

        //다국이름
        $('#MultiName').click(function (e) {
            e.preventDefault();
            CallMultiName($("#CTBMultiName").val(), "GetMultiName", MultiLang);
        });

        $('#CTBMultiName').attr("disabled", true);
    });
});

/**************************************************
작성자 : famo23
작성일자 : 2011-10-18
작성목적 : 폴더 추가
***************************************************/
Add.Add = function () {
    $('#CTBMultiName').attr("disabled", false);
    var validator = new Validator($("#form1"));
    validator.checkRequired("CTBName", R_InputFolderName, true);
    if (!validator.validate()) return;

    __doPostBack('lbtnConfirm', '');
}

/**************************************************
작성자 : famo23
작성일자 : 2011-10-18
작성목적 : 추가, 수정 후 트리 새로고침
***************************************************/
Add.GoTree = function () {
    ParamArray = {
        'accActive': 0
    };

    CallEncrypt(CallGetParam());

    var goUrl = "/DeskPlusEIP/PMS/Folder/Tree.aspx?p=" + GetEncryptValue;
    if (Add.TreeFrame) Add.TreeFrame.location.href = goUrl;
}

/**************************************************
작성자   : famo23
작성일자 : 2011-10-18
작성목적 : 다국이름값 가져와서 설정하기
***************************************************/
function GetMultiName(name_base, name_ko, name_en, name_ja, name_fr, name_zhcn, name_zhtw) {
    name_ko = decodeURIComponent(name_ko);
    name_en = decodeURIComponent(name_en);
    name_ja = decodeURIComponent(name_ja);
    name_fr = decodeURIComponent(name_fr);
    name_zhcn = decodeURIComponent(name_zhcn);
    name_zhtw = decodeURIComponent(name_zhtw);

    var multiNames = name_ko + "," + name_en + "," + name_ja + "," + name_fr + "," + name_zhcn + "," + name_zhtw;

    $("#CTBMultiName").val(multiNames);
}