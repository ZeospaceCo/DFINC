Tree = {}

/**************************************************
초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        Tree.Init();

    });
});


/**************************************************************
Tree.Init 폴더 초기화
작성자   : shimpark
작성일자 : 2008-08-11
작성목적 : 첫화면 확장 
**************************************************************/
Tree.Init = function () {
    
    parent.frmFolderContent.location.href = "Register.aspx";
}


/**************************************************
작성자   : shimpark
작성일자 : 2011-01-11
작성목적 : 부서 선택
***************************************************/
function FaxNodeSelect_onNodeSelect(node) {

    var faxID = node.ID;
    var branchID = node.GetProperty("BranchID");
    var pageUrl = node.GetProperty("PageUrl");    

    //파라미터값 배열에 담기
    ParamArray = {
        'faxID': faxID,
        'branchID': branchID,
        'pageUrl': pageUrl
    };
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환

    parent.frmFolderContent.location.href = pageUrl + "?p=" + GetEncryptValue;
}

/**************************************************************
onCallbackComplete 
**************************************************************/
function FaxNodeSelect_onNodeComplete(node) {

}

