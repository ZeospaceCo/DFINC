Tree = {};
Tree.TreeFrame = parent.frmLeft;
Tree.ContentFrame = parent.frmContent;
Tree.Control = null;

/**************************************************
작성자 : famo23
작성일자 : 2012-02-21
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        $(".accordion").accordion({
            fillSpace: true,
            animated: 'slide',
            //event: "mouseover"
            active: AccordionActive,
            overTop: 55
        });

        /*
        $("#accordionResizer").resizable({
            minHeight: 0,
            resize: function () {
                $("#accordion").accordion("resize");
            }
        });
        */

        //나의 예약
        $('#MyReserve').click(function (e) {
            parent.frmContent.location.href = "/DeskPlusEIP/RM/Content/MyList.aspx";
        });

        //나의 예약
        $('#MyReserveSub').click(function (e) {
            parent.frmContent.location.href = "/DeskPlusEIP/RM/Content/MyList.aspx";
        });

        //승인 대기 예약
        $('#AdminReserve').click(function (e) {
            parent.frmContent.location.href = "/DeskPlusEIP/RM/Content/Manager/Usable0List.aspx";
        });

        //승인 대기 예약
        $('#Usable0Reserve').click(function (e) {
            parent.frmContent.location.href = "/DeskPlusEIP/RM/Content/Manager/Usable0List.aspx";
        });

        // 승인 완료 예약
        $('#Usable1Reserve').click(function (e) {
            parent.frmContent.location.href = "/DeskPlusEIP/RM/Content/Manager/Usable1List.aspx";
        });

        // 승인 거부 예약
        $('#Usable2Reserve').click(function (e) {
            parent.frmContent.location.href = "/DeskPlusEIP/RM/Content/Manager/Usable2List.aspx";
        });

        // 자원예약
        $('#ReserveAdd').click(function (e) {

            e.preventDefault();
            Tree.Add();

        });
        
        if (parent.RMBasePage != "") {
            $('#' + parent.RMBasePage).click();
        }


        Tree.Init();

    });
});

/**************************************************************
onCallbackComplete
**************************************************************/
function onCallbackComplete(node) {
}

/**************************************************
작성자 : famo23
작성일자 : 2012-02-21
작성목적 : 초기화
***************************************************/
Tree.Init = function () {

    if (AccordionActive == "1") {
        parent.frmContent.location.href = "/DeskPlusEIP/RM/Content/MyList.aspx";
    }

    /*
    Tree.Control = tvFolderTree;

    if (Tree.Control != null) {
    if (Tree.Control.SelectedNode != undefined) {
    ParentCategoryID = Tree.Control.SelectedNode.GetProperty("ParentCategoryID");
    RealCategoryID = Tree.Control.SelectedNode.ID;

    var listPage = "../Content/ListMain.aspx";

    ParamArray = {
    'parentCategoryID': ParentCategoryID,
    'categoryID': RealCategoryID
    };

    if (listPage != "" && Tree.ContentFrame) {
    CallEncrypt(CallGetParam());
    Tree.ContentFrame.location.href = listPage + "?p=" + GetEncryptValue;
    }
    }
    else {
    var listPage = "../Content/ListMain.aspx";

    ParamArray = {
    'parentCategoryID': 0,
    'categoryID': 0
    };

    if (listPage != "" && Tree.ContentFrame) {
    CallEncrypt(CallGetParam());
    Tree.ContentFrame.location.href = listPage + "?p=" + GetEncryptValue;
    }
    }
    }
    */
}

/**************************************************
작성자 : famo23
작성일자 : 2012-02-21
작성목적 : onNodeSelect 트리 노드 선택
***************************************************/
function onNodeSelect(node) {
    ParentCategoryID = node.GetProperty("ParentCategoryID");
    RealCategoryID = node.ID;

    if (RealCategoryID == "0") {

        if (Tree.ContentFrame) {
            Tree.ContentFrame.location.href = "/DeskPlusEIP/RM/Home/Main.aspx";
        }
    }
    else {
        ParamArray = {
            'parentCategoryID': ParentCategoryID,
            'categoryID': RealCategoryID
        };

        if (Tree.ContentFrame) {
            CallEncrypt(CallGetParam());
            Tree.ContentFrame.location.href = "../Content/ListMain.aspx?p=" + GetEncryptValue;
        }
    }
}



/******************************************
작성자: famo23
작성일자: 2015-12-24
작성목적: 자원 예약
****************************************/
Tree.Add = function () {

    $(".accordion").accordion({
        fillSpace: true,
        animated: 'slide',
        //event: "mouseover"
        active: 1,
        overTop: 55
    });

    ParamArray = {
        'tagGubun': 'my'
    };

    CallEncrypt(CallGetParam());

    Tree.ContentFrame.location.href = "/DeskPlusEIP/RM/Content/AddN.aspx?p=" + GetEncryptValue;
}