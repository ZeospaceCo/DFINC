Tree = {}
Tree.Control = null;

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        Tree.Init();
    });   
});


/**************************************************************
작성자   : famo23
작성일자 : 2014-01-17
작성목적 : 화면 설정 
**************************************************************/
Tree.Init = function () {

    Tree.Control = UCMDMTreeScript1_MDMMenuTreeScript1;
    MenuID = Tree.Control.SelectedNode.ID;

    ParamArray = {
        'menuID': MenuID
    };

    CallEncrypt(CallGetParam());
    parent.frmContent.location.href = "Add.aspx?p=" + GetEncryptValue;

}


/**************************************************
작성자   : famo23
작성일자 : 2014-01-17
작성목적 : 메뉴 선택
***************************************************/
function MenuNodeSelect_onNodeSelect(node) {

    MenuID = node.ID;   

    //파라미터값 배열에 담기
    ParamArray = {
        'menuID': MenuID
    };
    CallEncrypt(CallGetParam());

    parent.frmContent.location.href = "Add.aspx?p=" + GetEncryptValue;
   
}

/**************************************************************
onCallbackComplete 
**************************************************************/
function onCallbackComplete(node) {

}

