Tree = {};
Tree.TreeFrame = parent.frmLeft;

Tree.Control = null;
Tree.InitNodeID = 0;
Tree.ContentFrame = null;


/**************************************************
작성자 : famo23
작성일자 : 2013-11-25
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        Tree.Init();
    });
});

// 메뉴 Accordion 초기화
$(function () {
    $(".accordion").accordion({
        fillSpace: true,
        animated: 'slide',
        //event: "mouseover"
        active: AccordionActive
    });

    $("#accordionResizer").resizable({
        minHeight: 0,
        resize: function () {
            $("#accordion").accordion("resize");
        }
    });
});


/**************************************************************
onCallbackComplete 
**************************************************************/
function onCallbackComplete(node) {

}

/**************************************************
작성자 : famo23
작성일자 : 2013-11-25
작성목적 : 초기화
***************************************************/
Tree.Init = function () {
    Tree.ContentFrame = parent.frmContent;
    Tree.Control = UCDisuseTree_tvDisuseTree;
}

/**************************************************
작성자 : famo23
작성일자 : 2013-11-25
작성목적 : onNodeSelect 트리 노드 선택
***************************************************/
function onNodeSelect(node) {

    FormID = node.ID;
    var listPage = "List.aspx";

    ParamArray = {
        'formID': FormID
    };

    if (listPage != "" && Tree.ContentFrame) {
        CallEncrypt(CallGetParam());
        Tree.ContentFrame.location.href = listPage + "?p=" + GetEncryptValue;
    }
}


