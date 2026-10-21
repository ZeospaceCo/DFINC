UserListView = {}

$(function () {

    $(document).ready(function () {

        //엑셀로보기
        $('#cbtnExcel').click(function (e) {
            e.preventDefault();
            UserListView.ExcelDown();
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            UserListView.Search();
        });

        //목록
        $('#cbtnCancel').click(function (e) {
            e.preventDefault();
            UserListView.List();
        });

        $("#startDate").datepick($.datepicker.regional[PreLang]);
        $("#startDate").val(StartDate);

        $("#endDate").datepick($.datepicker.regional[PreLang]);
        $("#endDate").val(EndDate);
    });
});

/**************************************************
작성자   : 이지원
작성일자 : 2018-01-16
작성목적 : 목록으로 가기
***************************************************/
UserListView.List = function () {

    StartDate = $("#startDate").val();
    EndDate = $("#endDate").val();

    ParamArray = {
        'startDate': StartDate,
        'endDate': EndDate,
        'deptID': DeptID,
        'accessAuth': AccessAuth,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'workType': WorkType
    };

    CallEncrypt(CallGetParam());
    location.href = "UserList.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : 이지원
작성일자 : 2018-01-05
작성목적 : 변경
***************************************************/
UserListView.Search = function () {

    StartDate = $("#startDate").val();
    EndDate = $("#endDate").val();

    ParamArray = {
        'startDate': StartDate,
        'endDate': EndDate,
        'userID': UserID,                //사용자코드
        'userName': UserName,
        'deptID': DeptID,
        'accessAuth': AccessAuth,
        'callPage': CallPage,
        'pageSize': RecordPerPage,
        'pageSizePageNo': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'workType': WorkType

    };

    CallEncrypt(CallGetParam());
    location = "/DeskPlusEIP/ETC/Attendance/UserListView.aspx?p=" + GetEncryptValue;

}

/**************************************************
작성자   : 이지원
작성일자 : 2018-01-17
작성목적 : 휴가원 문서보기
***************************************************/
UserListView.View = function (docID) {

    ParamArray = {
        'documentID': docID
    };

    CallEncrypt(CallGetParam());

    var url = "/DeskPlusEIP/Approval/Common/Popup/ViewTot.aspx?p=" + GetEncryptValue;

    window.open(url, "View" + docID, "width=1200px,height=800px,menubar=no,location=no,resizable=yes,scrollbars=yes,status=no");
}

/**********************************************************************************************
작성자   : 이지원
작성일자 : 2018-01-05
작성목적 : 엑셀
**********************************************************************************************/
UserListView.ExcelDown = function () {

    $("#userNamee").val(UserName);
    $("#userIDe").val(UserID);
    $("#startDatee").val(StartDate);
    $("#endDatee").val(EndDate);
    
    __doPostBack('lbtnexcel', "");
}