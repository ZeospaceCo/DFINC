Add = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

    });
});


/**************************************************
작성자   : 
작성일자 : 
작성목적 : 확인
***************************************************/
Add.Confirm = function () {
    //필수입력
    var validator = new Validator($("#form1"));
    validator.checkRequired("ctbSubject", R_Msg_ErrSubj, true);

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
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    CallEncrypt(CallGetParam());
    location.href = "List.aspx?p=" + GetEncryptValue;
}