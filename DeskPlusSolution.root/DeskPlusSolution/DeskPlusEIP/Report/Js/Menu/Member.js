Member = {};

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        if (parent.frmContent) {
            ParamArray = {
                'years': Years,
                'months': Months,
                'days': Days
            };

            var listPage = "/DeskPlusEIP/Report/Content/List.aspx";
            CallEncrypt(CallGetParam());
            parent.frmContent.location = listPage + "?p=" + GetEncryptValue;
        }

        Member.GetPageList();
    });
});

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 게시판 리스트 호출
**********************************************************************************************/
Member.GetPageList = function () {
    alert(1);
    var url = "/DeskPlusEIP/Report/Ws/Menu/Member.asmx/GetPageList";

    //파라미터값 배열에 담기
    //auth 관리자페이지 : 1; 일반인페이지 : 0
    ParamArray = {
        'years': Years,
        'months': Months,
        'days': Days
    };

    var obj = eval("Member.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 게시판 리스트 표시
**********************************************************************************************/
Member.SuccessPageList = function (data, status) {
    $('#placeholder').setTemplateURL('/DeskPlusEIP/Report/Jt/Menu/JTList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholder').processTemplate(data.d);
    $('#loading').html("");

    //List.Resource();
}

/**************************************************
작성자   : 심재운
작성일자 : 2011-04-11
작성목적 : 달력 선택
***************************************************/
function OnSelectionChanged(cal) {
    var selectedYear = cal.FormatDate(CalendarSingle.GetSelectedDate(), "yyyy");
    var selectedMonth = cal.FormatDate(CalendarSingle.GetSelectedDate(), "MM");
    var selectedDay = cal.FormatDate(CalendarSingle.GetSelectedDate(), "dd");

    if (parent.frmContent) {
        ParamArray = {
            'years': selectedYear,
            'months': selectedMonth,
            'days': selectedDay
        };

        var listPage = "/DeskPlusEIP/Report/Content/List.aspx";
        CallEncrypt(CallGetParam());
        parent.frmContent.location = listPage + "?p=" + GetEncryptValue;
    }
}