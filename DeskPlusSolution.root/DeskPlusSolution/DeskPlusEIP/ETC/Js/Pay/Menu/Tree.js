var Tree = {};

Tree.ContentFrame = null;

/**************************************************
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        $(".accordion").accordion({
            fillSpace: true,
            animated: 'slide',
            active: 0
        });

        Tree.Init();
    });
});

/**************************************************************
초기화
**************************************************************/
Tree.Init = function () {
    Tree.ContentFrame = parent.frmContent;

    Tree.GoListPage();
}

/**************************************************************
리스트 페이지 호출하기
**************************************************************/
Tree.GoListPage = function () {

    var listPage = "../List.aspx";

    ParamArray = {     
        'formID': FormID
    };

    if (listPage != "" && Tree.ContentFrame) {
        CallEncrypt(CallGetParam());
        Tree.ContentFrame.location.href = listPage + "?p=" + GetEncryptValue;        
    }
}

/**************************************************
onNodeSelect 트리 노드 선택
***************************************************/
function onNodeSelect(node) {
    
    var formID = node.FormID;
    var isPay = (node.IsPay == undefined) ? "" : node.IsPay;
    var targetPage = "../List.aspx";

    ParamArray = {         
        'formID': formID,
        'isPay': isPay
    };

    if (targetPage != "" && Tree.ContentFrame) {
        CallEncrypt(CallGetParam());
        Tree.ContentFrame.location.href = targetPage + "?p=" + GetEncryptValue;        
    }
}

/**************************************************************
onCallbackComplete 
**************************************************************/
function onCallbackComplete(node) {

}