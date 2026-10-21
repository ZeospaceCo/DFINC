View = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        //추가
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            View.Add();
        });

        //수정
        $('#cbtnMod').click(function (e) {
            e.preventDefault();
            //__doPostBack('cbsEdit_Click', '');
            View.Mod();
        });

        //삭제
        $('#cbtnDel').click(function (e) {
            e.preventDefault();
            //__doPostBack('cbsDel_Click', '');
            View.Del();
        });

        //목록
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            View.Cancel();
        });
    });
});

/**********************************************************************************************
[취소]
**********************************************************************************************/
View.Cancel = function () {
    ParamArray = {
        'menuID': MenuID,
        'projectID': ProjectID,
        'recordPerPage': RecordPerPage,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    CallEncrypt(CallGetParam());
    location.href = "JList.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
[작성하기]
**********************************************************************************************/
View.Add = function () {
    ParamArray = {
        'menuID': MenuID,
        'projectID': ProjectID,
        'recordPerPage': RecordPerPage,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order
    };

    CallEncrypt(CallGetParam());
    location.href = "AddN.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
[수정]
**********************************************************************************************/
View.Mod = function () {
    ParamArray = {
        'menuID': MenuID,
        'projectID': ProjectID,
        'recordPerPage': RecordPerPage,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'templateID': TemplateID
    };

    CallEncrypt(CallGetParam());
    location.href = "ModN.aspx?p=" + GetEncryptValue;
}

/**********************************************************************************************
[삭제]
**********************************************************************************************/
View.Del = function () {
    if (window.confirm(R_ApplyDelete)) {
        __doPostBack("LBTNProcess", "DEL");
    }
}