var Register = {}


/**************************************************
초기화
***************************************************/
$(function () {    

    $(document).ready(function () {
        

        //폴더명(다국이름)
        $('#MultiName').click(function (e) {
            e.preventDefault();

            CallMultiName($("#ctbMuliLang").val(), "GetMultiName", MultiLang);
        });


        //등록하기
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            Register.Confirm();
        });

        //삭제하기
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            Register.Del();
        });

        //목록 가기
        $('#cbtnList').click(function (e) {
            e.preventDefault();

            ParamArray = {                
                'recordPerPage': RecordPerPage,
                'currentPage': CurrentPage,
                'searchKey': SearchKey,
                'searchValue': SearchValue,
                'order': Order,
                'gubun': Gubun,
                'active': '0'
            };

            CallEncrypt(CallGetParam());

            location.href = "List.aspx?p=" + GetEncryptValue;
        });
        


        $('#ctbMuliLang').attr("disabled", true);  
    });
});




/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 추가/수정
***************************************************/
Register.Confirm = function () {

    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbName", R_FaxRegisterFaxGroupName, true);
    if (!validator.validate()) return;    

    $('#ctbMuliLang').attr("disabled", false);
    __doPostBack('lbtnRegister', '');
}


/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 삭제
***************************************************/
Register.Del = function () {

    if (FaxID != "") {
        if (window.confirm(R_FaxReallyDel)) {
            __doPostBack('lbtnRegister', 'del');
        }
    }
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

    $("#ctbMuliLang").val(multiNames);
}

