Main = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        Main.Init();
    });
});

/**************************************************
작성자   : 심재운
작성일자 :2011-06-15
작성목적 : 초기화 하기
***************************************************/
Main.Init = function () {
    Main.LoadBoaData();
    Main.LoadUnitData();
    Main.Resource();
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 리스트 초기화
***************************************************/
Main.LoadBoaData = function () {
    //검색시 페이지 카운트 및 리스트를 다시 가져옴
    var loadBoardSearchData = function () {
        //Main.GetBoardCount(); //검색 총 카운트 가져오기
        Main.GetBoardList(); //검색 리스트 가져오기
    }

    var boaList = BoaList.split(",");
    var boaHeaderList = BoaHeaderList.split(",");
    for (var i = 0; i < boaList.length; i++) {
        BoaID = boaList[i];
        BoaItem = i;
        loadBoardSearchData();

        switch (BoaItem) {
            case 0:
                $('#boa0').show();
                $('#header_0').text(boaHeaderList[i] + " " + R_MaxList);
                break;
            case 1:
                $('#boa1').show();
                $('#header_1').text(boaHeaderList[i] + " " + R_MaxList);
                break;
            case 2:
                $('#boa2').show();
                $('#header_2').text(boaHeaderList[i] + " " + R_MaxList);
                break;
            case 3:
                $('#boa3').show();
                $('#header_3').text(boaHeaderList[i] + " " + R_MaxList);
                break;
            case 4:
                $('#boa4').show();
                $('#header_4').text(boaHeaderList[i] + " " + R_MaxList);
                break;
        }
    }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 게시판 리스트 호출
**********************************************************************************************/
Main.GetBoardList = function () {
    var url = "/DeskPlusEIP/PMS/Ws/Project/Main.asmx/GetBoardPageList";

    //파라미터값 배열에 담기

    ParamArray = {
        'totalItemCount': TotalItemCount,
        'recordPerPage': RecordPerPage,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': OrderB,
        'projectID': ProjectID,
        'menuID': BoaID,
        'menuType': MenuType
    };

    var obj = eval("Main.SuccessGetBoardList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 게시판 리스트 표시
**********************************************************************************************/
Main.SuccessGetBoardList = function (data, status) {
    switch (BoaItem) {
        case 0:
            $('#placeholderB_0').setTemplateURL('/DeskPlusEIP/PMS/Jt/Project/JTBoardList.htm?seq=' + Guids(), null, { filter_data: false });
            $('#placeholderB_0').processTemplate(data.d);
            break;
        case 1:
            $('#placeholderB_1').setTemplateURL('/DeskPlusEIP/PMS/Jt/Project/JTBoardList.htm?seq=' + Guids(), null, { filter_data: false });
            $('#placeholderB_1').processTemplate(data.d);
            break;
        case 2:
            $('#placeholderB_2').setTemplateURL('/DeskPlusEIP/PMS/Jt/Project/JTBoardList.htm?seq=' + Guids(), null, { filter_data: false });
            $('#placeholderB_2').processTemplate(data.d);
            break;
        case 3:
            $('#placeholderB_3').setTemplateURL('/DeskPlusEIP/PMS/Jt/Project/JTBoardList.htm?seq=' + Guids(), null, { filter_data: false });
            $('#placeholderB_3').processTemplate(data.d);
            break;
        case 4:
            $('#placeholderB_4').setTemplateURL('/DeskPlusEIP/PMS/Jt/Project/JTBoardList.htm?seq=' + Guids(), null, { filter_data: false });
            $('#placeholderB_4').processTemplate(data.d);
            break;
    }
}

/**********************************************************************************************
작성자   : 심재운
작성일자 : 2011-04-14
작성목적 : 리스트 메뉴 다국어 처리
**********************************************************************************************/

Main.Resource = function () {
    //    $("#r_projectname")[0].innerHTML = R_projectname;
    $("#r_code")[0].innerHTML = R_Unit_Code;
    $("#r_title")[0].innerHTML = R_Unit_Title;
    $("#r_status")[0].innerHTML = R_Unit_Status;
    $("#r_name")[0].innerHTML = R_Unit_Owner;
    $("#r_planStartDate")[0].innerHTML = R_Unit_PlanStartDate;
    $("#r_planEndDate")[0].innerHTML = R_Unit_PlanEndDate1;
    $("#r_planday")[0].innerHTML = R_Unit_PlanDay;
    $("#r_weightpercent")[0].innerHTML = R_Unit_Register_ltlWeightPercent;
    $("#r_process")[0].innerHTML = R_ProjectList_Header6;
    $("#r_processpercent")[0].innerHTML = R_Unit_RealProcess;

    if ($(".r_username") != undefined) { $(".r_username").text(R_Board_List_Name); }
    if ($(".r_subject") != undefined) { $(".r_subject").text(R_Board_List_Title); }
    if ($(".r_writedate") != undefined) { $(".r_writedate").text(R_Board_List_DateTime); }
    if ($(".r_readcount") != undefined) { $(".r_readcount").text(R_Board_List_Viewer); }

    if ($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 웹서비스 게시판 카운트  호출
**********************************************************************************************/
Main.GetBoardCount = function () {
    var url = "/DeskPlusEIP/PMS/Ws/Project/Main.asmx/GetBoardListCount";

    ParamArray = {
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'projectID': ProjectID,
        'menuID': BoaID,
        'menuType': MenuType
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Main.SuccessGetBoardCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 게시물 카운트 이벤트
**********************************************************************************************/
Main.SuccessGetBoardCount = function (data, status) {
    TotalItemCount = data.d;
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 게시글 보기
***************************************************/
Main.ViewB = function (p_projectID, p_boardID, p_menuID, p_groupID, p_listOrder, p_depth) {
    ParamArray = {
        'projectID': p_projectID,
        'menuID': p_menuID,
        'menuType': MenuType,
        'rqProjectID': RqProjectID,
        'boardID': p_boardID,
        'groupID': p_groupID,
        'listOrder': p_listOrder,
        'depth': p_depth,
        'recordPerPage': '10'
    };

    CallEncrypt(CallGetParam());

    location = "/DeskPlusEIP/PMS/Board/View.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 리스트 초기화
***************************************************/
Main.LoadUnitData = function () {
    //Main.GetUnitCount(); //검색 총 카운트 가져오기
    Main.GetUnitList(); //검색 리스트 가져오기

    $('#headerUnit').text(UnitHeaderList + " " + R_MaxList);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 단위 리스트 호출
**********************************************************************************************/
Main.GetUnitList = function () {
    var url = "/DeskPlusEIP/PMS/Ws/Project/Main.asmx/GetUnitPageList";

    //파라미터값 배열에 담기

    ParamArray = {
        'totalItemCount': TotalItemCount,
        'recordPerPage': RecordPerPage,
        'currentPage': CurrentPage,
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'order': OrderB,
        'projectID': ProjectID,
        'menuID': BoaID,
        'menuType': MenuType,
        'weightTotal': WeightTotal
    };

    var obj = eval("Main.SuccessGetUnitList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-14
작성목적 : 성공시 단위 리스트 표시
**********************************************************************************************/
Main.SuccessGetUnitList = function (data, status) {
    $('#placeholderU').setTemplateURL('/DeskPlusEIP/PMS/Jt/Project/JTUnitList.htm?seq=' + Guids(), null, { filter_data: false });
    $('#placeholderU').processTemplate(data.d);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 웹서비스 단위 카운트  호출
**********************************************************************************************/
Main.GetUnitCount = function () {
    var url = "/DeskPlusEIP/PMS/Ws/Project/Main.asmx/GetUnitListCount";

    ParamArray = {
        'searchTerm': SearchTerm,
        'searchTermValue': SearchTermValue,
        'searchField': SearchField,
        'searchValue': SearchValue,
        'projectID': ProjectID,
        'menuID': MenuID,
        'menuType': MenuType
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Main.SuccessGetUnitCount");

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : shimpark
작성일자 : 2011-04-18
작성목적 : 성공시 , 단위 카운트 이벤트
**********************************************************************************************/
Main.SuccessGetUnitCount = function (data, status) {
    TotalItemCount = data.d;
}

/**************************************************
작성자   : shimpark
작성일자 : 2011-04-11
작성목적 : 게시글 보기
***************************************************/
Main.ViewU = function (p_projectID, p_unitID, p_menuID) {
    ParamArray = {
        'projectID': p_projectID,
        'menuID': UnitList,
        'menuType': '2',
        'rqProjectID': RqProjectID,
        'unitID': p_unitID,
        'recordPerPage': '10'
    };

    CallEncrypt(CallGetParam());

    location = "/DeskPlusEIP/PMS/Unit/View.aspx?p=" + GetEncryptValue;
}