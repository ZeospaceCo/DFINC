Tree = {};
Tree.ContentFrame = parent.frmContent;

$(function () {
    $(document).ready(function () {


    });
});



/**************************************************
작성자   : famo23
작성일자 : 2014-02-05
작성목적 : 달력 선택
***************************************************/
Tree.OnCalendarChange = function (cal) {

    var selectedYear = cal.FormatDate(CalendarSingle.GetSelectedDate(), "yyyy");
    var selectedMonth = cal.FormatDate(CalendarSingle.GetSelectedDate(), "MM");
    var selectedDay = cal.FormatDate(CalendarSingle.GetSelectedDate(), "dd");

    CalendarDate = selectedYear + '-' + selectedMonth + '-' + selectedDay;

    var tmpTagGubun = Tree.ContentFrame.TagGubun;

    ParamArray = {
        'tmpDate': CalendarDate,
        'tagGubun': tmpTagGubun
    };
    CallEncrypt(CallGetParam());

    var listPage = "ListMain.aspx";

    if (listPage != "" && Tree.ContentFrame) {
        CallEncrypt(CallGetParam());
        Tree.ContentFrame.location.href = listPage + "?p=" + GetEncryptValue;
    }
}


