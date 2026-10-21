Info = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //확인
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            Info.Confirm();
        });

        $('#cbtnUser').click(function (e) {
            e.preventDefault();
            Info.GoList();
        });

        $(".noVisible").hide();

        $("#ctbArmyJoinDate").datepick($.datepicker.regional[PreLang]);
        $("#ctbArmyRetireDate").datepick($.datepicker.regional[PreLang]);

        $("#ctbArmyJoinDate").keydown(function (e) { return false; });
        $("#ctbArmyRetireDate").keydown(function (e) { return false; });
    });
});

/**************************************************
작성자   : shine
작성일자 : 2012-06-19
작성목적 : 연동 정보 저장
***************************************************/
Info.Confirm = function () {

    // 종합검진대상유무 체크
    if ($("#ctbAllCheck").val() != "" && Info.GetValueCheck($("#ctbAllCheck").val()) == "0") {
        alert("종합검진대상유무 항목에 '유' 혹은 '무' 만 기재하세요.");
        $("#ctbAllCheck").focus();
        return;
    }

    // 인사평가대상
    if ($("#ctbMPDS").val() != "" && Info.GetValueCheck($("#ctbMPDS").val()) == "0") {
        alert("인사평가대상 항목에 '유' 혹은 '무' 만 기재하세요.");
        $("#ctbMPDS").focus();
        return;
    }

    // 1차평가자유무
    if ($("#ctbFGubun").val() != "" && Info.GetValueCheck($("#ctbFGubun").val()) == "0") {
        alert("1차평가자유무 항목에 '유' 혹은 '무' 만 기재하세요.");
        $("#ctbFGubun").focus();
        return;
    }

    // 2차평가자유무
    if ($("#ctbSGubun").val() != "" && Info.GetValueCheck($("#ctbSGubun").val()) == "0") {
        alert("2차평가자유무 항목에 '유' 혹은 '무' 만 기재하세요.");
        $("#ctbSGubun").focus();
        return;
    }

    // 병역필 유무
    if ($("#ctbArmyType").val() != "" && Info.GetValueCheck($("#ctbArmyType").val()) == "0") {
        alert("병역필 유무 항목에 '유' 혹은 '무' 만 기재하세요.");
        $("#ctbArmyType").focus();
        return;
    }

    // 보훈대상유무
    if ($("#ctbVeteranType").val() != "" && Info.GetValueCheck($("#ctbVeteranType").val()) == "0") {
        alert("보훈대상유무 항목에 '유' 혹은 '무' 만 기재하세요.");
        $("#ctbVeteranType").focus();
        return;
    }

    // 장애 유무
    if ($("#ctbObstacleType").val() != "" && Info.GetValueCheck($("#ctbObstacleType").val()) == "0") {
        alert("장애 유무 항목에 '유' 혹은 '무' 만 기재하세요.");
        $("#ctbObstacleType").focus();
        return;
    }

    __doPostBack('lbtnRegister', '');
}

/**************************************************
작성자   : shine
작성일자 : 2012-06-19
작성목적 : 기본정보로 이동
***************************************************/
Info.GoList = function () {
    var goList = "";
    goList = "/DeskPlusEIP/Account/Admin/User/Mod.aspx";

    ParamArray = {
        'userID': UserID,
        'deptID': DeptID,
        'currentPage': CurrentPage,
        'recordPerPage': RecordPerPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchGubun': SearchGubun,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': Order,
        'listPage': ListPage
    };

    CallEncrypt(CallGetParam());
    var url = goList + "?p=" + GetEncryptValue;
    location = url;
}

/**************************************************
작성자   : shine
작성일자 : 2012-06-19
작성목적 : "유","무" 체크
***************************************************/
Info.GetValueCheck = function (arg) {
    var check = "0";
    arg = arg.replace(/^\s\s*/, "").replace(/\s\s*$/, "");

    if (arg == "유" || arg == "무") check = "1";

    return check;
}