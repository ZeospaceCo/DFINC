ListMain = {}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2014-02-05
작성목적 : 초기화 
**********************************************************************************************/

$(function () {
    $(document).ready(function () {
       
        //관리자
        $('#cbtnAdmin').click(function (e) {
            e.preventDefault();
            parent.location.href = "/DeskPlusEIP/Visit/Admin/Default.aspx";
        });

        //예약
        $('#cbtnAdd').click(function (e) {
            e.preventDefault();
            ListMain.Add();
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

        ListMain.Init(TagGubun);
        //ListMain.TabPageMove(TagGubun);

        //검색어입력
        $("#ctbSearch").attr("placeholder", R_PlaceholderText);
    });
});

/******************************************
작성자: famo23
작성일자 : 2014-02-05
작성목적: 탭 페이지 호출
****************************************/
ListMain.Init = function (arg) {

    $("#aDay").attr("class", "");
    $("#aWeek").attr("class", "");
    $("#aMonth").attr("class", "");

    ParamArray = {
        'tmpDate': TmpDate,
        'currentPage': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam());

    TagGubun = arg;

    switch (arg) {
        case "day":
            $("#aDay").attr("class", "tab_on");
            $("#frmContent1").attr("src", "ListDay.aspx?p=" + GetEncryptValue);
          
            break;
        case "week":
            $("#aWeek").attr("class", "tab_on");
            $("#frmContent1").attr("src", "ListWeek.aspx?p=" + GetEncryptValue);
           
            break;
        case "month":
            $("#aMonth").attr("class", "tab_on");
            $("#frmContent1").attr("src", "ListMonth.aspx?p=" + GetEncryptValue);
        
            break;
    }
}



/******************************************
작성자:maniaysw
작성일:2017-01-20
용도: 마지막으로 클릭한 탭을 DB에 저장
****************************************/
ListMain.ModTabMenu = function (_type) {
    
    var url = "/DeskPlusEIP/Visit/Ws/Content/Mod.asmx/ModVisitTabMenu";
    ParamArray = {
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
작성자: famo23
작성일자 : 2014-02-05
작성목적: 탭 페이지 호출
****************************************/
ListMain.TabPageMove = function (arg) {

    TagGubun = arg;

    ParamArray = {
        'tmpDate': TmpDate,
        'currentPage': "1",
        'searchField': SearchField,
        'searchValue': SearchValue,
        'tagGubun': TagGubun
    };
    ListMain.ModTabMenu(TagGubun);
    CallEncrypt(CallGetParam());
    location.href = "ListMain.aspx?p=" + GetEncryptValue;  

/*
    $("#aDay").attr("class", "");
    $("#aWeek").attr("class", "");
    $("#aMonth").attr("class", "");

    ParamArray = {
        'tmpDate': TmpDate,
        'currentPage': CurrentPage,
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam());

    TagGubun = arg;

    switch (arg) {
        case "day":
            $("#aDay").attr("class", "tab_on");
            $("#frmContent1").attr("src", "ListDay.aspx?p=" + GetEncryptValue);

            break;
        case "week":
            $("#aWeek").attr("class", "tab_on");
            $("#frmContent1").attr("src", "ListWeek.aspx?p=" + GetEncryptValue);

            break;
        case "month":
            $("#aMonth").attr("class", "tab_on");
            $("#frmContent1").attr("src", "ListMonth.aspx?p=" + GetEncryptValue);

            break;
    }
*/

}


/******************************************
작성자: famo23
작성일자 : 2014-02-05
작성목적: 방문 예약
****************************************/
ListMain.Add = function () {

    ParamArray = {
        'tagGubun': TagGubun
    };

    CallEncrypt(CallGetParam());

    location.href = "AddN.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2014-02-05
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
작성일자 : 2014-02-05
작성목적 : 검색
***************************************************/
ListMain.Search = function () {

    var response = ListMain.SettingSearch();
    if (response == false) return;

    var tmpDate = frmContent1.TmpDate;

    ParamArray = {
        'tmpDate': tmpDate,
        'currentPage': "1",
        'searchField': SearchField,
        'searchValue': SearchValue,
        'tagGubun': TagGubun
    };

    CallEncrypt(CallGetParam());
    location.href = "ListMain.aspx?p=" + GetEncryptValue;  

    /*
    ParamArray = {
        'tmpDate': tmpDate,
        'currentPage': "1",
        'searchField': SearchField,
        'searchValue': SearchValue
    };

    CallEncrypt(CallGetParam());

    switch (TagGubun) {
        case "day":
            $("#aDay").attr("class", "tab_on");
            $("#frmContent1").attr("src", "ListDay.aspx?p=" + GetEncryptValue);

            break;
        case "week":
            $("#aWeek").attr("class", "tab_on");
            $("#frmContent1").attr("src", "ListWeek.aspx?p=" + GetEncryptValue);

            break;
        case "month":
            $("#aMonth").attr("class", "tab_on");
            $("#frmContent1").attr("src", "ListMonth.aspx?p=" + GetEncryptValue);

            break;
    }
    */
}

/**************************************************
작성자   : famo23
작성일자 : 2014-02-05
작성목적 : 인쇄
***************************************************/
ListMain.printElem = function (options) {
    frmContent1.$('#toPrint').append("<link rel='stylesheet' type='text/css' href='" + FullDomainPath + "/DeskPlusEIP/Util/Print/Print.css'>"); //크롬에서 배경 그래픽 적용
    frmContent1.$('#toPrint').printElement(options);
}