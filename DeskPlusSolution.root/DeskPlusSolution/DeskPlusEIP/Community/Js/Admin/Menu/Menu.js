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

        //커뮤니티 개설승인
        $('#aAdd').click(function (e) {
            e.preventDefault();
            Menu.GoListPage('Add');
        });

        //커뮤니티 삭제
        $('#aClose').click(function (e) {
            e.preventDefault();
            Menu.GoListPage('Close');
        });

        //분류관리
        $('#aCategory').click(function (e) {
            e.preventDefault();
            Menu.GoListPage('Category');
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

    Menu.GoListPage('Add');
}



/**************************************************************
리스트 페이지 호출하기
**************************************************************/
Menu.GoListPage = function (menuID) {

    var listPage = "../Site/ListAdd.aspx";

    if (menuID == "") {
        menuID = "Add";
    }


    switch (menuID) {
        case "Add": //승인요청
            listPage = "../Site/ListAdd.aspx";

            ParamArray = {
                'companyID': 1
            };

            break;
        case "Close": //폐쇄
            listPage = "../Site/ListClose.aspx";

            ParamArray = {
                'companyID': 1
            };
            break;       
        case "Category": //분류관리
            listPage = "../Category/List.aspx";

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
