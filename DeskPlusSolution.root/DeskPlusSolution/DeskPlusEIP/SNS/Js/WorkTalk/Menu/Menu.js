var Menu = {};

/**************************************************
작성자 : famo23
작성일자 : 2013-05-10
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        $("#accordionResizer").resizable({
            minHeight: 0,
            resize: function () { $("#accordion").accordion("resize"); }
        });

        Menu.Init();
    });
});

/**************************************************
작성자 : famo23
작성일자 : 2013-05-10
작성목적 : 초기화
***************************************************/
Menu.Init = function () {
    Menu.ContentFrame = parent.frmContent;
    Menu.GoListPage("2");

    /*
    //1이면 우측 보기페이지에서 갱신처리한것
    if (ReLoad == "") {
    var listType = $("#hdnCabinetID").val();
    if (listType != "") {
    var listTypeStr = listType.split(",");
    Tree.GoList(listTypeStr[1]);
    }
    }
    */
}

/**************************************************************
작성자 : famo23
작성일자 : 2013-05-10
작성목적 : 리스트 페이지 호출하기
**************************************************************/
Menu.GoListPage = function (menuID) {
    var listPage = "../Content/List.aspx";

    if (menuID == "") {
        menuID = "2";
    }

    ParamArray = {
        'menuID': menuID
    };

    if (listPage != "" && Menu.ContentFrame) {
        CallEncrypt(CallGetParam());
        Menu.ContentFrame.location.href = listPage + "?p=" + GetEncryptValue;
    }
}

/**************************************************
작성자 : famo23
작성일자 : 2013-05-10
작성목적 : onNodeSelect 트리 노드 선택
***************************************************/
function onNodeSelect(node) {
    var menuID = node.ID;
    Menu.GoListPage(menuID);
}

/**************************************************************
onCallbackComplete
**************************************************************/
function onCallbackComplete(node) {
}