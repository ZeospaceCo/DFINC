View = {}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 데이터 초기화 // 데이터베이스로부터 출력할 데이터를 가져와서 출력하는 메서드
**********************************************************************************************/
$(function () {

    $(document).ready(function () {

        //목록
        $('#CbtnCancel').click(function (e) { e.preventDefault(); View.Cancel(); });

    });
});

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 취소
***************************************************/
View.Cancel = function () {
    ParamArray = {
        'currentPage': CurrentPage
        , 'recordPerPage': RecordPerPage
        , 'searchField': SearchField
        , 'searchValue': SearchValue
        , 'searchTerm': SearchTerm
        , 'searchTermValue': SearchTermValue
        , 'order': Order
        , 'targetPage': TargetPage
        , 'cabinetID': CabinetID
    };
    CallEncrypt(CallGetParam());
    location.href = TargetPage + "?p=" + GetEncryptValue;
}
