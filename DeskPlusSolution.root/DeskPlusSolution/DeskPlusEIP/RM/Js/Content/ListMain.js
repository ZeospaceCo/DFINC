ListMain = {}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-02-20
작성목적 : 초기화
**********************************************************************************************/

$(function () {

    $(document).ready(function () {
        //관리자
        $('#cbtnAdmin').click(function (e) {
            e.preventDefault();
            parent.location.href = "/DeskPlusEIP/RM/Admin/Default.aspx";
        });

        //예약
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            ListMain.Add();
        });

        //리스트형 저장
        $('#listType1').click(function () {
            ListMain.GoPageListType("1");
        });

        //블로그형 저장
        $('#listType2').click(function () {
            ListMain.GoPageListType("2");
        });

        //검색입력필드 엔터 이벤트 기술
        $("#ctbSearch").keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                ListMain.Search();
            }
        });

        //검색
        $('#cbtnSearch').click(function (e) {
            e.preventDefault();
            ListMain.Search();
        });

        //인쇄
        $('#cbtnPrint').click(function (e) {
            e.preventDefault();
            ListMain.printElem({ leaveOpen: false, printMode: 'popup' });
        });

        //$("#frmContent1").height($(window).height() - 100);

        // 대기자원 카운트 메뉴트리에 보이게 처리
        if (CntYN == "Y") {
            if (RmCnt == 0) {
                if (parent.frmLeft.mCnt) {
                    parent.frmLeft.mCnt.innerText = "";
                    parent.frmLeft.mSubCnt.innerText = "";
                }
            }
            else {
                parent.frmLeft.mCnt.innerText = "(" + RmCnt + ")";
                parent.frmLeft.mSubCnt.innerText = "(" + RmCnt + ")";
            }
        }

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);

        ListMain.PageMove(TagGubun);
    });
});

/******************************************
작성자: famo23
작성일자: 2012-02-20
작성목적: 탭 페이지 호출
****************************************/
ListMain.PageMove = function (arg) {
    $("#aDay").attr("class", "");
    $("#aWeek").attr("class", "");
    $("#aMonth").attr("class", "");

    ParamArray = {
        'categoryID': CategoryID,
        'tmpDate': TmpDate,
        'groupGubun': GroupGubun,
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam());

    TagGubun = arg;

    if (GroupGubun == "1") {
        //$("#list").attr("src", ImagePath + "/Btn/list_board_on.gif");
        //$("#whole").attr("src", ImagePath + "/Btn/list_blog.gif");
        $("#list").addClass("on");
        $("#whole").removeClass("on");

        switch (TagGubun) {
            case "day":
                $("#aDay").attr("class", "tab_on");
                $("#TypeGubun").css("display", "");
                $("#frmContent1").attr("src", "Listday.aspx?p=" + GetEncryptValue);
                break;
            case "week":
                $("#aWeek").attr("class", "tab_on");
                $("#TypeGubun").css("display", "");
                $("#frmContent1").attr("src", "ListWeek.aspx?p=" + GetEncryptValue);
                break;
            case "month":
                $("#aMonth").attr("class", "tab_on");
                $("#TypeGubun").css("display", "none");
                $("#frmContent1").attr("src", "ListMonth.aspx?p=" + GetEncryptValue);
                break;
        }
    }
    else if (GroupGubun == "2") {
        $("#list").removeClass("on");
        $("#whole").addClass("on"); ;

        switch (TagGubun) {
            case "day":
                $("#aDay").attr("class", "tab_on");
                $("#TypeGubun").css("display", "");
                $("#frmContent1").attr("src", "WholeDay.aspx?p=" + GetEncryptValue);
                break;
            case "week":
                $("#aWeek").attr("class", "tab_on");
                $("#TypeGubun").css("display", "");
                $("#frmContent1").attr("src", "WholeWeek.aspx?p=" + GetEncryptValue);
                break;
            case "month":
                $("#aMonth").attr("class", "tab_on");
                $("#TypeGubun").css("display", "none");
                $("#frmContent1").attr("src", "ListMonth.aspx?p=" + GetEncryptValue);
                //$("#frmContent1").attr("src", "WholeMonth.aspx?p=" + GetEncryptValue);                
                break;
        }
    }
}



/******************************************
작성자:maniaysw
작성일:2017-01-20
용도: 마지막으로 클릭한 탭을 DB에 저장
****************************************/
ListMain.ModTabMenu = function (_type) {
    var url = "/DeskPlusEIP/RM/Ws/Home/Main.asmx/ModRMTabMenu";
    ParamArray = {
        'categoryID': CategoryID,
        'tmpDate': TmpDate,
        'tabType': _type
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListMain.SuccessTabMenu");

    AjaxService(url, data, obj);
}

ListMain.SuccessTabMenu = function (data, status) {
    //alert(data.d); //성공시 true
}

/******************************************
작성자:maniaysw
작성일:2017-02-01
용도: 마지막으로 클릭한 Type을 DB에 저장
****************************************/
ListMain.ModTypeMenu = function (_type) {
    var url = "/DeskPlusEIP/RM/Ws/Home/Main.asmx/ModRMTypeMenu";
    ParamArray = {
        'categoryID': CategoryID,
        'tmpDate': TmpDate,
        'type': _type
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("ListMain.SuccessTypeMenu");

    AjaxService(url, data, obj);
}

ListMain.SuccessTypeMenu = function (data, status) {
    //alert(data.d); //성공시 true
}


/******************************************
작성자: famo23
작성일자: 2012-02-20
작성목적: 탭 페이지 호출
****************************************/
ListMain.TabPageMove = function (arg) {
    $("#aDay").attr("class", "");
    $("#aWeek").attr("class", "");
    $("#aMonth").attr("class", "");

    ParamArray = {
        'categoryID': CategoryID,
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam());

    TagGubun = arg;

    if (GroupGubun == "1") {
        $("#list").addClass("on");
        $("#whole").removeClass("on");

        switch (TagGubun) {
            case "day":
                $("#aDay").attr("class", "tab_on");
                $("#TypeGubun").css("display", "");
                $("#frmContent1").attr("src", "Listday.aspx?p=" + GetEncryptValue);
                ListMain.ModTabMenu("day");
                break;
            case "week":
                $("#aWeek").attr("class", "tab_on");
                $("#TypeGubun").css("display", "");
                $("#frmContent1").attr("src", "ListWeek.aspx?p=" + GetEncryptValue);
                ListMain.ModTabMenu("week");
                break;
            case "month":
                $("#aMonth").attr("class", "tab_on");
                $("#TypeGubun").css("display", "none");
                $("#frmContent1").attr("src", "ListMonth.aspx?p=" + GetEncryptValue);
                ListMain.ModTabMenu("month");
                break;
        }
    }
    else if (GroupGubun == "2") {
        $("#list").removeClass("on");
        $("#whole").addClass("on");


        switch (TagGubun) {
            case "day":
                $("#aDay").attr("class", "tab_on");
                $("#TypeGubun").css("display", "");
                $("#frmContent1").attr("src", "WholeDay.aspx?p=" + GetEncryptValue);
                ListMain.ModTabMenu("day");
                break;

            case "week":
                $("#aWeek").attr("class", "tab_on");
                $("#TypeGubun").css("display", "");
                $("#frmContent1").attr("src", "WholeWeek.aspx?p=" + GetEncryptValue);
                ListMain.ModTabMenu("week");
                break;

            case "month":
                $("#aMonth").attr("class", "tab_on");
                $("#TypeGubun").css("display", "none");
                $("#frmContent1").attr("src", "ListMonth.aspx?p=" + GetEncryptValue);
                //$("#frmContent1").attr("src", "WholeMonth.aspx?p=" + GetEncryptValue);
                ListMain.ModTabMenu("month");
                break;
        }
    }
}

/******************************************
작성자: famo23
작성일자: 2012-02-20
작성목적: 자원 예약
****************************************/
ListMain.Add = function () {
    ParamArray = {
        'categoryID': CategoryID,
        'tagGubun': TagGubun,
        'groupGubun': GroupGubun
    };

    CallEncrypt(CallGetParam());

    location.href = "AddN.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2012-02-22
작성목적 : 검색 필드 설정하기
***************************************************/
ListMain.SettingSearch = function () {
    SearchField = $("#ddlSearchField").val();
    SearchValue = $("#ctbSearch").val().securityText();
    $("#ctbSearch").val(SearchValue);

    return true;
}

/**************************************************
작성자   : famo23
작성일자 : 2012-02-22
작성목적 : 검색
***************************************************/
ListMain.Search = function () {
    var response = ListMain.SettingSearch();
    if (response == false) return;

    var tmpDate = frmContent1.$("#timeList").text();
    //tmpDate = tmpDate.split('.').join('-');
    tmpDate = frmContent1.TmpDate;

    ParamArray = {
        'categoryID': CategoryID,
        'tmpDate': tmpDate,
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam());

    if (GroupGubun == "1") {
        $("#list").addClass("on");
        $("#whole").removeClass("on");

        switch (TagGubun) {
            case "day":
                $("#aDay").attr("class", "tab_on");
                $("#TypeGubun").css("display", "");
                $("#frmContent1").attr("src", "Listday.aspx?p=" + GetEncryptValue);
                break;
            case "week":
                $("#aWeek").attr("class", "tab_on");
                $("#TypeGubun").css("display", "");
                $("#frmContent1").attr("src", "ListWeek.aspx?p=" + GetEncryptValue);
                break;
            case "month":
                $("#aMonth").attr("class", "tab_on");
                $("#TypeGubun").css("display", "none");
                $("#frmContent1").attr("src", "ListMonth.aspx?p=" + GetEncryptValue);

                break;
        }
    }
    else if (GroupGubun == "2") {
        $("#list").removeClass("on");
        $("#whole").addClass("on");

        switch (TagGubun) {
            case "day":
                $("#aDay").attr("class", "tab_on");
                $("#TypeGubun").css("display", "");
                $("#frmContent1").attr("src", "WholeDay.aspx?p=" + GetEncryptValue);
                break;
            case "week":
                $("#aWeek").attr("class", "tab_on");
                $("#TypeGubun").css("display", "");
                $("#frmContent1").attr("src", "WholeWeek.aspx?p=" + GetEncryptValue);
                break;
            case "month":
                $("#aMonth").attr("class", "tab_on");
                $("#TypeGubun").css("display", "none");
                $("#frmContent1").attr("src", "ListMonth.aspx?p=" + GetEncryptValue);
                //$("#frmContent1").attr("src", "WholeMonth.aspx?p=" + GetEncryptValue);

                break;
        }
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2012-03-16
작성목적 : 인쇄
***************************************************/
ListMain.printElem = function (options) {
    frmContent1.$('#toPrint').append("<link rel='stylesheet' type='text/css' href='" + FullDomainPath + "/DeskPlusEIP/Util/Print/Print.css'>"); //크롬에서 배경 그래픽 적용
    frmContent1.$('#toPrint').printElement(options);
}

/**************************************************
작성자   : shine
작성일자 : 2012-03-16
작성목적 : 리스트 타입선택시 이동하기
***************************************************/
ListMain.GoPageListType = function (gubun) {
    var iPageUrl = $.url(PageUrl);
    var iPath = iPageUrl.attr('path');
    GroupGubun = gubun;

    if (GroupGubun == "1") {
        ListMain.ModTypeMenu("1");
        $("#list").addClass("on");
        $("#whole").removeClass("on");

        switch (TagGubun) {
            case "day":
                $("#aDay").attr("class", "tab_on");
                $("#TypeGubun").css("display", "");
                $("#frmContent1").attr("src", "Listday.aspx?p=" + GetEncryptValue);

                break;
            case "week":
                $("#aWeek").attr("class", "tab_on");
                $("#TypeGubun").css("display", "");
                $("#frmContent1").attr("src", "ListWeek.aspx?p=" + GetEncryptValue);

                break;
            case "month":
                $("#aMonth").attr("class", "tab_on");
                $("#TypeGubun").css("display", "none");
                $("#frmContent1").attr("src", "ListMonth.aspx?p=" + GetEncryptValue);

                break;
        }
    }
    else if (GroupGubun == "2") {
        ListMain.ModTypeMenu("2");
        $("#list").removeClass("on");
        $("#whole").addClass("on");

        switch (TagGubun) {
            case "day":
                $("#aDay").attr("class", "tab_on");
                $("#TypeGubun").css("display", "");
                $("#frmContent1").attr("src", "WholeDay.aspx?p=" + GetEncryptValue);
                break;

            case "week":
                $("#aWeek").attr("class", "tab_on");
                $("#TypeGubun").css("display", "");
                $("#frmContent1").attr("src", "WholeWeek.aspx?p=" + GetEncryptValue);
                break;

            case "month":
                $("#aMonth").attr("class", "tab_on");
                $("#TypeGubun").css("display", "none");
                $("#frmContent1").attr("src", "ListMonth.aspx?p=" + GetEncryptValue);
                //$("#frmContent1").attr("src", "WholeMonth.aspx?p=" + GetEncryptValue);
                break;
        }
    }
}