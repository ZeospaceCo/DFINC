Add = {};

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //메뉴 추가
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            Add.Add();
        });

        //순서
        $('#cbtnMove').click(function (e) {
            e.preventDefault();
            Add.ModOrder();
        });

        //다국이름
        $('#MultiName').click(function (e) {
            e.preventDefault();
            CallMultiName($("#ctbMultiName").val(), "GetMultiName", MultiLang);
        });

        $('#ctbMultiName').attr("disabled", true);
        $('#ctbMenuName').focus();
    });
});


/**********************************************************************************************
작성자   : 
작성일자 : 2011-11-21
작성목적 : 메뉴추가
**********************************************************************************************/
Add.Add = function () {

    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbMenuName", R_InputMenuName, true);                      //"이름을 입력해 주세요"
//    validator.checkRequired("ctbTargetURL", "대상 URL 을 입력해 주세요", true);                //"로그인 아이디를 입력해 주세요"    
    if (!validator.validate()) return;

    $('#ctbMultiName').attr("disabled", false);    
    __doPostBack('lbtnWrite', '');
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

    $("#ctbMultiName").val(multiNames);
}

/**********************************************************************************************
작성자   : 
작성일자 : 2011-11-21
작성목적 : 리스트 이동
**********************************************************************************************/
function GoTree() {
    parent.frmLeft.location = "Tree.aspx";
}


/**********************************************************************************************
작성자   : 
작성일자 : 2011-11-21
작성목적 : 순서변경
**********************************************************************************************/
Add.ModOrder = function () {

    ParamArray = {
        'menuID': '0'
    };

    CallEncrypt(CallGetParam());
    var url = "MoveUpdown.aspx?p=" + GetEncryptValue;

    PopupWin(400, 400, url, "", "", true);
}