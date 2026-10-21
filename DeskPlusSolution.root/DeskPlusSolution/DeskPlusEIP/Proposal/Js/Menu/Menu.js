var Menu = {};

/**************************************************
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        

        $(".accordion").accordion({
            fillSpace: true,
            animated: 'slide',
            //event: "mouseover",
            active: AccordionActive

        });

        $("#accordionResizer").resizable({
            minHeight: 0,
            resize: function () {
                $("#accordion").accordion("resize");
            }
        });

        //제안제도
        $('#Statistics').click(function () {
            Menu.GoListPage('Statistics');
        });

        //제안하기
        $('#Write').click(function () {
            Menu.GoListPage('Write');
        });

        //나의 제안보기
        $('#Mine').click(function () {
            Menu.GoListPage('Mine');
        });

        //전체 제안 보기
        $('#All').click(function () {
            Menu.GoListPage('All');
        });

        //평가하기
        $('#Estimate').click(function () {
            Menu.GoListPage('Estimate');
        });

        //심사처리
        $('#AdminCheck').click(function () {
            Menu.GoListPage('AdminCheck');
        });

        //평가항목 설정
        $('#Item').click(function () {
            Menu.GoListPage('Item');
        });

        //1차 심사위원
        $('#Judge').click(function () {
            Menu.GoListPage('Judge');
        });

        //전체 제안보기
        $('#AdminAll').click(function () {
            Menu.GoListPage('AdminAll');
        });
    });

    Menu.Init();

});

Menu.Init = function () {

    // 1차 심사가자 아니면 평가하기 메뉴가 보이지 않음
    if (!JudgeAuth)
        $("#Estimate").hide();
}

/**************************************************************
 리스트 페이지 호출하기
**************************************************************/
Menu.GoListPage = function (menuID) {
    var listPage = "../Content/Statistics.aspx";

    if (menuID == "") {
        menuID = "Statistics";
    }

    switch (menuID) {
        case "Statistics":      // 제안제도
            listPage = "../Content/Statistics.aspx";
            break;
        case "Write":         // 제안하기
            listPage = "../Content/Write.aspx";
            ParamArray = { 'menuID': "Write" };
            break;
        case "Mine":            // 나의 제안보기
            listPage = "../Content/MyList.aspx";
            ParamArray = { 'menuID': "MyList" };
            break;
        case "All":             // 전체 제안보기
            listPage = "../Content/List.aspx";
            ParamArray = { 'menuID': "List" };
            break;
        case "Estimate":        // 평가하기
            listPage = "../Content/Judge/JudgeList.aspx";
            ParamArray = { 'menuID': "Estimate", 'judgeAuth': JudgeAuth };
            break;
        case "AdminCheck":      // 심사처리
            listPage = "../Content/Admin/JudgeView.aspx";
            ParamArray = { 'menuID': "AdminCheck" };
            break;
        case "Item":     // 평가항목 설정
            listPage = "../Content/Admin/EstimateItem.aspx";
            ParamArray = { 'menuID': "Item" };
            break;
        case "Judge":           // 1차 심사위원
            listPage = "../Content/Admin/AuthList.aspx";
            ParamArray = { 'menuID': "Judge" };
            break;
        case "AdminCheck":      // 전체 제안보기
            listPage = "../Content/List.aspx";
            ParamArray = { 'menuID': "List" };
            break;
        default:
            break;
    }

    CallEncrypt(CallGetParam());
    parent.frmContent.location = listPage + "?p=" + GetEncryptValue;
}