Add = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        // [추가]
        $('#cbtnAdd').click(function (e) { e.preventDefault(); Add.Confirm(); });

        // [목록]
        $('#cbtnClose').click(function (e) { e.preventDefault(); Add.Close(); });

        // 다국이름
        $('#MultiName').click(function (e) {
            e.preventDefault();
            CallMultiName($("#ctbMultiLang").val(), "GetMultiName", MultiLang);
        });

        $('#ctbMultiLang').attr("disabled", true);
    });
});


/**************************************************
작성자   : 
작성일자 : 
작성목적 : 확인
***************************************************/
Add.Confirm = function () {

    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbAccessGradeName", R_Msg_ErrAccessGradeName, true);
    if (!validator.validate()) return;

    $('#ctbMultiLang').attr("disabled", false);

    __doPostBack('lbtnSubmit', "ADD");
}
/**************************************************
작성자   : 
작성일자 : 
작성목적 : 닫기
***************************************************/
Add.Close = function () {
    ParamArray = {
        'recordPerPage': RecordPerPage,
        'currentPage': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    CallEncrypt(CallGetParam());
    location.href = "List.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : 이지원
작성일자 : 2017-06-08
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

    $("#ctbMultiLang").val(multiNames);
}