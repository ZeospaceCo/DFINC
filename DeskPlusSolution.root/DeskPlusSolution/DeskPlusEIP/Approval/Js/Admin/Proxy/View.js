View = {}

/**********************************************************************************************
작성자   : 이문호
작성일자 : 2012-05-08
작성목적 : 초기화 
**********************************************************************************************/

$(function () {

    $(document).ready(function () {

        // [수정]
        $('#cbtnMod').click(function (e) { e.preventDefault(); View.Mod(); });
        // [삭제]
        $('#cbtnDel').click(function (e) { e.preventDefault(); View.Del(); });
        // [목록]
        $('#cbtnList').click(function (e) { e.preventDefault(); View.List(); });

        if (IsProxy == "Y") {
            $("#spProxyIng").show();
            $("#spProxyCall").hide();
            $("#spProxyCallEnd").hide();
        }
        else {
            $("#spProxyIng").hide();

            if ($("#hdIsCall").val() == "Y") {
                $("#spProxyCall").show();
                $("#spProxyCallEnd").hide();
            }
            else {
                $("#spProxyCallEnd").show();
                $("#spProxyCall").hide();
            }
        }

        if (AllDay == "1") {
            $("#spAllDay").show();
        }
        else {
            $("#spAllDay").hide();
        }

        View.Resource();

    });
});

/**********************************************************************************************
작성자   : 
작성일자 :
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/
View.Resource = function () {

    if ($(".spIng") != undefined) { $(".spIng").text(R_Msg_Resource06); }
    if ($(".spCall") != undefined) { $(".spCall").text(R_Msg_Resource08); } //대기
    if ($(".spCallEnd") != undefined) { $(".spCallEnd").text(R_Msg_Resource09); } //종료
}

/**************************************************************
작성자 : 
작성일자 : 
작성목적 : 대결자 수정
**************************************************************/
View.Mod = function () {
    ParamArray = {
        'currentPage': CurrentPage,
        'recordPerPage': RecordPerPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'targetProxyID': TargetProxyID
    };

    CallEncrypt(CallGetParam());

    location.href = "Mod.aspx?p=" + GetEncryptValue;
}

/**************************************************************
작성자 : 
작성일자 : 
작성목적 : 대결자 삭제
**************************************************************/
View.Del = function () {
    if (confirm(R_ConfirmDeleteProxyPerson)) {
        __doPostBack('lbtnDel', '');
    }
}

/**************************************************************
작성자 : 
작성일자 : 
작성목적 : 목록
**************************************************************/
View.List = function () {

    ParamArray = {
        'currentPage': CurrentPage,
        'recordPerPage': RecordPerPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam());

    location.href = "List.aspx?p=" + GetEncryptValue;
}