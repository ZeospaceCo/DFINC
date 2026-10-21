View = {}

$(function () {
    $(document).ready(function () {

        // [인쇄]
        $('#cbtnPrint').click(function (e) { e.preventDefault(); View.Print(); });
        // [취소]
        $('#cbtnCancel').click(function (e) { e.preventDefault(); View.Cancel(); });
    });
});

/**************************************************
작성자   : 
작성일자 : 
작성목적 : [인쇄]
***************************************************/
View.Print = function () {
//    var divToPrint = document.getElementById('ifrmForm');

//    //스타일정보+내용
//    var html = "<HTML><LINK rel=stylesheet type=text/css href='/DeskPlusEIP/Approval/Css/Func_ApprovalView.css'>"
//    html += divToPrint.contentWindow.document.body.outerHTML;
//    html += "</html>";

//    newWin = window.open();
//    newWin.document.write(html);
//    newWin.location.reload();
//    newWin.focus();
//    newWin.print();
//    newWin.close();
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : [취소]
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
        , 'formID': FormID
    };
    CallEncrypt(CallGetParam());
    location.href = "List.aspx?p=" + GetEncryptValue;
}
