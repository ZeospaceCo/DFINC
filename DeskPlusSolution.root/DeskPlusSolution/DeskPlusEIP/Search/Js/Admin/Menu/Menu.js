var Menu = {};

/**************************************************
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        $(".accordion").accordion({
            fillSpace: true,
            animated: 'slide',
            active: AccordionActive
            //event: "mouseover",
        });

        $("#accordionResizer").resizable({
            minHeight: 0,
            resize: function () { $("#accordion").accordion("resize"); }
        });

        //색인 관리
        $('#indexDefault').click(function (e) {
            e.preventDefault();
            Menu.GoListPage('indexDefault');
        });

        //색인 결과
        $('#indexResult').click(function (e) {
            e.preventDefault();
            Menu.GoListPage('indexResult');
        });

        //색인스케줄러
        $('#indexSchedule').click(function (e) {
            e.preventDefault();
            Menu.GoListPage('indexSchedule');
        });

        //검색결과
        $('#searchResult').click(function (e) {
            e.preventDefault();
            Menu.GoListPage('searchResult');
        });

        Menu.Init();
    });
});

/**************************************************************
초기화
**************************************************************/
Menu.Init = function () {
    Menu.ContentFrame = parent.frmContent;
    //    Menu.Control = UCStatusTreeScript_tvStatusTreeScript;
    //    Menu.InitNodeID = menuID;
    //    Menu.SelectedNodeID = menuID;

    Menu.GoListPage('indexDefault');
}

/**************************************************************
리스트 페이지 호출하기
**************************************************************/
Menu.GoListPage = function (menuID) {
    var listPage = "../Index/Default.aspx";

    if (menuID == "") {
        menuID = "indexDefault";
    }

    switch (menuID) {
        case "indexDefault": //색인관리
            listPage = "../Index/Default.aspx";

            ParamArray = {
                'companyID': 1
            };

            break;
        case "indexResult": //색인결과
            listPage = "../Index/List.aspx";

            ParamArray = {
                'companyID': 1
            };
            break;
        case "indexSchedule": //색인스케줄러
            listPage = "../Index/Schedule.aspx";

            ParamArray = {
                'companyID': 1
            };
            break;
        case "searchResult": //검색결과
            listPage = "../../List.aspx";

            ParamArray = {
                'companyID': 1
            };
            break;
        default:
            break;
    }

    if (listPage != "" && Menu.ContentFrame) {
        CallEncrypt(CallGetParam());
        Menu.ContentFrame.location.href = listPage + "?p=" + GetEncryptValue;
    }
}